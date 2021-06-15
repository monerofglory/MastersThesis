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
    }
}
