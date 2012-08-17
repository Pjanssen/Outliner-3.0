using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using MaxUtils;
using Outliner.LayerTools;

namespace Outliner.Scene
{
   public class SelectionSetWrapper : IMaxNodeWrapper
   {
      public static int SelSetIndexByName(String name)
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

      private String name;
      internal void UpdateName(String newName)
      {
         this.name = newName;
      }

      public SelectionSetWrapper(Int32 index) 
      {
         this.name = MaxInterfaces.SelectionSetManager.GetNamedSelSetName(index);
      }

      public SelectionSetWrapper(String name)
      {
         this.name = name;
      }

      public override object WrappedNode
      {
         get { return this.name; }
      }

      public override bool IsValid
      {
         get 
         {
            return true;
         }
      }

      public override bool Equals(object obj)
      {
         SelectionSetWrapper otherObj = obj as SelectionSetWrapper;
         return otherObj != null && this.name == otherObj.name;
      }

      public override int GetHashCode()
      {
         return this.name.GetHashCode();
      }


      public override IEnumerable<Object> ChildNodes
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


      public override bool CanAddChildNode(IMaxNodeWrapper node)
      {
         return node is IINodeWrapper && !this.WrappedChildNodes.Contains(node);
      }
      public override void AddChildNode(IMaxNodeWrapper node)
      {
         this.AddChildNodes(new List<IMaxNodeWrapper>() { node });
      }
      public override void AddChildNodes(IEnumerable<IMaxNodeWrapper> nodes)
      {
         if (nodes == null)
            return;

         IINodeTab nodeTab = HelperMethods.ToIINodeTab(this.ChildNodes);
         nodeTab.Resize(nodeTab.Count + nodes.Count());

         foreach (IMaxNodeWrapper node in nodes)
         {
            IINodeWrapper inodeWrapper = node as IINodeWrapper;
            if (inodeWrapper == null)
               continue;
            else
               nodeTab.AppendNode(inodeWrapper.IINode, false, 0);
         }

         MaxInterfaces.SelectionSetManager.ReplaceNamedSelSet(nodeTab, ref this.name);
      }


      public override bool CanRemoveChildNode(IMaxNodeWrapper node)
      {
         IINodeWrapper inodeWrapper = node as IINodeWrapper;
         if (inodeWrapper == null)
            return false;

         return this.ChildIINodes.Contains(inodeWrapper.IINode);
      }
      public override void RemoveChildNode(IMaxNodeWrapper node)
      {
         this.RemoveChildNodes(new List<IMaxNodeWrapper>() { node });
      }
      public override void RemoveChildNodes(IEnumerable<IMaxNodeWrapper> nodes)
      {
         if (nodes == null)
            return;

         IINodeTab nodeTab = HelperMethods.ToIINodeTab(this.ChildNodes);

         foreach (IMaxNodeWrapper node in nodes)
         {
            IINodeWrapper inodeWrapper = node as IINodeWrapper;
            if (inodeWrapper == null)
               continue;
            else
               nodeTab.RemoveNode(inodeWrapper.IINode);
         }

         MaxInterfaces.SelectionSetManager.ReplaceNamedSelSet(nodeTab, ref this.name);
      }


      public virtual void ReplaceNodeset(IEnumerable<IMaxNodeWrapper> nodes)
      {
         if (nodes == null)
            return;

         IINodeTab nodeTab = HelperMethods.ToIINodeTab(nodes);
         MaxInterfaces.SelectionSetManager.ReplaceNamedSelSet(nodeTab, ref this.name);
      }


      public override string Name
      {
         get { return this.name; }
         set 
         {
            MaxInterfaces.SelectionSetManager.SetNamedSelSetName(this.Index, ref value);
            this.name = value;
         }
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

      public override Autodesk.Max.IClass_ID ClassID
      {
         get { return null; }
      }

      public override Autodesk.Max.SClass_ID SuperClassID
      {
         get { return SClass_ID.Utility; }
      }

      public override bool Selected
      {
         get { return false; }
      }

      public override bool IsNodeType(MaxNodeTypes types)
      {
         return types.HasFlag(MaxNodeTypes.SelectionSet);
      }


      private Boolean getSelSetProperty(Func<IINode, Boolean> fn)
      {
         IEnumerable<IINode> childIINodes = this.ChildIINodes;
         if (childIINodes.Count() == 0)
            return false;
         else
            return this.ChildIINodes.All(fn);
      }
      private void setSelSetProperty(Action<IINode> fn)
      {
         this.ChildIINodes.ForEach(fn);
      }

      public override bool IsHidden
      {
         get
         {
            return this.getSelSetProperty(n => n.IsObjectHidden);
         }
         set
         {
            this.setSelSetProperty(n => n.Hide(value));
         }
      }

      public override bool IsFrozen
      {
         get
         {
            return this.getSelSetProperty(n => n.IsObjectFrozen);
         }
         set
         {
            this.setSelSetProperty(n => n.IsFrozen = value);
         }
      }

      public override bool Renderable
      {
         get
         {
            return getSelSetProperty(n => n.Renderable != 0);
         }
         set
         {
            this.setSelSetProperty(n => n.SetRenderable(value));
         }
      }

      public override bool BoxMode
      {
         get
         {
            return this.getSelSetProperty(n => n.BoxMode_ != 0);
         }
         set
         {
            this.setSelSetProperty(n => n.BoxMode(value));
         }
      }

      public override bool XRayMtl
      {
         get
         {
            return this.getSelSetProperty(n => n.XRayMtl_ != 0);
         }
         set
         {
            this.setSelSetProperty(n => n.XRayMtl(value));
         }
      }

      public override System.Drawing.Color WireColor
      {
         get
         {
            return System.Drawing.Color.Black;
         }
         set 
         {
            this.setSelSetProperty(n => n.WireColor = value);
         }
      }

      public override ColorTag ColorTag
      {
         get
         {
            IEnumerable<IINode> childIINodes = this.ChildIINodes;
            if (childIINodes.Count() == 0)
               return LayerTools.ColorTag.None;
            else
               return childIINodes.Aggregate(LayerTools.ColorTag.All, (tag, n) =>
                                                tag &= LayerTools.ColorTags.GetTag(n));
         }
         set
         {
            this.setSelSetProperty(n => ColorTags.SetTag(n, value));
         }
      }

      public const String ImgKeySelectionSet = "selectionset";
      public override string ImageKey
      {
         get { return ImgKeySelectionSet; }
      }
   }
}
