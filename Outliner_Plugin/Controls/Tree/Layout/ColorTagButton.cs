using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Drawing;
using Outliner.Scene;
using Outliner.LayerTools;
using Autodesk.Max;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Outliner.Commands;

namespace Outliner.Controls.Tree.Layout
{
public class ColorTagButton : TreeNodeButton
{
   [XmlAttribute("button_width")]
   [DefaultValue(10)]
   public Int32 ButtonWidth { get; set; }

   public ColorTagButton()
   {
      this.ButtonWidth = 10;
   }

   public override int GetWidth(TreeNode tn)
   {
      return this.ButtonWidth;
   }

   public override int GetHeight(TreeNode tn)
   {
      if (this.Layout == null)
         return 0;

      return this.Layout.ItemHeight - 4;
   }

   protected override string GetTooltipText(TreeNode tn)
   {
      IMaxNodeWrapper wrapper = HelperMethods.GetMaxNode(tn);
      if (wrapper == null)
         return String.Empty;
      IAnimatable anim = wrapper.WrappedNode as IAnimatable;
      if (anim == null)
         return String.Empty;

      return OutlinerResources.Tooltip_ColorTag + ColorTags.GetTag(anim).ToString();
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

      Int32 opacity = this.Layout.TreeView.GetNodeOpacity(tn);
      Boolean hasTag = ColorTags.HasTag(node.WrappedNode as IAnimatable);
      
      Rectangle rBounds = this.GetBounds(tn);
      using (Pen linePen = new Pen(Color.FromArgb(opacity, Color.Black)))
      {
         graphics.DrawLine(linePen, rBounds.Left + 1, rBounds.Top
                                  , rBounds.Right - 2, rBounds.Top);
         graphics.DrawLine(linePen, rBounds.Left + 1, rBounds.Bottom - 1
                                  , rBounds.Right - 2, rBounds.Bottom - 1);
         graphics.DrawLine(linePen, rBounds.Left, rBounds.Top + 1
                                  , rBounds.Left, rBounds.Bottom - 2);
         graphics.DrawLine(linePen, rBounds.Right - 1, rBounds.Top + 1
                                  , rBounds.Right - 1, rBounds.Bottom - 2);         
      }

      if (hasTag)
      {
         Color color = Color.FromArgb(opacity, ColorTags.GetColor(node.WrappedNode as IAnimatable));
         Color gradColor = Color.FromArgb(opacity, Math.Min(color.R + 40, 255)
                                                 , Math.Min(color.G + 40, 255)
                                                 , Math.Min(color.B + 40, 255));
         using (LinearGradientBrush brush = new LinearGradientBrush(rBounds, gradColor, color, LinearGradientMode.Vertical))
         {
            rBounds.Inflate(-1, -1);
            graphics.FillRectangle(brush, rBounds);
         }
      }
   }


   private TreeNode clickedTn;
   public override void HandleMouseUp(MouseEventArgs e, TreeNode tn)
   {
      this.clickedTn = tn;

      ColorTag currentTag = ColorTag.None;
      IMaxNodeWrapper wrapper = HelperMethods.GetMaxNode(tn);
      if (wrapper != null)
      {
         currentTag = ColorTags.GetTag(wrapper.WrappedNode as IAnimatable);
      }

      ToolStripDropDown strip = new ToolStripDropDown();
      strip.Renderer = new ToolStripProfessionalRenderer(new OutlinerColorTable());
      strip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
      strip.Padding = new Padding(3, 2, 1, 1);

      foreach (ColorTag tag in Enum.GetValues(typeof(ColorTag)).Cast<ColorTag>())
      {
         if (tag == ColorTag.WireColor)
            strip.Items.Add(new ToolStripSeparator());

         if (tag != ColorTag.All)
         {
            ColorTagToolStripButton btn = new ColorTagToolStripButton(tag);
            btn.AutoSize = false;
            btn.Size = new Size(24, 24);
            btn.Click += new EventHandler(btn_Click);
            btn.ToolTipText = tag.ToString();
            btn.Checked = (tag == currentTag);
            strip.Items.Add(btn);
         }

         if (tag == ColorTag.None)
            strip.Items.Add(new ToolStripSeparator());
      }

      Rectangle bounds = this.GetBounds(tn);
      Point location = new Point( bounds.Left + (bounds.Width / 2) - (strip.Width / 2)
                                , bounds.Bottom + (e.Location.Y - bounds.Top) + 4);
      strip.Show(this.Layout.TreeView, location);
   }

   void btn_Click(object sender, EventArgs e)
   {
      ColorTagToolStripButton btn = sender as ColorTagToolStripButton;
      if (btn != null && this.clickedTn != null)
      {
         ColorTag tag = btn.ColorTag;

         ICollection<TreeNode> selNodes = this.Layout.TreeView.SelectedNodes;
         IEnumerable<IMaxNodeWrapper> nodes;
         if (selNodes.Contains(this.clickedTn))
            nodes = HelperMethods.GetMaxNodes(selNodes);
         else
            nodes = new List<IMaxNodeWrapper>(1) { HelperMethods.GetMaxNode(this.clickedTn) };

         SetColorTagCommand cmd = new SetColorTagCommand(nodes, tag);
         cmd.Execute(false);
      }
   }
}
}
