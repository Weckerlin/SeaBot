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

using System.Collections.Generic;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using N = Newtonsoft.Json.NullValueHandling;

namespace SeaBotCore.Data.Definitions
{
    public class BuildingDefentions
    {
        public class Root : IDefinition
        {
            [J("items")] public Items Items { get; set; }
        }

        public class Items
        {
            [J("item")] public List<Item> Item { get; set; }
        }

        public class Item
        {
            [J("def_id")] public int DefId { get; set; }
            [J("version_id")] public int VersionId { get; set; }
            [J("name")] public string Name { get; set; }
            [J("name_loc")] public string NameLoc { get; set; }
            [J("desc_loc")] public string DescLoc { get; set; }
            [J("tile_width")] public int TileWidth { get; set; }
            [J("tile_height")] public int TileHeight { get; set; }
            [J("on_grid")] public int OnGrid { get; set; }
            [J("type")] public string Type { get; set; }
            [J("max_level")] public int MaxLevel { get; set; }
            [J("hide")] public int Hide { get; set; }
            [J("upgrade_priority")] public int UpgradePriority { get; set; }
            [J("levels")] public Levels Levels { get; set; }
        }

        public class Levels
        {
            [J("level")] public List<Level> Level { get; set; }
        }

        public class Level
        {
            [J("id")] public int Id { get; set; }
            [J("req_type")] public string ReqType { get; set; }
            [J("req_id")] public int ReqId { get; set; }
            [J("req_level")] public int ReqLevel { get; set; }
            [J("player_level")] public int PlayerLevel { get; set; }
            [J("player_level_gem")] public int PlayerLevelGem { get; set; }
            [J("xp")] public int Xp { get; set; }
            [J("sailors_regen")] public int SailorsRegen { get; set; }
            [J("sailors_cap")] public int SailorsCap { get; set; }
            [J("gem_price")] public int GemPrice { get; set; }
            [J("gem_upgrade_time")] public int GemUpgradeTime { get; set; }
            [J("upgrade_time")] public int UpgradeTime { get; set; }
            [J("boat_level")] public int BoatLevel { get; set; }
            [J("lighthouse_level")] public int LighthouseLevel { get; set; }
            [J("museum_level")] public int MuseumLevel { get; set; }
            [J("iso_x")] public int IsoX { get; set; }
            [J("iso_y")] public int IsoY { get; set; }
            [J("bounds")] public string Bounds { get; set; }
            [J("iso_width")] public int IsoWidth { get; set; }
            [J("iso_height")] public int IsoHeight { get; set; }
            [J("pivot_x")] public int PivotX { get; set; }
            [J("pivot_y")] public int PivotY { get; set; }
            [J("texture")] public string Texture { get; set; }
            [J("particles")] public string Particles { get; set; }
            [J("animations")] public string Animations { get; set; }
            [J("iso_x_new")] public int IsoXNew { get; set; }
            [J("iso_y_new")] public int IsoYNew { get; set; }
            [J("bounds_new")] public string BoundsNew { get; set; }
            [J("iso_width_new")] public int IsoWidthNew { get; set; }
            [J("iso_height_new")] public int IsoHeightNew { get; set; }
            [J("pivot_x_new")] public int PivotXNew { get; set; }
            [J("pivot_y_new")] public int PivotYNew { get; set; }
            [J("texture_new")] public string TextureNew { get; set; }
            [J("particles_new")] public string ParticlesNew { get; set; }
            [J("materials")] public Materials Materials { get; set; }
            [J("prod_outputs")] public ProdOutputs ProdOutputs { get; set; }

            [J("slots", NullValueHandling = N.Ignore)]
            public Slots Slots { get; set; }

            [J("particle_new", NullValueHandling = N.Ignore)]
            public ParticleNew ParticleNew { get; set; }
        }

        public class Materials
        {
            [J("material")] public List<Material> Material { get; set; }
        }

        public class Material
        {
            [J("id")] public int Id { get; set; }
            [J("amount")] public int Amount { get; set; }
        }

        public class ParticleNew
        {
            [J("particle")] public List<Particle> Particle { get; set; }
        }

        public class Particle
        {
            [J("particle_id")] public int ParticleId { get; set; }
            [J("data")] public string Data { get; set; }
        }

        public class ProdOutputs
        {
            [J("prod_output")] public List<ProdOutput> ProdOutput { get; set; }
        }

        public class ProdOutput
        {
            [J("id")] public int Id { get; set; }
            [J("material_id")] public int MaterialId { get; set; }
            [J("amount")] public int Amount { get; set; }
            [J("time")] public int Time { get; set; }

            [J("inputs", NullValueHandling = N.Ignore)]
            public Inputs Inputs { get; set; }
        }

        public class Inputs
        {
            [J("input")] public List<Material> Input { get; set; }
        }

        public class Slots
        {
            [J("slot")] public List<Slot> Slot { get; set; }
        }

        public class Slot
        {
            [J("type")] public string Type { get; set; }
            [J("id")] public int Id { get; set; }
            [J("amount")] public int Amount { get; set; }
        }
    }
}