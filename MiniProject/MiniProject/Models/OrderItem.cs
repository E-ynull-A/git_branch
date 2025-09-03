using MiniProject.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject.Models
{
    internal class OrderItem:BaseEntity
    {
        public Product Product { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }

        public decimal SubTotal { get; set; }

        public void GetOrderItemInfo()
        {
            Console.WriteLine($"Product Name: {Product.Name}\nCount: {Count}\nPrice: {Price}\nSubTotal: {SubTotal}\nProduct Id: {Product.Id}");
        }
    }
}
