using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using WinForms = System.Windows.Forms;
using Outliner.Scene;
using Autodesk.Max;
using Outliner.Commands;
using System.Xml.Serialization;
using System.ComponentModel;
using Outliner.MaxUtils;
using Outliner.NodeSorters;
using Outliner.Controls.Tree.Layout;
using Outliner.Controls.Tree;
using Outliner.Plugins;

namespace Outliner.TreeNodeButtons
{
[OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
public class WireColorButton : NodePropertyButton
{
   [XmlAttribute("button_width")]
   [DefaultValue(12)]
   public Int32 ButtonWidth { get; set; }

   [XmlAttribute("button_height")]
   [DefaultValue(8)]
   public Int32 ButtonHeight { get; set; }

   public WireColorButton()
   {
      this.ButtonWidth = 12;
      this.ButtonHeight = 8;
   }

   public override TreeNodeLayoutItem Copy()
   {
      WireColorButton newItem = new WireColorButton();

      newItem.PaddingLeft = this.PaddingLeft;
      newItem.PaddingRight = this.PaddingRight;
      newItem.VisibleTypes = this.VisibleTypes;
      newItem.InvertBehavior = this.InvertBehavior;
      newItem.ButtonHeight = this.ButtonHeight;
      newItem.ButtonWidth = this.ButtonWidth;

      return newItem;
   }

   public override int GetWidth(TreeNode tn)
   {
      return this.ButtonWidth;
   }

   public override int GetHeight(TreeNode tn)
   {
      return this.ButtonHeight;
   }

   public override bool CenterVertically
   {
      get { return true; }
   }

   protected override NodeProperty Property
   {
      get { return NodeProperty.WireColor; }
   }

   protected override string ToolTipEnabled
   {
      get { return Resources.Tooltip_WireColor; }
   }

   protected override Image ImageEnabled { get { return null; } }
   protected override Image ImageDisabled { get { return null; } }
   protected override Image ImageEnabled_Filtered { get { return null; } }
   protected override Image ImageDisabled_Filtered { get { return null; } }

   public override void Draw(Graphics graphics, TreeNode tn)
   {
      if (graphics == null || tn == null)
         return;

      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return;

      if (node.IsPropertyInherited(this.Property))
      //if (this.isInheritedFromLayer(tn))
      {
         base.Draw(graphics, tn);
         return;
      }

      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      Color wc = node.WireColor;
      Rectangle rBounds = this.GetBounds(tn);
      using (Pen linePen = new Pen(Color.Black))
      using (SolidBrush brush = new SolidBrush(wc))
      {
         graphics.FillRectangle(brush, rBounds);
         graphics.DrawRectangle(linePen, rBounds);

      }
   }

   public override void HandleMouseUp(System.Windows.Forms.MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return;

      Color wc = node.WireColor;
      IInterface ip = MaxInterfaces.Global.COREInterface;
      if (ip.NodeColorPicker(ip.MAXHWnd, ref wc))
      {
         TreeView tree = this.Layout.TreeView;
         IEnumerable<TreeNode> nodes = null;
         if (tn.IsSelected && !HelperMethods.ControlPressed)
            nodes = tree.SelectedNodes;
         else
            nodes = new List<TreeNode>(1) { tn };

         IEnumerable<IMaxNodeWrapper> maxNodes = HelperMethods.GetMaxNodes(nodes);
         SetNodePropertyCommand<Color> cmd = new SetNodePropertyCommand<Color>(maxNodes, "WireColor", ColorHelpers.FromMaxColor(wc));
         cmd.Execute(true);

         if (tree.NodeSorter is NodePropertySorter &&
            ((NodePropertySorter)tree.NodeSorter).Property == this.Property)
         {
            tree.StartTimedSort(nodes);
         }
      }
   }
}
}
