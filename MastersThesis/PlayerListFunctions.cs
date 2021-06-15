using System;
using System.Collections.Generic;
using System.Text;

namespace MastersThesis
{
    class PlayerListFunctions { 
    
        public static Player getTarget(int currentId, List<Player> players) {
            Random rd = new Random();
            Player target;
            bool found = false;
            while (!found)
            {
                target = players[rd.Next(0, players.Count)];
                if (target.playerID != currentId)
                {
                    found = true;
                    return target;
                }
            }
            return null;
        }

        public static List<string> getNewTrait()
        {
            Random rd = new Random(); //Initialise random number gen
            List<string> new_trait = new List<string>();
            string[] traits = { "Aggressive", "Deceitful", "Forgiving", "Trustworthy" }; //Predefined traits
            for (int i = 0; i < rd.Next(0, 2); i++) //Add a number of traits from list
            {
                new_trait.Add(traits[rd.Next(0, traits.Length)]);
            }
            return new_trait; //Return list
        }

        public static List<string> getNewStrategy()
        {
            Random rd = new Random(); //Initialise random number gen
            List<string> new_strategies = new List<string>();
            string[] strategies = { "Tit-For-Tat", "Attack Most Health", "Attack Lowest Health" }; //Predefined traits
            for (int i = 0; i < rd.Next(0, 2); i++) //Add a number of traits from list
            {
                new_strategies.Add(strategies[rd.Next(0, strategies.Length)]);
            }
            return new_strategies; //Return list
        }
    }
}
