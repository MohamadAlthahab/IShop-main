﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IShop.Data
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual IList<Product> Products { get; set; }
    }
}
