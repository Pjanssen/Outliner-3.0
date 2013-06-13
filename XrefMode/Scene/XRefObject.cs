using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Max;
using Outliner.MaxUtils;
using Outliner.Scene;

namespace Outliner.Modes.XRefMode.Scene
{
   public class XRefObject : INodeWrapper
   {
      private IIXRefObject xrefObject;

      public XRefObject(IIXRefObject xrefObject)
         : base(MaxInterfaces.Global.COREInterface7.FindNodeFromBaseObject(xrefObject))
      {
         this.xrefObject = xrefObject;
      }

      public override object BaseObject
      {
         get { return xrefObject; }
      }

      #region Equality
      
      public override bool Equals(object obj)
      {
         return xrefObject.Equals(obj);
      }

      public override int GetHashCode()
      {
         return xrefObject.GetHashCode();
      }

      #endregion

      #region Type

      protected override MaxNodeType MaxNodeType
      {
         get { return MaxNodeType.XRefObject | MaxNodeType.Object; }
      }

      #endregion

      #region Name

      public override string DisplayName
      {
         get
         {
            return this.Name;
         }
      }

      #endregion
   }
}
