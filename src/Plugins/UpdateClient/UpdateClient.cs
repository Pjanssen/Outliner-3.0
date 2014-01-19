using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Plugins;
//using PJanssen.Outliner.UpdateClient.Service;
using PJanssen.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;
using PJanssen.Outliner.UpdateService;
using System.ServiceModel.Channels;
using PJanssen.Outliner.Configuration;

namespace PJanssen.Outliner.UpdateClient
{
   [OutlinerPlugin(OutlinerPluginType.Utility)]
   public static class UpdateClient
   {
      //==========================================================================

      private const string ConfigurationSectionName = "UpdateClient";

      //==========================================================================

      public static UpdateClientConfigurationSection Configuration
      {
         get
         {
            if (OutlinerGUP.Instance.SettingsLoaded)
               return OutlinerGUP.Instance.Configuration.GetSection<UpdateClientConfigurationSection>(ConfigurationSectionName);
            else
               return new UpdateClientConfigurationSection();
         }
      }

      //==========================================================================

      [OutlinerPluginStart]
      public static void PluginStart()
      {
         try
         {
            if (ShouldCheckForUpdateNow())
               CheckForUpdate();
         }
         catch (Exception e)
         {
            OutlinerGUP.Instance.Log.Exception(e);
         }
      }

      //==========================================================================

      private static bool ShouldCheckForUpdateNow()
      {
         if (!Configuration.Enabled)
            return false;

         DateTime lastUpdate = Configuration.LastUpdateCheck;
         DateTime lastUpdateDay = new DateTime(lastUpdate.Year, lastUpdate.Month, lastUpdate.Day);

         return (DateTime.Today - lastUpdateDay) >= TimeSpan.FromDays(1);
      }

      //==========================================================================

      public static void CheckForUpdate()
      {
         OutlinerGUP.Instance.Log.Debug("Checking for new version...");

         IUpdateService service = CreateService();

         object asyncState = new Tuple<IUpdateService, Thread>(service, Thread.CurrentThread);
         OutlinerInstallation installation = OutlinerInstallation.GetCurrentInstallation();
         service.BeginGetUpdateData(installation, UpdateDataReceived, asyncState);

         Configuration.LastUpdateCheck = DateTime.Now;
      }

      //==========================================================================

      private static IUpdateService CreateService()
      {
         Binding binding = new BasicHttpBinding();
         EndpointAddress address = new EndpointAddress(Configuration.ServiceAddress);

         return ChannelFactory<IUpdateService>.CreateChannel(binding, address);
      }

      //==========================================================================

      private static void UpdateDataReceived(IAsyncResult ar)
      {
         try
         {
            HandleUpdateDataReceived(ar);
         }
         catch (Exception e)
         {
            OutlinerGUP.Instance.Log.Exception(e);
         }
      }

      //==========================================================================

      private static void HandleUpdateDataReceived(IAsyncResult result)
      {
         Tuple<IUpdateService, Thread> state = (Tuple<IUpdateService, Thread>)result.AsyncState;
         IUpdateService service = state.Item1;
         Thread targetThread = state.Item2;

         UpdateData updateData = service.EndGetUpdateData(result);
         if (!updateData.IsUpdateAvailable)
         {
            OutlinerGUP.Instance.Log.Debug("No new version available.");
            return;
         }

         if (IsSkippedVersion(updateData.NewVersion))
         {
            OutlinerGUP.Instance.Log.Debug("New version was skipped by user.");
            return;
         }

         HandleNewVersion(targetThread, updateData);
      }

      //==========================================================================

      private static bool IsSkippedVersion(OutlinerVersion newVersion)
      {
         OutlinerVersion skippedVersion = Configuration.SkippedVersion;

         return skippedVersion != null && skippedVersion >= newVersion;
      }

      //==========================================================================

      private static void HandleNewVersion(Thread targetThread, UpdateData updateData)
      {
         OutlinerGUP.Instance.Log.FormatDebug("Version {0} available!", updateData.NewVersion);

         Dispatcher dispatcher = Dispatcher.FromThread(targetThread);
         dispatcher.SyncInvoke(() => ShowUpdateDialog(updateData));
      }

      //==========================================================================

      private static void ShowUpdateDialog(UpdateData data)
      {
         UpdateDialog dialog = new UpdateDialog();
         dialog.UpdateData = data;
         dialog.ShowDialog();
      }

      //==========================================================================
   }
}
