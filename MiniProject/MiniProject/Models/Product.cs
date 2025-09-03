using MiniProject.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject.Models
{
    internal class Product:BaseEntity
    {
        
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public Product(string name, decimal price, int stock)
        {
            
            Name = name;
            Price = price;
            Stock = stock;
        }

        public virtual void PrintInfo()
        {
            if(Stock == 0)
            {
                Console.WriteLine(
                    $"_ _ _ _ _ _ _ _ _ _ _ _ _ " +
                    $"\nName: {Name}" +
                    $"\nPrice: {Price}" +
                    $"\nStock: Out of Stock" +
                    $"\nProduct Id: {Id}" +
                    $"\n- - - - - - - - - - - - - ");
            }
            else
            {
                Console.WriteLine(
                                    $"_ _ _ _ _ _ _ _ _ _ _ _ _ " +
                                    $"\nName: {Name}" +
                                    $"\nPrice: {Price}" +
                                    $"\nStock: {Stock}" +
                                    $"\nProduct Id: {Id}" +
                                    $"\n- - - - - - - - - - - - - ");
            }
                
        }

    }
}
