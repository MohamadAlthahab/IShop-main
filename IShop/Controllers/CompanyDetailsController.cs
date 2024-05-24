using AutoMapper;
using IShop.Data;
using IShop.IRepository;
using IShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace IShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyDetailsController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CompanyDetailsController> _logger;
        private readonly IMapper _mapper;

        public CompanyDetailsController(IUnitOfWork unitOfWork, ILogger<CompanyDetailsController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [Authorize(Roles = "Seller")]
        [HttpPost]
        public async Task<IActionResult> CreatCompanyDetails([FromForm] CreateCompanyDetailsDTO compDetailsDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid Post attempt in {nameof(CreatCompanyDetails)}");
                return BadRequest(ModelState);
            }
            try
            {
                var comDetails = _mapper.Map<CompanyDetails>(compDetailsDTO);
                await _unitOfWork.CompanyDetails.Insert(comDetails);
                await _unitOfWork.Save();

                return Ok(compDetailsDTO);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(CreatCompanyDetails)}");
                return StatusCode(500, "Internet Server Error. Please try again.");
            }
        }
    }
}
