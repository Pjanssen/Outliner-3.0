using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autodesk.Max.Remoting;
using Outliner;
using System.Diagnostics;
using MaxUtils;

namespace Outliner_Tests
{
internal static class MaxRemoting
{
   internal static IGlobal Global
   {
      get
      {
         IGlobal global = null;
         try
         {
            global = MaxInterfaces.Global;
         } catch(Exception) { }

         if (global == null)
            Assert.Inconclusive("Unable to connect to 3dsMax.");

         return global;
      }
   }

   internal static IINode CreateDummy()
   {
      IInterface ip = Global.COREInterface;
      IDummyObject dummy = ip.CreateInstance(SClass_ID.Helper, Global.Class_ID.Create((uint)BuiltInClassIDA.DUMMY_CLASS_ID, 0)) as IDummyObject;
      dummy.Box = Global.Box3.Create(Global.Point3.Create(-5, -5, -5), 
                                     Global.Point3.Create( 5,  5,  5));
      return ip.CreateObjectNode(dummy);
   }

   internal static IINode CreateBox()
   {
      IInterface ip = Global.COREInterface;
      IGenBoxObject boxObject = ip.CreateInstance(SClass_ID.Geomobject, Global.Class_ID.Create((uint)BuiltInClassIDA.BOXOBJ_CLASS_ID, 0)) as IGenBoxObject;
      boxObject.SetParams(10, 10, 10, 1, 1, 1, true);
      return ip.CreateObjectNode(boxObject);
   }

   internal static IILayer CreateLayer()
   {
      return MaxInterfaces.IILayerManager.CreateLayer();
   }

   internal static IINode SceneRoot
   {
      get
      {
         return Global.COREInterface.RootNode;
      }
   }

   internal static void DeleteNode(IINode node)
   {
      IInterface ip = Global.COREInterface;
      ip.DeleteNode(node, false, false);
   }

   internal static void ResetScene()
   {
      Global.COREInterface.FileReset(true);
   }
}

/// <summary>
/// A generic test baseclass, which automates 3dsMax scene cleanup after each test.
/// </summary>
[TestClass]
public abstract class MaxIntegrationTest
{
   [TestCleanup]
   public void TestCleanup()
   {
      MaxRemoting.ResetScene();
   }
}


[TestClass]
public class RemotingTests : MaxIntegrationTest
{
   /// <summary>
   /// Tests if the remoting to 3dsMax works.
   /// </summary>
   [TestMethod]
   public void RemotingTest()
   {
      IGlobal global = MaxRemoting.Global;
      IInterface ip = global.COREInterface;
      int newSceneNumChildren = ip.RootNode.NumberOfChildren;

      IINode boxNode = MaxRemoting.CreateBox();
      Assert.IsNotNull(boxNode, "Create box");
      Assert.AreEqual(newSceneNumChildren + 1, ip.RootNode.NumberOfChildren, "Add box");

      MaxRemoting.DeleteNode(boxNode);
      Assert.AreEqual(newSceneNumChildren, ip.RootNode.NumberOfChildren, "Delete box");
   }
}
}
