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
            //Initialising trust
            trust += rd.Next(-20, 20);
            trust += TrustModifiers(p);
            //Initialising deceitfulness
            deceitfulness += rd.Next(-20, 20);
            deceitfulness += DeceitfulnessModifiers(p);
            //Initialising deceitAbility
            deceitAbility += rd.Next(-20, 20);
            deceitAbility += DeceitAbilityModifiers(p);
        }

        private int TrustModifiers(Player p)
        {
            int trustModifier = 0;
            if (p.traits.Contains("Trusting"))
            {
                trustModifier += 20;
            }
            if (p.traits.Contains("Untrusting"))
            {
                trustModifier -= 20;
            }
            if (p.traits.Contains("Unsuspicious"))
            {
                trustModifier += 10;
            }
            if (p.traits.Contains("Suspicious"))
            {
                trustModifier -= 10;
            }
            return trustModifier;
        }

        private int DeceitfulnessModifiers(Player p)
        {
            int deceitfulnessModifier = 0;
            if (p.traits.Contains("Deceitful"))
            {
                deceitfulnessModifier += 20;
            }
            if (p.traits.Contains("Honest"))
            {
                deceitfulnessModifier -= 20;
            }
            if (p.traits.Contains("Calculating"))
            {
                deceitfulnessModifier += 10;
            }
            if (p.traits.Contains("Kind"))
            {
                deceitfulnessModifier -= 10;
            }
            return deceitfulnessModifier;
        }

        private int DeceitAbilityModifiers(Player p)
        {
            int deceitAbilityModifier = 0;
            if (p.traits.Contains("Cut-Throat"))
            {
                deceitAbilityModifier += 20;
            }
            if (p.traits.Contains("Virtuous"))
            {
                deceitAbilityModifier -= 20;
            }
            if (p.traits.Contains("Aggressive"))
            {
                deceitAbilityModifier += 10;
            }
            if (p.traits.Contains("Passive"))
            {
                deceitAbilityModifier -= 10;
            }
            return deceitAbilityModifier;
        }
    }
}
