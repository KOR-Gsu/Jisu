using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace CSWF_MineSweeper
{
    class TileManager
    {

        private Tile[,] TileList;
        private bool First;
        private int width = 30;
        private int height = 16;
        private int mineMax = 49;
        public int MineMax
        {
            get { return mineMax; }
        }
        private int flagNum = 0;
        public int FlagNum
        {
            get { return flagNum; }
        }

        private bool CheckMine(int x, int y)
        {
            if (x < 0 || y < 0 || x > width - 1 || y > height - 1)
                return false;
            if (TileList[y, x].Ismine)
                return true;

            return false;
        }

        private bool SearchTile(int x, int y)
        {
            if (x < 0 || y < 0 || x > width - 1 || y > height - 1)
                return false;
            if (TileList[y, x].Type == TILETYPE.TILETYPE_MINE)
            {
                TileList[y, x].Open();
                return true;
            }
            if (TileList[y, x].State != TILESTATE.TILESTATE_CLOSE)
                return false;

            TileList[y, x].Open();

            int Cnt = 0;
            bool mine = false;
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
                mine = SearchTile(x, y - 1);
                mine = SearchTile(x, y + 1);
                mine = SearchTile(x - 1, y);
                mine = SearchTile(x + 1, y);
            }
            return mine;
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
            First = true;
            TileList = new Tile[height, width];
            flagNum = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                    TileList[y, x] = new Tile(43 + x * 26, 45 + y * 26, 26, 26);
            }
        }

        public bool Input(MouseEventArgs e)
        {
            Point pt = new Point(e.Location.X, e.Location.Y);
            if (e.Button == MouseButtons.Left)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (TileList[y, x].MyRect.Contains(pt))
                        {
                            if (First)
                            {
                                SetMines(x, y);
                                First = false;
                            }

                            if (SearchTile(x, y))
                                return true;
                        }
                    }
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (TileList[y, x].State != TILESTATE.TILESTATE_OPEN)
                        {
                            if (TileList[y, x].MyRect.Contains(pt))
                            {
                                if (TileList[y, x].Flag())
                                    flagNum++;
                                else
                                    flagNum--;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public void Draw(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if(TileList[y, x].State == TILESTATE.TILESTATE_CLOSE)
                        g.DrawImage(Properties.Resources.block, 43 + 26 * x, 45 + 26 * y);
                    else if(TileList[y, x].State == TILESTATE.TILESTATE_FLAG)
                        g.DrawImage(Properties.Resources.block_9, 43 + 26 * x, 45 + 26 * y);
                    else
                    {
                        switch(TileList[y,x].Type)
                        {
                            case TILETYPE.TILETYPE_0:
                                g.DrawImage(Properties.Resources.block_0, 43 + 26 * x, 45 + 26 * y);
                                break;
                            case TILETYPE.TILETYPE_1:
                                g.DrawImage(Properties.Resources.block_1, 43 + 26 * x, 45 + 26 * y);
                                break;
                            case TILETYPE.TILETYPE_2:
                                g.DrawImage(Properties.Resources.block_2, 43 + 26 * x, 45 + 26 * y);
                                break;
                            case TILETYPE.TILETYPE_3:
                                g.DrawImage(Properties.Resources.block_3, 43 + 26 * x, 45 + 26 * y);
                                break;
                            case TILETYPE.TILETYPE_4:
                                g.DrawImage(Properties.Resources.block_4, 43 + 26 * x, 45 + 26 * y);
                                break;
                            case TILETYPE.TILETYPE_5:
                                g.DrawImage(Properties.Resources.block_5, 43 + 26 * x, 45 + 26 * y);
                                break;
                            case TILETYPE.TILETYPE_6:
                                g.DrawImage(Properties.Resources.block_6, 43 + 26 * x, 45 + 26 * y);
                                break;
                            case TILETYPE.TILETYPE_7:
                                g.DrawImage(Properties.Resources.block_7, 43 + 26 * x, 45 + 26 * y);
                                break;
                            case TILETYPE.TILETYPE_8:
                                g.DrawImage(Properties.Resources.block_8, 43 + 26 * x, 45 + 26 * y);
                                break;
                            case TILETYPE.TILETYPE_MINE:
                                g.DrawImage(Properties.Resources.block_10, 43 + 26 * x, 45 + 26 * y);
                                break;
                        }
                    }
                }
            }
        }
    }
}
