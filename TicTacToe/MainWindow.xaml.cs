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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Load Settings, if there are none, use defaults
            UserPreferences.DeSerializeUserPreferences();
        }

        /// <summary>
        /// Event fires when the start game button is pressed.
        /// </summary>
        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            //instantiate a game window
            GameWindow gameWindow = new GameWindow();

            //hide this window
            Hide();

            //show the game window and wait for results
            gameWindow.ShowDialog();

            //show this window again when the game window is closed
            Show();
        }

        /// <summary>
        /// Event triggers when the settings button is pressed.
        /// </summary>
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            //create a settingsWindow and set it's selected difficulty from the user preferences
            SettingsWindow settingsWindow = new SettingsWindow()
            {
                SelectedDifficulty = UserPreferences.SelectedDifficulty
            };

            //show the window and get its result
            bool? result = settingsWindow.ShowDialog();

            //if it has a result
            if (result.HasValue)
            {
                //and the result is true
                if (result.Value)
                {
                    //set the new difficulty and Serialize the new UserPreferences
                    UserPreferences.SelectedDifficulty = settingsWindow.SelectedDifficulty;
                    UserPreferences.SerializeUserPreferences();
                }
            }
        }

        /// <summary>
        /// Event triggers when the exit button is pressed.
        /// </summary>
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            //close the window
            Close();
        }
    }
}
