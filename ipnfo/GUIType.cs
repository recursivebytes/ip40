using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ipnfo
{
    /// <summary>
    /// Type of View
    /// </summary>
    public enum GUIType
    {
        /// <summary>
        /// Determined automatically based on screen capabilities
        /// </summary>
        Auto,
        /// <summary>
        /// Classic Desktop View
        /// </summary>
        Classic,
        /// <summary>
        /// Modern and Touch optimized view
        /// </summary>
        Modern
    }
}
