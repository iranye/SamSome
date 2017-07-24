using System;
using System.Collections.Generic;

namespace GenericsInTwoParts
{
    class Program
    {
        static void Main(string[] args)
        {
            Part2();
            RunJobRunner();
        }

        private static void RunJobRunner()
        {
            var jobRunner = new JobRunner<WriteToConsoleJob>();
            var highPriorityJob = new WriteToConsoleJob(1000, "high priority job");
            var lowPriorityJob = new WriteToConsoleJob(50, "low priority job");
            var mediumPriorityJob = new WriteToConsoleJob(500, "medium priority job");

            jobRunner.AddJob((lowPriorityJob));
            jobRunner.AddJob((highPriorityJob));
            jobRunner.AddJob((mediumPriorityJob));

            jobRunner.ExecuteJobs();
        }

        private static void Part2()
        {
            Console.WriteLine("GenericDemo<string>.GetBiggerValue(\"b\", \"a\"): {0}", GenericDemoComparable<string>.GetBiggerValue("b", "a"));
            Console.WriteLine("GenericDemo<string>.GetBiggerValue(123, 456): {0}", GenericDemoComparable<int>.GetBiggerValue(123, 456));
            var compiles = GenericDemoParameterlessConstructor<HasParameterlessConstructor>.CreateNewInstance();
            var alsoCompiles = GenericDemoParameterlessConstructor<List<int>>.CreateNewInstance();
            // var doesNotCompile = GenericDemoParameterlessConstructor<DoesNotHaveParameterlessConstructor>.CreateNewInstance();
        }
    }
}

public class GenericDemoComparable<T> where T : IComparable<T>
{
    public static T GetBiggerValue(T value1, T value2)
    {
        if (value1.CompareTo(value2) >= 0) return value1;
        return value2;
    }
}

public class GenericDemoParameterlessConstructor<T> where T : new()
{
    public static T CreateNewInstance()
    {
        var newInstance = new T();
        return newInstance;
    }
}

public class HasParameterlessConstructor
{
    public HasParameterlessConstructor()
    {}
}

public class DoesNotHaveParameterlessConstructor
{
    public DoesNotHaveParameterlessConstructor(int param1, int param2)
    {}
}