using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Plugins;
using Outliner.Modes;
using Outliner.Controls.Tree;
using Outliner.MaxUtils;
using Autodesk.Max;
using Outliner.Scene;

namespace Outliner.Modes.MaterialMode
{
/// <summary>
/// Defines a TreeMode which lists all materials and the objects using them.
/// </summary>
[OutlinerPlugin(OutlinerPluginType.TreeMode)]
[LocalizedDisplayName(typeof(Resources), "Mode_Materials")]
public class MaterialMode : TreeMode
{
   private UnassignedMaterialWrapper unassignedMaterialWrapper;

   /// <summary>
   /// Initializes a new instance of the MaterialMode class.
   /// </summary>
   /// <param name="tree">The TreeView control to fill.</param>
   public MaterialMode(TreeView tree) : base(tree) 
   {
      this.unassignedMaterialWrapper = new UnassignedMaterialWrapper();
      base.AddPermanentFilter(new UnassignedMaterialFilter());
   }


   public override void Start()
   {
      this.RegisterNodeEventCallbackObject(new MaterialModeNodeCallback(this));
      
      base.Start();
   }


   #region FillTree, AddNode
      
   protected override void FillTree()
   {
      this.Tree.BeginUpdate();

      this.AddNode(this.unassignedMaterialWrapper, this.Tree.Nodes);

      IMtlBaseLib matLib = MaxInterfaces.COREInterface.SceneMtls;
      for (int i = 0; i < matLib.Count; i++)
      {
         IMtl mat = matLib[new IntPtr(i)] as IMtl;
         if (mat != null)
         {
            this.AddNode(mat, this.Tree.Nodes);
         }
      }

      this.Tree.Sort();

      this.Tree.EndUpdate();
   }

   public override TreeNode AddNode(IMaxNode wrapper, TreeNodeCollection parentCol)
   {
      TreeNode tn = base.AddNode(wrapper, parentCol);

      MaterialWrapper mtlWrapper = wrapper as MaterialWrapper;
      if (mtlWrapper != null)
      {
         AddObjects(mtlWrapper, tn.Nodes);
         AddTextureMaps(mtlWrapper, tn.Nodes);

         foreach (IMtl mtl in mtlWrapper.ChildMaterials)
         {
            this.AddNode(mtl, tn.Nodes);
         }
      } 
      else if (!(wrapper is INodeWrapper))
      {
         foreach (IMaxNode node in wrapper.ChildNodes)
         {
            this.AddNode(node, tn.Nodes);
         }
      }

      return tn;
   }

   private void AddObjects(MaterialWrapper mtlWrapper, TreeNodeCollection parentCol)
   {
      TreeNode objectsTn = new TreeNode(Resources.Str_Objects);
      objectsTn.ImageKey = "geometry";
      objectsTn.FontStyle = System.Drawing.FontStyle.Italic;
      foreach (IINode node in mtlWrapper.ChildINodes)
      {
         this.AddNode(node, objectsTn.Nodes);
      }
      parentCol.Add(objectsTn);
   }

   private void AddTextureMaps(MaterialWrapper mtlWrapper, TreeNodeCollection parentCol)
   {
      TreeNode texmapsTn = new TreeNode(Resources.Str_TextureMaps);
      texmapsTn.FontStyle = System.Drawing.FontStyle.Italic;
      foreach (ITexmap texmap in mtlWrapper.ChildTextureMaps)
      {
         this.AddNode(texmap, texmapsTn.Nodes);
      }
      parentCol.Add(texmapsTn);
   }


   protected override IDragDropHandler CreateDragDropHandler(IMaxNode node)
   {
      if (node is MaterialWrapper || node is UnassignedMaterialWrapper)
         return new MaterialDragDropHandler(node);

      INodeWrapper inodeWrapper = node as INodeWrapper;
      if (inodeWrapper != null)
         return new INodeDragDropHandler(inodeWrapper);

      return base.CreateDragDropHandler(node);
   }


   protected override TreeNode GetParentTreeNode(IINode node)
   {
      if (node == null)
         return null;

      IMtl mtl = node.Mtl;
      if (mtl == null)
         return this.GetFirstTreeNode(this.unassignedMaterialWrapper);
      else
         return this.GetFirstTreeNode(mtl);
   }

   #endregion


   #region NodeEventCallback

   private class MaterialModeNodeCallback : TreeModeNodeEventCallbacks
   {
      private MaterialMode materialMode;
      public MaterialModeNodeCallback(MaterialMode mode) : base(mode)
      {
         this.materialMode = mode;
      }

      public override void MaterialStructured(ITab<UIntPtr> nodes)
      {
         foreach (IINode node in nodes.NodeKeysToINodeList())
         {
            TreeNode tn = this.TreeMode.GetFirstTreeNode(node);
            TreeNode matTn = this.TreeMode.GetFirstTreeNode(node.Mtl);

            if (tn == null || matTn == null)
               continue;

            matTn.Nodes.Add(tn);
            this.Tree.AddToSortQueue(tn);
         }

         this.Tree.StartTimedSort(true);
      }
   }

   #endregion
}
}
