using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;
using System.Globalization;
using MaxUtils;

namespace Outliner.Filters
{
   public class MaxscriptFilter : Filter<IMaxNodeWrapper>
   {
      public MaxscriptFilter()
      {
         _script = "";
      }

      private const String execFilterTemplate = "( local node = getAnimByHandle {0}; {1} )";

      private String _script;
      private String _filterFn;
      public String Script
      {
         get { return _script; }
         set
         {
            _script = value;
            _filterFn = String.Format(CultureInfo.InvariantCulture, execFilterTemplate, "{0:d}", value);
         }
      }

      public override Boolean ShowNode(IMaxNodeWrapper data)
      {
         if (String.IsNullOrEmpty(_script))
            return true;

         IINodeWrapper iinodeWrapper = data as IINodeWrapper;
         if (data == null)
            return false;

         String script = String.Format(CultureInfo.InvariantCulture, _filterFn, (iinodeWrapper.IINode.Handle));
         return MaxInterfaces.Global.ExecuteMAXScriptScript(script, true, null);
      }
   }
}
