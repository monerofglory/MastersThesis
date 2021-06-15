using System;
using System.Collections.Generic;
using System.Text;

namespace MastersThesis
{
    class Player
    {
        public int playerID;
        public PlayerModel playerModel;
        public PerceivedPlayerModel perceivedPlayerModel;
        public Player(int id)
        {
            playerID = id;
        }
    }
}
