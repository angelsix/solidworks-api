using System;
using SolidWorks.Interop.swconst;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// The type of command item in the <see cref="swCommandItemType_e"/> 
    /// </summary>
    [Flags]
    public enum CommandItemType
    {
        /// <summary>
        /// The item is a menu item
        /// </summary>
        MenuItem = 1,

        /// <summary>
        /// The item is a toolbar item
        /// </summary>
        ToolbarItem = 2,
    }
}
