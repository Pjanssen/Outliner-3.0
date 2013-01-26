using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Modes.MaterialMode;
using Outliner.Scene;

namespace Outliner.Scene
{
   public class UnassignedMaterialWrapper : MaxNodeWrapper
   {
      public UnassignedMaterialWrapper() { }

      public override object BaseObject
      {
         get { return "--Internal_UnassignedMaterial--"; }
      }


      #region Equality

      public override bool Equals(object obj)
      {
         return obj is UnassignedMaterialWrapper;
      }

      public override int GetHashCode()
      {
         return this.BaseObject.GetHashCode();
      }

      #endregion


      #region Childnodes

      public override int ChildNodeCount
      {
         get
         {
            return this.ChildNodes.Count();
         }
      }

      public override IEnumerable<object> ChildBaseObjects
      {
         get
         {
            return MaxScene.AllObjects.Where(n => n.INode.Mtl == null)
                                      .Select(n => n.INode);
         }
      }

      public override IEnumerable<IMaxNode> ChildNodes
      {
         get
         {
            return MaxScene.AllObjects.Where(n => n.INode.Mtl == null);
         }
      }

      #endregion


      #region Node Type

      protected override MaxNodeType MaxNodeType
      {
         get { return MaxNodeType.Material; }
      }

      #endregion


      #region Name
      
      public override string Name
      {
         get { return Resources.Name_Unassigned; }
         set { }
      }

      #endregion


      #region ImageKey

      public override string ImageKey
      {
         get { return "material_unassigned"; }
      }

      #endregion

      public override string ToString()
      {
         return "UnassignedMaterialWrapper";
      }
   }
}
