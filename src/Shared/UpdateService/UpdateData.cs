using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PJanssen.Outliner.UpdateService
{
   [DataContract(Namespace="http://outliner.pjanssen.nl/")]
   public class UpdateData
   {
      [DataMember(Order = 0)]
      public bool IsUpdateAvailable
      {
         get;
         set;
      }

      [DataMember(Order = 1)]
      public OutlinerVersion NewVersion
      {
         get;
         set;
      }

      [DataMember(Order = 2)]
      public string DownloadUrl
      {
         get;
         set;
      }

      [DataMember(Order = 3)]
      public string Signature
      {
         get;
         set;
      }

      [DataMember(Order = 4)]
      public string ReleaseNotesUrl
      {
         get;
         set;
      }
   }
}
