using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class GameModel
    {
        private int[,] field = new int[4,4];

        public GameModel() {}

        public int[,] GameState
        {
            get
            {
                return field;
            }
        }

        public void pushLeft() {}

        public void pushRight() {}

        public void pushDown() {
            
        }

        public void pushUp() {}
    }
}
