using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Controls;

namespace Knapsack0_1
{
    class Knapsack
    {
        private static string _itemSerializer(ItemCollection items, int maxWeight,int alg)
        {
            string result = alg.ToString()+"\r\n";
            result += maxWeight + "\r\n";
            result += items.Count.ToString() + "\r\n";
            foreach (KnapsackObject obj in items)
            {
                result += obj.Name + " " + obj.Weight + " " + obj.Price + "\r\n";
            }
            return result;
        }
        public static KnapsackSolution FindSolutionFullSearchV1(ItemCollection items,int maxWeight)
        {
            File.WriteAllText("cSharpBridge.txt",_itemSerializer(items,maxWeight,0));
            Process p = Process.Start("KnapsackAlgorighmProcess.exe");
            p.WaitForExit();
            ulong result = Convert.ToUInt64(File.ReadAllText("cPlusPlusBridge.txt"));
            List<KnapsackObject> temp = new List<KnapsackObject>();
            for (int i = 0; i < items.Count; i++)
            {
                if (((KnapsackObject)items[i]).Weight <= maxWeight)
                {
                    temp.Add((KnapsackObject)items[i]);
                }
            }            
            return new KnapsackSolution(result, items, maxWeight);
        }
        public static KnapsackSolution FindSolutionFullSearchV2(ItemCollection items, int maxWeight)
        {
            File.WriteAllText("cSharpBridge.txt", _itemSerializer(items, maxWeight,1));
            Process p = Process.Start("KnapsackAlgorighmProcess.exe");
            p.WaitForExit();
            ulong result = Convert.ToUInt64(File.ReadAllText("cPlusPlusBridge.txt"));
            List<KnapsackObject> temp = new List<KnapsackObject>();
            for (int i = 0; i < items.Count; i++)
            {
                if (((KnapsackObject)items[i]).Weight <= maxWeight)
                {
                    temp.Add((KnapsackObject)items[i]);
                }
            }
            temp.Sort();
            return new KnapsackSolution(result, items, maxWeight);
        }
        public static KnapsackSolution FindSolutionGreedySearch(ItemCollection items, int maxWeight)
        {
            File.WriteAllText("cSharpBridge.txt", _itemSerializer(items, maxWeight,2));
            Process p = Process.Start("KnapsackAlgorighmProcess.exe");
            p.WaitForExit();
            ulong result = Convert.ToUInt64(File.ReadAllText("cPlusPlusBridge.txt"));
            List<KnapsackObject> temp = new List<KnapsackObject>();
            for (int i = 0; i < items.Count; i++)
            {
                if (((KnapsackObject)items[i]).Weight <= maxWeight)
                {
                    temp.Add((KnapsackObject)items[i]);
                }
            }
            temp.Sort();
            return new KnapsackSolution(result, temp,maxWeight);
        }
    }
}
