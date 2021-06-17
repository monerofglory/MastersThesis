using System;
using System.Collections.Generic;
using System.Text;

namespace MastersThesis
{
    class Player
    {
        public int playerID;
        public int health;
        public PlayerModel playerModel;
        public List<PerceivedPlayerModel> perceivedPlayerModels = new List<PerceivedPlayerModel>();
        public List<String> traits = new List<string>();
        public List<String> strategies = new List<string>();
        public Player(int id, List<string> new_traits, List<string> new_strategies)
        {
            health = 20;
            playerID = id;
            traits.AddRange(new_traits);
            strategies.AddRange(new_strategies);
            playerModel = new PlayerModel(this);
        }

        public void AddPerceivedPlayerModels(List<Player> players)
        {
            foreach(Player p in players)
            {
                if (p != this)
                {
                    perceivedPlayerModels.Add(new PerceivedPlayerModel(p.playerID));
                }
            }
        }
        
        public PerceivedPlayerModel GetPerceivedPlayerModel(Player p)
        {
            foreach(PerceivedPlayerModel ppm in perceivedPlayerModels)
            {
                if (p.playerID == ppm.playerID)
                {
                    return ppm;
                }
            }
            return null;
        }

        public int GetHighestThreat(List<Player> players)
        {
            double highest = -9999999;
            int highest_player = -1;
            if (players.Count == 2)
            {
                Console.WriteLine("---");
                Console.WriteLine(perceivedPlayerModels.Count);
            }
            foreach (PerceivedPlayerModel ppm in perceivedPlayerModels)
            {
                
                if (PlayerListFunctions.GetPlayerByID(ppm.playerID, players).health >= 1)
                {
                    double threat = ppm.GetThreat();
                    if (threat > highest)
                    {
                        highest = threat;
                        highest_player = ppm.playerID;
                    }
                }
            }
            return highest_player;
        }
    }
}
