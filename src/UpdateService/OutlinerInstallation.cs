using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.UpdateService
{
   /// <summary>
   /// Defines the data sent to the update server, used to determine if 
   /// there's a newer version available.
   /// </summary>
   public class OutlinerInstallation
   {
      public OutlinerInstallation() { }

      public OutlinerInstallation(Version outlinerVersion, int maxVersion)
      {
         this.OutlinerVersion = outlinerVersion;
         this.MaxVersion = maxVersion;
      }

      /// <summary>
      /// Gets or set the currently installed Outliner version.
      /// </summary>
      public Version OutlinerVersion
      {
         get;
         set;
      }

      /// <summary>
      /// Gets or sets the version of 3dsMax being used. E.g. 2014.
      /// </summary>
      public int MaxVersion
      {
         get;
         set;
      }
   }
}
