using IShop.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace IShop.Models
{
    public class CreateCartDTO 
    {
        public int ProductId { get; set; }
    }


    public class CartDTO : CreateCartDTO
    {
        public ProductDTO Product { get; set; }
    }
}
