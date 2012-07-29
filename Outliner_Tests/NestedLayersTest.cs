using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autodesk.Max;
using Outliner.LayerTools;

namespace Outliner_Tests
{
[TestClass]
public class NestedLayersTest : MaxIntegrationTest
{
   [TestMethod]
   public void GetParentTest()
   {
      IILayer layer = MaxRemoting.CreateLayer();
      Assert.IsNotNull(layer);

      IILayer parent = NestedLayers.GetParent(null);
      Assert.IsNull(parent, "Passing null");

      parent = NestedLayers.GetParent(layer);
      Assert.IsNull(parent, "Passing layer without parent");
   }

   [TestMethod]
   public void SetParentTest()
   {
      IILayer childLayer = MaxRemoting.CreateLayer();
      IILayer parentLayer = MaxRemoting.CreateLayer();

      NestedLayers.SetParent(childLayer, childLayer);
      Assert.IsNull(NestedLayers.GetParent(childLayer), "Set parent to itself");

      NestedLayers.SetParent(childLayer, parentLayer);
      Assert.AreEqual(parentLayer, NestedLayers.GetParent(childLayer), "Parent layer set");

      NestedLayers.SetParent(childLayer, null);
      Assert.IsNull(NestedLayers.GetParent(childLayer), "Parent layer removed");
   }

   [TestMethod]
   public void GetChildrenTest()
   {
      Assert.Inconclusive();
   }

   [TestMethod]
   public void GetPropertyTest()
   {

   }
}
}
