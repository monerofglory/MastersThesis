using System;
using System.Collections.Generic;

namespace MastersThesis
{
    class Game
    {
        public static List<Player> players = new List<Player>();
        static void Main(string[] args)
        {
            //Initialising players
            for (int i = 0; i < 20; i++)
            {
                players.Add(new Player(i));
            }
            GameLoop();
        }

        static void GameLoop()
        {
            foreach( Player p in players )
            {
                Console.WriteLine(p.playerID);
            }
        }
    }
}
