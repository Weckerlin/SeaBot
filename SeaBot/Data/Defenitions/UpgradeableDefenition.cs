// SeaBotCore
// Copyright (C) 2019 Weespin
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace SeaBotCore.Data.Defenitions
{
    public class UpgradeableDefenition

    {
        public class Root
        {
            [J("items")] public Items Items { get; set; }
        }

        public class Items
        {
            [J("item")] public List<Item> Item { get; set; }
        }

        public class Item
        {
            [J("def_id")] public long DefId { get; set; }
            [J("version_id")] public long VersionId { get; set; }
            [J("name")] public string Name { get; set; }
            [J("name_loc")] public string NameLoc { get; set; }
            [J("speedupable")] public long Speedupable { get; set; }
            [J("slots")] public long Slots { get; set; }
            [J("depletable")] public long Depletable { get; set; }
            [J("material_id")] public long MaterialId { get; set; }
            [J("max_level")] public long MaxLevel { get; set; }
            [J("gives_commodity")] public long GivesCommodity { get; set; }
            [J("unlock_time")] public long UnlockTime { get; set; }
            [J("refresh_time")] public long RefreshTime { get; set; }
            [J("outpost_id")] public long OutpostId { get; set; }
            [J("contractor_id")] public long ContractorId { get; set; }
            [J("global_contractor_id")] public long GlobalContractorId { get; set; }
            [J("event_id")] public long EventId { get; set; }
            [J("map_x")] public long MapX { get; set; }
            [J("map_y")] public long MapY { get; set; }
            [J("atlas")] public string Atlas { get; set; }
            [J("texture")] public string Texture { get; set; }
            [J("iso_width")] public long IsoWidth { get; set; }
            [J("iso_height")] public long IsoHeight { get; set; }
            [J("points")] public string Points { get; set; }
            [J("controls")] public string Controls { get; set; }
            [J("levels")] public Levels Levels { get; set; }
        }

        public class Levels
        {
            [J("level")] public List<Level> Level { get; set; }
        }

        public class Level
        {
            [J("id")] public long Id { get; set; }
            [J("sailors")] public long Sailors { get; set; }
            [J("sailors_fid")] public long SailorsFid { get; set; }
            [J("travel_time")] public long TravelTime { get; set; }
            [J("material_koef")] public long MaterialKoef { get; set; }
            [J("material_koef_fid")] public long MaterialKoefFid { get; set; }
            [J("amount")] public long Amount { get; set; }
            [J("amount_fid")] public long AmountFid { get; set; }
            [J("xp")] public long Xp { get; set; }
            [J("xp_pct")] public long XpPct { get; set; }
        }
    }
}