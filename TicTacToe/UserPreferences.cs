using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    [Serializable]
    public class UserPreferences
    {
        //different possible difficulties for the game
        public enum Difficulty
        {
            Easy, Challenging, Impossible
        }

        //the currently selected game difficulty (default = Impossible)
        public static Difficulty SelectedDifficulty = Difficulty.Impossible;

        #region SERIALIZING AND DESERIALIZING

        /// <summary>
        /// Saves the selected preferences from the settings menu.
        /// </summary>
        public static void SerializeUserPreferences()
        {
            using (Stream fileStream = File.Create("UserPreferences.dat"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fileStream, new UserPreferences());
            }
        }

        /// <summary>
        /// Loads the users saved settings
        /// </summary>
        public static void DeSerializeUserPreferences()
        {
            //object
            UserPreferences preferences;
            //open file stream and deserialize
            try
            {
                using (Stream fileStream = File.Open("UserPreferences.dat", FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    preferences = (UserPreferences)formatter.Deserialize(fileStream);
                }
            }
            catch (Exception)
            {
                return; //will use defaults in the case of failure to load
            }

            //set static variables in user preferences in accordance to the loaded object
            SelectedDifficulty = preferences.difficulty;

            //don't waste space
            preferences = null;
        }

        #endregion

        #region SERIALIZABLE OBJECT CLASS
        //difficulty of the game
        public Difficulty difficulty;

        public UserPreferences()
        {
            difficulty = SelectedDifficulty;
        }
        #endregion
    }
}
