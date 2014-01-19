using PJanssen.Outliner.MaxUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PJanssen.Outliner
{
   /// <summary>
   /// Defines the data sent to the update server, used to determine if 
   /// there's a newer version available.
   /// </summary>
   [DataContract]
   public class OutlinerInstallation
   {
      //==========================================================================

      public OutlinerInstallation() { }

      public OutlinerInstallation(OutlinerVersion outlinerVersion, int maxVersion)
      {
         this.OutlinerVersion = outlinerVersion;
         this.MaxVersion = maxVersion;
      }

      //==========================================================================

      /// <summary>
      /// Gets or set the currently installed Outliner version.
      /// </summary>
      [DataMember]
      public OutlinerVersion OutlinerVersion
      {
         get;
         set;
      }

      //==========================================================================

      /// <summary>
      /// Gets or sets the version of 3dsMax being used. E.g. 2014.
      /// </summary>
      [DataMember]
      public int MaxVersion
      {
         get;
         set;
      }

      //==========================================================================

      public static OutlinerInstallation GetCurrentInstallation()
      {
         return new OutlinerInstallation()
         {
            OutlinerVersion = new OutlinerVersion() { Major = 3, Minor = 0, Build = 25, Stage = ReleaseStage.Beta },
            
            MaxVersion = 2013
         };
      }

      //==========================================================================
   }
}
