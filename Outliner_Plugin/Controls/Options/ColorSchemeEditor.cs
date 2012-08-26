using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using Outliner.Controls.Tree;
using System.Reflection;
using Outliner.Controls.Tree.Layout;

namespace Outliner.Controls.Options
{
public partial class ColorSchemeEditor : WinForms::UserControl
{
   private TreeViewColorScheme editingScheme;

   public ColorSchemeEditor()
   {
      InitializeComponent();

      TreeNodeLayout colorTreeLayout = new TreeNodeLayout();
      //colorTreeLayout.LayoutItems.Add(new WireColorButton());
      colorTreeLayout.LayoutItems.Add(new TreeNodeText());

      Type colorSchemeType = typeof(TreeViewColorScheme);
      PropertyInfo[] colors = colorSchemeType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
      foreach (PropertyInfo color in colors)
      {
         TreeNode tn = new TreeNode(color.Name);
         tn.Tag = color.GetValue(editingScheme, null);
         colorsTree.Nodes.Add(tn);
      }
   }
}
}
