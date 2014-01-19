using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;

namespace PJanssen.Outliner.UpdateClient
{
   public sealed class UpdateClientConfigurationSection : ConfigurationSection
   {
      //==========================================================================

      [ConfigurationProperty("serviceAddress", DefaultValue="http://outliner.pjanssen.nl/services/update_server.php")]
      public string ServiceAddress
      {
         get
         {
            return (string)this["serviceAddress"];
         }
         set
         {
            this["serviceAddress"] = value;
         }
      }
      
      //==========================================================================

      [ConfigurationProperty("enabled", DefaultValue=true)]
      public bool Enabled
      {
         get
         {
            return (bool)this["enabled"];
         }
         set
         {
            this["enabled"] = value;
         }
      }

      //==========================================================================

      [ConfigurationProperty("includeBetaVersions", DefaultValue=false)]
      public bool IncludeBetaVersions
      {
         get
         {
            return (bool)this["includeBetaVersions"];
         }
         set
         {
            this["includeBetaVersions"] = value;
         }
      }

      //==========================================================================

      [ConfigurationProperty("lastUpdateCheck")]
      public DateTime LastUpdateCheck
      {
         get
         {
            return (DateTime)this["lastUpdateCheck"];
         }
         set
         {
            this["lastUpdateCheck"] = value;
         }
      }

      //==========================================================================

      [ConfigurationProperty("skippedVersion")]
      [TypeConverter(typeof(OutlinerVersionConverter))]
      public OutlinerVersion SkippedVersion
      {
         get
         {
            return (OutlinerVersion)this["skippedVersion"];
         }
         set
         {
            this["skippedVersion"] = value;
         }
      }

      //==========================================================================
   }
}
