using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Outliner.Filters
{
    public abstract class CustomNodeFilter<T> : Filter<T>
    {
        /// <summary>
        /// Gets the name of the filter.
        /// </summary>
        public abstract String Name { get; }

        /// <summary>
        /// Gets the image shown for the filter.
        /// </summary>
        public abstract Image Image { get; }
    }
}
