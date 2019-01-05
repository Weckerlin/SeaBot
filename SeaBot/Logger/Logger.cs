// SeaBotCore
// Copyright (C) 2018 - 2019 Weespin
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;

namespace SeaBotCore.Logger
{
    public static class Logger
    {
        private const string FILE_EXT = ".log";
        private static readonly string datetimeFormat;
        private static readonly string logFilename;

        /// <summary>
        /// Initiate an instance of SimpleLogger class constructor.
        /// If log file does not exist, it will be created automatically.
        /// </summary>
        static Logger()
        {
            datetimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
            logFilename = DateTime.Now.ToString(@"yyyy-MM-dd HH-mm-ss") + FILE_EXT;

            // Log file header line
            var logHeader = logFilename + " is created.";
            if (!Directory.Exists("logs"))
            {
                Directory.CreateDirectory("logs");
            }

            if (!File.Exists("logs/" + logFilename))
            {
                WriteLine(DateTime.Now.ToString(datetimeFormat) + " " + logHeader, false);
            }
        }

        public static bool Muted = false;

        /// <summary>
        /// Log a DEBUG message
        /// </summary>
        /// <param name="text">Message</param>
        public static void Debug(string text)
        {
            WriteFormattedLog(LogLevel.DEBUG, text);
        }

        /// <summary>
        /// Log an ERROR message
        /// </summary>
        /// <param name="text">Message</param>
        public static void Error(string text)
        {
            WriteFormattedLog(LogLevel.ERROR, text);
        }

        /// <summary>
        /// Log a FATAL ERROR message
        /// </summary>
        /// <param name="text">Message</param>
        public static void Fatal(string text)
        {
            WriteFormattedLog(LogLevel.FATAL, text);
        }

        /// <summary>
        /// Log an INFO message
        /// </summary>
        /// <param name="text">Message</param>
        public static void Info(string text)
        {
            WriteFormattedLog(LogLevel.INFO, text);
        }

        /// <summary>
        /// Log a TRACE message
        /// </summary>
        /// <param name="text">Message</param>
        public static void Trace(string text)
        {
            WriteFormattedLog(LogLevel.TRACE, text);
        }

        /// <summary>
        /// Log a WARNING message
        /// </summary>
        /// <param name="text">Message</param>
        public static void Warning(string text)
        {
            WriteFormattedLog(LogLevel.WARNING, text);
        }

        private static void WriteLine(string text, bool append = true)
        {
            try
            {
                using (var writer = new StreamWriter("logs/" + logFilename, append, Encoding.UTF8))
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        writer.WriteLine(text);
                    }
                }
            }
            catch
            {
            }
        }


        private static void WriteFormattedLog(LogLevel level, string text)
        {
            if (Muted)
            {
                return;
            }

            var Message = new Message();
            string pretext;
            var onlylog = false;
            switch (level)
            {
                case LogLevel.NETWORK:
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [NTWRK]   ";
                    WriteLine(pretext + text);
                    break;
                case LogLevel.TRACE:
                    Message.color = Color.White;
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [TRACE]   ";
                    break;
                case LogLevel.INFO:
                    Message.color = Color.GreenYellow;
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [INFO]    ";
                    break;
                case LogLevel.DEBUG:
                    if (!Core.Debug)
                    {
                        onlylog = true;
                    }

                    Message.color = Color.Cyan;
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [DEBUG]   ";
                    break;
                case LogLevel.WARNING:
                    Message.color = Color.Yellow;
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [WARNING] ";
                    break;
                case LogLevel.ERROR:
                    Message.color = Color.Red;
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [ERROR]   ";
                    break;
                case LogLevel.FATAL:
                    Message.color = Color.DarkRed;
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [FATAL]   ";
                    break;
                default:
                    pretext = "";
                    break;
            }


            WriteLine(pretext + text);
            if (!onlylog)
            {
                Message.message = pretext + text;
                Event.LogMessageChat.Invoke(Message);
            }
        }

        public static class Event
        {
            public delegate void LogMessageHandler(Message e);

            public static class LogMessageChat
            {
                public static event LogMessageHandler OnLogMessage;

                public static void Invoke(Message e)
                {
                    OnChat(e);
                }

                private static void OnChat(Message e)
                {
                    OnLogMessage?.Invoke(e);
                }
            }
        }

        public class Message
        {
            public string message;
            public Color color;
        }

        [Flags]
        private enum LogLevel
        {
            NETWORK,
            TRACE,
            INFO,
            DEBUG,
            WARNING,
            ERROR,
            FATAL
        }
    }
}