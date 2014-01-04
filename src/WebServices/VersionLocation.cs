using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Outliner.WebServices
{
   public class VersionLocation
   {
      public Version Version
      {
         get;
         set;
      }

      public string DownloadUrl
      {
         get;
         set;
      }

      public string ChangelogUrl
      {
         get;
         set;
      }
   }
}