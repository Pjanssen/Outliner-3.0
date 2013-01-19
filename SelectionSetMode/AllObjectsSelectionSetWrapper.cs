using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.MaxUtils;
using Outliner.Scene;

namespace Outliner.Modes.SelectionSet
{
   public class AllObjectsSelectionSetWrapper : SelectionSetWrapper
   {
      public const String SelSetName = "--Internal_AllObjects--";
      public AllObjectsSelectionSetWrapper() : base(SelSetName) { }

      public override string Name
      {
         get { return Resources.Str_AllObjectsSelSet; }
         set { throw new NotSupportedException(); }
      }

      public override bool CanEditName
      {
         get { return false; }
      }

      public override bool CanDelete
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

      public override bool CanAddChildNode(IMaxNode node)
      {
         return false;
      }
      public override void AddChildNode(IMaxNode node) { }
      public override void AddChildNodes(IEnumerable<IMaxNode> nodes) { }

      public override bool CanRemoveChildNode(IMaxNode node)
      {
         return false;
      }
      public override void RemoveChildNode(IMaxNode node) { }
      public override void RemoveChildNodes(IEnumerable<IMaxNode> nodes) { }

      public override bool Equals(object obj)
      {
         return obj is AllObjectsSelectionSetWrapper;
      }

      public override int GetHashCode()
      {
         return SelSetName.GetHashCode();
      }

      public override string ToString()
      {
         return "AllObjectsSelectionSetWrapper";
      }
   }
}
