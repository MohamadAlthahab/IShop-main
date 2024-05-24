using AutoMapper;
using IShop.Data;
using IShop.IRepository;
using IShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Bcpg.Attr;
using static System.Net.Mime.MediaTypeNames;

namespace IShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, ILogger<ProductController> logger, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize(Roles = "Seller")]
        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid Post attempt in {nameof(CreateProduct)}");
                return BadRequest(ModelState);
            }
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                var user = await _unitOfWork.User.Get(u => u.Id == userId);

                var product = _mapper.Map<Product>(productDTO);

                product.UserId = user.Id;

                await _unitOfWork.Product.Insert(product);
                await _unitOfWork.Save();

                ProductImage productImage = null;

                if (productDTO.ImageProduct != null)
                {
                    var PathProject = Environment.CurrentDirectory;
                    foreach (var item in productDTO.ImageProduct)
                    {
                        var path = Path.Combine(PathProject, "Images", product.Id.ToString() + ".jpg");
                        try
                        {
                            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
                            {
                                await item.CopyToAsync(stream);
                                stream.Close();
                            }
                        }
                        catch (Exception e)
                        {
                            return BadRequest(e);
                        }
                        productImage = new ProductImage
                        {
                            ProductId = product.Id,
                            ImageName = item.FileName,
                            ImagePath = path
                        };
                        _unitOfWork.ProductImage.Add(productImage);
                    }
                }
                await _unitOfWork.Save();
                return Ok(productDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(CreateProduct)}");
                return StatusCode(500, "Internet Server Error. Please try again.");
            }
        }

        [Authorize(Roles = "Seller")]
        [HttpPut("{id=int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] UpdateProductDTO productDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid Update attempt in {nameof(UpdateProduct)}");
                return BadRequest(ModelState);
            }
            try
            {
                var product = await _unitOfWork.Product.Get(u => u.Id == id);
                if (product == null)
                {
                    _logger.LogError($"Invalid Update attempt in {nameof(UpdateProduct)}");
                    return BadRequest("Submitted data is invalid");
                }

                _mapper.Map(productDTO, product);
                _unitOfWork.Product.Update(product);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(UpdateProduct)}");
                return StatusCode(500, "Internet Server Error. Please try again.");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GettAllProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                
                var products = await _unitOfWork.Product.GetAll();

                IList<Product> productsList = new List<Product>();

                foreach (var product in products)
                {
                    if (product.ImageProduct != null)
                    {
                        var productId = await _unitOfWork.ProductImage.Get(u => u.ProductId == product.Id);
                        byte[] image = await System.IO.File.ReadAllBytesAsync(productId.ImagePath);
                        product.Image = image;
                        productsList.Add(product);
                    }
                    

                }
                return Ok(productsList);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetProducts)}");
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid Delete Product attempt in {nameof(DeleteProductDTO)}");
                return BadRequest();
            }
            try
            {
                var product = await _unitOfWork.Product.Get(u => u.Id == id);
                await _unitOfWork.Product.Remove(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the{nameof(DeleteProduct)}");
                return StatusCode(500, "Internet Server Error. Please Try Again. "); 
            }
        }

        [HttpGet]
        [Route("Search")]
        public async Task<ActionResult<IEnumerable<Product>>> Search(string name)
        {
            var result = await _unitOfWork.Product.Search(name);
            IList<Product> productsList = new List<Product>();

            foreach (var product in result)
            {
                if(product.ImageProduct != null)
                {
                    var productId = await _unitOfWork.ProductImage.Get(u => u.ProductId == product.Id);
                    byte[] image = System.IO.File.ReadAllBytes(productId.ImagePath);
                    product.Image = image;
                    productsList.Add(product);
                }
                

            }

            if (result.Any())
            {
                return Ok(productsList);
            }

            return NotFound();
        }

        [HttpGet]
        [Authorize(Roles = "Seller")]
        [Route("GetSellerProduct")]
        public async Task<IActionResult> SellerProduct()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;

            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await _unitOfWork.User.Get(u => u.Id == userId);

            var products = await _unitOfWork.Product.GetAll(u => u.UserId == user.Id);

            IList<Product> productsList = new List<Product>();

            foreach (var product in products)
            {
                if(product.ImageProduct != null)
                {
                    var productId = await _unitOfWork.ProductImage.Get(u => u.ProductId == product.Id);
                    byte[] image = System.IO.File.ReadAllBytes(productId.ImagePath);
                    product.Image = image;
                    productsList.Add(product);
                }
                

            }


            return Ok(productsList);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> CategoryProduct(int id)
        {
            var products = await _unitOfWork.Product.GetAll(u => u.CategoryId == id);
            IList<Product> productsList = new List<Product>();

            foreach (var product in products)
            {
                if (product.ImageProduct != null)
                {
                    var productId = await _unitOfWork.ProductImage.Get(u => u.ProductId == product.Id);
                    byte[] image = System.IO.File.ReadAllBytes(productId.ImagePath);
                    product.Image = image;
                    productsList.Add(product);
                }
                

            }

            return Ok(productsList);
        }
    }
}
