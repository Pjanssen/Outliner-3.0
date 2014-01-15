using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Configuration;
//using PJanssen.Outliner.UpdateClient.Service;

namespace PJanssen.Outliner.UpdateClient
{
   /// <summary>
   /// Provides a wrapper for the UpdateClient settings
   /// </summary>
   public static class UpdateSettings
   {
      private const string Category = "UpdateClient";
      private const string KeyEnabled = "Enabled";
      private const string KeyLastUpdateTime = "LastUpdateTime";
      private const string KeyIncludeBetaVersions = "IncludeBetaVersions";
      private const string KeySkippedVersion = "SkippedVersion";

      //==========================================================================

      private static SettingsCollection Settings
      {
         get
         {
            return OutlinerGUP.Instance.Settings;
         }
      }

      //==========================================================================

      public static bool Enabled
      {
         get
         {
            return Settings.GetValue<bool>(Category, KeyEnabled, true);
         }
         set
         {
            Settings.SetValue(Category, KeyEnabled, value);
         }
      }

      //==========================================================================

      public static DateTime LastUpdateCheck
      {
         get
         {
            return Settings.GetValue<DateTime>(Category, KeyLastUpdateTime, DateTime.MinValue);
         }
         set
         {
            Settings.SetValue(Category, KeyLastUpdateTime, value);
         }
      }

      //==========================================================================

      public static bool IncludeBetaVersions
      {
         get
         {
            return Settings.GetValue<bool>(Category, KeyIncludeBetaVersions, false);
         }
         set
         {
            Settings.SetValue(Category, KeyIncludeBetaVersions, value);
         }
      }

      //==========================================================================

      public static OutlinerVersion SkippedVersion
      {
         get
         {
            return Settings.GetValue<OutlinerVersion>(Category, KeySkippedVersion, new OutlinerVersion());
         }
         set
         {
            Settings.SetValue(Category, KeySkippedVersion, value);
         }
      }

      //==========================================================================
   }
}
