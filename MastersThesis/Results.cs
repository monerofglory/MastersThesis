using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MastersThesis
{
    class ResultRecord
    {
        public string trait;
        public int score;
        public int amount;
        public double average;

        public ResultRecord(string t)
        {
            trait = t;
            score = 0;
            amount = 0;
        }
    }
    class Results
    {
        public static void DisplayResults2(List<Player> players)
        {
            
            //Creating the dictionaries.
            Dictionary<string, int> scoreDict = new Dictionary<string, int>();
            Dictionary<string, int> averageDict = new Dictionary<string, int>();
            //Initialising the dictionaries.
            foreach (List<string> tL in PlayerListFunctions.traitsList)
            {
                foreach (string t in tL)
                {
                    scoreDict[t] = 0;
                    averageDict[t] = 0;
                }
            }
            //Looping through the players
            foreach (List<string> tL in PlayerListFunctions.traitsList)
            {
                foreach (string t in tL)
                {
                    foreach (Player p in players)
                    {
                        if (p.traits.Contains(t))
                        {
                            scoreDict[t] += p.score;
                            averageDict[t]++;
                        }
                    }
                }
            }
            foreach (List<string> tL in PlayerListFunctions.traitsList)
            {
                foreach (string t in tL)
                {
                    if (averageDict[t] != 0)
                    {
                        Console.WriteLine(t + " has an average score of " + (scoreDict[t] / averageDict[t]) + " (" + scoreDict[t].ToString() + "/" + averageDict[t].ToString() + ")");
                    }
                }
            }
        }
        public static void DisplayResults(List<Player> players)
        {
            Console.WriteLine("Total Players: " + players.Count);
            List<ResultRecord> resultsList = new List<ResultRecord>();
            foreach (List<string> tL in PlayerListFunctions.traitsList)
            {
                foreach (string t in tL)
                {
                    resultsList.Add(new ResultRecord(t));
                }
            }
            foreach(ResultRecord rr in resultsList)
            {
                foreach(Player p in players)
                {
                    if (p.traits.Contains(rr.trait))
                    {
                        rr.score += p.score;
                        rr.amount++;
                        rr.average = rr.score / rr.amount;
                    }
                }
            }
            List<ResultRecord> rrL = resultsList.OrderBy(o => o.average).ToList();
            foreach(ResultRecord rr in rrL)
            {
                Console.WriteLine(rr.trait + " with average of: " + rr.average + " (" + rr.score + "/" + rr.amount + ")");
            }
        }
    }
}
