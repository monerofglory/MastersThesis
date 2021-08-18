using System;
using System.Collections.Generic;

namespace MastersThesis
{
    class PerceivedPlayerModel
    {
        public int playerID;
        public double perceivedTrustfullness = 50;
        public double perceivedDeceitfulness = 50;
        public double perceivedDeceitAbility = 50;
        //Number of times this player has acted deceitfully or kindly towards you.
        public int goodTimes = 0;
        public int badTimes = 0;

        public PerceivedPlayerModel(int id)
        {
            playerID = id;
            Random rd = new Random();
            perceivedTrustfullness += rd.Next(-10, 10);
            perceivedDeceitfulness += rd.Next(-10, 10);
            perceivedDeceitAbility += rd.Next(-10, 10);

        }
        public double GetThreat(List<Player> players)
        {
            double threat = perceivedDeceitfulness + perceivedDeceitAbility + PlayerListFunctions.GetPlayerByID(playerID, players).health + (badTimes - goodTimes);
            return threat;
        }

        public double GetTrust(List<Player> players)
        {
            double trust = perceivedTrustfullness - PlayerListFunctions.GetPlayerByID(playerID, players).health + (goodTimes - badTimes);
            return trust;
        }
        public void AddDeceitfulness(PlayerModel pm, double amt)
        {
            perceivedDeceitfulness += amt * (1 - (pm.trust / 100));
            if (perceivedDeceitfulness > 100)
            {
                perceivedDeceitfulness = 100;
            }
            if (perceivedDeceitfulness < 0)
            {
                perceivedDeceitfulness = 0;
            }
        }

        public void AddDeceitAbility(PlayerModel pm, double amt)
        {
            perceivedDeceitAbility += amt * (1 - (pm.trust / 100));
            if (perceivedDeceitAbility > 100)
            {
                perceivedDeceitAbility = 100;
            }
            if (perceivedDeceitAbility < 0)
            {
                perceivedDeceitAbility = 0;
            }
        }
        public void AddTrust(PlayerModel pm, double amt)
        {
            perceivedTrustfullness += amt * (1 + (pm.trust / 100));
            if (perceivedTrustfullness > 100)
            {
                perceivedTrustfullness = 100;
            }
            if (perceivedTrustfullness < 0)
            {
                perceivedTrustfullness = 0;
            }
        }
    }
}
