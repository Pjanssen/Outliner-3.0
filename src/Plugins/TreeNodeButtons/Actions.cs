using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using PJanssen.Outliner.Commands;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Modes;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.TreeNodeButtons
{
   [OutlinerPlugin(OutlinerPluginType.ActionProvider)]
   public static class Actions
   {
      [OutlinerAction]
      public static void ToggleLight(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
      {
         IMaxNode clickedNode = TreeMode.GetMaxNode(contextTn);
         if (clickedNode == null)
            return;

         IINode inode = clickedNode.BaseObject as IINode;
         if (inode == null)
            return;

         ILightObject light = inode.ObjectRef as ILightObject;
         if (light == null)
            return;

         IEnumerable<IMaxNode> nodes;
         if (contextNodes.Contains(clickedNode))
            nodes = contextNodes;
         else
            nodes = clickedNode.ToIEnumerable();

         ToggleLightCommand cmd = new ToggleLightCommand(contextNodes, !light.UseLight);
         cmd.Execute();
         Viewports.Redraw();
      }

      [OutlinerAction]
      public static void SetActiveViewCamera(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
      {
         IMaxNode clickedNode = TreeMode.GetMaxNode(contextTn);
         if (clickedNode == null)
            return;

         SetViewCameraCommand cmd = new SetViewCameraCommand(clickedNode, Viewports.ActiveView);
         cmd.Execute();
         Viewports.Redraw();
      }


      public static Boolean AlwaysTrue(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
      {
         return true;
      }
   }
}
