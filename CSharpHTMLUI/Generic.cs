using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
