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
        public List<String> traits;
        public List<String> strategies;
        public Player(int id)
        {
            playerID = id;
        }
    }
}
