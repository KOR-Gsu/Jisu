using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace CSWF_MineSweeper
{
    enum TILESTATE
    {
        TILESTATE_OPEN,
        TILESTATE_CLOSE,
        TILESTATE_FLAG,
    }

    enum TILETYPE
    {
        TILETYPE_0,
        TILETYPE_1,
        TILETYPE_2,
        TILETYPE_3,
        TILETYPE_4,
        TILETYPE_5,
        TILETYPE_6,
        TILETYPE_7,
        TILETYPE_8,
        TILETYPE_MINE,
    }

    class Tile
    {
        private Rectangle myRect;
        public Rectangle MyRect
        {
            get { return myRect; }
        }

        private bool ismine;
        public bool Ismine
        {
            get { return ismine; }
        }
        private TILESTATE state;
        public TILESTATE State
        {
            get { return state; }
        }
        private TILETYPE type;
        public TILETYPE Type
        {
            get { return type; }
        }

        public Tile(int top, int left, int cx, int cy)
        {
            myRect = new Rectangle(top, left, cx, cy);

            ismine = false;
            state = TILESTATE.TILESTATE_CLOSE;
            type = TILETYPE.TILETYPE_0;
        }
        public void Open()
        {
            state = TILESTATE.TILESTATE_OPEN;
        }
        public bool Flag()
        {
            if (state != TILESTATE.TILESTATE_FLAG)
            {
                state = TILESTATE.TILESTATE_FLAG;
                return true;
            }
            else
            {
                state = TILESTATE.TILESTATE_CLOSE;
                return false;
            }
        }
        public void SetMine()
        {
            ismine = true;
            type = TILETYPE.TILETYPE_MINE;
        }
        public void SetNum(int num)
        {
            type = (TILETYPE)num;
        }
        public void Reset()
        {
            ismine = false;
            state = TILESTATE.TILESTATE_CLOSE;
            type = TILETYPE.TILETYPE_0;
        }
    }
}
