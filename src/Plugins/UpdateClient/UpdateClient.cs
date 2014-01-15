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

namespace PJanssen.Outliner.UpdateClient
{
   [OutlinerPlugin(OutlinerPluginType.Utility)]
   public static class UpdateClient
   {
      //==========================================================================

      private const string ServiceEndPointAddress = "http://outliner.pjanssen.nl/services/update_server.php";
      
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
         if (!UpdateSettings.Enabled)
            return false;

         DateTime lastUpdate = UpdateSettings.LastUpdateCheck;
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

         UpdateSettings.LastUpdateCheck = DateTime.Now;
      }

      //==========================================================================

      private static IUpdateService CreateService()
      {
         Binding binding = new BasicHttpBinding();
         EndpointAddress address = new EndpointAddress(ServiceEndPointAddress);

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
         if (updateData.IsUpdateAvailable && !IsSkippedVersion(updateData.NewVersion))
            HandleNewVersion(targetThread, updateData);
         else
            OutlinerGUP.Instance.Log.Debug("No new version available.");
      }

      //==========================================================================

      private static bool IsSkippedVersion(OutlinerVersion newVersion)
      {
         OutlinerVersion skippedVersion = UpdateSettings.SkippedVersion;

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

         //Label newVersionLbl = dialog.FindName("NewVersion") as Label;
         //newVersionLbl.DataContext = data;


         //dialog.UpdateData = data;

         //OutlinerVersion version = data.NewVersion;
         //Label newVersionLbl = dialog.FindName("NewVersion") as Label;
         //newVersionLbl.Content = string.Format(Resources.UpdateDialog.VersionFormat, version);

         //WebBrowser releaseNotesBrowser = dialog.FindName("ReleaseNotes") as WebBrowser;
         //releaseNotesBrowser.Source = new Uri(data.ReleaseNotesUrl);

         dialog.ShowDialog();
      }

      //==========================================================================
   }
}
