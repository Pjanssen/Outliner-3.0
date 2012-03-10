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
public class WireColorButton : TreeNodeLayoutItem
{
   [XmlAttribute("button_width")]
   [DefaultValue(11)]
   public Int32 ButtonWidth { get; set; }

   [XmlAttribute("button_height")]
   [DefaultValue(11)]
   public Int32 ButtonHeight { get; set; }
   
   public WireColorButton()
   {
      this.ButtonWidth = 11;
      this.ButtonHeight = 11;
   }

   public override Size GetSize(TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return Size.Empty;

      return new Size(this.ButtonWidth, this.ButtonHeight);
   }

   public override bool CenterVertically
   {
      get { return true; }
   }

   public override void Draw(Graphics g, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return;

      Color wc = node.WireColor;
      Rectangle rBounds = this.GetBounds(tn);
      using (Pen linePen = new Pen(Color.Black))
      {
         using (SolidBrush brush = new SolidBrush(wc))
         {
            g.FillRectangle(brush, rBounds);
            g.DrawRectangle(linePen, rBounds);
         }
      }
   }

   public override void HandleMouseUp(System.Windows.Forms.MouseEventArgs e, System.Windows.Forms.TreeNode tn)
   {
      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return;

      Color wc = node.WireColor;
      IInterface ip = Autodesk.Max.GlobalInterface.Instance.COREInterface;
      if (ip.NodeColorPicker(ip.MAXHWnd, ref wc))
      {
         SetWireColorCommand cmd = new SetWireColorCommand(new List<IINode>() { (Autodesk.Max.IINode)node.WrappedNode }, wc);
         cmd.Execute(true);
      }
   }
}
}
