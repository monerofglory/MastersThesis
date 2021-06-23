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
