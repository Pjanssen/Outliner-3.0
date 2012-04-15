using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Outliner.Scene;
using Outliner.Commands;
using Autodesk.Max;

namespace Outliner_Tests.Commands
{
[TestClass]
public class ToggleLightCommandTest : MaxIntegrationTest
{
   private IINode createLight()
   {
      IInterface ip = MaxRemoting.Global.COREInterface;
      ILightObject light = ip.CreateInstance(SClass_ID.Light, MaxRemoting.Global.Class_ID.Create((uint)BuiltInClassIDA.OMNI_LIGHT_CLASS_ID, 0)) as ILightObject;
      return ip.CreateObjectNode(light);
   }

   [TestMethod]
   public void ToggleLightTest()
   {
      IINodeWrapper light = IMaxNodeWrapper.Create(this.createLight()) as IINodeWrapper;
      Assert.IsNotNull(light);

      Boolean useLight = ((ILightObject)light.IINode.ObjectRef).UseLight;
      List<IMaxNodeWrapper> nodes = new List<IMaxNodeWrapper>(1) { light };
      ToggleLightCommand cmd = new ToggleLightCommand(nodes, !useLight);

      cmd.Do();
      Assert.AreEqual(!useLight, ((ILightObject)light.IINode.ObjectRef).UseLight);

      cmd.Undo();
      Assert.AreEqual(useLight, ((ILightObject)light.IINode.ObjectRef).UseLight);
   }


   [TestMethod]
   public void NonLightObjectTest()
   {
      IINodeWrapper node = IMaxNodeWrapper.Create(MaxRemoting.CreateBox()) as IINodeWrapper;
      Assert.IsNotNull(node);

      ToggleLightCommand cmd = new ToggleLightCommand(new List<IMaxNodeWrapper>(1) { node }, false);

      try
      {
         cmd.Do();
      }
      catch (Exception) 
      {
         Assert.Fail("ToggleLightCommand.Do() should not throw exception when using non-light object");
      }

      try
      {
         cmd.Undo();
      }
      catch (Exception)
      {
         Assert.Fail("ToggleLightCommand.Undo() should not throw exception when using non-light object");
      }
   }
}
}
