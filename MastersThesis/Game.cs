using System;
using System.Collections.Generic;
using System.Linq;

namespace MastersThesis
{
    class Game
    {
        public static List<Player> players = new List<Player>();
        public static List<Player> deadPlayers = new List<Player>();
        public static List<Player> allDeadPlayers = new List<Player>();
        private static Random rd = new Random();
        //Game details
        public static int gameLength = 0;
        public static int numberOfPlayers = 50;

        public static int consecNoChanges = 0;
        static void Main(string[] args)
        {
            for (int j = 0; j < 10; j++) //Loop for running a new game
            {
                //Clear variables for new game start
                gameLength = 0;
                deadPlayers.Clear();
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
                allDeadPlayers.AddRange(deadPlayers);
            }
            //Outputting results of ALL games that have happened.
            Console.WriteLine("ALL GAMES RESULTS");
            Results.DisplayResults(allDeadPlayers);
        }

        static void GameLoop()
        {
            while (players.Count > 1) //Whilst players are still in the game
            {
               
                gameLength++;
                //GAMEPLAY PHASE
                GameplayPhase();
                int deadPlayerCount = deadPlayers.Count;
                //Removing perceivedModels that are out
                PlayerListFunctions.RemoveDeadPlayers(players);
                if (deadPlayers.Count == deadPlayerCount) //If no players have been eliminated this round.
                {
                    consecNoChanges++;
                }
                else
                {
                    consecNoChanges = 0;
                }
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
            }
            //Outputting results of the game.
            deadPlayers.Add(players[0]);
            //Results.DisplayResults(deadPlayers);
            //Console.WriteLine("Length of Game: " + gameLength + " rounds");
        }

        //Deliberation phase where arguments are created and resolved.
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
            int startingPlayer = rd.Next(0, players.Count);
            int playersDone = 0;
            while (playersDone < players.Count)
            {
                Player p = players[startingPlayer];
                if (p.health > 0) //Check the player isnt out
                {
                    Random rd = new Random();
                    int score = players.Count;
                    bool intentionToDeceive = p.GetIntention(consecNoChanges, players.Count);
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
                        target.GetPerceivedPlayerModel(p).AddDeceitfulness(target.playerModel, -4);
                        foreach(Player p3 in players)
                        {
                            if (p3.playerID != p.playerID)
                            {
                                p3.GetPerceivedPlayerModel(p).AddDeceitfulness(p3.playerModel, -2);
                            }
                        }
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
                            target.GetPerceivedPlayerModel(p).goodTimes++;
                            target.GetPerceivedPlayerModel(p).AddDeceitfulness(target.playerModel, -4);
                            foreach (Player p2 in players)
                            {
                                if (p2.playerID != p.playerID)
                                {
                                    p2.GetPerceivedPlayerModel(p).AddDeceitfulness(p2.playerModel, -2);
                                }
                            }

                        }
                        else //If opponent DID lie
                        {
                            target.GetPerceivedPlayerModel(p).badTimes++;
                            target.GetPerceivedPlayerModel(p).AddDeceitfulness(target.playerModel, 4);
                            target.GetPerceivedPlayerModel(p).AddDeceitAbility(target.playerModel, diff);
                            foreach (Player p2 in players)
                            {
                                if (p2.playerID != p.playerID)
                                {
                                    p2.GetPerceivedPlayerModel(p).AddDeceitfulness(p2.playerModel, 2);
                                }
                            }
                        }
                    }
                    //If the player believed the statement
                    if (guess == Convert.ToInt32(statement))
                    {
                        foreach(Player p4 in players)
                        {
                            if (p4.playerID != p.playerID)
                            {
                                p4.GetPerceivedPlayerModel(p).AddTrust(p4.playerModel, 2);
                            }
                        }
                    }
                    else
                    {
                        foreach (Player p4 in players)
                        {
                            if (p4.playerID != p.playerID)
                            {
                                p4.GetPerceivedPlayerModel(p).AddTrust(p4.playerModel, -2);
                            }
                        }
                    }
                }
                if (startingPlayer == players.Count - 1)
                {
                    startingPlayer = 0;
                }
                else
                {
                    startingPlayer++;
                }
                playersDone++;
                
            }
        }
    }
}
