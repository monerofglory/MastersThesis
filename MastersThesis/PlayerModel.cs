using System;
using System.Collections.Generic;
using System.Text;

namespace MastersThesis
{
    class PlayerModel
    {
        //Baseline variables are 50
        public int deceitfulness = 50;
        public int deceitAbility = 50;
        public int trust = 50;
        
        public PlayerModel(Player p)
        {
            Random rd = new Random();
            //Random trust variable
            trust += rd.Next(-20, 20);
            trust += TrustModifiers(p);
        }

        private int TrustModifiers(Player p)
        {
            int trustModifier = 0;
            if (p.traits.Contains("Trusting"))
            {
                trust += 20;
            }
            if (p.traits.Contains("Untrusting"))
            {
                trust -= 20;
            }
            if (p.traits.Contains("Unsuspicious"))
            {
                trust += 10;
            }
            if (p.traits.Contains("Suspicious"))
            {
                trust -= 10;
            }
            return trustModifier;
        }
    }
}
