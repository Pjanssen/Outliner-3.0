using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace PJanssen.Outliner.WebServices
{
   [WebService(Namespace = "http://outliner.pjanssen.nl/")]
   [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
   [ToolboxItem(false)]
   public class Update : WebService
   {
      [WebMethod]
      public UpdateData GetUpdateData(OutlinerInstallation installation)
      {
         throw new NotImplementedException();
      }
   }
}
