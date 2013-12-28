using System;
using System.Text.RegularExpressions;
using Outliner.Scene;
using Outliner.MaxUtils;
using Outliner.Plugins;
using System.Xml.Serialization;
using System.ComponentModel;
using PJanssen;

namespace Outliner.Filters
{
/// <summary>
/// A Filter that filters IMaxNodes based on their name.
/// </summary>
[OutlinerPlugin(OutlinerPluginType.Filter)]
[LocalizedDisplayName(typeof(OutlinerResources), "Filter_Name")]
public class NameFilter : Filter<IMaxNode>
{
   private const String RegexBeginsWith = "^";
   private const String TextEscapedWildcard = "\\*";
   private const String RegexWildcard = ".*";
   private Regex searchRegex;
   private String searchString;
   private Boolean caseSensitive;
   private Boolean useWildcard;

   /// <summary>
   /// Initializes a new instance of the NameFilter class.
   /// </summary>
   public NameFilter() 
   {
      this.searchString = "";
      this.caseSensitive = false;
      this.useWildcard = false;

      CreateRegex();
   }

   private void CreateRegex()
   {
      string regexString = Regex.Escape(searchString).Replace(TextEscapedWildcard, RegexWildcard);

      if (this.UseWildcard)
         regexString = RegexWildcard + regexString;

      regexString = RegexBeginsWith + regexString;

      RegexOptions options = (this.CaseSensitive) ? RegexOptions.None : RegexOptions.IgnoreCase;

      this.searchRegex = new Regex(regexString, options);
   }

   /// <summary>
   /// Gets or sets the string to filter the nodes by.
   /// </summary>
   /// <remarks>An asterisk character can be used as a wildcard.</remarks>
   [XmlAttribute("searchstring")]
   public String SearchString 
   {
      get 
      {
         return searchString;
      }
      set
      {
         Throw.IfNull(value, "value");

         this.searchString = value;
         this.CreateRegex();
         this.OnFilterChanged();
      }
   }

   /// <summary>
   /// Gets or sets whether the search should be case sensitive.
   /// If true, a searchstring "S" matches "Sphere", but not "sphere"
   /// </summary>
   [XmlAttribute("casesensitive")]
   public Boolean CaseSensitive 
   {
      get { return this.caseSensitive; }
      set
      {
         this.caseSensitive = value;
         this.CreateRegex();
         this.OnFilterChanged();
      }
   }

   /// <summary>
   /// Gets or sets whether a wildcard should be prepended to the search string by default.
   /// If true, a searchstring "e" matches "sphere".
   /// </summary>
   [XmlAttribute("usewildcard")]
   public Boolean UseWildcard 
   {
      get { return useWildcard; }
      set
      {
         this.useWildcard = value;
         this.CreateRegex();
         this.OnFilterChanged();
      }
   }

   [XmlIgnore]
   [Browsable(false)]
   public override bool Enabled
   {
      get { return !String.IsNullOrEmpty(searchString); }
      set { }
   }

   protected override Boolean ShowNodeInternal(IMaxNode data) 
   {
      if (data == null)
         return false;

      return searchRegex.IsMatch(data.Name);
   }
}
}
