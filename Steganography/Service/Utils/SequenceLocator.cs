using System.Numerics;

namespace Steganography.Service.Utils;

public static class SequenceLocator
{
    /// <summary>
    /// Attempts to locate a sequence in data source
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dataSource"></param>
    /// <param name="sequence"></param>
    /// <returns>
    /// Returns the sequence start index. 
    /// If sequence is not found, or is passed incorrectly, returns null.
    /// </returns>
    public static int? LocateSequence<T>(T[] dataSource, T[] sequence) where T : IEqualityOperators<T,T,bool>
    {
        if(sequence.Length==0) return null;

        if(!dataSource.Contains(sequence[0])) return null;

        int startingIndex = Array.IndexOf(dataSource, sequence[0]);

        int maxFirstCharSlot = dataSource.Length - sequence.Length + 1;

        for (int i = startingIndex; i < maxFirstCharSlot; i++)
        {
            if (dataSource[i] != sequence[0]) // compare only first element
                continue;
            
            // found a match on first element, now try to match rest of the pattern
            for (int j = sequence.Length - 1; j >= 1; j--) 
            {
                if (dataSource[i + j] != sequence[j]) break;
                if (j == 1) return i;
            }
        }

        return null;
    }
}