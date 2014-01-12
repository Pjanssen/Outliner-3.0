using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PJanssen.Outliner.Controls.ContextMenu;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Modes.XRef.Commands;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Scene;
using WinForms = System.Windows.Forms;

namespace PJanssen.Outliner.Modes.XRef
{
   [OutlinerPlugin(OutlinerPluginType.ContextMenuItemModel)]
   [LocalizedDisplayName(typeof(Resources), "Str_SetXrefSceneFlagsItemModel")]
   public class SetXRefSceneFlagsItemModel : MenuItemModel
   {
      [XmlAttribute("flags")]
      public XRefSceneFlags Flags { get; set; }

      protected override bool Checked(WinForms::ToolStripMenuItem clickedItem, TreeView treeView, TreeNode clickedTn)
      {
         IEnumerable<IMaxNode> selNodes = TreeMode.GetMaxNodes(treeView.SelectedNodes);

         return selNodes.OfType<XRefSceneRecord>()
                        .Any(x => x.HasFlags(this.Flags));
      }

      protected override void OnClick(WinForms::ToolStripMenuItem clickedItem, TreeView treeView, TreeNode clickedTn)
      {
         IEnumerable<IMaxNode> selNodes = TreeMode.GetMaxNodes(treeView.SelectedNodes);

         XRefSceneRecord xrefScene = TreeMode.GetMaxNode(clickedTn) as XRefSceneRecord;
         if (xrefScene == null)
            return;

         Boolean newValue = !xrefScene.HasFlags(this.Flags);
         SetXRefSceneFlagsCommand cmd = new SetXRefSceneFlagsCommand(selNodes, this.Flags, newValue);
         cmd.Execute();
         Viewports.Redraw();
      }
   }
}
