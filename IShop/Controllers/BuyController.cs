using AutoMapper;
using IShop.Data;
using IShop.IRepository;
using IShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BuyController> _logger;
        private readonly IMapper _mapper;

        public BuyController(IUnitOfWork unitOfWork, ILogger<BuyController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Buy(SelectStreetDTO selectStreetDTO)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await _unitOfWork.User.Get(u => u.Id == userId);

            var cart = await _unitOfWork.Cart.Get(u => u.Id == user.CartId);

            var cartProduct = await _unitOfWork.Cart_Product.Get(u => u.CartId == cart.Id);

            var product = await _unitOfWork.Product.Get(u => u.Id == cartProduct.ProductId);


            

            

            Log log = new()
            {
                Date = DateTime.Now,
                UserID = user.Id,
                Total = cart.Total,
            };
            _unitOfWork.Log.Add(log);
            await _unitOfWork.Save();

            Log_Product log_Product = new()
            {
                ProductId = product.Id,
                LogId = log.Id,
            };
            

            _unitOfWork.Log_Product.Add(log_Product);
            await _unitOfWork.Save();

            return Ok();

        }
    }
}
