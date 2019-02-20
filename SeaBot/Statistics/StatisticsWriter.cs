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
namespace SeaBotCore.Statistics
{
    #region

    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using SeaBotCore.Logger;
    using SeaBotCore.Utils;

    #endregion

    public static class StatisticsWriter
    {
        private const int MinuteInterval = 20;

        private static readonly Thread _thread = new Thread(ThreadLoop) { IsBackground = true };

        private static readonly string statfolder = "stats";

        internal static void Start()
        {
            if (!_thread.IsAlive)
            {
                _thread.Start();
            }
        }

        internal static void Stop()
        {
            new Task(
                () =>
                    {
                        ThreadKill.KillTheThread(_thread); // todo fix
                    }).Start();
        }

        private static void LogStatistics()
        {
            try
            {
                if (!Directory.Exists(statfolder))
                {
                    Directory.CreateDirectory(statfolder);
                }

                File.WriteAllText(
                    statfolder + "/" + DateTime.Now.ToString("yyyyMMddTHHmmss"),
                    JsonConvert.SerializeObject(Core.LocalPlayer));
                Logger.Debug("Saved a new stat");
            }
            catch (Exception)
            {
                // Ignored
            }
        }

        private static void ThreadLoop()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (Core.LocalPlayer != null)
                {
                    LogStatistics();
                    Thread.Sleep(MinuteInterval * 60 * 1000);
                }
            }
        }
    }
}