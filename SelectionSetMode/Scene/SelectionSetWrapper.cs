using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.MaxUtils;
using Outliner.Modes.SelectionSet;

namespace Outliner.Scene
{
   public class SelectionSetWrapper : MaxNodeWrapper
   {
      private String name;

      public SelectionSetWrapper(Int32 index)
      {
         this.name = MaxInterfaces.SelectionSetManager.GetNamedSelSetName(index);
      }

      public SelectionSetWrapper(String name)
      {
         Throw.IfArgumentIsNull(name, "name");
         this.name = name;
      }

      public override object BaseObject
      {
         get { return this.name; }
      }

      public override bool IsAggregate
      {
         get { return true; }
      }

      public override bool IsValid
      {
         get
         {
            IINamedSelectionSetManager selSetManager = MaxInterfaces.SelectionSetManager;
            for (int i = 0; i < selSetManager.NumNamedSelSets; i++)
            {
               if (selSetManager.GetNamedSelSetName(i) == this.Name)
                  return true;
            }

            return false;
         }
      }

      public override bool IsSelected
      {
         get { return false; }
         set
         {
            this.ChildNodes.ForEach(n => n.IsSelected = value);
         }
      }


      #region SelectionSet Specific

      private Int32 GetSelSetIndex(String name)
      {
         IINamedSelectionSetManager selSetManager = MaxInterfaces.SelectionSetManager;
         for (int i = 0; i < selSetManager.NumNamedSelSets; i++)
         {
            if (selSetManager.GetNamedSelSetName(i) == name)
               return i;
         }
         return -1;
      }

      public static int SelSetIndexByName(String name)
      {
         Throw.IfArgumentIsNull(name, "name");

         IINamedSelectionSetManager selSetMan = MaxInterfaces.SelectionSetManager;
         for (int i = 0; i < selSetMan.NumNamedSelSets; i++)
         {
            String selSetName = selSetMan.GetNamedSelSetName(i);
            if (selSetName == name)
            {
               return i;
            }
         }

         return -1;
      }

      public void UpdateName(String newName)
      {
         Throw.IfArgumentIsNull(newName, "newName");

         this.name = newName;
      }

      public virtual void ReplaceNodeset(IEnumerable<IMaxNode> nodes)
      {
         Throw.IfArgumentIsNull(nodes, "nodes");

         IINodeTab nodeTab = HelperMethods.ToIINodeTab(nodes);
         MaxInterfaces.SelectionSetManager.ReplaceNamedSelSet(nodeTab, ref this.name);
      }

      public virtual int Index
      {
         get
         {
            IINamedSelectionSetManager selSetMan = MaxInterfaces.SelectionSetManager;
            for (int i = 0; i < selSetMan.NumNamedSelSets; i++)
            {
               String selSetName = selSetMan.GetNamedSelSetName(i);
               if (selSetName == name)
               {
                  return i;
               }
            }

            return -1;
         }
      }

      #endregion


      #region Equality

      public override bool Equals(object obj)
      {
         SelectionSetWrapper otherObj = obj as SelectionSetWrapper;
         return otherObj != null && this.name == otherObj.name;
      }

      public override int GetHashCode()
      {
         return this.name.GetHashCode();
      }

      #endregion


      #region Delete

      public override bool CanDelete
      {
         get { return true; }
      }

      public override void Delete()
      {
         if (this.CanDelete)
         {
            MaxInterfaces.SelectionSetManager.RemoveNamedSelSet(ref this.name);
         }
      }

      #endregion


      #region Childnodes

      public override int ChildNodeCount
      {
         get
         {
            return MaxInterfaces.SelectionSetManager.GetNamedSelSetItemCount(this.Index);
         }
      }

      public override IEnumerable<object> ChildBaseObjects
      {
         get
         {
            return this.ChildIINodes;
         }
      }

      public virtual IEnumerable<IINode> ChildIINodes
      {
         get
         {
            int index = this.Index;
            IINodeTab nodeTab = MaxInterfaces.Global.INodeTabNS.Create();
            MaxInterfaces.SelectionSetManager.GetNamedSelSetList(nodeTab, index);
            return nodeTab.ToIEnumerable();
         }
      }

      public override bool CanAddChildNode(IMaxNode node)
      {
         if (node == null)
            return false;

         if (node.IsAggregate)
            return this.CanAddChildNodes(node.ChildNodes);

         return node is INodeWrapper && !this.ChildNodes.Contains(node);
      }

      public override void AddChildNode(IMaxNode node)
      {
         Throw.IfArgumentIsNull(node, "node");

         if (node.IsAggregate)
            this.AddChildNodes(node.ChildNodes);
         
         this.AddChildNodes(node.ToEnumerable());
      }

