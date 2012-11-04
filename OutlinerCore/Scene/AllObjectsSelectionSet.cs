using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.MaxUtils;

namespace Outliner.Scene
{
   public class AllObjectsSelectionSet : SelectionSetWrapper
   {
      public const String SelSetName = "--Internal_AllObjects--"; 
      public AllObjectsSelectionSet() : base(SelSetName) { }

      public override string Name
      {
         get { return OutlinerResources.AllObjectsSelSet; }
         set { throw new NotSupportedException(); }
      }

      public override bool CanEditName
      {
         get { return false; }
      }


      public override IEnumerable<IINode> ChildIINodes
      {
         get { return this.getChildren(MaxInterfaces.COREInterface.RootNode); }
      }

      private IEnumerable<IINode> getChildren(IINode node)
      {
         List<IINode> nodes = new List<IINode>();
         for (int i = 0; i < node.NumberOfChildren; i++)
         {
            IINode child = node.GetChildNode(i);
            nodes.Add(child);
            nodes.AddRange(getChildren(child));
         }
         return nodes;
      }

      public override bool CanAddChildNode(IMaxNodeWrapper node)
      {
         return false;
      }
      public override void AddChildNode(IMaxNodeWrapper node) { }
      public override void AddChildNodes(IEnumerable<IMaxNodeWrapper> nodes) { }

      public override bool CanRemoveChildNode(IMaxNodeWrapper node)
      {
         return false;
      }
      public override void RemoveChildNode(IMaxNodeWrapper node) { }
      public override void RemoveChildNodes(IEnumerable<IMaxNodeWrapper> nodes) { }

      public override bool Equals(object obj)
      {
         return obj is AllObjectsSelectionSet;
      }

      public override int GetHashCode()
      {
         return SelSetName.GetHashCode();
      }
   }
}
