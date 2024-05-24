using AutoMapper;
using IShop.Data;
using IShop.IRepository;
using IShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace IShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FavoriteController> _logger;
        private readonly IMapper _mapper;

        public FavoriteController(IUnitOfWork unitOfWork, ILogger<FavoriteController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToFavorite([FromBody] FavoriteProductDTO favoriteProductDTO)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await _unitOfWork.User.Get(u => u.Id == userId);

            var favorite = await _unitOfWork.Favorite.Get(u => u.Id == user.FavoriteId);

            var product = await _unitOfWork.Product.Get(u => u.Id == favoriteProductDTO.ProductId);

            favorite.Product = product;

            _unitOfWork.Favorite.Update(favorite);
            await _unitOfWork.Save();
            Favorite_Product favorite_Product = new()
            {
                ProductId = product.Id,
                FavoriteId = user.FavoriteId,
            };
            _unitOfWork.Favorite_Product.Add(favorite_Product);
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

            var product = (await _unitOfWork.Favorite_Product.GetAll(u => u.FavoriteId == user.FavoriteId)).Select(u => u.ProductId);
            var pro = await _unitOfWork.Product.GetAll(u => product.Contains(u.Id));
            return Ok(pro);

        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _unitOfWork.Favorite_Product.Get(u => u.Id == id);
                await _unitOfWork.Favorite_Product.Remove(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internet Server Error. Please Try Again. ");
            }
        }
    }
}
