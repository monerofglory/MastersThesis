using System;
using System.Collections.Generic;
using System.Linq;

namespace MastersThesis
{
    class ResultRecord
    {
        public string trait;
        public int score;
        public int amount;
        public double average;

        private List<ResultRecord> resultsList_WinnerTakesAll = new List<ResultRecord>();
        public ResultRecord(string t)
        {
            trait = t;
            score = 0;
            amount = 0;
        }
    }
    class Results
    {
        public static void DisplayResults_WinnerTakesAll(List<String> winningTraits)
        {
            Dictionary<string, double> results = new Dictionary<string, double>();
            Dictionary<string, double> adjustedResults = new Dictionary<string, double>();
            foreach (List<string> tL in PlayerListFunctions.traitsList)
            {
                foreach (string t in tL)
                {
                    results[t] = 0;
                }
            }
            foreach(String trait in winningTraits)
            {
                results[trait]++;
            }
            var sortedResults = results.OrderByDescending(o => o.Value);
            foreach(KeyValuePair<string, double> k in sortedResults)
            {
                Console.WriteLine(k.Key + " = " + k.Value);
            }
        }

        public static void DisplayResults_Survivor(List<Player> players)
        {
            List<ResultRecord> resultsList = new List<ResultRecord>();
            foreach (List<string> tL in PlayerListFunctions.traitsList)
            {
                foreach (string t in tL)
                {
                    resultsList.Add(new ResultRecord(t));
                }
            }
            foreach (ResultRecord rr in resultsList)
            {
                foreach (Player p in players)
                {
                    if (p.traits.Contains(rr.trait))
                    {
                        rr.score += p.roundsSurvived;
                        rr.amount++;
                        rr.average = (double)rr.score / (double)rr.amount;
                    }
                }
            }
            List<ResultRecord> rrL = resultsList.OrderBy(o => o.average).ToList();
            foreach (ResultRecord rr in rrL)
            {
                Console.WriteLine(rr.trait + " with average of: " + rr.average.ToString("F") + " (" + rr.score + "/" + rr.amount + ")");
            }
        }
        public static void DisplayResults_FinalPosition(List<Player> players)
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
            foreach (ResultRecord rr in resultsList)
            {
                foreach (Player p in players)
                {
                    if (p.traits.Contains(rr.trait))
                    {
                        rr.score += p.score;
                        rr.amount++;
                        rr.average = (double)rr.score / (double)rr.amount;
                    }
                }
            }
            List<ResultRecord> rrL = resultsList.OrderBy(o => o.average).ToList();
            foreach (ResultRecord rr in rrL)
            {
                Console.WriteLine(rr.trait + " with average of: " + rr.average.ToString("F") + " (" + rr.score + "/" + rr.amount + ")");
            }
        }
    }
}
