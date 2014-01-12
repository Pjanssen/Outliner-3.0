using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Max;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.Scene
{
   [OutlinerPlugin(OutlinerPluginType.Utility)]
   public class XRefItemFactory : IMaxNodeFactory
   {
      public IMaxNode CreateMaxNode(object baseNode)
      {
         IIXRefObject xrefObject = baseNode as IIXRefObject;
         if (xrefObject != null)
            return new XRefObject(xrefObject);

         IIObjXRefRecord xrefRecord = baseNode as IIObjXRefRecord;
         if (xrefRecord != null)
            return new XRefObjectRecord(xrefRecord);

         return null;
      }

      [OutlinerPluginStart]
      public static void RegisterFactory()
      {
         MaxNodeWrapper.RegisterMaxNodeFactory(new XRefItemFactory());
      }
   }
}
