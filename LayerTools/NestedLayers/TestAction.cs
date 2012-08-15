using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiViewModels.Actions;
using Autodesk.Max;
using MaxUtils;
using System.Runtime.InteropServices;

namespace Outliner.LayerTools
{
   public class TestAction : CuiActionCommandAdapter
   {
      public override string ActionText
      {
         get { return "SetParent"; }
      }

      public override string Category
      {
         get { return "Nested Layers"; }
      }

      public override void Execute(object parameter)
      {
         HelperMethods.WriteToListener(HelperMethods.MtlEditorHwnd.ToString());
         //mgr.EnableDandD()
         IILayerManager layerManager = MaxInterfaces.IILayerManager;
         IILayer child = layerManager.GetLayer(1);
         IILayer parent = layerManager.GetLayer(0);
         NestedLayers.SetParent(child, parent);
      }

      public override string InternalActionText
      {
         get { return ActionText; }
      }

      public override string InternalCategory
      {
         get { return Category; }
      }
   }
}
