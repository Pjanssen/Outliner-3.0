using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Plugins;
using Outliner.Scene;

namespace Outliner.Modes.SelectionSet.Scene
{
   [OutlinerPlugin(OutlinerPluginType.Utility)]
   public class SelectionSetWrapperFactory : IMaxNodeFactory
   {
      public IMaxNode CreateMaxNode(object baseNode)
      {
         String selSetName = baseNode as String;
         if (selSetName != null)
            return new SelectionSetWrapper(selSetName);

         return null;
      }

      [OutlinerPluginStart]
      public static void RegisterFactory()
      {
         MaxNodeWrapper.RegisterMaxNodeFactory(new SelectionSetWrapperFactory());
      }
   }
}
