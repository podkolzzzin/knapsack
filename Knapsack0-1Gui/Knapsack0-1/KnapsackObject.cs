using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Knapsack0_1
{
    public class KnapsackObject:IComparable<KnapsackObject>
    {
        public int Weight, Price;
        public string Name;
        public string SName { get { return Name; } }
        public string SWeight { get { return Weight.ToString()+" kg";} }
        public string SPrice { get { return Price.ToString() + " $"; } }
        public int CompareTo(KnapsackObject obj)
        {
            double a=(double)Price / Weight;
            double b=(double)obj.Price / obj.Weight;
            if (a > b)
                return -1;
            else if (a < b)
                return 1;
            else
                return 0;
        }
    }
}
