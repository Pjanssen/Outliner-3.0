using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;

namespace Outliner.Modes.MaterialMode
{
   public class UnassignedMaterialWrapper : IMaxNodeWrapper
   {
      public override int ChildNodeCount
      {
         get { return 0; }
      }

      public override IEnumerable<object> ChildNodes
      {
         get 
         { 
            return MaxScene.AllObjects.Where(n => n.IINode.Mtl != null)
                                       .Select(n => n.IINode); 
         }
      }

      public override IEnumerable<IMaxNodeWrapper> WrappedChildNodes
      {
         get
         {
            return MaxScene.AllObjects.Where(n => n.IINode.Mtl == null);
         }
      }

      public override Autodesk.Max.IClass_ID ClassID
      {
         get { return null; }
      }

      public override Autodesk.Max.SClass_ID SuperClassID
      {
         get { return Autodesk.Max.SClass_ID.Material; }
      }

      public override bool Equals(object obj)
      {
         return obj is UnassignedMaterialWrapper;
      }

      public override int GetHashCode()
      {
         return 0;
      }

      public override bool IsNodeType(MaxNodeTypes types)
      {
         return (types & MaxNodeTypes.Material) == MaxNodeTypes.Material;
      }

      public override string Name
      {
         get { return Resources.Name_Unassigned; }
         set { }
      }

      public override bool CanEditName
      {
         get { return false; }
      }

      public override bool Selected
      {
         get { return false; }
      }

      public override object WrappedNode
      {
         get { return "--Internal_UnassignedMaterial--"; }
      }

      public override string ImageKey
      {
         get
         {
            return "material_unassigned";
         }
      }
   }
}
