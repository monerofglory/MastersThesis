using System;
using System.Collections.Generic;
using System.Text;

namespace MastersThesis
{
    class Debug
    {
        public static void Log(List<Player> players)
        {
            foreach(Player p in players)
            {
                Console.WriteLine("---");
                Console.WriteLine("ID: " + p.playerID + ", Health: " + p.health);
                Console.WriteLine("Personal Model");
                Console.WriteLine("Trust\tDeceit\tDeceitAbil");
                Console.WriteLine(NF(p.playerModel.trust) + "\t" + NF(p.playerModel.deceitfulness) + "\t" + NF(p.playerModel.deceitAbility));
                Console.WriteLine("PPMS");
                foreach(PerceivedPlayerModel ppm in p.perceivedPlayerModels)
                {
                    Console.WriteLine("PPM ID: " + ppm.playerID);
                    Console.WriteLine("Trust\tDeceit\tDeceitAbil");
                    Console.WriteLine(NF(ppm.perceivedTrustfullness) + "\t" + NF(ppm.perceivedDeceitfulness) + "\t" + NF(ppm.perceivedDeceitAbility));
                }
            }
            Console.WriteLine("###");
            Console.ReadLine();
        }


        //Function which takes a double and returns it to 2dp
        private static string NF(double d)
        {
            return String.Format("{0:.##}", d);
        }
    }
}
