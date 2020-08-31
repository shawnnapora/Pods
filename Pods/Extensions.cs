using System;
using System.Collections.Generic;

namespace Pods
{
    public static class Extensions
    {
        public static List<T> Clone<T>(this List<T> toClone)
        {
            var clone = new List<T>();
            foreach (var item in toClone)
            {
                if (item is ICloneable itemToClone)
                {
                    clone.Add((T)itemToClone.Clone());
                }
                else
                {
                    clone.Add(item);
                }
            }

            return clone;
        }
    }
}