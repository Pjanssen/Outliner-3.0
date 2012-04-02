using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;
using Outliner.Controls.Tree;
using System.Drawing;
using System.IO;
using Outliner.Controls;

namespace Outliner_Tests.Controls.Tree
{
   [TestClass]
   public class TreeViewColorsTest
   {
      private SerializableColor randomColor(int seed)
      {
         Random r = new Random(seed);
         return new SerializableColor(r.Next(255), r.Next(255), r.Next(255), r.Next(255));
      }
      [TestMethod]
      public void SerializeTest()
      {
         TreeViewColors source = new TreeViewColors();
         TreeViewColors result = null;

         Random r = new Random();

         //Assign random values.
         source.AltBackground        = this.randomColor(r.Next());
         source.Background           = new SerializableColor(Color.Black);
         source.ForegroundDark       = this.randomColor(r.Next());
         source.ForegroundLight      = this.randomColor(r.Next());
         source.DropTargetBackground = this.randomColor(r.Next());
         source.DropTargetForeground = this.randomColor(r.Next());
         source.ParentBackground     = this.randomColor(r.Next());
         source.ParentForeground     = new SerializableColor(Color.Fuchsia);
         source.SelectionBackground  = new SerializableColor(Color.ForestGreen);
         source.SelectionForeground  = this.randomColor(r.Next());

         //Serialize, then deserialize.
         XmlSerializer xs = new XmlSerializer(typeof(TreeViewColors));
         using (MemoryStream stream = new MemoryStream())
         {
            xs.Serialize(stream, source);
            stream.Position = 0;
            result = xs.Deserialize(stream) as TreeViewColors;
         }

         Assert.IsNotNull(result);
         Assert.AreEqual(source.AltBackground, result.AltBackground);
         Assert.AreEqual(source.Background, result.Background);
         Assert.AreEqual(source.ForegroundDark, result.ForegroundDark);
         Assert.AreEqual(source.ForegroundLight, result.ForegroundLight);
         Assert.AreEqual(source.DropTargetBackground, result.DropTargetBackground);
         Assert.AreEqual(source.DropTargetForeground, result.DropTargetForeground);
         Assert.AreEqual(source.ParentBackground, result.ParentBackground);
         Assert.AreEqual(source.ParentForeground, result.ParentForeground);
         Assert.AreEqual(source.SelectionBackground, result.SelectionBackground);
         Assert.AreEqual(source.SelectionForeground, result.SelectionForeground);
      }
   }
}
