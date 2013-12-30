using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Outliner.UpdateService
{
   [WebService(Namespace = "http://outliner.pjanssen.nl/")]
   [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
   [ToolboxItem(false)]
   public class Update : WebService
   {
      /// <summary>
      /// Tests if a newer version of the Outliner is available for the given Outliner installation.
      /// </summary>
      [WebMethod]
      public bool IsNewVersionAvailable(OutlinerInstallation installation)
      {
         throw new NotImplementedException();
      }

      [WebMethod]
      public VersionLocation GetNewVersionLocation(OutlinerInstallation installation)
      {
         throw new NotImplementedException();
      }
   }
}
