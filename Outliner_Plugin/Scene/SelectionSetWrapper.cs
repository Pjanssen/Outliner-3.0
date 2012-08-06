using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using MaxUtils;

namespace Outliner.Scene
{
   public class SelectionSetWrapper : IMaxNodeWrapper
   {
      private Int32 index;

      public SelectionSetWrapper(Int32 index) 
      {
         this.index = index;
      }

      public override object WrappedNode
      {
         get { return null; }
      }

      public override bool Equals(object obj)
      {
         SelectionSetWrapper otherObj = obj as SelectionSetWrapper;
         return otherObj != null && this.index == otherObj.index;
      }

      public override int GetHashCode()
      {
         return this.WrappedNode.GetHashCode();
      }

      public override IEnumerable<Object> ChildNodes
      {
         get
         {
            int numChildren = MaxInterfaces.SelectionSetManager.GetNamedSelSetItemCount(this.index);
            List<IINode> nodes = new List<IINode>(numChildren);
            for (int i = 0; i < numChildren; i++)
               nodes.Add(MaxInterfaces.SelectionSetManager.GetNamedSelSetItem(this.index, i));
            return nodes;
         }
      }

      public override string Name
      {
         get { return MaxInterfaces.SelectionSetManager.GetNamedSelSetName(this.index); }
         set { MaxInterfaces.SelectionSetManager.SetNamedSelSetName(this.index, ref value); }
      }

      public override Autodesk.Max.IClass_ID ClassID
      {
         get { return null; }
      }

      public override Autodesk.Max.SClass_ID SuperClassID
      {
         get { return MaxInterfaces.SelectionSetManager.Cd.SuperClassID; }
      }

      public override bool Selected
      {
         get { return false; }
      }

      public override bool IsNodeType(MaxNodeTypes types)
      {
         return types.HasFlag(MaxNodeTypes.SelectionSet);
      }


      public const String IMGKEY_SELECTIONSET = "selectionset";
      public override string ImageKey
      {
         get { return IMGKEY_SELECTIONSET; }
      }
   }
}
