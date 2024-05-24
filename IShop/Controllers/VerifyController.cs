using AutoMapper;
using IShop.Data;
using IShop.IRepository;
using IShop.Models;
using IShop.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using UserMangementService.Models;
using UserMangementService.Services;

namespace IShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<VerifyController> _Logger;
        private readonly IMapper _mapper;
        private readonly IAuthManger _authManger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;


        public VerifyController(UserManager<User> userManager, ILogger<VerifyController> logger, IMapper mapper, IAuthManger authManger, IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _userManager = userManager;
            _Logger = logger;
            _mapper = mapper;
            _authManger = authManger;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCode([FromBody] ConfirmCodeDTO confirmCodeDTO)
        {
            var code = _mapper.Map<ConfirmCode>(confirmCodeDTO);

            var user = await _unitOfWork.User.Get(u => u.Email == confirmCodeDTO.Email);            
            var c = await _unitOfWork.ConfirmCode.Get(u => u.UserId == user.Id);
            if (code.Number == c.Number)
            {
                user.EmailConfirmed= true;
                _unitOfWork.User.Update(user);
                await _unitOfWork.Save();
            }
            
            return Ok();
        }
        [HttpPost]
        [Route(nameof(VerifyCode))]
        public async Task<IActionResult> VerifyCodePassword([FromBody] ConfirmCodeDTO confirmCodeDTO)
        {

            Random random = new Random();
            var Number = random.Next(10000, 99999);

            var code = _mapper.Map<ConfirmCode>(confirmCodeDTO);
            var user = await _unitOfWork.User.Get(u => u.Email == confirmCodeDTO.Email);

            var forgotPasswordLink = Url.Action(nameof(ResetPassword), "Account", new { Number }, Request.Scheme);
            var message = new Message(new string[] { user.Email }, "Forgot Password link", forgotPasswordLink);
            _emailService.SendEmail(message);

            ConfirmCode confirmCode = new()
            {
                Number = Number,
                UserId = user.Id,
            };
            await _unitOfWork.ConfirmCode.Insert(confirmCode);
            await _unitOfWork.Save();

            return Ok();
        }

        [HttpGet("Reset-Password")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var model = new Reset_Password { Token = token, Email = email };
            return Ok(new
            {
                model
            });
        }
    }
}
