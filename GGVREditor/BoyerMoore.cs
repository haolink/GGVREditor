using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace GGVREditor
{
    static class BoyerMoore
    {
        public static List<long> IndexesOf(this byte[] value, byte[] pattern)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (pattern == null)
                throw new ArgumentNullException("pattern");

            long valueLength = value.LongLength;
            long patternLength = pattern.LongLength;

            if ((valueLength == 0) || (patternLength == 0) || (patternLength > valueLength))
                return (new List<long>());

            long[] badCharacters = new long[256];

            for (long i = 0; i < 256; ++i)
                badCharacters[i] = patternLength;

            long lastPatternByte = patternLength - 1;

            for (long i = 0; i < lastPatternByte; ++i)
                badCharacters[pattern[i]] = lastPatternByte - i;

            // Beginning

            long index = 0;
            List<long> indexes = new List<long>();

            while (index <= (valueLength - patternLength))
            {
                for (long i = lastPatternByte; value[(index + i)] == pattern[i]; --i)
                {
                    if (i == 0)
                    {
                        indexes.Add(index);
                        break;
                    }
                }

                index += badCharacters[value[(index + lastPatternByte)]];
            }

            return indexes;
        }
    }
}
