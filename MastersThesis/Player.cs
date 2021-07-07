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
        public List<PerceivedPlayerModel> perceivedPlayerModels = new List<PerceivedPlayerModel>();
        public List<String> traits = new List<string>();
        public List<String> strategies = new List<string>();
        public Player(int id, List<string> new_traits, List<string> new_strategies)
        {
            health = 100;
            playerID = id;
            traits.AddRange(new_traits);
            strategies.AddRange(new_strategies);
            playerModel = new PlayerModel(this);
        }

        public void AddPerceivedPlayerModels(List<Player> players)
        {
            foreach(Player p in players)
            {
                if (p != this)
                {
                    perceivedPlayerModels.Add(new PerceivedPlayerModel(p.playerID));
                }
            }
        }
        
        public PerceivedPlayerModel GetPerceivedPlayerModel(Player p)
        {
            foreach(PerceivedPlayerModel ppm in perceivedPlayerModels)
            {
                if (p.playerID == ppm.playerID)
                {
                    return ppm;
                }
            }
            return null;
        }

        public bool GetIntention()
        {
            Random rd = new Random();
            int intention = rd.Next(0, Convert.ToInt32(playerModel.trust + playerModel.deceitfulness));
            if (intention <= playerModel.trust)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int GetHighestTrust(List<Player> players)
        {
            double highest = -9999999;
            int highest_player = -1;
            foreach (PerceivedPlayerModel ppm in perceivedPlayerModels)
            {

                if (PlayerListFunctions.GetPlayerByID(ppm.playerID, players).health >= 1)
                {
                    double trust = ppm.GetTrust(players);
                    if (trust> highest)
                    {
                        highest = trust;
                        highest_player = ppm.playerID;
                    }
                }
            }
            return highest_player;
        }

        //Get the player with the highest threat that is still alive.
        public int GetHighestThreat(List<Player> players)
        {
            double highest = -9999999;
            int highest_player = -1;
            foreach (PerceivedPlayerModel ppm in perceivedPlayerModels)
            {
                
                if (PlayerListFunctions.GetPlayerByID(ppm.playerID, players).health >= 1)
                {
                    double threat = ppm.GetThreat(players);
                    if (threat > highest)
                    {
                        highest = threat;
                        highest_player = ppm.playerID;
                    }
                }
            }
            return highest_player;
        }

        //Get the card that you're going to say.
        public int GetCard()
        {
            Random rd = new Random();
            return rd.Next(0, 9);
        }

        //Generate a statement based on how deceitful you are, and by how much.
        public string GenerateStatement(int card, bool intentToDeceive)
        {
            int new_card = card;
            Random rd = new Random();
            if (intentToDeceive) //Check how deceitful this player is
            {
                //I am going to deceive
                //Find out by HOW MUCH by
                //Convert deceitAbility / 10, rounded. E.g deceitAbility 68 => 6.8 => 7 => random number between -7 and 7
                int bound = Convert.ToInt32(Math.Round(playerModel.deceitAbility / 10));
                //Fetch new deceit card
                new_card += rd.Next(-1 * bound, bound);
                if (new_card > 9) { new_card = 9; } //If greater than 9, set to 9
                else if (new_card < 1) { new_card = 1; } //If <1 set to 1
                return (new_card).ToString();
            }
            else
            {
                //I am going to tell the truth
                return new_card.ToString();
            }
        }

        //Guess the card based on the statement made by the opponent, along with their perceived model.
        public int GuessCard(string statement, Player p)
        {
            Random rd = new Random();
            //Fetch perceivedDeceit of opponent
            PerceivedPlayerModel ppm = GetPerceivedPlayerModel(p);
            double pD = ppm.perceivedDeceitfulness;
            if (rd.Next(0, 100) <= pD)
            {
                //Console.WriteLine("Target believes they are being deceived");
                //I believe them to be deceiving me
                //bound is how much they are deceiving me by
                double pDA = ppm.perceivedDeceitAbility;
                int bound = Convert.ToInt32(Math.Round(pDA / 10));
                int guessed_card = Convert.ToInt32(statement);
                while (guessed_card == Convert.ToInt32(statement)) //If deceived, dont guess card made in statement.
                {
                    guessed_card = rd.Next(-1 * bound, bound);
                    if (guessed_card > 9) { guessed_card = 9; }
                    else if (guessed_card < 1) { guessed_card = 1; }
                }
                return guessed_card;
            }
            //I think they are telling the truth
            //Console.WriteLine("Target believes they are telling the truth");
            return Convert.ToInt32(statement);
        }

        public Argument GetArgument(Player p, List<Player> players)
        {
            Player target = PlayerListFunctions.GetPlayerByID(p.GetHighestThreat(players), players);
            Random rd = new Random();
            //Get statement
            string[] statements = { "Deceitful", "NotDeceitful", "Aggressive", "NotAggressive", "Trustful", "NotTrustful" };
            Argument a = new Argument(statements[rd.Next(statements.Length)], p, target);
            return a;
        }

        //Decay is the natural tendency over time to 'forgive' players for their transgressions.
        //Perceived player models tend towards 50 (the middle) over time.
        public void Decay()
        {
            foreach(PerceivedPlayerModel ppm in perceivedPlayerModels)
            {
                //Decay for perceivedTrustfulness
                if (ppm.perceivedTrustfullness > 50) { ppm.perceivedTrustfullness-= 0.1; }
                else if (ppm.perceivedTrustfullness < 50) { ppm.perceivedTrustfullness+= 0.1; }
                //Decay for perceivedDeceit
                if (ppm.perceivedDeceitfulness > 50) { ppm.perceivedDeceitfulness-= 0.1; }
                else if (ppm.perceivedDeceitfulness < 50) { ppm.perceivedDeceitfulness+=0.1; }
                //Decay for perceivedDeceitAbility
                if (ppm.perceivedDeceitAbility > 50) { ppm.perceivedDeceitAbility-= 0.1 ; }
                else if (ppm.perceivedDeceitAbility < 50) { ppm.perceivedDeceitAbility+=0.1; }

            }
        }
    }
}
