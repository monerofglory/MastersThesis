using System;
using System.Collections.Generic;
using System.Text;

namespace MastersThesis
{
    class PerceivedPlayerModel
    {
        public int playerID;
        public double perceivedTrustfullness = 50;
        public double perceivedDeceitfulness = 50;
        public double perceivedDeceitAbility = 50;

        public PerceivedPlayerModel(int id)
        {
            playerID = id;
        }
        public double GetThreat()
        {
            double threat = perceivedDeceitfulness + perceivedDeceitAbility - (perceivedTrustfullness * 2);
            return threat;
        }

        public void AddTrust(PlayerModel pm, double amt)
        {
            perceivedTrustfullness += amt * (1 + pm.trust);
        }
    }
}
