using System;
using System.Collections.Generic;
using System.Text;

namespace SQL_Injection.Database
{
    public class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
