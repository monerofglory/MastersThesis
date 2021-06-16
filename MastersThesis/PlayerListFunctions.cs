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
                    return target;
                }
            }
            return null;
        }

        public static List<string> getNewTrait()
        {
            Random rd = new Random(); //Initialise random number gen
            List<string> new_trait = new List<string>();
            string[] traits = { "Trusting", "Untrusting", "Suspicious", "Unsuspicious", "Aggressive", "Passive", "Calculating", "Virtuous"}; //Predefined traits
            for (int i = 0; i < rd.Next(1, 3); i++) //Add a number of traits from list
            {
                string new_trait_to_add = traits[rd.Next(0, traits.Length)];
                if (!new_trait.Contains(new_trait_to_add)) //Check if trait is already added.
                {
                    new_trait.Add(new_trait_to_add);
                }
            }
            return new_trait; //Return list
        }

        public static List<string> getNewStrategy()
        {
            Random rd = new Random(); //Initialise random number gen
            List<string> new_strategies = new List<string>();
            string[] strategies = { "Tit-For-Tat", "Attack Most Health", "Attack Lowest Health" }; //Predefined strategies
            for (int i = 0; i < rd.Next(1, 3); i++) //Add a number of strategies from list
            {
                new_strategies.Add(strategies[rd.Next(0, strategies.Length)]);
            }
            return new_strategies; //Return list
        }

        public static Player GetPlayerByID(int id, List<Player> players)
        {
            foreach(Player p in players)
            {
                if (p.playerID == id)
                {
                    return p;
                }
            }
            return null;
        }

        public static void RemovePerceivedModels(int id, List<Player> players)
        {
            foreach(Player p in players)
            {
                p.perceivedPlayerModels.RemoveAll(item => item.playerID == id);
            }
        }
    }
}
