using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject.Utilits
{
    internal static class InputHelper
    {
        public static int GetNum()
        {
            int num;

            while (!int.TryParse(Console.ReadLine(), out num))
            {
                Console.WriteLine("Wrong Input");
            }
            if (num == 0) { Console.Clear(); Console.WriteLine("Operation is stopped"); return 0; }
            return num;
        }
    }
}
