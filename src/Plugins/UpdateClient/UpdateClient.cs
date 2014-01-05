using Outliner.MaxUtils;
using Outliner.Plugins;
using Outliner.UpdateClient.Service;
using PJanssen.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Outliner.UpdateClient
{
   [OutlinerPlugin(OutlinerPluginType.Utility)]
   public class UpdateClient
   {
      //==========================================================================

      private const string ServiceEndPointAddress = "http://outliner.pjanssen.nl/services/update_server.php";

      //==========================================================================

      [OutlinerPluginStart]
      public static void PluginStart()
      {
         CheckNewVersion();
      }

      //==========================================================================

      private static void CheckNewVersion()
      {
         OutlinerGUP.Instance.Log.Debug("Checking for new version...");

         UpdateSoapClient client = CreateClient();
         object asyncState = new Tuple<UpdateSoapClient, Thread>(client, Thread.CurrentThread);

         client.BeginGetUpdateData(GetCurrentInstallation(), UpdateDataReceived, asyncState);
      }

      //==========================================================================

      private static void UpdateDataReceived(IAsyncResult ar)
      {
         Tuple<UpdateSoapClient, Thread> state = (Tuple<UpdateSoapClient, Thread>)ar.AsyncState;
         UpdateSoapClient client = state.Item1;
         Thread targetThread = state.Item2;

         UpdateData updateData = client.EndGetUpdateData(ar);

         if (updateData.IsUpdateAvailable)
         {
            OutlinerGUP.Instance.Log.Debug("New version available!");

            Dispatcher dispatcher = Dispatcher.FromThread(targetThread);
            dispatcher.SyncInvoke(() => ShowUpdateDialog(updateData));
         }
         else
         {
            OutlinerGUP.Instance.Log.Debug("Using latest version.");
         }
      }

      //==========================================================================

      private static UpdateSoapClient CreateClient()
      {
         BasicHttpBinding binding = new BasicHttpBinding();
         EndpointAddress endpointAddress = new EndpointAddress(ServiceEndPointAddress);

         return new UpdateSoapClient(binding, endpointAddress);
      }

      //==========================================================================

      private static void ShowUpdateDialog(UpdateData data)
      {
         UpdateDialog dialog = new UpdateDialog();

         OutlinerVersion version = data.NewVersion;
         Label newVersionLbl = dialog.FindName("NewVersion") as Label;
         newVersionLbl.Content = string.Format(Resources.UpdateDialog.VersionFormat, version.Major, version.Minor, version.Build, version.Revision, version.IsBeta ? "beta" : "");

         WebBrowser releaseNotesBrowser = dialog.FindName("ReleaseNotes") as WebBrowser;
         releaseNotesBrowser.Source = new Uri(data.ReleaseNotesUrl);

         dialog.ShowDialog();
      }

      //==========================================================================

      private static OutlinerInstallation GetCurrentInstallation()
      {
         return new OutlinerInstallation()
         {
            OutlinerVersion = new OutlinerVersion() { Major = 3, Minor = 0, Build = 25 },
            MaxVersion = 2013
         };
      }

      //==========================================================================
   }
}
