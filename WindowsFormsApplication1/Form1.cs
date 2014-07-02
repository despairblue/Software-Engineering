using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int offset = 10;
        private int cellsize = 64;
        private GameModel gameModel = new GameModel();

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    Rectangle rect = new Rectangle(offset + cellsize * x, offset + cellsize * y, cellsize, cellsize);
                    RectangleF rectF = new RectangleF(offset + cellsize * x, offset + cellsize * y, cellsize, cellsize);
                    e.Graphics.DrawRectangle(new Pen(Color.Purple), rect);
                    e.Graphics.DrawString(gameModel.GameState[x,y].ToString(), new Font(FontFamily.GenericMonospace, cellsize/4), new SolidBrush(Color.Purple), rectF);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Size formSize = new Size(offset * 2 + cellsize * 4, offset * 2 + cellsize * 4);

            this.ClientSize = formSize;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

        }
    }
}
