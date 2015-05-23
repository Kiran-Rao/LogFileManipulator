using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace LogFileManipulator
{
    public class RegexTasks
    {
        public static string Match(string input, string regexMatch)
        {
            StringBuilder output = new StringBuilder(input.Length + 1);
            Regex rgx;
            rgx = new Regex(regexMatch);
            StringReader reader = new StringReader(input);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (rgx.IsMatch(line))
                {
                    output.AppendLine(line);
                }
            }
            return output.ToString();
        }

        public static string FindAndReplace(string input, string regexMatch, string replacement)
        {
            Regex rgx = new Regex(regexMatch);
            return rgx.Replace(input, replacement);
        }
        public static string Reformat(string input, string regexMatch, string replacement)
        {
            StringBuilder output = new StringBuilder(input.Length + 1);
            Regex rgx = new Regex(regexMatch);
            StringReader reader = new StringReader(input);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Match m = rgx.Match(line);
                if (m.Success)
                {
                    //int i = 0;
                    string outputLine = replacement;
                    for (int i = 0; i < m.Groups.Count; i++)
                    {
                        outputLine.Replace("$" + i.ToString(), m.Groups[i].Value);
                    }
                }
                else
                {
                    output.AppendLine(line);
                }
            }
            return output.ToString();
        }
    }
}
