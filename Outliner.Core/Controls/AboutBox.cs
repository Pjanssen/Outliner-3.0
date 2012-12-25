using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Outliner.Plugins;

namespace Outliner.Controls
{
   partial class AboutBox : Form
   {
      public AboutBox()
      {
         InitializeComponent();
         this.labelVersion.Text = String.Format("Core Version: {0}", AssemblyVersion);
         this.labelCopyright.Text = String.Format("{0} {1}", AssemblyCopyright, AssemblyCompany);
         this.textBoxPlugins.Text = PluginVersions;
      }

      protected override void OnLoad(EventArgs e)
      {
         ControlHelpers.SetControlColors(this);
      }

      public string AssemblyVersion
      {
         get
         {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
         }
      }

      public string AssemblyCopyright
      {
         get
         {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (attributes.Length == 0)
            {
               return "";
            }
            return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
         }
      }

      public string AssemblyCompany
      {
         get
         {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            if (attributes.Length == 0)
            {
               return "";
            }
            return ((AssemblyCompanyAttribute)attributes[0]).Company;
         }
      }

      public string PluginVersions
      {
         get
         {
            StringBuilder versions = new StringBuilder();
            foreach (Assembly pluginAssembly in OutlinerPlugins.PluginAssemblies)
            {
               if (pluginAssembly == this.GetType().Assembly)
                  continue;

               if (versions.Length > 0)
                  versions.Append(Environment.NewLine);

               AssemblyName name = pluginAssembly.GetName();
               versions.Append(name.Name);
               versions.Append(" - ");
               versions.Append(name.Version.ToString());

               versions.Append(Environment.NewLine);

               foreach (OutlinerPluginData plugin in OutlinerPlugins.GetPlugins(pluginAssembly))
               {
                  versions.Append("   ");
                  versions.Append(plugin.Type.FullName);
                  versions.Append(Environment.NewLine);
               }
            }

            return versions.ToString();
         }
      }

      private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         Process.Start(@"http://outliner.pjanssen.nl/issuetracker");
      }
   }
}
