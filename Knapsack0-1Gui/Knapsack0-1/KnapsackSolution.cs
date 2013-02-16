using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Knapsack0_1
{
    public class KnapsackSolution
    {
        private ulong _mask;
        private List<KnapsackObject> _objects;
        private int _totalPrice, _totalWeight;
        private int _prev = 0;
        private int _numObjects;
        private int _maxWeight;
        public int TotalPrice { get { return _totalPrice; } }
        public int TotalWeight { get { return _totalWeight; } }
        public int NumObjects { get { return _numObjects; } }
        public int MaxWeight { get { return _maxWeight; } }
        public KnapsackSolution(ulong mask, ItemCollection objects, int maxWeight)
        {
            _mask = mask;
            List<KnapsackObject> temp = new List<KnapsackObject>();
            for (int i = 0; i < objects.Count; i++)
            {
                temp.Add((KnapsackObject)objects[i]);
            }
            _objects = temp;
            int one = 1;
            _maxWeight = maxWeight;
            for (int i = 0; i < 64; i++)
            {
                if (((ulong)(one << i) & _mask) > 0 && _objects.Count > i)
                {
                    _totalPrice += ((KnapsackObject)objects[i]).Price;
                    _totalWeight += ((KnapsackObject)objects[i]).Weight;
                    _numObjects++;
                }
            }
        }
        public KnapsackSolution(ulong mask, List<KnapsackObject> objects, int maxWeight)
        {
            _mask = mask;
            _objects = objects;
            int one = 1;
            _maxWeight = maxWeight;
            for (int i = 0; i < 64; i++)
            {
                if (((ulong)(one << i) & _mask) > 0 && _objects.Count > i)
                {
                    _totalPrice += objects[i].Price;
                    _totalWeight += objects[i].Weight;
                    _numObjects++;
                }
            }
        }
        public KnapsackObject GetFirst()
        {
            _prev = 0;
            return GetNextObject();
        }
        public KnapsackObject GetNextObject()
        {
            for (int i = _prev; i < _objects.Count; i++)
            {
                if (((ulong)(1 << i) & _mask) > 0)
                {
                    _prev = i + 1;
                    return (KnapsackObject)_objects[i];
                }
            }
            return null;
        }
    }
}
