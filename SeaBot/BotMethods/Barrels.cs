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

    using System;
    using System.Linq;

    using SeaBotCore.Data;
    using SeaBotCore.Data.Definitions;
    using SeaBotCore.Data.Materials;
    using SeaBotCore.Localizaion;
    using SeaBotCore.Logger;
    using SeaBotCore.Utils;

    #endregion

    public static class Barrels
    {
        public static void CollectBarrel()
        {
            var nearestev = TimeUtils.GetCurrentEvent().Barrel.Integer.Value;
            var barrelcontroller = Definitions.BarrelDef.Barrels.Item.Where(n => n.DefId == nearestev).First();
            var nextbarrel = BarrelController.GetNextBarrel(barrelcontroller);
            if (nextbarrel.Definition.Id != 0)
            {
                Logger.Info(
                    string.Format(
                        Localization.BARREL_COLLECTING_ITEM,
                        nextbarrel.Amount,
                        MaterialDB.GetLocalizedName(nextbarrel.Definition.Id)));
                if (Core.LocalPlayer.Inventory.FirstOrDefault(n => n.Id == nextbarrel.Definition.Id) != null)
                {
                    Core.LocalPlayer.Inventory.First(n => n.Id == nextbarrel.Definition.Id).Amount += nextbarrel.Amount;
                }
                else
                {
                    Core.LocalPlayer.Inventory.Add(
                        new Item { Amount = nextbarrel.Amount, Id = nextbarrel.Definition.Id });
                }
            }
            else
            {
                Logger.Info(string.Format(Localization.BARREL_COLLECTING_SAILORS, nextbarrel.Amount));
            }

            Networking.AddTask(
                new Task.ConfirmBarrelTask(
                    barrelcontroller.DefId,
                    nextbarrel.get_type(),
                    nextbarrel.Amount,
                    nextbarrel.Definition.Id,
                    Core.LocalPlayer.Level));
        }

        public static class BarrelController
        {
            public static double _lastBarrelSeed;

            private static readonly double _multiplier = 87981.0;

            private static double _seed = 1;

            public static BarrelMaterial GetNextBarrel(BarrelDefenitions.Barrel _definition)
            {
                SetSeed(_lastBarrelSeed);

                var material = _definition.Materials.Material[NextIntInRange(
                    0,
                    _definition.Materials.Material.Count - 1)];
                var min = Ceil(Math.Pow(Core.LocalPlayer.Level, material.ExponentMin) + material.OffsetMin);
                var max = Ceil(Math.Pow(Core.LocalPlayer.Level, material.ExponentMax) + material.OffsetMax);
                var cur = NextIntInRange(min, max);
                var curcelling = Ceil(cur * material.Koef);
                _lastBarrelSeed = NextInt();
                material.DefId = _definition.DefId.ToString();
                var ret = new BarrelMaterial(material, (int)curcelling, Core.LocalPlayer.Level);
                return ret;
            }

            public static double NextDouble()
            {
                return Generate() / 2147483647;
            }

            public static int NextInt()
            {
                return (int)Generate();
            }

            public static int NextIntInRange(double p1, double p2)
            {
                p1 = p1 - 0.4999;
                p2 = p2 + 0.4999;
                return (int)Math.Round(p1 + (p2 - p1) * NextDouble());
            }

            public static void SetSeed(double seed)
            {
                _seed = seed;
            }

            private static double Ceil(double param1)
            {
                return Math.Ceiling(Math.Round(param1 * 10000) / 10000);
            }

            private static double Generate()
            {
                _seed = _seed * _multiplier % 2147483647;
                return _seed;
            }
        }

        public class BarrelMaterial
        {
            public BarrelMaterial(BarrelDefenitions.Material param1, int param2, int param3)
            {
                this.Definition = param1;
                this.PlayerLevel = param3;
                this.Amount = param2;
            }

            public int Amount { get; set; }

            public BarrelDefenitions.Material Definition { get; set; }

            public int PlayerLevel { get; set; }

            public string get_type()
            {
                return this.Definition.Type;
            }
        }
    }
}