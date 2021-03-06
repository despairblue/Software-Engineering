﻿using System;
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

        // renders the game field
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            int[,] field = gameModel.playingField;

            // draws a rectangle for every cell and the containing number, except for zeros.
            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    Pen pen;
                    Rectangle rect = new Rectangle(offset + cellsize * x, offset + cellsize * y, cellsize, cellsize);

                    if (gameModel.playingField[x, y] > 0)
                    {
                        RectangleF rectF = new RectangleF(offset + cellsize * x, offset + cellsize * y, cellsize, cellsize);
                        e.Graphics.DrawString(gameModel.playingField[x, y].ToString(), new Font(FontFamily.GenericMonospace, cellsize / 4), new SolidBrush(Color.Purple), rectF);

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

        // sets some initial windows form properties
        private void Form1_Load(object sender, EventArgs e)
        {
            Size formSize = new Size(offset * 2 + cellsize * gameModel.playingField.GetLength(0), offset * 2 + cellsize * gameModel.playingField.GetLength(1));

            // set double buffering to remove flickering
            this.DoubleBuffered = true;

            this.ClientSize = formSize;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

        }

        // handles input 
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

            // check the game's state and outputs a corresponding message
            if (gameModel.gameState == GameModel.States.LOST)
            {
                this.Text = "You Lost! Restart the App!";
            }
            else if (gameModel.gameState == GameModel.States.WON)
            {
                this.Text = "Congrats, You Won!";
            }
            else
            {
                this.Text = "Good, Next Move!";
            }

            this.Invalidate();
        }
    }
}