      public override void AddChildNodes(IEnumerable<IMaxNode> nodes)
      {
         Throw.IfArgumentIsNull(nodes, "nodes");

         IINodeTab nodeTab = HelperMethods.ToIINodeTab(this.ChildNodes);
         nodeTab.Resize(nodeTab.Count + nodes.Count());

         foreach (IMaxNode node in nodes)
         {
            if (node.IsAggregate)
            {
               AppendNodesToNodeTab(node.ChildBaseObjects, nodeTab);
               continue;
            }

            INodeWrapper inodeWrapper = node as INodeWrapper;
            if (inodeWrapper == null)
               continue;
            else
               nodeTab.AppendNode(inodeWrapper.INode, false, 0);
         }

         MaxInterfaces.SelectionSetManager.ReplaceNamedSelSet(nodeTab, ref this.name);
      }

      private static void AppendNodesToNodeTab(IEnumerable<Object> nodes, IINodeTab nodeTab)
      {
         foreach (object child in nodes)
         {
            IINode inode = child as IINode;
            if (inode != null)
               nodeTab.AppendNode(inode, false, 0);
         }
      }

      public override bool CanRemoveChildNode(IMaxNode node)
      {
         if (node.IsAggregate)
            return this.CanRemoveChildNodes(node.ChildNodes);

         INodeWrapper nodeWrapper = node as INodeWrapper;
         return nodeWrapper != null && this.ChildBaseObjects.Contains(nodeWrapper.INode);
      }

      public override void RemoveChildNode(IMaxNode node)
      {
         Throw.IfArgumentIsNull(node, "node");

         if (node.IsAggregate)
            this.RemoveChildNodes(node.ChildNodes);

         this.RemoveChildNodes(node.ToEnumerable());
      }

      public override void RemoveChildNodes(IEnumerable<IMaxNode> nodes)
      {
         Throw.IfArgumentIsNull(nodes, "nodes");

         IINodeTab nodeTab = HelperMethods.ToIINodeTab(this.ChildNodes);

         foreach (IMaxNode node in nodes)
         {
            if (node.IsAggregate)
            {
               AppendNodesToNodeTab(node.ChildBaseObjects, nodeTab);
               continue;
            }

            INodeWrapper inodeWrapper = node as INodeWrapper;
            if (inodeWrapper == null)
               continue;
            else
               nodeTab.RemoveNode(inodeWrapper.INode);
         }

         MaxInterfaces.SelectionSetManager.ReplaceNamedSelSet(nodeTab, ref this.name);
      }

      #endregion


      #region Node Type

      protected override MaxNodeType MaxNodeType
      {
         get { return MaxNodeType.SelectionSet; }
      }

      public override IClass_ID ClassID
      {
         get { return null; }
      }

      public override SClass_ID SuperClassID
      {
         get { return SClass_ID.Utility; }
      }

      #endregion


      #region Name

      public override string Name
      {
         get { return this.name; }
         set
         {
            Throw.IfArgumentIsNull(value, "value");
            MaxInterfaces.SelectionSetManager.SetNamedSelSetName(this.Index, ref value);
            this.name = value;
         }
      }

      public override bool CanEditName
      {
         get { return true; }
      }

      public override string NodeTypeDisplayName
      {
         get { return Resources.Str_SelSet; }
      }

      #endregion


      #region Node Properties

      private Boolean IntToBool(int i)
      {
         return i != 0;
      }

      private int BoolToInt(bool b)
      {
         return b ? 1 : 0;
      }

      private Boolean GetSelSetProperty(Func<IINode, Boolean> fn)
      {
         IEnumerable<IINode> childIINodes = this.ChildIINodes;
         if (childIINodes.Count() == 0)
            return false;
         else
            return this.ChildIINodes.All(fn);
      }
      private void SetSelSetProperty(Action<IINode> fn)
      {
         this.ChildIINodes.ForEach(fn);
      }

