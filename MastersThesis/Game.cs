using System;
using System.Collections.Generic;
using System.Linq;

namespace MastersThesis
{
    class Game
    {
        public static List<Player> players = new List<Player>();
        static void Main(string[] args)
        {
            //Initialising players
            int num = 20; //Number of players
            for (int i = 0; i < num; i++)
            {
                players.Add(new Player(i, PlayerListFunctions.getNewTrait(), PlayerListFunctions.getNewStrategy()));
            }
            for (int i = 0; i < num; i++)
            {
                players[i].AddPerceivedPlayerModels(players);
            }
            GameLoop();
        }

        static void GameLoop()
        {
            Console.WriteLine("Player " + players[0].playerID);
            Console.WriteLine("Traits:");
            foreach(string t in players[0].traits)
            {
                Console.WriteLine(t);
            }
            Console.WriteLine("Values:");
            Console.WriteLine("Trust: " + players[0].playerModel.trust);
            Console.WriteLine("Deceitfulness: " + players[0].playerModel.deceitfulness);
            Console.WriteLine("DeceitAbility: " + players[0].playerModel.deceitAbility);
            while (players.Count > 1)
            {
                foreach (Player p in players)
                {
                    if (p.health > 0)
                    {
                        Console.WriteLine("PLAYER TURN: " + p.playerID);
                        Random rd = new Random();
                        int targetID = p.GetHighestThreat(players);
                        Console.WriteLine(targetID);
                        Player target = PlayerListFunctions.GetPlayerByID(targetID, players);
                        int card = rd.Next(0, 9);
                        int guess = rd.Next(0, 9);
                        Console.WriteLine(p.playerID + " targets " + target.playerID);
                        Console.WriteLine(p.playerID + " says " + card + ", " + target.playerID + " guesses " + guess);
                        if (guess == card)
                        {
                            p.health++;
                            target.health++;
                            target.GetPerceivedPlayerModel(p).AddTrust(target.playerModel, 1);
                        }
                        else
                        {
                            int diff = Math.Abs(card - guess);
                            target.health -= diff;
                        }
                        Console.WriteLine("Player " + target.playerID + " has health = " + target.health);
                    }
                }
                //Removing perceivedModels
                foreach(Player p in players)
                {
                    if (p.health < 1)
                    {
                        PlayerListFunctions.RemovePerceivedModels(p.playerID, players);
                    }
                }
                players.RemoveAll(item => item.health < 1);
                Console.WriteLine("Players remaining: " + players.Count);
                foreach(Player p in players)
                {
                    Console.WriteLine("Player " + p.playerID + " with health = " + p.health);
                }
            }
            Console.WriteLine("Player " + players[0].playerID + " wins! With " + players[0].health + " remaining!");
        }
    }
}
