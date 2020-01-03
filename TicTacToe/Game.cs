using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TicTacToe
{
    public class Game
    {
        //Array of cells representing the game board
        public Cell[,] gameBoard;
        //the cell used to capture the preferred move of the AI
        private Cell minimaxMove;
        //images used for the various states of the game cells
        public BitmapImage EMPTY, X, O;

        private int MaxDepth_AIdifficulty;

        //constructor
        public Game()
        {
            //load the cell state images
            EMPTY = new BitmapImage(new Uri("Resources/Empty.png", UriKind.Relative));
            X = new BitmapImage(new Uri("Resources/X.png", UriKind.Relative));
            O = new BitmapImage(new Uri("Resources/O.png", UriKind.Relative));

            //create the game board
            gameBoard = new Cell[,]
            {
                { new Cell(this), new Cell(this), new Cell(this) },
                { new Cell(this), new Cell(this), new Cell(this) },
                { new Cell(this), new Cell(this), new Cell(this) }
            };

            //set the state tree traversal depth based on the difficulty set by the user
            switch (UserPreferences.SelectedDifficulty)
            {
                case UserPreferences.Difficulty.Easy:
                    MaxDepth_AIdifficulty = 2;
                    break;
                case UserPreferences.Difficulty.Challenging:
                    MaxDepth_AIdifficulty = 5;
                    break;
                case UserPreferences.Difficulty.Impossible:
                    MaxDepth_AIdifficulty = 9;
                    break;
                default:
                    MaxDepth_AIdifficulty = 9;
                    break;
            }
        }

        /// <summary>
        /// Checks if there is a stalemate for the game.
        /// </summary>
        /// <returns>Returns true if there is no more moves left.</returns>
        public bool CheckStalemate()
        {
            foreach (Cell cell in gameBoard)
                if (cell.CellType == Cell.Type.Empty)
                    return false;
            return true;
        }

        /// <summary>
        /// Checks if the specified player has won the game
        /// </summary>
        /// <param name="type">Player to check for</param>
        /// <returns>Returns true if the player has won the game</returns>
        public bool IsGameOver(Cell.Type type)
        {
            //check rows
            int row = 0;
            for (int i = 0; i < 3; i++)
            {
                //if the cell for that row matches the player
                if (gameBoard[row, i].CellType == type)
                {
                    //if we've made it this far, the other two cells match, and it's a win
                    if (i == 2)
                    {
                        return true;
                    }
                }
                else
                {
                    //if that cell didn't match the player
                    //on to the next row
                    i = -1;
                    row++;

                    //if the next row is out of bounds, break the loop
                    if (row == 3)
                        break;
                    //none of the rows were winners
                }
            }

            //check columns (using same logic)
            int col = 0;
            for (int i = 0; i < 3; i++)
            {
                if (gameBoard[i, col].CellType == type)
                {
                    if (i == 2)
                    {
                        return true;
                    }
                }
                else
                {
                    i = -1;
                    col++;

                    if (col == 3)
                        break;
                }
            }

            //check diagonals literally
            if (gameBoard[0, 0].CellType == type &&
                gameBoard[1, 1].CellType == type &&
                gameBoard[2, 2].CellType == type)
            {
                return true;
            }

            if (gameBoard[0, 2].CellType == type &&
                gameBoard[1, 1].CellType == type &&
                gameBoard[2, 0].CellType == type)
            {
                return true;
            }
            
            //the player hasn't won... return false
            return false;
        }

        /// <summary>
        /// Resets the game
        /// </summary>
        public void ResetGame()
        {
            foreach (Cell cell in gameBoard)
                cell.ResetCell();
        }

        /// <summary>
        /// Runs the minimax algorithm so the AI can make a decision
        /// </summary>
        public void MakeAIMove()
        {
            //run minimax
            MiniMax(0, true, int.MinValue, int.MaxValue);

            //place the AI's decision on the board
            minimaxMove.SetCell(Cell.Type.O);
        }

        /// <summary>
        /// Minimax creates a state tree for the game representing each move that can be made in the 
        /// game from the current state.
        /// </summary>
        /// <param name="depth">The depth this node is within the state tree.</param>
        /// <param name="isMaximizer">Represents the player that is playing, the AI is the maximizer, the player is the minimizer.</param>
        /// <param name="alpha">Represents the best possible minimax score that the maximizer has achieved thus far.</param>
        /// <param name="beta">Represents the best possible score the minimizer has achieved thus far.</param>
        /// <returns></returns>
        private int MiniMax(int depth, bool isMaximizer, int alpha, int beta)
        {
            //If the minimizer has won the game, return a score of -1
            if (IsGameOver(Cell.Type.X))
                return -1;
            //If the maximizer has won the game, return a score of 1
            if (IsGameOver(Cell.Type.O))
                return 1;
            //If the game was a stalemate, return a score of 0
            if (CheckStalemate())
                return 0;
            //if at the max depth depending on the game difficulty and we don't have an evaluation, return 0
            if (depth >= MaxDepth_AIdifficulty)
                return 0;

            //constains the current best evaluation
            int bestValue;
            //lists of evaluations and moves
            List<int> scores = new List<int>();
            List<Cell> moves = new List<Cell>();

            //If this node is a maximizing node (player O)
            if (isMaximizer)
            {
                //best value is the worst possible initial evaluation
                bestValue = int.MinValue;

                //foreach empty cell in the game
                foreach (Cell cell in gameBoard)
                    if (cell.CellType == Cell.Type.Empty)
                    {
                        //place an O in the cell
                        cell.SetCell(Cell.Type.O);

                        //call minimax on this new grid, and get the evaluation
                        int score = MiniMax(depth + 1, false, alpha, beta);

                        //save the evaluation and this move we just tested
                        scores.Add(score);
                        moves.Add(cell);

                        //reset the cell back to an unplayed state
                        cell.ResetCell();

                        //check to see if the new score is better than our current best
                        if (score >= bestValue)
                        {
                            //if it is, make it the best one
                            bestValue = score;
                        }

                        //alpha-beta pruning part

                        //alpha is always the best possible move we have made so far
                        alpha = Math.Max(bestValue, alpha);

                        //if it's ever >= to beta, we don't need to continue
                        if (alpha >= beta)
                            break;
                    }

                //save the best move in the global variable for the best move
                //when this is the root node, the best move from the initial game state will be the
                //one saved
                minimaxMove = moves[scores.IndexOf(scores.Max())];
                //return the best value so the previous node can score this evaluation
                return bestValue;
            } else //Else this node is a minimizing node (player X)
            {
                //follows similar logic to the maximizer

                //the worst starting value is actually the highest this time
                bestValue = int.MaxValue;

                foreach (Cell cell in gameBoard)
                    if (cell.CellType == Cell.Type.Empty)
                    {
                        //the player is X
                        cell.SetCell(Cell.Type.X);

                        int score = MiniMax(depth + 1, true, alpha, beta);

                        cell.ResetCell();

                        //our best evaluation is the lowest one
                        if (score <= bestValue)
                        {
                            bestValue = score;
                        }

                        beta = Math.Min(bestValue, beta);

                        //however, we are still checking if alpha is >= beta
                        if (alpha >= beta)
                            break;
                    }
                //return the best evaluation to the previous node, we don't need to 
                //save a move from the minimizer
                return bestValue;
            }
        }
    }
}
