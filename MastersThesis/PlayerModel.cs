using System;
using System.Collections.Generic;
using System.Text;

namespace MastersThesis
{
    class PlayerModel
    {
        //Baseline variables are 50
        public double deceitfulness = 50;
        public double deceitAbility = 50;
        public double trust = 50;
        
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

        private double TrustModifiers(Player p)
        {
            double trustModifier = 0;
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


        private double DeceitfulnessModifiers(Player p)
        {
            double deceitfulnessModifier = 0;
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
            if (p.traits.Contains("Fair"))
            {
                deceitfulnessModifier -= 10;
            }
            return deceitfulnessModifier;
        }

        private double DeceitAbilityModifiers(Player p)
        {
            double deceitAbilityModifier = 0;
            if (p.traits.Contains("Aggressive"))
            {
                deceitAbilityModifier += 20;
            }
            if (p.traits.Contains("Passive"))
            {
                deceitAbilityModifier -= 20;
            }
            if (p.traits.Contains("Audacious"))
            {
                deceitAbilityModifier += 10;
            }
            if (p.traits.Contains("Timid"))
            {
                deceitAbilityModifier -= 10;
            }
            return deceitAbilityModifier;
        }
    }
}
