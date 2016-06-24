using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

class Program
{
    static void Main(String[] args)
    {
        Console.Write("Enter number of numbers you're entering: ");
        int n = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Reading {0} numbers", n);
        string[] arr_temp = Console.ReadLine().Split(' ');

        int[] arr = Array.ConvertAll(arr_temp, Int32.Parse);
        //foreach (var el in arr)
        //{
        //    Console.Write("{0} ", el);
        //}
    }
}
