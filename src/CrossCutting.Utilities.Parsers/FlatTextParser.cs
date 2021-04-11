using System;
using System.Collections.Generic;
using System.Text;

namespace CrossCutting.Utilities.Parsers
{
    public static class FlatTextParser
    {
        public static string[] Parse(string instance, char delimiter, char? textQualifier = null)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var result = new List<string>();
            var currentSection = new StringBuilder();
            var inText = false;
            foreach (var character in instance)
            {
                if (character == delimiter && !inText)
                {
                    result.Add(currentSection.ToString());
                    currentSection.Clear();
                }
                else if (textQualifier != null && character == textQualifier)
                {
                    // skip this character
                    inText = !inText;
                }
                else
                {
                    currentSection.Append(character);
                }
            }

            if (currentSection.Length > 0)
            {
                result.Add(currentSection.ToString());
            }

            return result.ToArray();
        }
    }
}
