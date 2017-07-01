using System;
using System.Collections.Generic;

// CSharp_in_Depth_Third_Edition
namespace CSharpBooks
{
    /// <summary>
    /// Helper class for less cluttered instantiating.
    /// </summary>
    public static class Pair
    {
        public static Pair<T1,T2> Of<T1,T2>(T1 first, T2 second)
        {
            return new Pair<T1,T2>(first, second);
        }
    }

    /// <summary>
    /// A generic type that holds two values (e.g., key/value pair) w/ no expectations on the relationship between the values.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public sealed class Pair<T1, T2> : IEquatable<Pair<T1, T2>>
    {
        private static readonly IEqualityComparer<T1> FirstComparer = EqualityComparer<T1>.Default;
        private static readonly IEqualityComparer<T2> SecondComparer = EqualityComparer<T2>.Default;

        private readonly T1 first;
        private readonly T2 second;

        public Pair(T1 first, T2 second)
        {
            this.first = first;
            this.second = second;
        }

        public T1 First { get { return first; } }
        public T2 Second { get { return second; } }

        public bool Equals(Pair<T1, T2> other)
        {
            return other != null &
                FirstComparer.Equals(this.First, other.First) &&
                SecondComparer.Equals(this.Second, other.Second);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Pair<T1, T2>);
        }

        public override int GetHashCode()
        {
            return FirstComparer.GetHashCode(first) * 37 +
                SecondComparer.GetHashCode(second);
        }
    }
}
