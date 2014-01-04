using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.WebServices
{
   public class UpdateData
   {
      public bool IsUpdateAvailable
      {
         get;
         set;
      }

      public Version NewVersion
      {
         get;
         set;
      }

      public string DownloadUrl
      {
         get;
         set;
      }

      public string Signature
      {
         get;
         set;
      }

      public string ReleaseNotesUrl
      {
         get;
         set;
      }
   }
}
