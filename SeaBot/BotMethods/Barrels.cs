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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaBotCore.Data;
using SeaBotCore.Data.Defenitions;
using SeaBotCore.Data.Materials;
using SeaBotCore.Utils;

namespace SeaBotCore.BotMethods
{
    public static class Barrels
    {
        public static void CollectBarrel()
        {
            var nearestev = TimeUtils.GetCurrentEvent().Barrel.Integer.Value;
            var bar = BarrelController.GetNextBarrel(Defenitions.BarrelDef.Items.Item
                .Where(n => n.DefId == nearestev).First());
            if (bar.Definition.Id != 0)
            {
                Logger.Logger.Info(
                    $"Barrel! Collecting {bar.Amount} {MaterialDB.GetItem(bar.Definition.Id).Name}");
                if (Core.GlobalData.Inventory.Where(n => n.Id == bar.Definition.Id).FirstOrDefault() != null)
                {
                    Core.GlobalData.Inventory.Where(n => n.Id == bar.Definition.Id).First().Amount += bar.Amount;
                }
                else
                {
                    Core.GlobalData.Inventory.Add(new Item {Amount = bar.Amount, Id = (int) bar.Definition.Id});
                }
            }
            else
            {
                Logger.Logger.Info(
                    $"Barrel! Collecting {bar.Amount} sailors!");
            }

            Networking.AddTask(new Task.ConfirmBarrelTask("21", bar.get_type(), bar.Amount.ToString(),
                bar.Definition.Id.ToString(), Core.GlobalData.Level.ToString()));
        }

        public class BarrelMaterial
        {
            public string get_type()
            {
                return _definition.Type;
            }

            private BarrelDefenitions.Material _definition;

            private int _playerLevel;

            private int _amount;

            public BarrelMaterial(BarrelDefenitions.Material param1, int param2, int param3)
            {
                Definition = param1;
                PlayerLevel = param3;
                Amount = param2;
            }

            public int Amount
            {
                get { return _amount; }
                set { _amount = value; }
            }

            public int PlayerLevel
            {
                get { return _playerLevel; }
                set { _playerLevel = value; }
            }

            public BarrelDefenitions.Material Definition
            {
                get { return _definition; }
                set { _definition = value; }
            }
        }


        public static class BarrelController
        {
            private static double _seed = 1;

            private static double _multiplier = 87981.0;

            public static int NextInt()
            {
                return (int) Generate();
            }

            public static double NextDouble()
            {
                return Generate() / 2147483647;
            }

            public static Int32 NextIntInRange(double p1, double p2)
            {
                p1 = p1 - 0.4999;
                p2 = p2 + 0.4999;
                return (int) Math.Round(p1 + (p2 - p1) * NextDouble());
            }

            private static double Generate()
            {
                _seed = _seed * _multiplier % 2147483647;
                return _seed;
            }

            public static void SetSeed(double seed)
            {
                _seed = seed;
            }

            private static double Ceil(double param1)
            {
                return Math.Ceiling(Math.Round(param1 * 10000) / 10000);
            }

            public static double _lastBarrelSeed;

            public static BarrelMaterial GetNextBarrel(BarrelDefenitions.Item _definition)
            {
                SetSeed(_lastBarrelSeed);

                var material =
                    _definition.Materials.Material[NextIntInRange(0, _definition.Materials.Material.Count - 1)];
                var min = Ceil(Math.Pow(Core.GlobalData.Level, material.ExponentMin) + material.OffsetMin);
                var max = Ceil(Math.Pow(Core.GlobalData.Level, material.ExponentMax) + material.OffsetMax);
                var cur = NextIntInRange(min, max);
                var curcelling = Ceil(cur * material.Koef);
                _lastBarrelSeed = NextInt();
                material.DefId = _definition.DefId.ToString();
                var ret = new BarrelMaterial(material, (int) curcelling, (Core.GlobalData.Level));
                return ret;
            }
        }
    }
}