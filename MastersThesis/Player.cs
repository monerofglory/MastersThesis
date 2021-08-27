using System;
using System.Collections.Generic;

namespace MastersThesis
{
    class Player
    {
        public int playerID;
        public int health;
        public PlayerModel playerModel;
        public List<PerceivedPlayerModel> perceivedPlayerModels = new List<PerceivedPlayerModel>();
        public List<String> traits = new List<string>();
        public int score = 0;
        public int roundsSurvived = 0;

        private Random rd = new Random();

        //Player constructor
        public Player(int id, List<string> new_traits)
        {
            health = 20;
            playerID = id;
            traits.AddRange(new_traits);
            playerModel = new PlayerModel(this);
        }

        //Add perceived model of each player
        public void AddPerceivedPlayerModels(List<Player> players)
        {
            foreach (Player p in players)
            {
                if (p != this) //Make sure a player doesnt add themselves
                {
                    perceivedPlayerModels.Add(new PerceivedPlayerModel(p.playerID));
                }
            }
        }

        //Fetch a perceived player model of a particular player P
        public PerceivedPlayerModel GetPerceivedPlayerModel(Player p)
        {
            return perceivedPlayerModels.Find(o => o.playerID == p.playerID);
        }

        //Get the intention
        //Intention is whether a player is going to deceive or act truthfully.
        public bool GetIntention(int consecNoChanges, int remainingPlayers)
        {
            if (remainingPlayers == 2) { return true; } //Always deceive if two players remain.
            int intention = rd.Next(0, Convert.ToInt32(playerModel.trust + playerModel.deceitfulness + consecNoChanges));
            if (intention <= playerModel.trust)
            {
                return false; //Do NOT deceive
            }
            else
            {
                return true; //Deceive
            }
        }

        //Get the player out of all perceived models whom you trust the most
        public int GetHighestTrust(List<Player> players)
        {
            double highest = -9999999;
            int highest_player = -1;
            foreach (PerceivedPlayerModel ppm in perceivedPlayerModels)
            {

                if (PlayerListFunctions.GetPlayerByID(ppm.playerID, players).health >= 1) //Make sure the player is still alive.
                {
                    double trust = ppm.GetTrust(players); //Get the trust calculation
                    if (trust > highest)
                    {
                        highest = trust;
                        highest_player = ppm.playerID;
                    }
                }
            }
            return highest_player; //Return highest trust.
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
            return rd.Next(0, 9);
        }

        //Generate a statement based on how deceitful you are, and by how much.
        public string GenerateStatement(int card, bool intentToDeceive)
        {
            int new_card = card;
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
            //Fetch perceivedDeceit of opponent
            PerceivedPlayerModel ppm = GetPerceivedPlayerModel(p);
            double upperBound = ppm.perceivedDeceitfulness + p.playerModel.trust;
            double lowerBound = ppm.perceivedDeceitfulness - p.playerModel.trust;
            if (rd.Next(0, Convert.ToInt32(upperBound)) <= lowerBound)
            {
                //I believe them to be deceiving me
                //bound is how much they are deceiving me by
                int bound = Convert.ToInt32(Math.Round(ppm.perceivedDeceitAbility / 10));
                int guessed_card = Convert.ToInt32(statement);
                while (guessed_card == Convert.ToInt32(statement)) //If deceived, dont guess card made in statement.
                {
                    int skew = rd.Next(-1 * bound, bound);
                    guessed_card = guessed_card + skew;
                    //Check to break infinite loop]
                    if ((bound == 1) && (guessed_card == 1))
                    {
                        return 2;
                    }
                    if (bound == 0) { return guessed_card; }
                    if (guessed_card > 9) { guessed_card = 9; }
                    else if (guessed_card < 1) { guessed_card = 1; }
                }
                return guessed_card;
            }
            //I think they are telling the truth
            return Convert.ToInt32(statement);
        }

        public Argument GetArgument(Player p, List<Player> players)
        {
            bool intention = p.GetIntention(Game.consecNoChanges, players.Count);
            Player target = PlayerListFunctions.GetPlayerByID(p.GetHighestThreat(players), players);
            //Get statement
            string[] good_statements = { "NotDeceitful", "NotAggressive", "Trustful" };
            string[] bad_statements = { "Deceitful", "Aggressive", "NotTrustful" };
            Argument a;
            if (intention)
            {
                a = new Argument(good_statements[rd.Next(good_statements.Length)], p, target);
            }
            else
            {
                a = new Argument(bad_statements[rd.Next(bad_statements.Length)], p, target);
            }
            return a;
        }

        //Decay is the natural tendency over time to 'forgive' players for their transgressions.
        //Perceived player models tend towards 50 (the middle) over time.
        public void Decay()
        {
            foreach (PerceivedPlayerModel ppm in perceivedPlayerModels)
            {
                //Decay for perceivedTrustfulness
                if (ppm.perceivedTrustfullness > 50) { ppm.perceivedTrustfullness -= Options.decayAmount; }
                else if (ppm.perceivedTrustfullness < 50) { ppm.perceivedTrustfullness += Options.decayAmount; }
                //Decay for perceivedDeceit
                if (ppm.perceivedDeceitfulness > 50) { ppm.perceivedDeceitfulness -= Options.decayAmount; }
                else if (ppm.perceivedDeceitfulness < 50) { ppm.perceivedDeceitfulness += Options.decayAmount; }
                //Decay for perceivedDeceitAbility
                if (ppm.perceivedDeceitAbility > 50) { ppm.perceivedDeceitAbility -= Options.decayAmount; }
                else if (ppm.perceivedDeceitAbility < 50) { ppm.perceivedDeceitAbility += Options.decayAmount; }
            }
        }
    }
}
