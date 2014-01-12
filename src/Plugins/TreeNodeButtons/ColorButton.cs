using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using WinForms = System.Windows.Forms;
using PJanssen.Outliner.Scene;
using Autodesk.Max;
using PJanssen.Outliner.Commands;
using System.Xml.Serialization;
using System.ComponentModel;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.NodeSorters;
using PJanssen.Outliner.Controls.Tree.Layout;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Modes;
using PJanssen.Outliner.Controls;

namespace PJanssen.Outliner.TreeNodeButtons
{
public abstract class ColorButton : TreeNodeButton
{
   [XmlAttribute("button_width")]
   [DefaultValue(12)]
   public Int32 ButtonWidth { get; set; }

   [XmlAttribute("button_height")]
   [DefaultValue(8)]
   public Int32 ButtonHeight { get; set; }
   
   protected ColorButton()
   {
      this.ButtonWidth = 12;
      this.ButtonHeight = 8;
   }

   protected abstract Color GetNodeColor(IMaxNode node);
   protected abstract void PreviewNodeColor(IEnumerable<IMaxNode> maxNodes, Color color);
   protected abstract void CommitNodeColor(IEnumerable<IMaxNode> maxNodes, Color color);

   protected override int GetAutoWidth(TreeNode tn)
   {
      return this.ButtonWidth;
   }

   public override int GetHeight(TreeNode tn)
   {
      //if (!this.isInheritedFromLayer(tn))
         return this.ButtonHeight;
      //else
         //return base.GetHeight(tn);
   }

   public override bool CenterVertically
   {
      get { return true; }
   }

   public override void Draw(Graphics graphics, TreeNode tn)
   {
      if (graphics == null || tn == null)
         return;

      IMaxNode node = TreeMode.GetMaxNode(tn);
      if (node == null)
         return;

      //if (node.IsNodePropertyInherited(this.Property))
      //{
      //   base.Draw(graphics, tn);
      //   return;
      //}

      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      Color color = GetNodeColor(node);
      Rectangle rBounds = this.GetBounds(tn);
      using (Pen linePen = new Pen(Color.Black))
      using (SolidBrush brush = new SolidBrush(color))
      {
         graphics.FillRectangle(brush, rBounds);
         graphics.DrawRectangle(linePen, rBounds);
      }
   }

   //==========================================================================

   private class TestCallback : Autodesk.Max.Plugins.HSVCallback
   {
      private Action<IEnumerable<IMaxNode>, Color> previewChange;
      private Action<IEnumerable<IMaxNode>, Color> commitChange;
      private IEnumerable<IMaxNode> nodes;
      private IAColor initialColor;
      private IAColor color;

      public TestCallback(IEnumerable<IMaxNode> nodes, IAColor initialColor,
                          Action<IEnumerable<IMaxNode>, Color> previewChange, 
                          Action<IEnumerable<IMaxNode>, Color> commitChange)
      {
         this.nodes = nodes;
         this.initialColor = initialColor;
         this.color = initialColor;
         this.previewChange = previewChange;
         this.commitChange = commitChange;
      }

      public override void ButtonUp(bool accept)
      {
         if (accept)
            previewChange(nodes, Colors.FromMaxColor(this.color));
      }

      public override void ColorChanged(IAColor col, bool ButtonUp)
      {
         this.color = col;
      }

      public override void OnCancel()
      {
         previewChange(this.nodes, Colors.FromMaxColor(this.initialColor));
      }

      public override void OnOK()
      {
         commitChange(nodes, Colors.FromMaxColor(this.color));
      }

      public override void BeingDestroyed(IIPoint2 pos)
      {
      }
   }

   public override void HandleMouseUp(System.Windows.Forms.MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      IMaxNode node = TreeMode.GetMaxNode(tn);
      if (node == null)
         return;

      IEnumerable<IMaxNode> contextNodes = TreeMode.GetMaxNodes(this.GetContextNodes(tn));

      IntPtr hwnd = MaxInterfaces.MaxHwnd.Handle;
      Color nodeColor = GetNodeColor(node);
      IAColor initColor = MaxInterfaces.Global.AColor.Create(nodeColor.R / 255f, nodeColor.G / 255f, nodeColor.B / 255f, nodeColor.A / 255f);
      IIPoint2 initPos = MaxInterfaces.Global.IPoint2NS.Create(200, 200);
      IHSVCallback colorPickerCallback = new TestCallback(contextNodes, initColor, PreviewNodeColor, CommitNodeColor);
      IColorPicker colorPicker = MaxInterfaces.Global.CreateColorPicker(hwnd, initColor, initPos, colorPickerCallback, "Test", 0);

      //Color color = GetNodeColor(node);
      //IInterface ip = MaxInterfaces.Global.COREInterface;
      //if (ip.NodeColorPicker(ip.MAXHWnd, ref color))
      //{
      //   TreeView tree = this.Layout.TreeView;
      //   IEnumerable<TreeNode> nodes = null;
      //   if (tn.IsSelected && !ControlHelpers.ControlPressed)
      //      nodes = tree.SelectedNodes;
      //   else
      //      nodes = new List<TreeNode>(1) { tn };

      //   IEnumerable<IMaxNode> maxNodes = TreeMode.GetMaxNodes(nodes);
      //   SetNodeColor(maxNodes, Colors.FromMaxColor(color));

      //   //if (tree.NodeSorter is NodePropertySorter &&
      //   //   ((NodePropertySorter)tree.NodeSorter).Property == this.Property)
      //   //{
      //   //   tree.StartTimedSort(nodes);
      //   //}
      //}
   }

}
}
