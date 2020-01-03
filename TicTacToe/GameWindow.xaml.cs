using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        //game object for this game of TicTacToe
        Game game;

        public GameWindow()
        {
            InitializeComponent();

            //this method initializes the game
            InitializeGameGrid();

            switch(UserPreferences.SelectedDifficulty)
            {
                case UserPreferences.Difficulty.Easy:
                    Title = "Tic Tac Toe (Easy)";
                    break;
                case UserPreferences.Difficulty.Challenging:
                    Title = "Tic Tac Toe (Challenging)";
                    break;
                case UserPreferences.Difficulty.Impossible:
                    Title = "Tic Tac Toe (Impossible)";
                    break;
            }
        }

        /// <summary>
        /// Clears the gameGrid and resets the game
        /// </summary>
        private void InitializeGameGrid()
        {
            //create new game
            game = new Game();
            //clear the grid
            gameGrid.Children.Clear();

            //populate the grid with game cells
            for (int x = 0; x < 5; x += 2)
                for (int y = 0; y < 5; y += 2)
                {
                    //get relevant cell from the game
                    Cell cell = game.gameBoard[x / 2, y / 2];

                    //set that cell's row and column on the grid
                    Grid.SetColumn(cell, x);
                    Grid.SetRow(cell, y);

                    //add it to the grid's children
                    gameGrid.Children.Add(cell);

                    //give the cell an event handler for clicks
                    cell.MouseUp += GameCellClicked;
                }

            //draw vertical bars
            for (int x = 1; x < 5; x += 2)
                for (int y = 0; y < 5; y += 1)
                {
                    Rectangle rectangle = new Rectangle()
                    {
                        Fill = Brushes.Black,
                    };

                    Grid.SetColumn(rectangle, x);
                    Grid.SetRow(rectangle, y);

                    gameGrid.Children.Add(rectangle);
                }

            //draw horizontal bars
            for (int x = 0; x < 5; x += 2)
                for (int y = 1; y < 5; y += 2)
                {
                    Rectangle rectangle = new Rectangle()
                    {
                        Fill = Brushes.Black,
                    };

                    Grid.SetColumn(rectangle, x);
                    Grid.SetRow(rectangle, y);

                    gameGrid.Children.Add(rectangle);
                }
        }

        /// <summary>
        /// Checks if the game is over.
        /// </summary>
        /// <returns>True if the game is over, false if the game is not.</returns>
        private bool IsGameOver()
        {
            //The Game class has a method to check if a specified player has won, we use it to see if the game is over.
            //If neither player has won, we check for stalemates.
            if (game.IsGameOver(Cell.Type.X))
            {
                MessageBox.Show("X wins!");
                return true;
            } else if (game.IsGameOver(Cell.Type.O))
            {
                MessageBox.Show("O wins!");
                return true;
            } else if (game.CheckStalemate())
            {
                MessageBox.Show("Stalemate!");
                return true;
            }

            //if there is no stalemate, and neither player has won, then the game must not be over, return false.
            return false;
        }

        /// <summary>
        /// Asks the user if they would like to play another round.
        /// </summary>
        public void PlayNewGameDialog()
        {
            //create and display message box to ask user
            MessageBoxResult result = MessageBox.Show("Would you like to play again?", "Game Over", MessageBoxButton.YesNo, MessageBoxImage.Question);

            //depending on the result...
            switch (result)
            {
                case MessageBoxResult.Yes:
                    //reset the game
                    game.ResetGame();
                    break;
                case MessageBoxResult.No:
                    //close the game window
                    DialogResult = true;
                    Close();
                    break;
            }
        }

        /// <summary>
        /// The event handler for the game cells when they are clicked.
        /// </summary>
        /// <param name="sender">The cell that was clicked.</param>
        /// <param name="e">EventArgs</param>
        private void GameCellClicked(object sender, RoutedEventArgs e)
        {
            //Cast the cell and make sure it's valid
            Cell clickedCell = (Cell)sender;
            if (clickedCell != null)
            {
                //Make sure the user actually clicked an empty cell.
                if (clickedCell.CellType == Cell.Type.Empty)
                {
                    //Set the cell to the user's piece type
                    clickedCell.SetCell(Cell.Type.X);

                    //Check if the game is over
                    if (!IsGameOver())
                    {
                        //if it isn't, let the AI make a move
                        game.MakeAIMove();

                        //Check if it is over again
                        if (IsGameOver())
                        {
                            //if it is, ask the user if they would like to play again
                            PlayNewGameDialog();
                        }
                    } else
                    {
                        //if the game is over, ask the user if they would like to play again.
                        PlayNewGameDialog();
                    }
                } else
                {
                    //if the user didn't click an empty cell, let them know they made a mistake.
                    MessageBox.Show("You can't make a move here");
                }
            }
            e.Handled = true;
        }
    }
}
