using System;
using System.Collections.Generic;

namespace MastersThesis
{
    class PlayerListFunctions
    {

        public static List<List<string>> traitsList = new List<List<string>>();
        //Trait groups
        //Trust trait group
        static List<string> trust_traits = new List<string>() { "Trusting", "Untrusting", "Unsuspicious", "Suspicious" };
        //Deceit trait group
        static List<string> deceit_traits = new List<string>() { "Deceitful", "Honest", "Calculating", "Fair" };
        //Deceit ability trait group
        static List<string> deceitAbility_traits = new List<string>() { "Aggressive", "Passive", "Audacious", "Timid" };
        private static Random rd = new Random();

        //Gets new traits for a new player
        public static List<string> getNewTraits(int amt)
        {
            List<string> new_traits = new List<string>();
            //Create a traits master list from the group traits
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

        //Resolve argument made by player
        public static void ResolveArguments(Argument a, List<Player> players)
        {
            double trustChange = 0;
            double deceitChange = 0;
            double deceitAbilityChange = 0;
            //Loop through all players
            foreach (Player p in players)
            {
                //Check what the argument is, and add it to variable based on weight.
                switch (a.statement)
                {
                    case "Deceitful":
                        deceitChange = Options.argumentWeight;
                        break;
                    case "NotDeceitful":
                        deceitChange = Options.argumentWeight * -1;
                        break;
                    case "Aggressive":
                        deceitAbilityChange = Options.argumentWeight;
                        break;
                    case "NotAggressive":
                        deceitAbilityChange = Options.argumentWeight * -1;
                        break;
                    case "Trustful":
                        trustChange = Options.argumentWeight;
                        break;
                    case "NotTrustful":
                        trustChange = Options.argumentWeight * -1;
                        break;
                }
                if (p != a.receiver)
                {
                    //Assign values to player's PPMs
                    if (trustChange != 0) { p.GetPerceivedPlayerModel(a.receiver).AddTrust(p.playerModel, trustChange); }
                    else if (deceitChange != 0) { p.GetPerceivedPlayerModel(a.receiver).AddDeceitfulness(p.playerModel, deceitChange); }
                    else { p.GetPerceivedPlayerModel(a.receiver).AddDeceitAbility(p.playerModel, deceitAbilityChange); }
                }
            }
        }
        
        //Get player by ID
        public static Player GetPlayerByID(int id, List<Player> players)
        {
            return players.Find(o => o.playerID == id);
        }

        public static void RemoveDeadPlayers(List<Player> players)
        {
            //Remove PPMs of dead players
            RemovePerceivedModels(players);
            foreach (Player p in players)
            {
                if (p.health <= 0)
                {
                    Game.deadPlayers.Add(p); //Add dead player to list of dead players
                }
            }
            //Removing players from the players list that are out
            players.RemoveAll(item => item.health < 1);
        }

        //Remove PPMs of dead players
        public static void RemovePerceivedModels(List<Player> players)
        {
            foreach (Player p in players)
            {
                if (p.health < 1)
                {
                    foreach (Player p2 in players)
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
