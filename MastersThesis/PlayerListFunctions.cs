using System;
using System.Collections.Generic;
using System.Linq;

namespace MastersThesis
{
    class PlayerListFunctions {

        public static List<List<string>> traitsList = new List<List<string>>();
        static List<string> trust_traits = new List<string>() { "Trusting", "Untrusting", "Suspicious", "Unsuspicious" };
        static List<string> deceit_traits = new List<string>() { "Deceitful", "Honest", "Calculating", "Fair" };
        static List<string> deceitAbility_traits = new List<string>() { "Aggressive", "Passive", "Audacious", "Timid" };
        private static Random rd = new Random();

        public static List<string> getNewTraits(int amt)
        {
            List<string> new_traits = new List<string>();
            if (traitsList.Count == 0)
            {
                traitsList.Add(trust_traits);
                traitsList.Add(deceit_traits);
                traitsList.Add(deceitAbility_traits);
            }
            while (new_traits.Count < amt)
            {
                //Get new trait
                int traitListToQuery = rd.Next(0, traitsList.Count);
                //Check if a trait from that list already exists
                bool found = false;
                foreach (string t in new_traits)
                {
                    if (traitsList[traitListToQuery].Contains(t))
                    {
                        found = true;
                    }
                }
                //If not, add it
                if (!found)
                {
                    new_traits.Add(traitsList[traitListToQuery][rd.Next(0, traitsList[traitListToQuery].Count)]);
                }
            }
            return new_traits;
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
                    p.GetPerceivedPlayerModel(a.receiver).AddDeceitfulness(p.playerModel, deceitChange);
                    p.GetPerceivedPlayerModel(a.receiver).AddDeceitAbility(p.playerModel, deceitAbilityChange);
                    p.GetPerceivedPlayerModel(a.receiver).AddTrust(p.playerModel, trustChange);
                }
            }
        }
        public static Player GetPlayerByID(int id, List<Player> players)
        {
            /*foreach(Player p in players)
            {
                if (p.playerID == id)
                {
                    return p;
                }
            }
            return null;
            */
            return players.Find(o => o.playerID == id);
        }

        public static void RemoveDeadPlayers(List<Player> players)
        {
            RemovePerceivedModels(players);
            foreach(Player p in players)
            {
                if (p.health <= 0 )
                {
                    Game.deadPlayers.Add(p);
                }
            }
            //Removing players from the players list that are out
            players.RemoveAll(item => item.health < 1);
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
