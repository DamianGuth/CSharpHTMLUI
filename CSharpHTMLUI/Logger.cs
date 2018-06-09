using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpHTMLUI
{
    /// <summary>
    /// Logger used to print data. This class allows easy change of output methods
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Logs a message, with specified loglevel, to the console and saves it into a file. Default loglevel is "INFO"
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="logLevel">The loglevel. Default is "INFO"</param>
        public static void Log(string message, string logLevel = "INFO")
        {

            // Set the logging color
            switch (logLevel)
            {
                case "ERROR":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case "WARNING":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case "INFO":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case "HTTPINFO":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;

                case "HTTPPARAMETER":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;

                case "WEBSERVER":
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }

            string logMessage = "[" + DateTime.Now + " - " + logLevel + "] " + message;

            Console.WriteLine(logMessage);
            File.AppendAllText("log.txt", logMessage + Environment.NewLine);
        }
    }

}
