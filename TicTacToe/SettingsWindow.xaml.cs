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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        //sets the radio buttons in the mnu according to the difficulty of the game
        //returns the UserPreferences.Difficulty depending upon the set radio button
        public UserPreferences.Difficulty SelectedDifficulty
        {
            get
            {

                if ((bool)challengingRadioButton.IsChecked)
                    return UserPreferences.Difficulty.Challenging;
                else if ((bool)impossibleRadioButton.IsChecked)
                    return UserPreferences.Difficulty.Impossible;
                else
                    return UserPreferences.Difficulty.Easy;
            }
            set
            {
                switch (value)
                {
                    case UserPreferences.Difficulty.Easy:
                        easyRadioButton.IsChecked = true;
                        challengingRadioButton.IsChecked = false;
                        impossibleRadioButton.IsChecked = false;
                        break;
                    case UserPreferences.Difficulty.Challenging:
                        easyRadioButton.IsChecked = false;
                        challengingRadioButton.IsChecked = true;
                        impossibleRadioButton.IsChecked = false;
                        break;
                    case UserPreferences.Difficulty.Impossible:
                        easyRadioButton.IsChecked = false;
                        challengingRadioButton.IsChecked = false;
                        impossibleRadioButton.IsChecked = true;
                        break;
                }
            }
        }

        public SettingsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Dialog finished OK and closes the window
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Preferences are not to be saved, closes the window
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
