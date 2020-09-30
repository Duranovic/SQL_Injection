using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SQL_Injection.Database
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
