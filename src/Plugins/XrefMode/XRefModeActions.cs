using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Max.MaxSDK.AssetManagement;
using PJanssen.Outliner.Controls;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Modes;
using PJanssen.Outliner.Modes.XRef.Commands;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Scene;
using WinForms = System.Windows.Forms;

namespace PJanssen.Outliner.Modes.XRef
{
   [OutlinerPlugin(OutlinerPluginType.ActionProvider)]
   public static class XRefModeActions
   {
      [OutlinerAction]
      public static void AddXrefScene(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
      {
         string path = ManagedServices.PathSDK.BrowseForMaxFile();
         
         if (path != null)
         {
            IIAssetManager assetManager = MaxInterfaces.Global.MaxSDK.AssetManagement.IAssetManager.Instance;
            IAssetUser asset = assetManager.GetAsset( path
                                                    , AssetType.XRefAsset, true);
            MaxInterfaces.COREInterface.RootNode.AddNewXRefFile(asset, true, false);
         }
      }

      [OutlinerAction]
      public static void OpenXrefSceneInNewInstance(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
      {
         IEnumerable<XRefSceneRecord> xrefScenes = contextNodes.Where(n => n is XRefSceneRecord)
                                                         .Cast<XRefSceneRecord>();

         Boolean openProcesses = true;
         if (xrefScenes.Count() > 1)
         {
            WinForms::DialogResult result = WinForms::MessageBox.Show( String.Format(Resources.Str_OpenManyInstances, xrefScenes.Count())
                                                                     , Resources.Str_OpenManyInstancesTitle
                                                                     , WinForms.MessageBoxButtons.YesNo
                                                                     , WinForms.MessageBoxIcon.Warning
                                                                     , WinForms.MessageBoxDefaultButton.Button1
                                                                     , ControlHelpers.CreateLocalizedMessageBoxOptions());
            openProcesses = result == WinForms.DialogResult.Yes;
         }

         if (openProcesses)
         {
            string maxProcessName = Process.GetCurrentProcess().MainModule.FileName;

            foreach (XRefSceneRecord xrefScene in xrefScenes)
            {
               Process.Start(maxProcessName, xrefScene.FileName);
            }
         }
      }

      [OutlinerAction]
      public static void MergeXrefScene(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
      {
         IEnumerable<XRefSceneRecord> xrefScenes = contextNodes.OfType<XRefSceneRecord>();

         foreach (XRefSceneRecord xrefScene in xrefScenes)
         {
            MaxInterfaces.COREInterface.RootNode.BindXRefFile(xrefScene.Index);
         }
      }

      [OutlinerAction]
      public static void OpenContainingFolder(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
      {
         foreach (IXRefRecord record in contextNodes.OfType<IXRefRecord>())
         {
            String filePath = Path.GetDirectoryName(record.Filename);
            Process.Start(filePath);
         }
      }

      #region Enabled
      
      [OutlinerPredicate]
      public static Boolean XRefRecordEnabled(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
      {
         return contextNodes.OfType<IXRefRecord>()
                            .Any(r => r.Enabled);
      }

      [OutlinerAction]
      public static void XRefRecordEnabledToggle(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
      {
         Boolean newValue = !XRefRecordEnabled(contextTn, contextNodes);
         contextNodes.OfType<IXRefRecord>()
                     .ForEach(r => r.Enabled = newValue);
      }

      #endregion

      #region Update

      [OutlinerAction]
      public static void XRefRecordUpdate(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
      {
         contextNodes.OfType<IXRefRecord>()
                     .ForEach(r => r.Update());
      }

      #endregion
   }
}