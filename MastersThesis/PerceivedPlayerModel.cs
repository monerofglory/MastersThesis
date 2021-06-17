using System;
using System.Collections.Generic;
using System.Text;

namespace MastersThesis
{
    class PerceivedPlayerModel
    {
        public int playerID;
        public double trust = 50;
        public double perceivedTrustfullness = 50;
        public double perceivedDeceitfulness = 50;

        public PerceivedPlayerModel(int id)
        {
            playerID = id;
        }
        public double GetThreat()
        {
            double threat = perceivedDeceitfulness + perceivedTrustfullness;
            return threat;
        }

        public void AddTrust(PlayerModel pm, int amt)
        {
            perceivedTrustfullness += amt * (1 + pm.trust);
        }
    }
}
