using AutoMapper;
using IShop.IRepository;
using IShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectCountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SelectCountryController> _logger;
        private readonly IMapper _mapper;

        public SelectCountryController(IUnitOfWork unitOfWork, ILogger<SelectCountryController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SelectCountry(SelectStreetDTO selectStreetDTO)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await _unitOfWork.User.Get(u => u.Id == userId);

            var cart = await _unitOfWork.Cart.Get(u => u.Id == user.CartId);
            
            var streetName = _mapper.Map<SelectStreetDTO>(selectStreetDTO);

            var street = await _unitOfWork.Street.Get(u => u.Name == selectStreetDTO.Name);
            cart.Total = cart.Total + street.Price;

            _unitOfWork.Cart.Update(cart);
            await _unitOfWork.Save();

            return Ok(cart.Total);

        }
    }
}