      public override bool GetNodeProperty(BooleanNodeProperty property)
      {
         switch (property)
         {
            case BooleanNodeProperty.IsHidden:
               return GetSelSetProperty(n => n.IsObjectHidden);
            case BooleanNodeProperty.IsFrozen:
               return GetSelSetProperty(n => n.IsObjectFrozen);
            case BooleanNodeProperty.SeeThrough:
               return GetSelSetProperty(n => IntToBool(n.XRayMtl_));
            case BooleanNodeProperty.BoxMode:
               return GetSelSetProperty(n => IntToBool(n.BoxMode_));
            case BooleanNodeProperty.BackfaceCull:
               return GetSelSetProperty(n => IntToBool(n.BackCull_));
            case BooleanNodeProperty.AllEdges:
               return GetSelSetProperty(n => IntToBool(n.AllEdges_));
            case BooleanNodeProperty.VertexTicks:
               return GetSelSetProperty(n => IntToBool(n.VertTicks));
            case BooleanNodeProperty.Trajectory:
               return GetSelSetProperty(n => IntToBool(n.TrajectoryON));
            case BooleanNodeProperty.IgnoreExtents:
               return GetSelSetProperty(n => IntToBool(n.IgnoreExtents_));
            case BooleanNodeProperty.FrozenInGray:
               return GetSelSetProperty(n => IntToBool(n.ShowFrozenWithMtl));
            case BooleanNodeProperty.Renderable:
               return GetSelSetProperty(n => IntToBool(n.Renderable));
            case BooleanNodeProperty.InheritVisibility:
               return GetSelSetProperty(n => n.InheritVisibility);
            case BooleanNodeProperty.PrimaryVisibility:
               return GetSelSetProperty(n => IntToBool(n.PrimaryVisibility));
            case BooleanNodeProperty.SecondaryVisibility:
               return GetSelSetProperty(n => IntToBool(n.SecondaryVisibility));
            case BooleanNodeProperty.ReceiveShadows:
               return GetSelSetProperty(n => IntToBool(n.RcvShadows));
            case BooleanNodeProperty.CastShadows:
               return GetSelSetProperty(n => IntToBool(n.CastShadows));
            case BooleanNodeProperty.ApplyAtmospherics:
               return GetSelSetProperty(n => IntToBool(n.ApplyAtmospherics));
            case BooleanNodeProperty.RenderOccluded:
               return GetSelSetProperty(n => n.RenderOccluded);
            default:
               return base.GetNodeProperty(property);
         }
      }

      public override void SetNodeProperty(BooleanNodeProperty property, bool value)
      {
         switch (property)
         {
            case BooleanNodeProperty.IsHidden:
               this.SetSelSetProperty(n => n.Hide(value));
               break;
            case BooleanNodeProperty.IsFrozen:
               this.SetSelSetProperty(n => n.IsFrozen = value);
               break;
            case BooleanNodeProperty.SeeThrough:
               this.SetSelSetProperty(n => n.XRayMtl(value));
               break;
            case BooleanNodeProperty.BoxMode:
               this.SetSelSetProperty(n => n.BoxMode(value));
               break;
            case BooleanNodeProperty.BackfaceCull:
               this.SetSelSetProperty(n => n.BackCull(value));
               break;
            case BooleanNodeProperty.AllEdges:
               this.SetSelSetProperty(n => n.AllEdges(value));
               break;
            case BooleanNodeProperty.VertexTicks:
               this.SetSelSetProperty(n => n.VertTicks = BoolToInt(value));
               break;
            case BooleanNodeProperty.Trajectory:
               this.SetSelSetProperty(n => n.SetTrajectoryON(value));
               break;
            case BooleanNodeProperty.IgnoreExtents:
               this.SetSelSetProperty(n => n.IgnoreExtents(value));
               break;
            case BooleanNodeProperty.FrozenInGray:
               this.SetSelSetProperty(n => n.SetShowFrozenWithMtl(value));
               break;
            case BooleanNodeProperty.Renderable:
               this.SetSelSetProperty(n => n.SetRenderable(value));
               break;
            case BooleanNodeProperty.InheritVisibility:
               this.SetSelSetProperty(n => n.InheritVisibility = value);
               break;
            case BooleanNodeProperty.PrimaryVisibility:
               this.SetSelSetProperty(n => n.SetPrimaryVisibility(value));
               break;
            case BooleanNodeProperty.SecondaryVisibility:
               this.SetSelSetProperty(n => n.SetSecondaryVisibility(value));
               break;
            case BooleanNodeProperty.ReceiveShadows:
               this.SetSelSetProperty(n => n.SetRcvShadows(value));
               break;
            case BooleanNodeProperty.CastShadows:
               this.SetSelSetProperty(n => n.SetCastShadows(value));
               break;
            case BooleanNodeProperty.ApplyAtmospherics:
               this.SetSelSetProperty(n => n.SetApplyAtmospherics(value));
               break;
            case BooleanNodeProperty.RenderOccluded:
               this.SetSelSetProperty(n => n.RenderOccluded = value);
               break;
            default:
               base.SetNodeProperty(property, value);
               break;
         }
      }


      public override bool IsNodePropertyInherited(NodeProperty property)
      {
         return this.ChildNodes.All(n => n.IsNodePropertyInherited(property));
      }


      public override Color WireColor
      {
         get
         {
            return System.Drawing.Color.Black;
         }
         set
         {
            Throw.IfArgumentIsNull(value, "value");
            this.SetSelSetProperty(n => n.WireColor = value);
         }
      }

      #endregion


      #region ImageKey

      public const String ImgKeySelectionSet = "selectionset";
      public override string ImageKey
      {
         get { return ImgKeySelectionSet; }
      }

      #endregion


      public override String ToString()
      {
         return "SelectionSetWrapper (" + this.Name + ")";
      }
   }
}
