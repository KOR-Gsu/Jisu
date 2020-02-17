using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class main
    {
        static void Main(string[] args)
        {
            TileManager tilemanager = new TileManager();
            tilemanager.Init();
            while (true)
            {
                Console.Clear();
                tilemanager.Draw();
                if (tilemanager.Input())
                {
                    Console.Write("GameOver");
                    Console.ReadKey();
                    tilemanager.Init();
                }
            }
        }
    }
}
