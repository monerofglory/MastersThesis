using System;

namespace MastersThesis
{
    class PlayerModel
    {
        //PlayerModel Modifier Values
        private int largeMod = Options.traitLargeModifier;
        private int smallmod = Options.traitSmallModifier;
        private int negLargeMod = Options.traitLargeModifier * -1;
        private int negSmallMod = Options.traitSmallModifier * -1;

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
                trustModifier += largeMod;
            }
            if (p.traits.Contains("Untrusting"))
            {
                trustModifier -= negLargeMod;
            }
            if (p.traits.Contains("Unsuspicious"))
            {
                trustModifier += smallmod;
            }
            if (p.traits.Contains("Suspicious"))
            {
                trustModifier -= negSmallMod;
            }
            return trustModifier;
        }


        private double DeceitfulnessModifiers(Player p)
        {
            double deceitfulnessModifier = 0;
            if (p.traits.Contains("Deceitful"))
            {
                deceitfulnessModifier += largeMod;
            }
            if (p.traits.Contains("Honest"))
            {
                deceitfulnessModifier -= negLargeMod;
            }
            if (p.traits.Contains("Calculating"))
            {
                deceitfulnessModifier += smallmod;
            }
            if (p.traits.Contains("Fair"))
            {
                deceitfulnessModifier -= negSmallMod;
            }
            return deceitfulnessModifier;
        }

        private double DeceitAbilityModifiers(Player p)
        {
            double deceitAbilityModifier = 0;
            if (p.traits.Contains("Aggressive"))
            {
                deceitAbilityModifier += largeMod;
            }
            if (p.traits.Contains("Passive"))
            {
                deceitAbilityModifier -= negLargeMod;
            }
            if (p.traits.Contains("Audacious"))
            {
                deceitAbilityModifier += smallmod;
            }
            if (p.traits.Contains("Timid"))
            {
                deceitAbilityModifier -= negSmallMod;
            }
            return deceitAbilityModifier;
        }
    }
}
