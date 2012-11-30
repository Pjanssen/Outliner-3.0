using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outliner.MaxUtils;

namespace Outliner.Tests.MaxUtils
{
   [TestClass]
   public class NodePropertyTest
   {
      [TestMethod]
      public void IsDisplayPropertyTest()
      {
         Assert.IsTrue(NodePropertyHelpers.IsDisplayProperty(NodeProperty.AllEdges));
         Assert.IsTrue(NodePropertyHelpers.IsDisplayProperty(NodeProperty.BackfaceCull));
         Assert.IsTrue(NodePropertyHelpers.IsDisplayProperty(NodeProperty.BoxMode));
         Assert.IsTrue(NodePropertyHelpers.IsDisplayProperty(NodeProperty.FrozenInGray));
         Assert.IsTrue(NodePropertyHelpers.IsDisplayProperty(NodeProperty.IgnoreExtents));
         Assert.IsTrue(NodePropertyHelpers.IsDisplayProperty(NodeProperty.IsFrozen));
         Assert.IsTrue(NodePropertyHelpers.IsDisplayProperty(NodeProperty.IsHidden));
         Assert.IsTrue(NodePropertyHelpers.IsDisplayProperty(NodeProperty.SeeThrough));
         Assert.IsTrue(NodePropertyHelpers.IsDisplayProperty(NodeProperty.Trajectory));
         Assert.IsTrue(NodePropertyHelpers.IsDisplayProperty(NodeProperty.VertexTicks));

         Assert.IsFalse(NodePropertyHelpers.IsDisplayProperty(NodeProperty.ApplyAtmospherics));
         Assert.IsFalse(NodePropertyHelpers.IsDisplayProperty(NodeProperty.CastShadows));
         Assert.IsFalse(NodePropertyHelpers.IsDisplayProperty(NodeProperty.InheritVisibility));
         Assert.IsFalse(NodePropertyHelpers.IsDisplayProperty(NodeProperty.PrimaryVisibility));
         Assert.IsFalse(NodePropertyHelpers.IsDisplayProperty(NodeProperty.ReceiveShadows));
         Assert.IsFalse(NodePropertyHelpers.IsDisplayProperty(NodeProperty.Renderable));
         Assert.IsFalse(NodePropertyHelpers.IsDisplayProperty(NodeProperty.RenderOccluded));
         Assert.IsFalse(NodePropertyHelpers.IsDisplayProperty(NodeProperty.SecondaryVisibility));

         Assert.IsFalse(NodePropertyHelpers.IsDisplayProperty(NodeProperty.None));
         Assert.IsFalse(NodePropertyHelpers.IsDisplayProperty(NodeProperty.Name));
         Assert.IsFalse(NodePropertyHelpers.IsDisplayProperty(NodeProperty.WireColor));

         //Combined flags
         Assert.IsTrue(NodePropertyHelpers.IsDisplayProperty(NodeProperty.IsFrozen | NodeProperty.IsHidden), "combined flags 1");
         Assert.IsFalse(NodePropertyHelpers.IsDisplayProperty(NodeProperty.PrimaryVisibility | NodeProperty.Name), "combined flags 2");

         //Mixed flags
         Assert.IsTrue(NodePropertyHelpers.IsDisplayProperty(NodeProperty.Name | NodeProperty.IsHidden), "mixed flags");
      }

      [TestMethod]
      public void IsRenderPropertyTest()
      {
         Assert.IsFalse(NodePropertyHelpers.IsRenderProperty(NodeProperty.AllEdges));
         Assert.IsFalse(NodePropertyHelpers.IsRenderProperty(NodeProperty.BackfaceCull));
         Assert.IsFalse(NodePropertyHelpers.IsRenderProperty(NodeProperty.BoxMode));
         Assert.IsFalse(NodePropertyHelpers.IsRenderProperty(NodeProperty.IgnoreExtents));
         Assert.IsFalse(NodePropertyHelpers.IsRenderProperty(NodeProperty.IsFrozen));
         Assert.IsFalse(NodePropertyHelpers.IsRenderProperty(NodeProperty.IsHidden));
         Assert.IsFalse(NodePropertyHelpers.IsRenderProperty(NodeProperty.SeeThrough));
         Assert.IsFalse(NodePropertyHelpers.IsRenderProperty(NodeProperty.Trajectory));
         Assert.IsFalse(NodePropertyHelpers.IsRenderProperty(NodeProperty.VertexTicks));

         Assert.IsTrue(NodePropertyHelpers.IsRenderProperty(NodeProperty.ApplyAtmospherics));
         Assert.IsTrue(NodePropertyHelpers.IsRenderProperty(NodeProperty.CastShadows));
         Assert.IsTrue(NodePropertyHelpers.IsRenderProperty(NodeProperty.InheritVisibility));
         Assert.IsTrue(NodePropertyHelpers.IsRenderProperty(NodeProperty.PrimaryVisibility));
         Assert.IsTrue(NodePropertyHelpers.IsRenderProperty(NodeProperty.ReceiveShadows));
         Assert.IsTrue(NodePropertyHelpers.IsRenderProperty(NodeProperty.Renderable));
         Assert.IsTrue(NodePropertyHelpers.IsRenderProperty(NodeProperty.RenderOccluded));
         Assert.IsTrue(NodePropertyHelpers.IsRenderProperty(NodeProperty.SecondaryVisibility));

         Assert.IsFalse(NodePropertyHelpers.IsRenderProperty(NodeProperty.None));
         Assert.IsFalse(NodePropertyHelpers.IsRenderProperty(NodeProperty.Name));
         Assert.IsFalse(NodePropertyHelpers.IsRenderProperty(NodeProperty.WireColor));

         //Combined flags
         Assert.IsFalse(NodePropertyHelpers.IsRenderProperty(NodeProperty.IsFrozen | NodeProperty.IsHidden), "combined flags 1");
         Assert.IsTrue(NodePropertyHelpers.IsRenderProperty(NodeProperty.PrimaryVisibility | NodeProperty.SecondaryVisibility), "combined flags 2");

         //Mixed flags
         Assert.IsTrue(NodePropertyHelpers.IsRenderProperty(NodeProperty.Name | NodeProperty.PrimaryVisibility), "mixed flags");
      }

   }
}
