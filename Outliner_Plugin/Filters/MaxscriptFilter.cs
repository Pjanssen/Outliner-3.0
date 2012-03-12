using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Controls.FiltersBase;
using Outliner.Scene;

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
            _filterFn = String.Format(execFilterTemplate, "{0:d}", value);
         }
      }

      public override FilterResult ShowNode(IMaxNodeWrapper data)
      {
         if (_script == "")
            return FilterResult.Show;

         if (data is IINodeWrapper)
         {
            if (GlobalInterface.Instance.ExecuteMAXScriptScript(String.Format(_filterFn, ((IINode)data.WrappedNode).Handle), true, null))
               //if (MaxscriptSDK.ExecuteBooleanMaxscriptQuery(String.Format(_filterFn, n.Handle)))
               return FilterResult.Hide;
            else
               return FilterResult.Show;
         }
         else
            return FilterResult.Show;
      }
   }
}
