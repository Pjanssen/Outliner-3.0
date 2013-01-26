using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Plugins;
using Outliner.Scene;

namespace Outliner.Scene
{
   [OutlinerPlugin(OutlinerPluginType.Utility)]
   public class MaterialWrapperFactory : IMaxNodeFactory
   {
      public IMaxNode CreateMaxNode(object baseNode)
      {
         IMtl mtl = baseNode as IMtl;
         if (mtl != null)
            return new MaterialWrapper(mtl);

         ITexmap texmap = baseNode as ITexmap;
         if (texmap != null)
            return new TextureMapWrapper(texmap);

         return null;
      }

      [OutlinerPluginStart]
      public static void RegisterFactory()
      {
         MaxNodeWrapper.RegisterMaxNodeFactory(new MaterialWrapperFactory());
      }
   }
}
