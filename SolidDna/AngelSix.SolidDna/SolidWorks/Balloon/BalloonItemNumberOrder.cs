using System;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Balloon item ordering options
    /// </summary>
    [Flags]
    public enum BalloonItemNumberOrder
    {
        /// <summary>
        /// Do not change the item numbers
        /// </summary>
        DoNotChangeItemNumbers = 1,

        /// <summary>
        /// Follow the same order as the assembly
        /// </summary>
        FollowAssemblyOrder = 2,

        /// <summary>
        /// Order sequentially
        /// </summary>
        OrderSequentially = 4
    }
}
