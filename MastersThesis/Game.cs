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
            for (int j = 0; j < 5000; j++)
            {
                players.Clear();
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
        }

        static void GameLoop()
        {
            while (players.Count > 1) //Whilst players are still in the game
            {
                foreach (Player p in players) //Loop through each player for their turn
                {
                    if (p.health > 0) //Check the player isnt out
                    {
                        Random rd = new Random();
                        //Get the target ID with the highest perceived threat
                        int targetID = p.GetHighestThreat(players);
                        //Fetch that player object as target
                        Player target = PlayerListFunctions.GetPlayerByID(targetID, players);
                        //Get the card the player is gonna say
                        int card = p.GetCard();
                        //Get the statement about the card (truth or lie)
                        string statement = p.GenerateStatement(card);
                        //Target makes guess about card
                        int guess = target.GuessCard(statement, p);
                        if (guess == card) //If correct, both gain 1 health and add 1 trust to the model.
                        {
                            p.health++;
                            target.health++;
                            target.GetPerceivedPlayerModel(p).AddTrust(target.playerModel, 1);
                        }
                        else //If incorrect
                        { 
                            //Lose health based on difference
                            int diff = Math.Abs(card - guess);
                            target.health -= diff;
                            //If the opponent DID lie
                            target.GetPerceivedPlayerModel(p).AddTrust(target.playerModel, diff * -1);
                            foreach(Player p2 in players)
                            {
                                if (p2.playerID != p.playerID)
                                {
                                    p2.GetPerceivedPlayerModel(p).AddTrust(p2.playerModel, -1);
                                }
                            }
                            //If the opponent told the TRUTH, but you thought he was lying
                            //TODO ADD THIS
                        }
                    }
                }
                //Removing perceivedModels that are out
                foreach(Player p in players)
                {
                    if (p.health < 1)
                    {
                        PlayerListFunctions.RemovePerceivedModels(p.playerID, players);
                    }
                }
                //Removing players from the players list that are out
                players.RemoveAll(item => item.health < 1);
            }
            Console.WriteLine("Player " + players[0].playerID + " wins!");
        }
    }
}
