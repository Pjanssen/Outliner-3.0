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
         Assert.IsTrue(NodeProperties.IsDisplayProperty(NodeProperty.AllEdges));
         Assert.IsTrue(NodeProperties.IsDisplayProperty(NodeProperty.BackfaceCull));
         Assert.IsTrue(NodeProperties.IsDisplayProperty(NodeProperty.BoxMode));
         Assert.IsTrue(NodeProperties.IsDisplayProperty(NodeProperty.FrozenInGray));
         Assert.IsTrue(NodeProperties.IsDisplayProperty(NodeProperty.IgnoreExtents));
         Assert.IsTrue(NodeProperties.IsDisplayProperty(NodeProperty.IsFrozen));
         Assert.IsTrue(NodeProperties.IsDisplayProperty(NodeProperty.IsHidden));
         Assert.IsTrue(NodeProperties.IsDisplayProperty(NodeProperty.SeeThrough));
         Assert.IsTrue(NodeProperties.IsDisplayProperty(NodeProperty.Trajectory));
         Assert.IsTrue(NodeProperties.IsDisplayProperty(NodeProperty.VertexTicks));

         Assert.IsFalse(NodeProperties.IsDisplayProperty(NodeProperty.ApplyAtmospherics));
         Assert.IsFalse(NodeProperties.IsDisplayProperty(NodeProperty.CastShadows));
         Assert.IsFalse(NodeProperties.IsDisplayProperty(NodeProperty.InheritVisibility));
         Assert.IsFalse(NodeProperties.IsDisplayProperty(NodeProperty.PrimaryVisibility));
         Assert.IsFalse(NodeProperties.IsDisplayProperty(NodeProperty.ReceiveShadows));
         Assert.IsFalse(NodeProperties.IsDisplayProperty(NodeProperty.Renderable));
         Assert.IsFalse(NodeProperties.IsDisplayProperty(NodeProperty.RenderOccluded));
         Assert.IsFalse(NodeProperties.IsDisplayProperty(NodeProperty.SecondaryVisibility));

         Assert.IsFalse(NodeProperties.IsDisplayProperty(NodeProperty.None));
         Assert.IsFalse(NodeProperties.IsDisplayProperty(NodeProperty.Name));
         Assert.IsFalse(NodeProperties.IsDisplayProperty(NodeProperty.WireColor));

         //Combined flags
         Assert.IsTrue(NodeProperties.IsDisplayProperty(NodeProperty.IsFrozen | NodeProperty.IsHidden), "combined flags 1");
         Assert.IsFalse(NodeProperties.IsDisplayProperty(NodeProperty.PrimaryVisibility | NodeProperty.Name), "combined flags 2");

         //Mixed flags
         Assert.IsTrue(NodeProperties.IsDisplayProperty(NodeProperty.Name | NodeProperty.IsHidden), "mixed flags");
      }

      [TestMethod]
      public void IsRenderPropertyTest()
      {
         Assert.IsFalse(NodeProperties.IsRenderProperty(NodeProperty.AllEdges));
         Assert.IsFalse(NodeProperties.IsRenderProperty(NodeProperty.BackfaceCull));
         Assert.IsFalse(NodeProperties.IsRenderProperty(NodeProperty.BoxMode));
         Assert.IsFalse(NodeProperties.IsRenderProperty(NodeProperty.IgnoreExtents));
         Assert.IsFalse(NodeProperties.IsRenderProperty(NodeProperty.IsFrozen));
         Assert.IsFalse(NodeProperties.IsRenderProperty(NodeProperty.IsHidden));
         Assert.IsFalse(NodeProperties.IsRenderProperty(NodeProperty.SeeThrough));
         Assert.IsFalse(NodeProperties.IsRenderProperty(NodeProperty.Trajectory));
         Assert.IsFalse(NodeProperties.IsRenderProperty(NodeProperty.VertexTicks));

         Assert.IsTrue(NodeProperties.IsRenderProperty(NodeProperty.ApplyAtmospherics));
         Assert.IsTrue(NodeProperties.IsRenderProperty(NodeProperty.CastShadows));
         Assert.IsTrue(NodeProperties.IsRenderProperty(NodeProperty.InheritVisibility));
         Assert.IsTrue(NodeProperties.IsRenderProperty(NodeProperty.PrimaryVisibility));
         Assert.IsTrue(NodeProperties.IsRenderProperty(NodeProperty.ReceiveShadows));
         Assert.IsTrue(NodeProperties.IsRenderProperty(NodeProperty.Renderable));
         Assert.IsTrue(NodeProperties.IsRenderProperty(NodeProperty.RenderOccluded));
         Assert.IsTrue(NodeProperties.IsRenderProperty(NodeProperty.SecondaryVisibility));

         Assert.IsFalse(NodeProperties.IsRenderProperty(NodeProperty.None));
         Assert.IsFalse(NodeProperties.IsRenderProperty(NodeProperty.Name));
         Assert.IsFalse(NodeProperties.IsRenderProperty(NodeProperty.WireColor));

         //Combined flags
         Assert.IsFalse(NodeProperties.IsRenderProperty(NodeProperty.IsFrozen | NodeProperty.IsHidden), "combined flags 1");
         Assert.IsTrue(NodeProperties.IsRenderProperty(NodeProperty.PrimaryVisibility | NodeProperty.SecondaryVisibility), "combined flags 2");

         //Mixed flags
         Assert.IsTrue(NodeProperties.IsRenderProperty(NodeProperty.Name | NodeProperty.PrimaryVisibility), "mixed flags");
      }

   }
}
