using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using WinForms = System.Windows.Forms;
using Outliner.Scene;
using Autodesk.Max;
using Outliner.Commands;
using Outliner.Controls.ContextMenu;
using Outliner.Plugins;
using Outliner.Controls.Tree.Layout;
using Outliner.Controls.Tree;

namespace Outliner.ColorTags
{
[OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
[LocalizedDisplayName(typeof(Resources), "Button_ColorTag")]
public class ColorTagButton : TreeNodeButton
{
   [XmlAttribute("button_width")]
   [DefaultValue(10)]
   public Int32 ButtonWidth { get; set; }

   public ColorTagButton()
   {
      this.ButtonWidth = 10;
   }

   [XmlAttribute("visible_types")]
   [DefaultValue(MaxNodeTypes.Layer | MaxNodeTypes.Object | MaxNodeTypes.SelectionSet)]
   public override MaxNodeTypes VisibleTypes
   {
      get { return MaxNodeTypes.Layer | MaxNodeTypes.Object | MaxNodeTypes.SelectionSet; }
      set { }
   }


   public override TreeNodeLayoutItem Copy()
   {
      ColorTagButton newItem = new ColorTagButton();

      newItem.ButtonWidth = this.ButtonWidth;
      newItem.PaddingLeft = this.PaddingLeft;
      newItem.PaddingRight = this.PaddingRight;

      return newItem;
   }


   public override int GetAutoWidth(TreeNode tn)
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

      return String.Format(Resources.Tooltip_NodeButton, wrapper.GetColorTag().ToString());
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
      ColorTag tag = node.GetColorTag();
      Boolean hasTag = tag != ColorTag.None;

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
         Color color = Color.FromArgb(opacity, ColorTags.GetColor(node.WrappedNode as IAnimatable, tag));
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
   public override void HandleMouseUp(WinForms::MouseEventArgs e, TreeNode tn)
   {
      this.clickedTn = tn;

      ColorTag currentTag = ColorTag.None;
      IMaxNodeWrapper wrapper = HelperMethods.GetMaxNode(tn);
      if (wrapper != null)
      {
         currentTag = wrapper.GetColorTag();
      }

      WinForms::ToolStripDropDown strip = new WinForms::ToolStripDropDown();
      strip.Renderer = new WinForms::ToolStripProfessionalRenderer(new OutlinerColorTable());
      strip.LayoutStyle = WinForms::ToolStripLayoutStyle.HorizontalStackWithOverflow;
      strip.Padding = new WinForms::Padding(3, 2, 1, 1);

      foreach (ColorTag tag in Enum.GetValues(typeof(ColorTag)).Cast<ColorTag>())
      {
         if (tag == ColorTag.WireColor)
            strip.Items.Add(new WinForms::ToolStripSeparator());

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
            strip.Items.Add(new WinForms::ToolStripSeparator());
      }

      Rectangle bounds = this.GetBounds(tn);
      Point location = new Point(bounds.Left + (bounds.Width / 2) - (strip.Width / 2)
                                 , bounds.Bottom + (e.Location.Y - bounds.Top) + 4);
      strip.Show(this.Layout.TreeView, location);
   }

   void btn_Click(object sender, EventArgs e)
   {
      ColorTagToolStripButton btn = sender as ColorTagToolStripButton;
      if (btn != null && this.clickedTn != null)
      {
         ColorTag tag = btn.ColorTag;

         IEnumerable<TreeNode> selNodes = this.Layout.TreeView.SelectedNodes;
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
