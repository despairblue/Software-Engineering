using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class GameModel
    {
        private int[,] field = new int[4, 4];

        public GameModel()
        {
            // init with two random 2s and/or 4s
            for (int i = 0; i < 2; i++)
            {
                insertRandomNumber();
            }
        }

        public int[,] GameState
        {
            get
            {
                return field;
            }
        }

        private void insertRandomNumber()
        {
            Random ran = new Random();
            bool repeat = true;

            while (repeat)
            {
                int column = ran.Next(0, field.GetLength(0));
                int row = ran.Next(0, field.GetLength(1));

                if (field[column, row] == 0)
                {
                    repeat = false;
                    field[column, row] = ran.Next(1, 3) * 2;
                }
            }
        }

        private int[,] rotateRight(int[,] matrix)
        {
            int[,] newMatrix = new int[matrix.GetLength(1), matrix.GetLength(0)];

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    newMatrix[matrix.GetLength(1) - 1 - y, x] = matrix[x, y];
                }
            }

            return newMatrix;
        }

        public void pushLeft()
        {
            field = rotateRight(rotateRight(rotateRight(field)));
            pushDown();
            field = rotateRight(field);
        }

        public void pushRight()
        {
            field = rotateRight(field);
            pushDown();
            field = rotateRight(rotateRight(rotateRight(field)));
        }

        public void pushDown()
        {
            int columnStart = 0;
            int columnEnd = field.GetLength(0);
            int columnDirection = 1;
            int rowStart = field.GetLength(1) - 1;
            int rowEnd = 0;
            int rowDirection = -1;

            // go through each collumn
            for (int column = columnStart; column != columnEnd; column += columnDirection)
            {
                // get rid of zeros
                for (int row = rowStart; row != rowEnd; row += rowDirection)
                {
                    // if the current field is 0 compress collumn
                    if (field[column, row] == 0)
                    {
                        // note if there are any numbers left, if so we start from the beginning
                        bool repeatColumn = false;

                        // remove zero and move other rows in it's place
                        for (int innerRow = row; innerRow != rowEnd; innerRow += rowDirection)
                        {
                            if (field[column, innerRow] == 0 && field[column, innerRow + rowDirection] != 0)
                            {
                                repeatColumn = true;
                            }

                            field[column, innerRow] = field[column, innerRow + rowDirection];

                            // note if anything was moved beside zeros
                            //if (field[collumn, innerRow] != 0)
                            //{
                            //    repeatCollumn = true;
                            //}
                        }
                        // fill last row with a zero
                        field[column, rowEnd] = 0;

                        if (repeatColumn)
                        {
                            row -= rowDirection;
                        }

                    }
                }

                // through each row
                for (int row = rowStart; row != rowEnd; row += rowDirection)
                {
                    if (field[column, row] == field[column, row + rowDirection])
                    {
                        field[column, row] *= 2;

                        for (int innerRow = row + rowDirection; innerRow != rowEnd; innerRow += rowDirection)
                        {
                            field[column, innerRow] = field[column, innerRow + rowDirection];
                        }

                        field[column, rowEnd] = 0;
                    }
                }
            }

            insertRandomNumber();
        }

        public void pushUp()
        {
            field = rotateRight(rotateRight(field));
            pushDown();
            field = rotateRight(rotateRight(field));
        }
    }
}
