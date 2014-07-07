using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class GameModel
    {
        // the states the game can be in
        public enum States
        {
            RUNNING, LOST, WON
        }

        // the game field is saved as an two dimensial array
        private int[,] field = new int[4, 4];
        // the game starts obviously with the state RUNNING
        private States state = States.RUNNING;

        public GameModel()
        {
            // init with two random 2s and/or 4s
            for (int i = 0; i < 2; i++)
            {
                insertRandomNumber();
            }
        }

        // setter for the fild
        public int[,] playingField
        {
            get
            {
                return field;
            }
        }

        // setter for the game state
        public States gameState
        {
            get
            {
                return state;
            }
        }

        // tries to find a free cell in the game field
        // runs indifinite if the game field is full, so call placeLeft(() before.
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

        // rotate the game field 90 degrees to the right.
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

        // see documentation
        public void pushLeft()
        {
            field = rotateRight(rotateRight(rotateRight(field)));
            pushDown();
            field = rotateRight(field);
        }

        // see documentation
        public void pushRight()
        {
            field = rotateRight(field);
            pushDown();
            field = rotateRight(rotateRight(rotateRight(field)));
        }

        // pushes all numbers down 
        public void pushDown()
        {
            if (state == States.RUNNING)
            {
                int columnStart = 0;
                int columnEnd = field.GetLength(0);
                int columnDirection = 1;
                int rowStart = field.GetLength(1) - 1;
                int rowEnd = 0;
                int rowDirection = -1;
                bool somethingMoved = false;

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
                                    // if the current number is a 0 and the one above isn't the one above will move down, that counts as a game field change, thus we can add a new random number
                                    somethingMoved = true;
                                }

                                field[column, innerRow] = field[column, innerRow + rowDirection];
                                ;
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
                        if (field[column, row] != 0 && field[column, row] == field[column, row + rowDirection])
                        {
                            // if we merge something the game field changes and it's ok to add another number later
                            somethingMoved = true;

                            field[column, row] *= 2;

                            // move all numbers above down
                            for (int innerRow = row + rowDirection; innerRow != rowEnd; innerRow += rowDirection)
                            {
                                field[column, innerRow] = field[column, innerRow + rowDirection];
                            }

                            field[column, rowEnd] = 0;
                        }
                    }
                }

                // check if won
                if (any2048())
                {
                    state = States.WON;
                }
                else
                {
                    // check empty cells left
                    if (placeLeft())
                    {
                        // only inster number if something changed
                        if (somethingMoved)
                        {
                            insertRandomNumber();
                        }

                        // check if lost
                        if (!anyMovesLeft())
                        {
                            state = States.LOST;
                        }
                    }
                    else if (!anyMovesLeft())
                    {
                        state = States.LOST;
                    }
                }

            }
        }

        // returns true if any cell contains the nummber 2048
        private bool any2048()
        {
            for (int column = 0; column < field.GetLength(0); column++)
            {
                for (int row = 0; row < field.GetLength(1); row++)
                {
                    if (field[column, row] == 2048)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // return true if it find an empty cell (a cell containing a zero)
        private bool placeLeft()
        {
            for (int column = 0; column < field.GetLength(0); column++)
            {
                for (int row = 0; row < field.GetLength(1); row++)
                {
                    if (field[column, row] == 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // check if there are any viable moves left.
        private bool anyMovesLeft()
        {
            // if the game field isn't full there are moves left
            if (placeLeft())
            {
                return true;
            }
            // checks for every cell if any neighbouring cells contain the same number, that would count as a viable move, so the function would return true
            for (int column = 0; column < field.GetLength(0); column++)
            {
                for (int row = 0; row < field.GetLength(1); row++)
                {
                    if (column > 0)
                    {
                        if (field[column, row] == field[column - 1, row])
                        {
                            return true;
                        }
                    }
                    if (column < field.GetLength(0) - 1)
                    {
                        if (field[column, row] == field[column + 1, row])
                        {
                            return true;
                        }
                    }
                    if (row > 0)
                    {
                        if (field[column, row] == field[column, row - 1])
                        {
                            return true;
                        }
                    }
                    if (row < field.GetLength(1) - 1)
                    {
                        if (field[column, row] == field[column, row + 1])
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        // see documentation
        public void pushUp()
        {
            field = rotateRight(rotateRight(field));
            pushDown();
            field = rotateRight(rotateRight(field));
        }
    }
}
