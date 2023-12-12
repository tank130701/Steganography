using System.Numerics;

namespace Steganography.Service.Utils;

public static class SequenceLocator
{
    /// <summary>
    /// Attempts to locate a sequence in data source
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data_source"></param>
    /// <param name="sequence"></param>
    /// <returns>
    /// Returns the sequence start index. 
    /// If sequence is not found, or is passed incorrectly, returns null.
    /// </returns>
    public static int? LocateSequence<T>(T[] data_source, T[] sequence) where T : IEqualityOperators<T,T,bool>
    {
        if(sequence.Length==0) return null;

        if(!data_source.Contains(sequence[0])) return null;

        int starting_index = Array.IndexOf(data_source, sequence[0]);

        int maxFirstCharSlot = data_source.Length - sequence.Length + 1;

        for (int i = starting_index; i < maxFirstCharSlot; i++)
        {
            if (data_source[i] != sequence[0]) // compare only first element
                continue;
            
            // found a match on first element, now try to match rest of the pattern
            for (int j = sequence.Length - 1; j >= 1; j--) 
            {
                if (data_source[i + j] != sequence[j]) break;
                if (j == 1) return i;
            }
        }

        return null;
    }
}