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
            for (int j = 0; j < 1; j++)
            {
                players.Clear();
                //Initialising players
                int num = 200; //Number of players
                for (int i = 0; i < num; i++)
                {
                    players.Add(new Player(i, PlayerListFunctions.getNewTrait(), PlayerListFunctions.getNewStrategy()));
                }
                for (int i = 0; i < num; i++)
                {
                    players[i].AddPerceivedPlayerModels(players);
                }
                int a = 0;
                int b = 0;
                int c = 0;
                foreach (Player p in players)
                {
                    if (p.traits.Contains("Timid"))
                    {
                        a++;
                    }
                    else if (p.traits.Contains("Aggressive"))
                    {
                        b++;
                    }
                    else if (p.traits.Contains("Calculating"))
                    {
                        c++;
                    }

                }
                Console.WriteLine(a);
                Console.WriteLine(b);
                Console.WriteLine(c);
                GameLoop();
            }
        }

        static void GameLoop()
        {
            while (players.Count > 1) //Whilst players are still in the game
            {
                //GAMEPLAY PHASE
                GameplayPhase();
                //Removing perceivedModels that are out
                PlayerListFunctions.RemovePerceivedModels(players);
                //Removing players from the players list that are out
                players.RemoveAll(item => item.health < 1);
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
            Console.WriteLine("Player " + players[0].playerID + " wins!");
            foreach(string t in players[0].traits)
            {
                Console.WriteLine(t);
            }
            Console.WriteLine("Model:");
            Console.WriteLine(players[0].playerModel.trust);
            Console.WriteLine(players[0].playerModel.deceitfulness);
            Console.WriteLine(players[0].playerModel.deceitAbility);
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
                foreach(Player p in players)
                {
                    if (a.receiver != p)
                    {
                        p.GetPerceivedPlayerModel(a.receiver).AddTrust(p.playerModel, 1);
                    }
                }
            }
        }
        static void GameplayPhase()
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
                    //Console.WriteLine("--------");
                    //Console.WriteLine("Player Turn: " + p.playerID);
                    //Console.WriteLine("Targeting " + target.playerID + " with card " + card + " , saying " + statement);
                    //Target makes guess about card
                    int guess = target.GuessCard(statement, p);
                    //Console.WriteLine("Target guesses: " + guess);
                    //Console.ReadLine();
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
                        //If the opponent DID NOT lie
                        if (guess == Convert.ToInt32(statement))
                        {
                            target.GetPerceivedPlayerModel(p).AddTrust(target.playerModel, 1);
                            foreach (Player p2 in players)
                            {
                                if (p2.playerID != p.playerID)
                                {
                                    p2.GetPerceivedPlayerModel(p).AddTrust(p2.playerModel, 0.1);
                                }
                            }

                        }
                        else //If opponent DID lie
                        {
                            target.GetPerceivedPlayerModel(p).AddTrust(target.playerModel, diff * -0.1);
                            foreach (Player p2 in players)
                            {
                                if (p2.playerID != p.playerID)
                                {
                                    p2.GetPerceivedPlayerModel(p).AddTrust(p2.playerModel, -0.1);
                                }
                            }
                        }
                    }
                    //Console.WriteLine("Player " + p.playerID + " health = " + p.health);
                    //Console.WriteLine("PLayer " + target.playerID + " health = " + target.health);
                }
            }
        }
    }
}
