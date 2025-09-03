using MiniProject.Models;
using MiniProject.Repository;
using MiniProject.Utilits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MiniProject.Services
{
    internal class ProductServices
    {

        protected Repository<Product> repo = new();

        protected string _path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"../../../Data/Data.json"));

        protected List<Product> products = new List<Product>();
        

        public string GetName()
        {
            Console.WriteLine("Please, Enter the Product name                        0)-Exit:");

            string name = Console.ReadLine();


            while (String.IsNullOrEmpty(name) || IsDublicate(name))
            {
                Console.WriteLine("Wrong Input or Dublicate Name");
                name = Console.ReadLine();
            }

            Console.Clear();
            return name;

        }

        public decimal GetPrice()
        {
            Console.WriteLine("Please, Enter the Price size                        0)-Exit:");

            decimal price = 0;


            while (!decimal.TryParse(Console.ReadLine(), out price) || price < 0)
            {
                Console.WriteLine("Wrong Input");
            }
            if (price == 0) { Console.Clear(); return 0; }

            Console.Clear();
            return price;

        }

        public int GetStock(string message)
        {
            Console.WriteLine(message);

            int stock = 0;

            while (!int.TryParse(Console.ReadLine(), out stock) || stock < 0)
            {
                Console.WriteLine("Wrong Input");
            }
            if (stock == 0) { Console.Clear(); return 0; }

            Console.Clear();
            return stock;
        }

        public bool IsDublicate(string name)
        {
            products = repo.Deserialize(_path);

            foreach (Product product in products)
            {
                if (name.ToLower() == product.Name.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public Product CreateProduct()
        {
            string name = GetName();
            if (name == "0") { return null; }

            decimal price = GetPrice();
            if (price == 0) { return null; }

            int stock = GetStock("Please, Enter the Stock Size:                 0)Exit");
            if (stock == 0) { return null; }


            Product product = new(name, price, stock);

            products = repo.Deserialize(_path);

            products.Add(product);

            repo.Serialize(_path, products);

            return product;
        }

        public void DeleteProduct(int Count)
        {
            Console.WriteLine("Please, Select the Enter Value:                 0)Exit \n1)Name\n2)Id:");

            int number = InputHelper.GetNum();

            if (number == 0) { Console.Clear(); return; }


            string item = string.Empty;
            string get_choose = string.Empty;
            products = repo.Deserialize(_path);

            if (products.Count == 0)
            {
                Console.WriteLine("There are no any products left");
                return;
            }

            switch (number)
            {
                case 1:
                    Console.Clear();

                    Console.WriteLine("Please, Enter the Product Name"
                        + new string(' ', 15)
                        + "0)Exit\n"
                        + new string(' ', 44)
                        + " 1)See the Product Name List First ");

                    item = Console.ReadLine();

                    if (item == "1")
                    {
                        ProductList(item);
                        Console.WriteLine("\nNow Enter the Product Name\n");
                        item = Console.ReadLine();
                    }
                    for (int i = 0; i < Count; i++)
                    {
                        while (!products.Any(p => p.Name.ToLower() == item.ToLower()) || item == "0")
                        {
                            Console.WriteLine("The Product wasn't Found. Try Again                  0)Exit");

                            if (item == "0")
                            {
                                Console.Clear();
                                Console.WriteLine("The Process was stopped");
                                return;
                            }
                            item = Console.ReadLine();
                        }
                        get_choose =
                            products.Find(x => x.Name.ToString() == item)
                            .Id.ToString();

                        DeleteProcess(get_choose);

                        if (Count - 1 == i) { Console.Clear() ; return; }
                        Console.WriteLine("Enter the another Product Name");
                        item = Console.ReadLine();
                    }
                    Console.Clear();
                    return;

                case 2:
                    Console.Clear();


                    Console.WriteLine("Please, Enter the Product Id"
                        + new string(' ', 15)
                        + "0)Exit\n"
                        + new string(' ', 42)
                        + " 1)See the Product Id List First ");

                    item = Console.ReadLine();

                    if (item == "1")
                    {
                        ProductList("2");
                        Console.WriteLine("Now Enter the Product Id");
                        item = Console.ReadLine();
                    }
                    for (int i = 0; i < Count; i++)
                    {
                        while (!products.Any(x => x.Id.ToString() == item) || item == "0")
                        {
                            //Console.Clear();
                            Console.WriteLine("The Product wasn't Found. Try Again                  0)Exit");

                            if (item == "0")
                            {
                                Console.Clear();
                                Console.WriteLine("The Process was stopped");
                                return;
                            }
                            item = Console.ReadLine();
                        }

                        get_choose =
                            products.Find(x => x.Id.ToString() == item)
                            .Id.ToString();

                        DeleteProcess(get_choose);
                        if (Count - 1 == i) { Console.Clear() ; return; }
                        Console.WriteLine("Enter the another Product Id");
                        item = Console.ReadLine();
                    }
                    Console.Clear();
                    return;

                default:
                    Console.Clear();
                    Console.WriteLine("There is no choice that you choose");

                    return;
            }


        }

        public void DeleteProcess(string get_choose)
        {

            products = repo.Deserialize(_path);

            foreach (Product product in products)
            {
                if (product.Id.ToString() == get_choose)
                {
                    products.Remove(product);
                    repo.Serialize(_path, products);

                    //Console.Clear();

                    Console.WriteLine("The Product was deleted\n");
                    return;
                }
            }
        }

        public void DeleteProductsById()
        {
            Console.WriteLine("Please, Enter the Count of Deleted Products                   0)Exit");

            int Count;
            while (!int.TryParse(Console.ReadLine(), out Count) || Count < 0)
            {
                Console.Clear();
                Console.WriteLine("Please Enter the Positive Number                   0)Exit");
            }
            if (Count == 0)
            {
                Console.Clear();
                Console.WriteLine("The Process was stopped");
                return;
            }
            products = repo.Deserialize(_path);
            if (products.Count < Count) { Console.Clear(); Console.WriteLine($"There aren't as many products as you want! There are only {products.Count} left!"); return; }

            int before = products.Count;

            DeleteProduct(Count);
            if (products.Count == before)
            {
                Console.Clear();
                Console.WriteLine("Process was stopped");
                return;
            }

            Console.Clear();
            Console.WriteLine("The Products were Deleted");

            return;
        }

        public virtual void GetProductById()
        {

            Console.WriteLine("Please, Enter the Product Id:\n");

            products = repo.Deserialize(_path);

            string get_Id = Console.ReadLine();

            foreach (Product product in products)
            {
                if (product.Id.ToString() == get_Id)
                {
                    product.PrintInfo();
                    Console.WriteLine("Press any key for exit");
                    Console.ReadKey();

                    Console.Clear();
                    return;

                }
            }

            Console.Clear();
            Console.WriteLine("The Product wasn't found");
            return;

        }

        public void ShowAllProduct()
        {
            products = repo.Deserialize(_path);

            foreach (Product product in products)
            {
                product.PrintInfo();
            }

            Console.WriteLine("Press any key for exit");
            Console.ReadKey();

        }

        public void RefillProduct()
        {
            Console.WriteLine("Please, Enter the Product Id"
                         + new string(' ', 15)
                         + "0)Exit\n"
                         + new string(' ', 42)
                         + " 1)See the Product Id List First ");

            products = repo.Deserialize(_path);

            string choose = Console.ReadLine();

            if (choose == "1")
            {
                ProductList("2");
                Console.WriteLine("Now Enter the Product Id");
                choose = Console.ReadLine();
            }

            if (choose == "0")
            {
                Console.WriteLine("The Process was stopped");
                return;
            }


            foreach (Product product in products)
            {
                if (product.Id.ToString() == choose.Trim())
                {
                    Console.WriteLine("The Product was found\n");


                    int get = GetStock("Please, Enter the Count:                0)Exit");
                    Console.Clear();

                    if (get == 0)
                    {
                        Console.WriteLine("The Process was stopped");
                        return;
                    }


                    product.Stock += get;

                    repo.Serialize(_path, products);

                    Console.WriteLine("The Product was Refilled\n");


                    return;
                }
            }


            Console.Clear();
            Console.WriteLine("The Product wasn't found");
            return;
        }

        public void ProductList(string num)
        {
            products = repo.Deserialize(_path);

            if (num == "1")
            {
                Console.WriteLine($"Product Name List\n{new string('-', 18)}");
                foreach (Product product in products)
                {
                    Console.WriteLine($"{product.Name}");
                }
            }
            else if (num == "2")
            {

                Console.WriteLine($"Product Id List\n{new string('-', 18)}");
                foreach (Product product in products)
                {
                    Console.WriteLine($"{product.Id}");// {product.Id}--> {product.Name} not Logicly correct
                }
            }
            Console.WriteLine(new string('-', 18));

        }
    }
}
