using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autodesk.Max;
using PJanssen.Outliner.LayerTools;

namespace PJanssen.Outliner.IntegrationTests.LayerTools
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
      IILayer layer = MaxRemoting.CreateLayer();

      layer.IsHidden = false;
      Assert.IsFalse(layer.IsHidden, "Property should be set correctly");

      Assert.IsFalse(NestedLayers.GetProperty(layer, MaxUtils.BooleanNodeProperty.IsHidden), "Uninherited property");

      IILayer parent = MaxRemoting.CreateLayer();
      NestedLayers.SetParent(layer, parent);
      Assert.AreEqual(parent, NestedLayers.GetParent(layer), "Parenting should be successful");

      parent.IsHidden = true;
      Assert.IsTrue(parent.IsHidden);
      Assert.IsTrue(layer.IsHidden, "Property was inherited");
      Assert.IsFalse(NestedLayers.GetProperty(layer, MaxUtils.BooleanNodeProperty.IsHidden));
   }
}
}
