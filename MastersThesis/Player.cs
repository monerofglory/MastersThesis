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
        public PerceivedPlayerModel perceivedPlayerModel;
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
    }
}
