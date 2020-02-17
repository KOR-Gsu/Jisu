using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
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
        private bool ismine = false;
        public bool Ismine
        {
            get { return ismine;}
        }
        private TILESTATE state = TILESTATE.TILESTATE_CLOSE;
        public TILESTATE State
        {
            get { return state; }
        }
        private TILETYPE type = TILETYPE.TILETYPE_0;
        public TILETYPE Type
        {
            get { return type; }
        }
      
        public void Open()
        {
            state = TILESTATE.TILESTATE_OPEN;
        }
        public void Flag()
        {
            if(state != TILESTATE.TILESTATE_FLAG)
                state = TILESTATE.TILESTATE_FLAG;
            else
                state = TILESTATE.TILESTATE_CLOSE;
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
