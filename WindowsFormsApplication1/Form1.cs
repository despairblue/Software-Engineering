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
        private int offset;
        private int cellsize;
        private GameModel gameModel;

        public Form1(GameModel gameModel, int offset = 10, int cellsize = 64)
        {
            InitializeComponent();

            this.offset = offset;
            this.cellsize = cellsize;
            this.gameModel = gameModel;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    Pen pen;
                    Rectangle rect = new Rectangle(offset + cellsize * x, offset + cellsize * y, cellsize, cellsize);

                    if (gameModel.GameState[x, y] > 0)
                    {
                        RectangleF rectF = new RectangleF(offset + cellsize * x, offset + cellsize * y, cellsize, cellsize);
                        e.Graphics.DrawString(gameModel.GameState[x, y].ToString(), new Font(FontFamily.GenericMonospace, cellsize / 4), new SolidBrush(Color.Purple), rectF);

                        pen = new Pen(Color.Purple);
                    }
                    else
                    {
                        pen = new Pen(Color.Gray);
                    }

                    e.Graphics.DrawRectangle(pen, rect);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Size formSize = new Size(offset * 2 + cellsize * 4, offset * 2 + cellsize * 4);

            // set double buffering to remove flickering
            this.DoubleBuffered = true;

            this.ClientSize = formSize;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    gameModel.pushDown();
                    break;
                case Keys.Left:
                    gameModel.pushLeft();
                    break;
                case Keys.Right:
                    gameModel.pushRight();
                    break;
                case Keys.Up:
                    gameModel.pushUp();
                    break;
            }

            this.Invalidate();
        }
    }
}
