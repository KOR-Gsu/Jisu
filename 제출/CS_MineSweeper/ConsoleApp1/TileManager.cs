using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ConsoleApp1
{
    class TileManager
    {
        private Tile[,] TileList;
        private int width = 16;
        private int height = 16;
        private int MineMax = 40;
        private int CursorX;
        private int CursorY;
        private bool First;

        private bool CheckMine(int x, int y)
        {
            if (x < 0 || y < 0 || x > width - 1 || y > height - 1)
                return false;
            if (TileList[y, x].Type == TILETYPE.TILETYPE_MINE)
                return true;

            return false;
        }

        private void SearchTile(int x, int y)
        {
            if (x < 0 || y < 0 || x > width - 1 || y > height - 1)
                return;
            if (TileList[y, x].State == TILESTATE.TILESTATE_OPEN)
                return;
            if (TileList[y, x].Type == TILETYPE.TILETYPE_MINE)
                return;

            TileList[y, x].Open();

            int Cnt = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int k = -1; k < 2; k++)
                {
                    if (CheckMine(x + k, y + i))
                        Cnt++;
                }
            }
            TileList[y, x].SetNum(Cnt);

            if (Cnt == 0)
            {
                SearchTile(x, y - 1);
                SearchTile(x, y + 1);
                SearchTile(x - 1, y);
                SearchTile(x + 1, y);
            }
        }
        
        public void SetMines(int x, int y)
        {
            int cnt = 0;
            Random r = new Random();
            while (cnt < MineMax)
            {
                int ranx = r.Next(width);
                int rany = r.Next(height);
                if (ranx == x && rany == y)
                    continue;
                if (TileList[rany, ranx].Type != TILETYPE.TILETYPE_MINE)
                {
                    TileList[rany, ranx].SetMine();
                    cnt++;
                }
            }
        }

        public void Init()
        {
            CursorX = width / 2 - 1;
            CursorY = height / 2 - 1;
            First = true;
            TileList = new Tile[width, height];
            for(int y = 0; y < height; y++)
            {
                for(int x= 0; x < width; x++)
                {
                    TileList[y, x] = new Tile();
                    TileList[y, x].Reset();
                }
            }
        }

        public bool Input()
        {
            ConsoleKeyInfo input = Console.ReadKey();
            switch(input.Key)
            {
                case ConsoleKey.UpArrow:
                    if (CursorY > 0)
                        CursorY--;
                    else
                        CursorY = height - 1;
                    break;
                case ConsoleKey.DownArrow:
                    if (CursorY + 1 < height)
                        CursorY++;
                    else
                        CursorY = 0;
                    break;
                case ConsoleKey.LeftArrow:
                    if (CursorX > 0)
                        CursorX--;
                    else
                        CursorX = width - 1;
                    break;
                case ConsoleKey.RightArrow:
                    if (CursorX + 1 < width)
                        CursorX++;
                    else
                        CursorX = 0;
                    break;
                case ConsoleKey.Z:
                    if(First)
                    {
                        SetMines(CursorX, CursorY);
                        First = false;
                    }
                    if (TileList[CursorY, CursorX].State == TILESTATE.TILESTATE_FLAG)
                        break;
                    else
                    {
                        if (TileList[CursorY, CursorX].Type == TILETYPE.TILETYPE_MINE)
                            return true;
                        if (TileList[CursorY, CursorX].State == TILESTATE.TILESTATE_CLOSE)
                            SearchTile(CursorX, CursorY);
                        break;
                    }
                case ConsoleKey.X:
                    if (TileList[CursorY, CursorX].State != TILESTATE.TILESTATE_OPEN)
                        TileList[CursorY, CursorX].Flag();
                        break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
            }
            return false;
        }

        public void Draw()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x == CursorX && y == CursorY)
                        Console.Write("♣");
                    else
                    {
                        switch (TileList[y, x].State)
                        {
                            case TILESTATE.TILESTATE_OPEN:
                                switch (TileList[y, x].Type)
                                {
                                    case TILETYPE.TILETYPE_MINE:
                                        Console.Write("※");
                                        break;
                                    case TILETYPE.TILETYPE_0:
                                        Console.Write("○");
                                        break;
                                    case TILETYPE.TILETYPE_1:
                                        Console.Write("①");
                                        break;
                                    case TILETYPE.TILETYPE_2:
                                        Console.Write("②");
                                        break;
                                    case TILETYPE.TILETYPE_3:
                                        Console.Write("③");
                                        break;
                                    case TILETYPE.TILETYPE_4:
                                        Console.Write("④");
                                        break;
                                    case TILETYPE.TILETYPE_5:
                                        Console.Write("⑤");
                                        break;
                                    case TILETYPE.TILETYPE_6:
                                        Console.Write("⑥");
                                        break;
                                    case TILETYPE.TILETYPE_7:
                                        Console.Write("⑦");
                                        break;
                                    case TILETYPE.TILETYPE_8:
                                        Console.Write("⑧");
                                        break;
                                }
                                break;
                            case TILESTATE.TILESTATE_CLOSE:
                                Console.Write("■");
                                break;
                            case TILESTATE.TILESTATE_FLAG:
                                Console.Write("★");
                                break;
                        }
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
