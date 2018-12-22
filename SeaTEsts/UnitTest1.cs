using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeaBotCore.Data;
using SeaBotCore.Data.Defenitions;
using SeaBotCore.Utils;

namespace SeaTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PseudoRandomBarrel()
        {
            SeaBotCore.Utils.BarrelController._lastBarrelSeed=1147034909;
            SeaBotCore.Core.GolobalData = new GlobalData(){Level=49};
            var bar = BarrelController.GetNextBarrel(Defenitions.BarrelDef.Items.Item
                .Where(n => n.DefId == 21).First());
            Assert.AreEqual(1, bar.Definition.Id);
            Assert.AreEqual(70,bar.Amount);
           
        }
    }
}
