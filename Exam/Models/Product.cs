using System;
using System.Collections.Generic;

namespace Exam.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Article { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string? Image { get; set; }
        public int? Discount { get; set; }
        public int? QuantityStock { get; set; }
        public int UnitId { get; set; }
        public int ProviderId { get; set; }
        public int ManufacturerId { get; set; }
        public int CategoryId { get; set; }
        public Unit Unit { get; set; }
        public Provider Provider { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public Category Category { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
