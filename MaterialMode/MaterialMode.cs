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
   [OutlinerPlugin(OutlinerPluginType.TreeMode)]
   [LocalizedDisplayName(typeof(Resources), "Mode_Materials")]
   public class MaterialMode : TreeMode
   {
      public MaterialMode(TreeView tree) : base(tree) 
      {
         this.PermanentFilter.Filters.Add(new UnassignedMaterialFilter());
      }

      protected override void FillTree()
      {
         this.AddNode(new UnassignedMaterialWrapper(), this.Tree.Nodes);

         IMtlBaseLib matLib = MaxInterfaces.COREInterface.SceneMtls;
         for (int i = 0; i < matLib.Count; i++)
         {
            IMtl mat = matLib[new IntPtr(i)] as IMtl;
            if (mat != null)
            {
               this.AddNode(mat, this.Tree.Nodes);
            }
         }
      }

      public override TreeNode AddNode(object node, TreeNodeCollection parentCol)
      {
         Throw.IfArgumentIsNull(node, "node");
         Throw.IfArgumentIsNull(parentCol, "parentCol");

         return this.AddNode(new MaterialWrapper(node as IMtl), parentCol);
      }

      public override TreeNode AddNode(IMaxNode wrapper, TreeNodeCollection parentCol)
      {
         TreeNode tn = base.AddNode(wrapper, parentCol);

         foreach (IMaxNode node in wrapper.ChildNodes)
         {
            this.AddNode(node, tn.Nodes);
         }

         return tn;
      }
   }
}
