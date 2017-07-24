using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericsInTwoParts
{
    public interface IJob
    {
        int GetPriority();
        void Execute();
    }

    public class JobRunner<T> where T : IJob, IComparable<T>
    {
        private List<T> _jobList = new List<T>();

        private class JobComparer : IComparer<T>
        {
            public int Compare(T x, T y)
            {
                return x.CompareTo(y);
            }
        }

        public void AddJob(T job)
        {
            _jobList.Add(job);
        }

        public void ExecuteJobs()
        {
            var sortedByPriority = _jobList.OrderByDescending(x => x, new JobComparer());
            foreach (var job in sortedByPriority)
            {
                job.Execute();
            }
        }
    }

    public class WriteToConsoleJob : IJob, IComparable<IJob>
    {
        public WriteToConsoleJob(int priority, string toWrite)
        {
            Priority = priority;
            ToWrite = toWrite;
        }

        public int Priority { get; private set; }
        public string ToWrite { get; private set; }

        public int GetPriority()
        {
            return Priority;
        }

        public void Execute()
        {
            Console.WriteLine(ToWrite);
        }

        public int CompareTo(IJob other)
        {
            return this.GetPriority().CompareTo(other.GetPriority());
        }
    }
}
