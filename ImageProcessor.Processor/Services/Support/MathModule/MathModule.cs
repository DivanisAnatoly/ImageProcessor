using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ImageProcessor.Processor.Services.Support.MathModule
{
    internal class MathModule
    {
        internal static double DistanceToLine(Point from, Point to, Point point)
        {
            int dx = to.X - from.X;
            int dy = to.Y - from.Y;
            int x1 = from.X;
            int y1 = from.Y;
            int x = point.X;
            int y = point.Y;

            double numerator = Math.Abs((dx * y) - (dy * x) + ((dy * x1) - (dx * y1)));
            double denominator = Math.Sqrt((dx * dx) + (dy * dy));

            return numerator / denominator;
        }

    }


    static public class Qsort
    {
        public static IEnumerable<T> QuickSort<T>(this IEnumerable<T> list) where T : IComparable<T>
        {
            if (!list.Any())
            {
                return Enumerable.Empty<T>();
            }
            var pivot = list.First();
            var smaller = list.Skip(1).Where(item => item.CompareTo(pivot) <= 0).QuickSort();
            var larger = list.Skip(1).Where(item => item.CompareTo(pivot) > 0).QuickSort();

            return smaller.Concat(new[] { pivot }).Concat(larger);
        }
    }


    
}
