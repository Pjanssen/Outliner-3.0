﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Commands;
using Autodesk.Max;
using System.Xml.Serialization;
using System.ComponentModel;
using PJanssen.Outliner.Controls.Tree.Layout;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.Plugins;
using System.Globalization;
using PJanssen.Outliner.Modes;
using PJanssen.Outliner.MaxUtils;

namespace PJanssen.Outliner.TreeNodeButtons
{
[OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
[LocalizedDisplayName(typeof(Resources), "Str_AddButton")]
public class AddButton : ImageButton
{
   public AddButton()
      : base(NodeButtonImages.GetButtonImages(NodeButtonImages.Images.Add)) { }


   [XmlAttribute("visible_types")]
   [DefaultValue(MaxNodeType.Layer | MaxNodeType.Material | MaxNodeType.SelectionSet)]
   public override MaxNodeType VisibleTypes
   {
      get { return base.VisibleTypes & (MaxNodeType.SelectionSet | MaxNodeType.Layer | MaxNodeType.Material); }
      set { base.VisibleTypes = value; }
   }

   public override bool ShowForNonMaxNodes
   {
      get { return false; }
   }

   public override bool IsEnabled(TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return false;

      IMaxNode node = TreeMode.GetMaxNode(tn);
      IEnumerable<TreeNode> selTreeNodes = this.Layout.TreeView.SelectedNodes;
      if (node == null || selTreeNodes.Count() == 0)
         return false;

      return node.CanAddChildNodes(TreeMode.GetMaxNodes(selTreeNodes));
   }

   public override void HandleMouseUp(WinForms::MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      if (!this.IsEnabled(tn))
         return;

      IMaxNode target = TreeMode.GetMaxNode(tn);
      if (target == null)
         return;

      IEnumerable<IMaxNode> nodes = TreeMode.GetMaxNodes(this.Layout.TreeView.SelectedNodes);
      String description = Resources.Command_AddTo + target.NodeTypeDisplayName;

      AddNodesCommand cmd = new AddNodesCommand(target, nodes, description);
      cmd.Execute();
      Viewports.Redraw();
   }


   protected override string GetTooltipText(TreeNode tn)
   {
      IMaxNode node = TreeMode.GetMaxNode(tn);
      if (node == null || !this.IsEnabled(tn))
         return null;

      return Resources.Tooltip_Add + node.NodeTypeDisplayName.ToLower(CultureInfo.InvariantCulture);
   }


   public override TreeNodeLayoutItem Copy()
   {
      AddButton newItem = new AddButton();

      newItem.PaddingLeft = this.PaddingLeft;
      newItem.PaddingRight = this.PaddingRight;
      newItem.VisibleTypes = this.VisibleTypes;
      newItem.InvertBehavior = this.InvertBehavior;
      newItem.imageDisabled = this.imageDisabled;
      newItem.imageDisabled_Filtered = this.imageDisabled_Filtered;
      newItem.imageEnabled = this.imageEnabled;
      newItem.imageEnabled_Filtered = this.imageEnabled_Filtered;

      return newItem;
   }
}
}
