using System;
using System.Collections.Generic;
using System.Text;

namespace MastersThesis
{
    class PlayerListFunctions {

        private static Random rd = new Random();
        public static List<string> getNewTrait()
        {
            List<string> new_trait = new List<string>();
            //Traits seperated into seperate lists to avoid mutex pairs (e.g trusting AND untrusting)
            string[,] traits = 
            {{ "Trusting", "Untrusting", "Suspicious", "Unsuspicious" },
            { "Deceitful", "Honest", "Calculating", "Fair" }, //Predefined traits
            { "Aggressive", "Passive", "Audacious", "Timid" } };
            for (int i = 0; i < rd.Next(1, 3); i++) //Add a number of traits from list
            {
                int dim1_random = rd.Next(0, traits.GetLength(0));
                int dim2_random = rd.Next(traits.GetLength(1));
                string new_trait_to_add = traits[dim1_random, dim2_random];
                if (!new_trait.Contains(new_trait_to_add)) //Check if trait is already added.
                {
                    new_trait.Add(new_trait_to_add);
                }
            }
            return new_trait; //Return list
        }

        public static List<string> getNewStrategy()
        {
            List<string> new_strategies = new List<string>();
            string[] strategies = { "Tit-For-Tat", "Attack Most Health", "Attack Lowest Health" }; //Predefined strategies
            for (int i = 0; i < rd.Next(1, 3); i++) //Add a number of strategies from list
            {
                new_strategies.Add(strategies[rd.Next(0, strategies.Length)]);
            }
            return new_strategies; //Return list
        }

        public static void ResolveArguments(Argument a, List<Player> players)
        {
            double trustChange = 0;
            double deceitChange = 0;
            double deceitAbilityChange = 0;
            foreach (Player p in players)
            {
                switch(a.statement)
                {
                    case "Deceitful":
                        deceitChange = 1;
                        break;
                    case "NotDeceitful":
                        deceitChange = -1;
                        break;
                    case "Aggressive":
                        deceitAbilityChange = 1;
                        break;
                    case "NotAggressive":
                        deceitAbilityChange = -1;
                        break;
                    case "Trustful":
                        trustChange = 1;
                        break;
                    case "NotTrustful":
                        trustChange = -1;
                        break;
                }
                if (p != a.receiver)
                {
                    p.GetPerceivedPlayerModel(a.receiver).perceivedDeceitfulness += deceitChange;
                    p.GetPerceivedPlayerModel(a.receiver).perceivedDeceitAbility += deceitAbilityChange;
                    p.GetPerceivedPlayerModel(a.receiver).perceivedTrustfullness += trustChange;
                }
            }
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

        public static void RemovePerceivedModels(List<Player> players)
        {
            foreach(Player p in players)
            {
                if (p.health < 1)
                {
                    foreach(Player p2 in players)
                    {
                        if (p2 != p)
                        {
                            p2.perceivedPlayerModels.RemoveAll(item => item.playerID == p.playerID);
                        }
                    }
                }
            }
        }
    }
}
