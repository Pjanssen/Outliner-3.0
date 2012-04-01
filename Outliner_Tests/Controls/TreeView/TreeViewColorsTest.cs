using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;
using Outliner.Controls;
using System.Drawing;
using System.IO;

namespace Outliner_Tests.Controls.TreeView
{
   [TestClass]
   public class TreeViewColorsTest
   {
      private Color randomColor(int seed)
      {
         Random r = new Random(seed);
         return Color.FromArgb(r.Next(255), r.Next(255), r.Next(255), r.Next(255));
      }
      [TestMethod]
      public void SerializeTest()
      {
         TreeViewColors source = new TreeViewColors();
         TreeViewColors result = null;

         Random r = new Random();

         //Assign random values.
         source.AltBackColor       = this.randomColor(r.Next());
         source.BackColor          = Color.Black;
         source.ForeColorDark      = this.randomColor(r.Next());
         source.ForeColorLight     = this.randomColor(r.Next());
         source.LinkBackColor      = this.randomColor(r.Next());
         source.LinkForeColor      = this.randomColor(r.Next());
         source.ParentBackColor    = this.randomColor(r.Next());
         source.ParentForeColor    = Color.Fuchsia;
         source.SelectionBackColor = Color.ForestGreen;
         source.SelectionForeColor = this.randomColor(r.Next());

         //Serialize, then deserialize.
         XmlSerializer xs = new XmlSerializer(typeof(TreeViewColors));
         using (MemoryStream stream = new MemoryStream())
         {
            xs.Serialize(stream, source);
            stream.Position = 0;
            result = xs.Deserialize(stream) as TreeViewColors;
         }

         Assert.IsNotNull(result);
         Assert.AreEqual(source.AltBackColor, result.AltBackColor);
         Assert.AreEqual(source.BackColor, result.BackColor);
         Assert.AreEqual(source.ForeColorDark, result.ForeColorDark);
         Assert.AreEqual(source.ForeColorLight, result.ForeColorLight);
         Assert.AreEqual(source.LinkBackColor, result.LinkBackColor);
         Assert.AreEqual(source.LinkForeColor, result.LinkForeColor);
         Assert.AreEqual(source.ParentBackColor, result.ParentBackColor);
         Assert.AreEqual(source.ParentForeColor, result.ParentForeColor);
         Assert.AreEqual(source.SelectionBackColor, result.SelectionBackColor);
         Assert.AreEqual(source.SelectionForeColor, result.SelectionForeColor);
      }
   }
}
