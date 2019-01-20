using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CSharpHTMLUI
{
    /// <summary>
    /// This class contains helper methods
    /// </summary>
    public class Generic
    {
        private static Random Random = new Random();

        /// <summary>
        /// The length to use for generated IDs
        /// </summary>
        public static int IDLength = 32;

        /// <summary>
        /// Returns a random string
        /// </summary>
        /// <param name="length">Defines the length of the random string</param>
        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Removes overhead and makes the html file harder to read.
        /// </summary>
        /// <param name="html">The parsed html file</param>
        /// <returns>The new html content</returns>
        public static string MinifyHTML(string html)
        {
            html = Regex.Replace(html, @"[a-zA-Z]+#", "#");
            html = Regex.Replace(html, @"[\n\r]+\s*", string.Empty);
            html = Regex.Replace(html, @"\s+", " ");
            html = Regex.Replace(html, @"\s?([:,;{}])\s?", "$1");
            html = html.Replace(";}", "}");
            html = Regex.Replace(html, @"([\s:]0)(px|pt|%|em)", "$1");

            /// Remove comments
            html = Regex.Replace(html, @"/\*[\d\D]*?\*/", string.Empty);
            return html;
        }

    }
}
