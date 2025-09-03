using MiniProject.Models.Base;
using MiniProject.Utilits.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject.Models
{
    internal class Order:BaseEntity
    {
        public List<OrderItem> items = new();
        public decimal Total { get; set; }

        public string Email { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]        
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public DateTime OrderedAt { get; set; } = DateTime.Now;
        public void PrintInfo()
        {
            Console.WriteLine($"Total: {Total}\nOrder Status: {Status}\nEmail: {Email}\nOrdered At: {OrderedAt:dd.MM.yyyy HH:mm}\nOrder Id: {Id} ");
        }
        public Order(string email)
        {
            Email = email;
        }
       
    }
}
