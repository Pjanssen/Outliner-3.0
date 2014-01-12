using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Commands;
using Autodesk.Max;

namespace PJanssen.Outliner.IntegrationTests.Commands
{
[TestClass]
public class ToggleLightCommandTest : MaxIntegrationTest
{
   private IINode createLight()
   {
      IInterface ip = MaxRemoting.Global.COREInterface;
      ILightObject light = ip.CreateInstance(SClass_ID.Light, MaxRemoting.Global.Class_ID.Create((uint)BuiltInClassIDA.OMNI_LIGHT_CLASS_ID, 0)) as ILightObject;
      IINode inode = ip.CreateObjectNode(light);
      ip.AddLightToScene(inode);

      return inode;
   }

   //[TestMethod]
   //public void ToggleLightTest()
   //{
   //   INodeWrapper light = MaxNodeWrapper.Create(this.createLight()) as INodeWrapper;
   //   Assert.IsNotNull(light);

   //   Boolean useLight = ((ILightObject)light.INode.ObjectRef).UseLight;
   //   List<MaxNodeWrapper> nodes = new List<MaxNodeWrapper>(1) { light };
   //   ToggleLightCommand cmd = new ToggleLightCommand(nodes, !useLight);

   //   cmd.Redo();
   //   Assert.AreEqual(!useLight, ((ILightObject)light.INode.ObjectRef).UseLight);

   //   cmd.Restore(true);
   //   Assert.AreEqual(useLight, ((ILightObject)light.INode.ObjectRef).UseLight);
   //}


   //[TestMethod]
   //public void NonLightObjectTest()
   //{
   //   INodeWrapper node = MaxNodeWrapper.Create(MaxRemoting.CreateBox()) as INodeWrapper;
   //   Assert.IsNotNull(node);

   //   ToggleLightCommand cmd = new ToggleLightCommand(new List<MaxNodeWrapper>(1) { node }, false);

   //   try
   //   {
   //      cmd.Redo();
   //   }
   //   catch (Exception) 
   //   {
   //      Assert.Fail("ToggleLightCommand.Do() should not throw exception when using non-light object");
   //   }

   //   try
   //   {
   //      cmd.Restore(true);
   //   }
   //   catch (Exception)
   //   {
   //      Assert.Fail("ToggleLightCommand.Undo() should not throw exception when using non-light object");
   //   }
   //}
}
}
