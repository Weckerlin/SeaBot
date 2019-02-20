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
        public static List<MaterialsData.Material> GetAllItems()
        {
            return LocalDefinitions.Materials.ToList();
        }

        public static MaterialsData.Material GetItem(int id)
        {
            return  LocalDefinitions.Materials.FirstOrDefault(n => n.DefId == id);
        }

        public static MaterialsData.Material GetItem(long id)
        {
            return  LocalDefinitions.Materials.FirstOrDefault(n => n.DefId == id);
        }

        public static MaterialsData.Material GetItem(string name)
        {
            return  LocalDefinitions.Materials.FirstOrDefault(n => n.Name == name);
        }

        public static string GetLocalizedName(int id)
        {
            return LocalizationCache.GetNameFromLoc(
                LocalDefinitions.Materials.FirstOrDefault(n => n.DefId == id)?.NameLoc.ToLower(),
                LocalDefinitions.Materials.FirstOrDefault(n => n.DefId == id)?.Name);
        }
    }
}