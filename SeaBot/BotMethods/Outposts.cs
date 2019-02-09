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
namespace SeaBotCore.BotMethods
{
    #region

    using System.Linq;

    using SeaBotCore.Data.Definitions;

    #endregion

    public static class Outposts
    {
        public static void ConfirmOutpost()
        {
            //var readyoutposts = Core.GlobalData.Outposts.Where(n => n.RequiredCrew <= n.Crew).ToList();
            //foreach (var outpost in readyoutposts)
            //{
            //    if (outpost.CargoOnTheWay == 0)
            //    {
            //        Core.GlobalData.Outposts.FirstOrDefault(n => n.DefId == outpost.DefId).Done = true;
            //        Networking.AddTask(new Task.ConfirmOutpostTask(outpost.DefId));

            //        // unlock all
            //        var outdef = Definitions.OutpostDef.Items.Item.Where(n => n.DefId == outpost.DefId)
            //            .FirstOrDefault();
            //        foreach (var unlock in outdef.UnlockedLocations.Location)
            //        {
            //            if (unlock.Type == "contractor")
            //            {
            //                // todo: unlock all 0.9.0
            //            }
            //        }
            //    }
            //}
        }
    }
}