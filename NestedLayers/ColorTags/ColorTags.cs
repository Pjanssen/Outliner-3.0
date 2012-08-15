using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using System.Drawing;
using MaxUtils;

namespace Outliner.LayerTools
{
[Flags]
public enum ColorTag : byte
{
   None      = 0x00,
   Red       = 0x01,
   Orange    = 0x02,
   Yellow    = 0x04,
   Green     = 0x08,
   Blue      = 0x10,
   Purple    = 0x20,
   Grey      = 0x40,
   WireColor = 0x80,
   All       = 0xFF
}

public static class ColorTags
{
   private const uint CID_A = 0x68D53413;
   private const uint CID_B = 0x42B40170;
   private static IClass_ID classID;

   public const SystemNotificationCode TagChanged = (SystemNotificationCode)0x00000200;

   private static readonly Dictionary<ColorTag, Tuple<uint, Color>> colors = 
      new Dictionary<ColorTag, Tuple<uint, Color>>() {
        { ColorTag.Red,    new Tuple<uint, Color>(0x588B5E2D, Color.FromArgb(230, 96, 93)) }
      , { ColorTag.Orange, new Tuple<uint, Color>(0x3663775D, Color.FromArgb(241, 169, 74)) }
      , { ColorTag.Yellow, new Tuple<uint, Color>(0x011713D3, Color.FromArgb(239, 220, 80)) }
      , { ColorTag.Green,  new Tuple<uint, Color>(0x4C600228, Color.FromArgb(179, 217, 79)) }
      , { ColorTag.Blue,   new Tuple<uint, Color>(0x51D06AFF, Color.FromArgb(92, 162, 250)) }
      , { ColorTag.Purple, new Tuple<uint, Color>(0x385D0131, Color.FromArgb(193, 140, 215)) }
      , { ColorTag.Grey,   new Tuple<uint, Color>(0x4F014CFB, Color.FromArgb(169, 169, 169)) }
      };

   internal static void Start()
   {
      ColorTags.classID = MaxInterfaces.Global.Class_ID.Create(CID_A, CID_B);

      IIColorManager colorMan = MaxInterfaces.Global.ColorManager;

      foreach (KeyValuePair<ColorTag, Tuple<uint, Color>> color in ColorTags.colors)
      {
         colorMan.RegisterColor( color.Value.Item1
                               , color.Key.ToString()
                               , "Outliner"
                               , color.Value.Item2);
      }
   }

   public static Int32 NumTags
   {
      get { return colors.Count; }
   }

   /// <summary>
   /// Returns true if the supplied node has a tag set on it.
   /// </summary>
   public static Boolean HasTag(IAnimatable node)
   {
      if (node == null)
         return false;

      ColorTag tag = ColorTags.GetTag(node);
      return tag != ColorTag.None;
   }

   /// <summary>
   /// Tests if the supplied node has got one or more of the supplied tags set.
   /// </summary>
   public static Boolean HasTag(IAnimatable node, ColorTag tags)
   {
      if (node == null)
         return false;

      ColorTag nodeTag = ColorTags.GetTag(node);
      return (nodeTag & tags) != 0;
   }

   /// <summary>
   /// Gets the tag index for the supplied node.
   /// </summary>
   public static ColorTag GetTag(IAnimatable node)
   {
      if (node == null)
         return ColorTag.None;

      IAppDataChunk chunk = node.GetAppDataChunk(ColorTags.classID, SClass_ID.Utility, 0);
      if (chunk == null || chunk.Data == null || chunk.Data.Length == 0)
         return ColorTag.None;
      else
         return (ColorTag)chunk.Data[0];
   }

   /// <summary>
   /// Gets the tag (or wire-) color of the supplied node.
   /// </summary>
   public static Color GetColor(IAnimatable node)
   {
      if (node == null)
         return Color.Empty;

      ColorTag tag = ColorTags.GetTag(node);

      if (tag == ColorTag.None)
         return Color.Empty;
      else if (tag == ColorTag.WireColor)
         return ColorTags.getWireColor(node);
      else
         return ColorTags.GetTagColor(tag);
   }

   /// <summary>
   /// Sets the tag index on the supplied node.
   /// </summary>
   public static void SetTag(IAnimatable node, ColorTag tag)
   {
      if (node == null)
         throw new ArgumentNullException("node");

      node.RemoveAppDataChunk(ColorTags.classID, SClass_ID.Utility, 0);

      if (tag != ColorTag.None)
      {
         byte[] data = new byte[1] { (byte)tag };
         node.AddAppDataChunk(ColorTags.classID, SClass_ID.Utility, 0, data);
      }

      MaxInterfaces.Global.BroadcastNotification(ColorTags.TagChanged, node);
   }

   /// <summary>
   /// Clears the color tag from the supplier node.
   /// </summary>
   public static void RemoveTag(IAnimatable node)
   {
      if (node == null)
         throw new ArgumentNullException("node");

      node.RemoveAppDataChunk(ColorTags.classID, SClass_ID.Utility, 0);

      MaxInterfaces.Global.BroadcastNotification(ColorTags.TagChanged, node);
   }

   /// <summary>
   /// Gets the color of the supplied tag.
   /// </summary>
   public static Color GetTagColor(ColorTag tag)
   {
      if (tag == ColorTag.WireColor)
         throw new ArgumentException("ColorTag.WireColor is not a valid value for GetTagColor");

      if (tag == ColorTag.None)
         return Color.Empty;

      IIColorManager colorMan = MaxInterfaces.Global.ColorManager;
      Tuple<uint, Color> colorEntry;
      if (ColorTags.colors.TryGetValue(tag, out colorEntry))
      {
         Color color = colorMan.GetColor((GuiColors)colorEntry.Item1);
         return ColorHelpers.FromMaxColor(color);
      }
      else
         return Color.Empty;
   }


   private static Color getWireColor(IAnimatable node)
   {
      Color color = Color.Empty;

      if (node is IINode)
         color = ((IINode)node).WireColor;
      else if (node is IILayer)
         color = ((IILayer)node).WireColor;

      return ColorHelpers.FromMaxColor(color);
   }
}
}
