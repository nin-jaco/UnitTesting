using System;
using System.Collections.Generic;
using System.Linq;

namespace TestNinja.Fundamentals
{
    public class FizzBuzz
    {
        public static string GetOutput(int number)
        {
            if ((number % 3 == 0) && (number % 5 == 0))
                return "FizzBuzz";

            if (number % 3 == 0)
                return "Fizz";

            if (number % 5 == 0)
                return "Buzz";

            return number.ToString(); 
        }

        private const int _initialCharge = 50;
    private const int _max = 100;
    private const int _min = 0;
        
        public static int getBattery(List<int> events)
        {
            int charge = _initialCharge;
            for (int i = 0; i < events.Count; i++)
            {
                charge = charge + events[i];
                if (charge < _min)
                {
                    charge = 0;
                }

                if (charge > _max)
                {
                    charge = 100;
                }
            }

            return charge;

            /*
             * var useList = events.Where(p => p < 0).ToList();
        var chargeList = events.Where(p => p >= 0).ToList();
        var max = new List<int>() {useList.Count, chargeList.Count}.Max();
        int charge = _initialCharge;
             
        for (int i = 0; i < max; i++)
        {
            charge = charge + chargeList[i] != null ? chargeList[i] :0 - useList[i] != null ? useList[i] : 0;
            if (charge < _min)
            {
                charge = 0;
            }

            if (charge > _max)
            {
                charge = 100;
            }
            
        }

        return charge;
        */
        }
        
        public static int longestSubarray(List<int> arr)
        {
            var max = arr.Max(p => p);
            var subArray = arr.Where(p => p % max < 1);
            return subArray.Count();
        }
    }
}