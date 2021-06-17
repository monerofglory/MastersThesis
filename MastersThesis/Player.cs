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

        public int GetCard()
        {
            Random rd = new Random();
            return rd.Next(0, 9);
        }

        public string GenerateStatement(int card)
        {
            int new_card = card;
            Random rd = new Random();
            if (rd.Next(0, 100) <= playerModel.deceitfulness)
            {
                int bound = Convert.ToInt32(Math.Round(playerModel.deceitAbility / 10));
                new_card += rd.Next(-1 * bound, bound);
                if (new_card > 9) { new_card = 9; } //If greater than 9, set to 9
                else if (new_card < 1) { new_card = 1; } 
                return (new_card).ToString();
            }
            else
            {
                return new_card.ToString();
            }
        }
        public int GuessCard(string statement, Player p)
        {
            Random rd = new Random();
            double pD = GetPerceivedPlayerModel(p).perceivedDeceitfulness;
            if (rd.Next(0, 100) <= pD)
            {
                return rd.Next(0, 9);
            }
            return rd.Next(0, 9);
        }
    }
}
