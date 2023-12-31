﻿using System.ComponentModel.DataAnnotations;

namespace GuitarShop.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Please enter a category name.")]
        public string CategoryName { get; set; }

        //Navigation property
        public List<Product> Products { get; set; }
    }
}
