using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Outliner.Scene;
using Autodesk.Max;
using Outliner.Commands;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Outliner.Controls.Layout
{
public class WireColorButton : TreeNodeButton
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

   private Boolean inheritFromLayer(IMaxNodeWrapper node)
   {
      return node is IINodeWrapper 
             && ((IINodeWrapper)node).NodeLayerProperties.ColorByLayer;
   }

   public override void Draw(Graphics graphics, TreeNode tn)
   {
      if (graphics == null || tn == null)
         return;

      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return;

      Color wc = node.WireColor;
      Rectangle rBounds = this.GetBounds(tn);
      using (Pen linePen = new Pen(Color.Black))
      {
         if (this.inheritFromLayer(node))
         {
            Image img = OutlinerResources.layer_small;
            graphics.DrawImage(img, rBounds.Left + (int)Math.Ceiling((rBounds.Width - img.Width) / 2f), 
                                    tn.Bounds.Top + (int)Math.Ceiling((tn.Bounds.Height - img.Height) / 2f), 
                                    img.Width, img.Height);
         }
         else
         {
            using (SolidBrush brush = new SolidBrush(wc))
            {
               graphics.FillRectangle(brush, rBounds);
               graphics.DrawRectangle(linePen, rBounds);
            }
         }
      }
   }

   protected override string GetTooltipText(TreeNode tn)
   {
      if (tn == null)
         return null;

      String tooltip = OutlinerResources.Tooltip_WireColor;

      if (this.inheritFromLayer(HelperMethods.GetMaxNode(tn)))
         tooltip += " " + OutlinerResources.Tooltip_Inherited;

      return tooltip;
   }

   public override void HandleClick(System.Windows.Forms.MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return;

      Color wc = node.WireColor;
      IInterface ip = Autodesk.Max.GlobalInterface.Instance.COREInterface;
      if (ip.NodeColorPicker(ip.MAXHWnd, ref wc))
      {
         TreeView tree = this.Layout.TreeView;
         IEnumerable<IMaxNodeWrapper> nodes = null;
         if (HelperMethods.ControlPressed && tree.IsSelectedNode(tn))
            nodes = HelperMethods.GetMaxNodes(tree.SelectedNodes);
         else
            nodes = new List<IMaxNodeWrapper>(1) { node };

         SetWireColorCommand cmd = new SetWireColorCommand(nodes, ColorHelpers.FromMaxColor(wc));
         cmd.Execute(true);
      }
   }
}
}
