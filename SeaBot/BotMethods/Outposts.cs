using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBotCore.Data.Definitions;

namespace SeaBotCore.BotMethods
{
    public static class Outposts
    {
        public static void ConfirmOutpost()
        {
           var readyoutposts = Core.GlobalData.Outposts.Where(n => n.RequiredCrew <= n.Crew).ToList();
           foreach (var outpost in readyoutposts)
           {
               if (outpost.CargoOnTheWay == 0)
               {
                   Core.GlobalData.Outposts.FirstOrDefault(n => n.DefId == outpost.DefId).Done = true;
                   Networking.AddTask(new Task.ConfirmOutpostTask(outpost.DefId));
                   //unlock all
                   var outdef = Definitions.OutpostDef.Items.Item.Where(n => n.DefId == outpost.DefId).FirstOrDefault();
                   foreach (var unlock in outdef.UnlockedLocations.Location)
                   {
                       if (unlock.Type == "contractor")
                       {
                           //todo: unlock all
                       }
                   }
               }
           }
        }
    }
}
