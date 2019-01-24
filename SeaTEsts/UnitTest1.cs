// SeaTests
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

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaBotCore;
using SeaBotCore.BotMethods;
using SeaBotCore.Data;
using SeaBotCore.Data.Definitions;

namespace SeaTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PseudoRandomBarrel()
        {
            Barrels.BarrelController._lastBarrelSeed = 1147034909;
            Core.GlobalData = new GlobalData {Level = 49};
            var bar = Barrels.BarrelController.GetNextBarrel(Defenitions.BarrelDef.Items.Item
                .Where(n => n.DefId == 21).First());
            Assert.AreEqual(1, bar.Definition.Id);
            Assert.AreEqual(70, bar.Amount);
        }
    }
}