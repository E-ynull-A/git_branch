using MiniProject.Models;
using MiniProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject
{
    internal class ManageApp
    {

        ProductServices productServices = new ProductServices();
        OrderServices orderServices = new OrderServices();
        public void Run()
        {
            int num = 0;

            string choose = null;
            bool answer = false;

            while (!(num == 0 && answer))
            {
                Console.WriteLine("- - - - - - - - - -  - - - - - - - - -   - - - - - - - - - -\n" +
                    "1.Create Product   | 4.Show All Product| 7.Shaw All Orders    | " +
                    "\n2.Delete Product   | 5.Refill Product  | 8.Change Order Status|" +
                    "\n3.Get Product By Id| 6.Order Product   | 9.Delete Products    |" +
                    "\n- - - - - - - - - -  - - - - - - - - -   - - - - - - - - - - \n0.Exit |\n- - - -");

                choose = Console.ReadLine();
                answer = int.TryParse(choose, out num);

                Console.Clear();

                switch (num)
                {
                    case 1:   
                        
                        Product product = productServices.CreateProduct();

                        if (product is null)
                        {
                            Console.WriteLine("Operation is stopped");
                        }
                        else
                        {
                            Console.WriteLine("Product Created!");
                        }                        
                        break;               
                    case 2:

                        productServices.DeleteProduct(1);
                        Console.WriteLine("Product was Deleted");
                        break;
                    case 3:

                        productServices.GetProductById();                       
                        break;
                    case 4:

                        productServices.ShowAllProduct();
                        Console.Clear();
                        Console.WriteLine("The Products are Showed");
                        break;
                    case 5:

                        productServices.RefillProduct();                       
                        break;
                    case 6:

                        orderServices.OrderProduct();
                        break;
                    case 7:

                        orderServices.ShowAllOrders();
                        
                        break;
                    case 8:

                        orderServices.ChangeOrderStatus();
                        break;
                    case 9:

                        productServices.DeleteProductsById();
                        break;
                    default:
                        Console.WriteLine("Wrong Input!");
                        break;

                }
            }
            Console.Clear();
            Console.WriteLine("Thank You For Choosing Us:)");
        }
    }
}
