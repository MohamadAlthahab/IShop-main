using AutoMapper;
using IShop.Data;
using IShop.IRepository;
using IShop.Models;
using IShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using UserMangementService.Models;
using UserMangementService.Services;

namespace IShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AccountController> _Logger;
        private readonly IMapper _mapper;
        private readonly IAuthManger _authManger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;


        public AccountController(UserManager<User> userManager, ILogger<AccountController> logger, IMapper mapper, IAuthManger authManger, IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _userManager = userManager;
            _Logger = logger;
            _mapper = mapper;
            _authManger = authManger;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            _Logger.LogInformation($"Registration Attempt for {userDTO.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = _mapper.Map<User>(userDTO);
                user.Email = userDTO.Email;
                user.UserName = userDTO.Username;
                Cart cart = new Cart();
                cart.Total = 0;
                await _unitOfWork.Cart.Insert(cart);
                await _unitOfWork.Save();
                user.CartId = cart.Id;

                Favorite favorite = new Favorite();
                await _unitOfWork.Favorite.Insert(favorite);
                await _unitOfWork.Save();
                user.FavoriteId = favorite.Id;
               

                var result = await _userManager.CreateAsync(user, userDTO.Password);
                if (!result.Succeeded)
                {
                    return BadRequest("User Registration Attempt Failed");
                }

                await _userManager.AddToRolesAsync(user,userDTO.Roles);
                
                Random random = new Random();
                var Number = random.Next(10000, 99999);
                

                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { Number }, Request.Scheme);
                var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink);
                _emailService.SendEmail(message);

                ConfirmCode confirmCode = new()
                {
                    Number = Number,
                    UserId = user.Id,
                };
                await _unitOfWork.ConfirmCode.Insert(confirmCode);
                await _unitOfWork.Save();

                return Accepted();
            }
            catch(Exception ex)
            {
                _Logger.LogError(ex, $"Something Went Wrong in the {nameof(Register)}");
                return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDTO)
        {
            _Logger.LogInformation($"Login Attempt for {loginUserDTO.Username}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = await _unitOfWork.User.Get(u => u.UserName == loginUserDTO.Username);
                var role = await _userManager.GetRolesAsync(user);
                if (!await _authManger.ValidateUser(loginUserDTO))
                {
                    return Unauthorized();
                }
                
                return Accepted((new { Token = await _authManger.CreateToken() },  role));
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Something Went Wrong in the {nameof(Login)}");
                return Problem($"Something Went Wrong in the {nameof(Login)}", statusCode: 500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status202Accepted);
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        [Route("Reset-Password")]
        public async Task<IActionResult> ResetPassword(Reset_Password reset_Password)
        {
            var user = await _userManager.FindByEmailAsync(reset_Password.Email);

            if (user != null)
            {
                var c = await _unitOfWork.ConfirmCode.Get(u => u.UserId == user.Id);
                var code = await _unitOfWork.ConfirmCode.Get(u => u.UserId == user.Id);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                reset_Password.Token = token;
                reset_Password.Email = user.Email;

                if (reset_Password.Code == code.Number)
                {
                    var resetPassResult = await _userManager.ResetPasswordAsync(user, reset_Password.Token, reset_Password.Password);
                    if (!resetPassResult.Succeeded)
                    {
                        foreach (var error in resetPassResult.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                        return Ok(ModelState);
                    }
                    return StatusCode(StatusCodes.Status202Accepted);

                }

            }
            return StatusCode(StatusCodes.Status400BadRequest);
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
