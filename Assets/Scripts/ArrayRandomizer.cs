using System.Collections.Generic;
using System;

/// <summary>
/// Class used to randomize array.
/// </summary>
public static class ArrayRandomizer
{
    /// <summary>
    /// Rng variable.
    /// </summary>
    private static readonly Random rng = new Random();


    /// <summary>
    /// Method used to shuffle an array.
    /// </summary>
    /// <param name="list">The list to shuffle</param>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}