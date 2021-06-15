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
                players.Add(new Player(i, PlayerListFunctions.getNewTrait(), PlayerListFunctions.getNewStrategy()));
            }
            GameLoop();
        }

        static void GameLoop()
        {
            while (players.Count > 1)
            {
                foreach (Player p in players)
                {
                    if (p.health > 0)
                    {
                        Random rd = new Random();
                        Player target = PlayerListFunctions.getTarget(p.playerID, players);
                        int card = rd.Next(0, 9);
                        int guess = rd.Next(0, 9);
                        Console.WriteLine(p.playerID + " targets " + target.playerID);
                        Console.WriteLine(p.playerID + " says " + card + ", " + target.playerID + " guesses " + guess);
                        if (guess == card)
                        {
                            p.health++;
                            target.health++;
                        }
                        else
                        {
                            int diff = Math.Abs(card - guess);
                            target.health -= diff;
                        }
                        Console.WriteLine("Player " + target.playerID + " has health = " + target.health);
                    }
                }
                players.RemoveAll(item => item.health < 1);
                Console.WriteLine("Players remaining: " + players.Count);
            }
            Console.WriteLine("Player " + players[0].playerID + " wins! With " + players[0].health + " remaining!");
        }
    }
}
