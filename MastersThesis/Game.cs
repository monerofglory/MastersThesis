using System;
using System.Collections.Generic;
using System.Linq;

namespace MastersThesis
{
    class Game
    {
        public static List<Player> players = new List<Player>();
        public static List<Player> deadPlayers = new List<Player>();
        private static Random rd = new Random();
        //Game details
        public static int gameLength = 0;
        public static int numberOfPlayers = 500;
        static void Main(string[] args)
        {
            
            for (int j = 0; j < 1; j++)
            {
                players.Clear();
                //Initialising players
                for (int i = 0; i < numberOfPlayers; i++)
                {
                    players.Add(new Player(i, PlayerListFunctions.getNewTraits(rd.Next(1, 4)), PlayerListFunctions.getNewStrategy()));
                }
                for (int i = 0; i < numberOfPlayers; i++)
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
                gameLength++;
                //GAMEPLAY PHASE
                GameplayPhase();
                //Removing perceivedModels that are out
                //PlayerListFunctions.RemovePerceivedModels(players);
                PlayerListFunctions.RemoveDeadPlayers(players);
                Console.WriteLine("Remaining players: " + players.Count);
                //DELIBERATION PHASE
                if (players.Count > 1)
                {
                    DeliberationPhase();
                }
                //Decay each players perceived player models
                foreach(Player p in players)
                {
                    p.Decay();
                }
                //Debug.Log(players);
                


            }
            Console.WriteLine("Player " + players[0].playerID + " wins!");
            foreach(string t in players[0].traits)
            {
                Console.WriteLine(t);
            }
            Console.WriteLine("Model:");
            Console.WriteLine(players[0].playerModel.trust);
            Console.WriteLine(players[0].playerModel.deceitfulness);
            Console.WriteLine(players[0].playerModel.deceitAbility);
            Console.WriteLine("------");
            deadPlayers.Add(players[0]);
            Results.DisplayResults(deadPlayers);
            Console.WriteLine("------");
            Console.WriteLine("Length of Game: " + gameLength + " rounds");
        }

        static void DeliberationPhase()
        {
            List<Argument> arguments = new List<Argument>();
            foreach (Player p in players)
            {
                arguments.Add(p.GetArgument(p, players)); //Fetch arguments and add them to list of arguments.
            }
            foreach(Argument a in arguments) //Loop through all the arguments and resolve them (add to ppms).
            {
                PlayerListFunctions.ResolveArguments(a, players);
            }
        }
        static void GameplayPhase()
        {
            foreach (Player p in players) //Loop through each player for their turn
            {
                if (p.health > 0) //Check the player isnt out
                {
                    Random rd = new Random();
                    int score = players.Count;
                    bool intentionToDeceive = p.GetIntention(players.Count);
                    //Get the target ID with the highest perceived threat
                    int targetID;
                    if (intentionToDeceive)
                    {
                        targetID = p.GetHighestThreat(players);
                    }
                    else
                    {
                        targetID = p.GetHighestTrust(players);
                    }
                    //Fetch that player object as target
                    Player target = PlayerListFunctions.GetPlayerByID(targetID, players);
                    //Get the card the player is gonna say
                    int card = p.GetCard();
                    //Get the statement about the card (truth or lie)
                    string statement = p.GenerateStatement(card, intentionToDeceive);
                    //Target makes guess about card
                    int guess = target.GuessCard(statement, p);
                    if (guess == card) //If correct, both gain 1 health and add 1 trust to the model.
                    {
                        p.health++;
                        target.health++;
                        target.GetPerceivedPlayerModel(p).AddTrust(target.playerModel, 4);
                    }
                    else //If incorrect
                    {
                        //Lose health based on difference
                        int diff = Math.Abs(card - guess);
                        target.health -= diff;
                        //If out of game, assign score
                        if (target.health <= 0)
                        {
                            target.score = score;
                            score--;
                        }
                        //If the opponent DID NOT lie
                        if (guess == Convert.ToInt32(statement))
                        {
                            target.GetPerceivedPlayerModel(p).AddTrust(target.playerModel, 4);
                            foreach (Player p2 in players)
                            {
                                if (p2.playerID != p.playerID)
                                {
                                    p2.GetPerceivedPlayerModel(p).AddTrust(p2.playerModel, 2);
                                }
                            }

                        }
                        else //If opponent DID lie
                        {
                            target.GetPerceivedPlayerModel(p).AddTrust(target.playerModel, diff * -1);
                            foreach (Player p2 in players)
                            {
                                if (p2.playerID != p.playerID)
                                {
                                    p2.GetPerceivedPlayerModel(p).AddTrust(p2.playerModel, -1);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
