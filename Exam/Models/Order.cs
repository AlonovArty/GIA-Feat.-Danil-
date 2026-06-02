using System;
using System.Collections.Generic;

namespace Exam.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateOnly OrderDate { get; set; }
        public DateOnly DeliveryDate { get; set; }
        public int Code { get; set; }
        public int OrderStatusId { get; set; }
        public int PickupPointId { get; set; }
        public int UserId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PickupPoint PickupPoint { get; set; }
        public User User { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
