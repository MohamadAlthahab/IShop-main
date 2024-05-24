using AutoMapper;
using IShop.Data;
using IShop.IRepository;
using IShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CartController> _logger;
        private readonly IMapper _mapper;

        public CartController(IUnitOfWork unitOfWork, ILogger<CartController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
         [HttpPost]
         [Authorize]
         public async Task<IActionResult> AddToCart([FromBody] CartProductDTO cartProductDTO)
         {
             var claimsIdentity = (ClaimsIdentity)User.Identity;
             var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

             var user = await _unitOfWork.User.Get(u => u.Id ==  userId);

             var cart = await _unitOfWork.Cart.Get(u => u.Id == user.CartId);

             var product = await _unitOfWork.Product.Get(u => u.Id == cartProductDTO.ProductId);

            cart.Count = 1;

             cart.Total =cart.Total + ((product.Price * 0.01 + product.Price) * cart.Count);

            if (cart.Count > 0)
            {
                cart.Count += cart.Count;
                _unitOfWork.Cart.Update(cart);
            }
            else
            {
                _unitOfWork.Cart.Update(cart);
            }
            await _unitOfWork.Save();
             Cart_Product cart_Product = new()
             {
                 ProductId = product.Id,
                 CartId = user.CartId,
             };
             _unitOfWork.Cart_Product.Add(cart_Product);
             await _unitOfWork.Save();
             return Ok();
         }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllProduct()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await _unitOfWork.User.Get(u => u.Id == userId);

            var product = (await _unitOfWork.Cart_Product.GetAll(u => u.CartId == user.CartId)).Select(u => u.ProductId);

            var pro = await _unitOfWork.Product.GetAll(u => product.Contains(u.Id));
            return Ok(pro);

        }
        [HttpGet]
        [Authorize]
        [Route("GetDetailsCart")]
        public async Task<IActionResult> GetDetailsCart()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await _unitOfWork.User.Get(u => u.Id == userId);

            var cartId = await _unitOfWork.Cart.Get(u => u.Id == user.CartId);

            var cart = await _unitOfWork.Cart.Get(u => u == cartId);

            var cartDetails = _mapper.Map<Cart>(cart);

            return Ok(cartDetails);
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _unitOfWork.Cart_Product.Get(u => u.Id == id);
                await _unitOfWork.Cart_Product.Remove(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internet Server Error. Please Try Again. ");
            }
        }

        [HttpDelete]
        [Route("DeleteCart")]
        public async Task<IActionResult> DeleteCart()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await _unitOfWork.User.Get(u => u.Id == userId);

            var cartId = await _unitOfWork.Cart.Get(u => u.Id == user.CartId);

            var CP = await _unitOfWork.Cart_Product.Get(u => u.CartId == user.CartId);

            await _unitOfWork.Cart_Product.Remove(CP.Id);
            await _unitOfWork.Cart.Remove(cartId.Id);
            await _unitOfWork.Save();

            return NoContent();
        }
    }
}
