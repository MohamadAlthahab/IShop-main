using AutoMapper;
using IShop.Data;
using IShop.IRepository;
using IShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RateController> _logger;
        private readonly IMapper _mapper;

        public RateController(IUnitOfWork unitOfWork, ILogger<RateController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> RateProduct(int id, [FromBody] CreateRateDTO createRateDTO)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await _unitOfWork.User.Get(u => u.Id == userId);
            var product = await _unitOfWork.Product.Get(u => u.Id == id);
            
            var RateProduct = _mapper.Map<CreateRateDTO>(createRateDTO);

                Rate rate = new()
                {
                    Ratee = RateProduct.Ratee,
                    UserID = user.Id,
                    ProductID = product.Id,
                };
                await _unitOfWork.Rate.Insert(rate);
                await _unitOfWork.Save();
            return Ok();
        }
        [HttpGet]
        public  async Task<IActionResult> GetHighRate()
        {
            var ratee = await _unitOfWork.Rate.GetAll();
            var results = _mapper.Map<IList<Rate>>(ratee);
            
           /* if ( >= 3.5)
            {
                var product = await _unitOfWork.Product.Get(u => u.Id == rate.product.Id);
                var results = _mapper.Map<IList<ProductDTO>>(product);
                return Ok(results);
            }*/
            return Ok(results);
        }
    }
}
