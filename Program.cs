using System;

namespace AVLBaum
{
    class Program
    {
        private static int compare(int value1, int value2)
        {
            return Math.Sign(value1 - value2);
        }

        static void Main(string[] args)
        {
            Func<int, int, int> comparer = compare;

            AVL<int> tree = new AVL<int>(comparer);

            tree.Add(1);
            tree.Add(2);
            tree.Add(3);
            tree.Add(4);
            tree.Add(5);
            tree.Add(6);
            tree.Add(7);
            tree.Add(8);
            tree.Add(9);
            tree.Add(10);

            if (tree.Contains(1)) Console.WriteLine(1);
            if (tree.Contains(2)) Console.WriteLine(2);
            if (tree.Contains(3)) Console.WriteLine(3);
            if (tree.Contains(4)) Console.WriteLine(4);
            if (tree.Contains(5)) Console.WriteLine(5);
            if (tree.Contains(6)) Console.WriteLine(6);
            if (tree.Contains(7)) Console.WriteLine(7);
            if (tree.Contains(8)) Console.WriteLine(8);
            if (tree.Contains(9)) Console.WriteLine(9);
            if (tree.Contains(10)) Console.WriteLine(10);

        }
    }
}
