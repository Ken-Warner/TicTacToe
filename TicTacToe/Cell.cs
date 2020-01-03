using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TicTacToe
{
    public class Cell : Image
    {
        //used to determine the state of the cell
        public enum Type : byte
        {
            X, O, Empty
        }

        //the game the cell belongs to
        public Game ParentGame;
        //the Cell's state
        public Type CellType { get; set; }

        //constructor
        public Cell(Game parent) : base()
        {
            //set the initial state and the parent game of the cell
            CellType = Type.Empty;
            ParentGame = parent;

            //the cell is also an image for the UI of the game, these attributes are set to
            //satisfy the base class
            Width = 100;
            Height = 100;
            Source = ParentGame.EMPTY;
            Stretch = System.Windows.Media.Stretch.Fill;
        }

        /// <summary>
        /// Sets the cell's state according to the specified cell type.
        /// </summary>
        /// <param name="cellType">New Cell state</param>
        /// <returns>returns true if the cell has been set, false if it can't be</returns>
        public bool SetCell(Type cellType)
        {
            if (CellType == Type.Empty && cellType != Type.Empty)
            {
                CellType = cellType;
                switch (CellType)
                {
                    case Type.X:
                        Source = ParentGame.X;
                        break;
                    case Type.O:
                        Source = ParentGame.O;
                        break;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Resets the cell to it's initial state.
        /// </summary>
        public void ResetCell()
        {
            CellType = Type.Empty;
            Source = ParentGame.EMPTY;
        }
    }
}
