using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace PJanssen.Outliner.UpdateService
{
   [ServiceContract(Name = "UpdateService", Namespace = "http://outliner.pjanssen.nl/")]
   public interface IUpdateService
   {
      [OperationContract]
      UpdateData GetUpdateData(OutlinerInstallation installation);

      [OperationContract(AsyncPattern = true)]
      IAsyncResult BeginGetUpdateData(OutlinerInstallation installation, AsyncCallback callback, object state);

      UpdateData EndGetUpdateData(IAsyncResult result);
   }
}
