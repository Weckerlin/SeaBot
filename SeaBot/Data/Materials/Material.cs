// // SeaBotCore
// // Copyright (C) 2018 - 2019 Weespin
// // 
// // This program is free software: you can redistribute it and/or modify
// // it under the terms of the GNU General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// // GNU General Public License for more details.
// // 
// // You should have received a copy of the GNU General Public License
// // along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace SeaBotCore.Data.Materials
{
    #region

    using System.Collections.Generic;
    using System.Linq;

    using SeaBotCore.Cache;
    using SeaBotCore.Data.Definitions;

    #endregion

    public static class MaterialDB
    {
        public static List<MaterialsData.Item> GetAllItems()
        {
            return Definitions.MatDef.Items.Item.ToList();
        }

        public static MaterialsData.Item GetItem(int id)
        {
            return Definitions.MatDef.Items.Item.FirstOrDefault(n => n.DefId == id);
        }

        public static MaterialsData.Item GetItem(long id)
        {
            return Definitions.MatDef.Items.Item.FirstOrDefault(n => n.DefId == id);
        }

        public static MaterialsData.Item GetItem(string name)
        {
            return Definitions.MatDef.Items.Item.FirstOrDefault(n => n.Name == name);
        }

        public static string GetLocalizedName(int id)
        {
            return LocalizationCache.GetNameFromLoc(
                Definitions.MatDef.Items.Item.FirstOrDefault(n => n.DefId == id)?.NameLoc.ToLower(),
                Definitions.MatDef.Items.Item.FirstOrDefault(n => n.DefId == id)?.Name);
        }
    }
}