using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PJanssen.Outliner.Scene
{
   public enum XRefSceneFlags : uint
   {
      /// <summary>
      /// Automatic XRef file updating is ON. 
      /// </summary>
      AutoUpdate = 1 << 0,
 	   
      /// <summary>
      /// The Box display option is set. 
      /// </summary>
      BoxMode = 1 << 1,
 	
      /// <summary>
      /// The XRef is hidden.
      /// </summary>
      Hidden = 1 << 2,
 	
      /// <summary>
      /// The XRef is disabled.
      /// </summary>
      Disabled = 1 << 3,
 	
      /// <summary>
      /// The XRef ignores lights in the file. 
      /// </summary>
      IgnoreLights = 1 << 4,
 	
      /// <summary>
      /// The XRef ignores cameras in the file.
      /// </summary>
      IgnoreCameras = 1 << 5,
 	
      /// <summary>
      /// The XRef ignores shapes in the file.
      /// </summary>
      IgnoreShapes = 1 << 6,
 	
      /// <summary>
      /// The XRef ignores helpers in the file.
      /// </summary>
      IgnoreHelpers = 1 << 7,
 	
      /// <summary>
      /// The XRef ignores the animation in the file.
      /// </summary>
      IgnoreAnimation = 1 << 8,
 	
      /// <summary>
      /// It is not certain that the file has actually changed but the XRef should be reloaded.
      /// </summary>
      FileChanged = 1 << 10,
 	
      /// <summary>
      /// Is set when an XRef can not be resolved.
      /// </summary>
      LoadError = 1 << 11,

      /// <summary>
      /// An overlay XRef will be loaded only if it is a direct descendant of the master file.
      /// </summary>
      Overlay = 1 << 12,
 	   
      /// <summary>
      /// Is set when a scene XRef is not displayed in scene XREF manager UI.
      /// </summary>
      HideInManagerUI = 1 << 13
   }
}
