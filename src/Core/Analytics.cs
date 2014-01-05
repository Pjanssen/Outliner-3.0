using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.AnalyticsService;
using System.ServiceModel;

namespace Outliner
{
   public static class Analytics
   {
      //==========================================================================

      private const string ServiceEndPointAddress = "http://outliner.pjanssen.nl/services/analytics_server.php";

      //==========================================================================
      
      public static void TreeModeStarted(string name)
      {
         AnalyticsSoapClient client = CreateClient();
         client.BeginTreeModeStarted(GetCurrentInstallation(), name, null, null);
      }

      //==========================================================================

      private static AnalyticsSoapClient CreateClient()
      {
         BasicHttpBinding binding = new BasicHttpBinding();
         EndpointAddress remoteAddress = new EndpointAddress(ServiceEndPointAddress);

         return new AnalyticsSoapClient(binding, remoteAddress);
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
