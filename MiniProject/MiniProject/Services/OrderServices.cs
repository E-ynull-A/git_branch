using MiniProject.Models;
using MiniProject.Repository;
using MiniProject.Utilits;
using MiniProject.Utilits.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MiniProject.Services
{
    internal class OrderServices : ProductServices
    {

        public static string _OrderPath = Path.GetFullPath
            (Path.Combine
            (Directory.GetCurrentDirectory(), @"../../../Data/Orders.json"));

        Repository<Order> orderRepo = new Repository<Order>();

        List<Order> orders = new();


        public void OrderProduct()
        {
            string email = GetEmail();

            if (email is null)
            {
                return;
            }

            Order order = new Order(email);
            OrderItem item;

            bool result = false;

            bool choose;

            do
            {
                Console.Clear();

                item = BuyProduct();
                if (item is null && order.Total == 0)
                {
                    Console.WriteLine("The Process was Stopped");
                    return;
                }
                if(item is null)
                {
                    Console.WriteLine("The Process was Stopped");
                    break;
                }

                order.Total += item.SubTotal;

                foreach (OrderItem Item in order.items)
                {
                    if (Item.Product.Id == item.Product.Id)
                    {
                        int index = order.items.FindIndex(x => x.Product.Id == item.Product.Id);
                        order.items[index].SubTotal += item.SubTotal;
                        order.items[index].Count += item.Count;
                        result = true;
                        break;
                    }
                }
                if (result == false)
                {
                    order.items.Add(item);
                }

                choose = ChooseContinue("yes", "no");
            }
            while (choose);

            

            orders = orderRepo.Deserialize(_OrderPath);
            orders.Add(order);
            orderRepo.Serialize(_OrderPath, orders);

            return;
        }

        public OrderItem BuyProduct()
        {

            Product product = GetProductById();

            if (product is null) { return null; }


            products = repo.Deserialize(_path);

            int bougthSize = GetStock("Please, Enter the Count:              0)Exit");

            if (bougthSize is 0) { return null; }


            if (product.Stock >= bougthSize)
            {
                OrderItem item = new OrderItem();
                item.Count = bougthSize;
                item.Price = product.Price;
                item.SubTotal = item.Count * item.Price;
                item.Product = product;

                foreach (Product prd in products)
                {
                    if (prd.Id == product.Id)
                    {
                        prd.Stock -= bougthSize;
                        repo.Serialize(_path, products);
                        break;
                    }
                }
                return item;
            }
            else
            {
                Console.WriteLine("There aren't enought product in stock\n");
            }

            bool otherChoose = ChooseContinue("yes", "no");

            if (otherChoose)
            {
                return BuyProduct();
            }
            else { return null; }

        }

        public bool ChooseContinue(string choose1, string choose2)
        {
            Console.WriteLine($"Do you want to continue?? ({Char.ToUpper(choose1[0])}->({choose1}) or {Char.ToUpper(choose2[0])}->({choose2}))");
            string choose = Console.ReadLine().Trim().ToLower();

            while (!(
                choose != choose1.ToLower() ||
                choose != Char.ToLower(choose1[0]).ToString() ||
                choose != choose2.ToLower() ||
                choose != Char.ToLower(choose2[0]).ToString()))
            {
                Console.WriteLine("Please, Enter correctly");
                choose = Console.ReadLine();
            }
            if (choose.ToLower() == choose1.ToLower() || choose.ToLower() == Char.ToLower(choose1[0]).ToString())
            {
                Console.Clear();
                return true;
            }
            else { Console.Clear(); return false; }
        }

        public Product GetProductById()
        {
            Console.WriteLine("Please, Enter the Product Id"
                         + new string(' ', 15)
                         + "0)Exit\n"
                         + new string(' ', 42)
                         + " 1)See the Product Id List First ");


            string get_Id = Console.ReadLine();
            Console.Clear();

            if (get_Id == "1")
            {
                ProductList("2");
                Console.WriteLine("Now Enter the Product Id");
                get_Id = Console.ReadLine();
            }

            if (get_Id == "0")
            {
                Console.WriteLine("The Process was stopped");
                return null;
            }

            products = repo.Deserialize(_path);


            while (get_Id != "0")
            {
                foreach (Product product in products)
                {
                    if (product.Id.ToString() == get_Id.Trim())
                    {
                        if (product.Stock != 0)
                        {
                            Console.Clear();
                            Console.WriteLine("The Product is found");
                            product.PrintInfo();
                            return product;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("The Product is out of stock");
                            return null;
                        }
                    }
                }
                Console.Clear();
                Console.WriteLine("The Product isn't found\n Enter the correct Id!             0)Exit");
                get_Id = Console.ReadLine();
            }
            Console.Clear();
            return null;
        }

        public string GetEmail()
        {
            Console.WriteLine("Please, Enter the Email:              0)Exit");
            string email = Console.ReadLine();

            while (!CheckEmail(email))
            {
                Console.Clear();
                if (email == "0") { Console.Clear(); return null; }
                Console.WriteLine("Enter the Correct Email, Please!            0)Exit\n");
                email = Console.ReadLine();
                Console.Clear();
            }
            return email;
        }

        //RFC 1035 və RFC 5321 standartları
        public bool CheckLocalPartEmail(string email)
        {
            int index = email.LastIndexOf("@");
            string userPart = email.Substring(0, index);

            if (!string.IsNullOrEmpty(userPart) && userPart.Length <= 320)
            {
                if (!char.IsWhiteSpace(userPart[0]) && !char.IsWhiteSpace(userPart[userPart.Length - 1]))
                {
                    return true;
                }
                
            }
            return false;
        }

        public bool CheckDomainPartEmail(string email)
        {
            int index = email.LastIndexOf("@");
            string domainPart = email.Substring(index);

            if (domainPart.Length > 255) { return false; }

            if (!string.IsNullOrEmpty(domainPart))
            {
                string[] domains = domainPart.Split(".");

                for (int i = 0; i < domains.Length; i++)
                {
                    if (domains[i].Length > 63 || domains[i].Length < 2)
                    {
                        return false;
                    }
                    else
                    {
                        char first = domains[i][1];
                        char end = domains[i][domains[i].Length - 1];

                        if (char.IsSymbol(first)
                            || Char.IsSymbol(end)
                            || char.IsWhiteSpace(first)
                            || char.IsWhiteSpace(end))
                        { return false; }

                        for (int j = 2; j < domains[i].Length - 1; j++)
                        {
                            char ch = domains[i][j];
                            if (Char.IsSymbol(ch) && ch != '-')
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            else return false;


        }

        public bool CheckEmail(string email)
        {
            if (!string.IsNullOrEmpty(email) && email.Contains("@"))
            {
                if (CheckLocalPartEmail(email) && CheckDomainPartEmail(email))
                {
                    return true;
                }
                else return false;
            }
            return false;

        }

        public void ShowAllOrders()
        {
            Console.WriteLine("Please Choose the Sort Type:");
            Console.WriteLine("1)Order Time\n2)Order Status\n\n0)Exit");
            orders = orderRepo.Deserialize(_OrderPath);
            int number = InputHelper.GetNum();

            if(number == 0) { return; }

            int check = 0;
            int days;

            

            switch (number)
            {

                case 1:
                    {
                        Console.WriteLine("How many days in advance do you need to order?               0)Exit");

                        days = InputHelper.GetNum();
                        Console.Clear();
                        if(days == 0) { Console.WriteLine("The Process was stopped"); return; }

                        foreach (Order order in orders.Where(o => DateTime.Now <= o.OrderedAt.AddDays(days)).OrderBy(o => o.OrderedAt))
                        {
                            ShowProcess(order);
                            check++;
                        }
                        if (check == 0)
                        {                            
                            Console.Clear();
                            Console.WriteLine("There are no Order that meet the condition");
                            return;
                        }

                        break;
                    }
                case 2:
                    {
                        Console.Clear();

                        foreach (Order order in orders.OrderBy(o => o.Status))
                        {
                            ShowProcess(order);
                        }

                        break;
                    }
                default:
                    {
                        Console.Clear();
                        Console.WriteLine("There is no choice that you choose");
                        return;
                    }
            }

            Console.WriteLine("Press any key for exit to menu");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("The Orders are Shown");
        }

        public void ShowProcess(Order order)
        {
            foreach (OrderItem item in order.items)
            {
                item.GetOrderItemInfo();
                Console.WriteLine(new string(' ', 7) + "- - - - - -" + new string(' ', 7));
            }
            order.PrintInfo();
            Console.WriteLine(new string('_', 30));
        }

        public void ChangeOrderStatus()
        {
            Order order = GetOrderId();

            if (order is null)
            {
                Console.Clear();
                Console.WriteLine("The Process was Stopped");
                return;
            }


            Console.WriteLine("1) Pending\n2) Confirmed\n3) Completed\n                 0)Exit");

            int num = InputHelper.GetNum();

            if (num == 0) { return; }

            switch (num)
            {
                case 1:
                    ChangeStatus(order, num);
                    break;
                case 2:
                    ChangeStatus(order, num);
                    break;
                case 3:
                    ChangeStatus(order, num);
                    break;
                default:
                    Console.WriteLine("Wrong Input");
                    if (ChooseContinue("yes", "no"))
                    {
                        Console.WriteLine("Enter the number again");
                        num = InputHelper.GetNum();

                        while (Enum.IsDefined(typeof(OrderStatus), num))
                        {
                            ChangeStatus(order, num);
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("The Process are Stoped");
                        return;
                    }
                    break;
            }
        }

        public Order GetOrderId()
        {
            orders = orderRepo.Deserialize(_OrderPath);

            Console.WriteLine("Please, Enter the Product Id"
             + new string(' ', 15)
             + "0)Exit\n"
             + new string(' ', 42)
             + " 1)See the Product Id List First ");

            string OrderId = Console.ReadLine();


            if (OrderId == "1")
            {
                OrderList();
                Console.WriteLine("Now Enter the Product Id\n");
                OrderId = Console.ReadLine();
            }

            while (OrderId is not "0")
            {
                foreach (Order order in orders)
                {
                    if (order.Id.ToString() == OrderId)
                    {
                        Console.WriteLine("The Order is Found");
                        return order;
                    }
                }
                Console.WriteLine("The Order isn't Found. Trt Again                 0)Exit");
                OrderId = Console.ReadLine();
            }
            return null;
        }

        public void ChangeStatus(Order order, int num)
        {

            if (order.Status != (Utilits.Enums.OrderStatus)num)
            {
                orders[orders.FindIndex(x => x.Equals(order))].
                    Status = (Utilits.Enums.OrderStatus)num;
                orderRepo.Serialize(_OrderPath, orders);
                Console.Clear();
                Console.WriteLine("The Order Status is Changed");
                return;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Nothing is Changed");
            }
            return;
        }

        public void OrderList()
        {
            orders = orderRepo.Deserialize(_OrderPath);

            Console.WriteLine($"Order Id List\n{new string('-', 18)}");
            foreach (Order order in orders)
            {
                Console.WriteLine($"{order.Id}");
            }
            Console.WriteLine(new string('-', 18));
        }
    }
}
