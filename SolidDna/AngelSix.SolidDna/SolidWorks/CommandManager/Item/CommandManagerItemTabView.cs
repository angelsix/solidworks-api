using SolidWorks.Interop.swconst;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// The view style of a <see cref="CommandManagerItem"/> for a Tab (the large icons above opened files).
    /// References <see cref="swCommandTabButtonTextDisplay_e"/>
    /// </summary>
    public  enum CommandManagerItemTabView
    {
        /// <summary>
        /// The item is not shown in the tab
        /// </summary>
        None = 0,

        /// <summary>
        /// The item is shown with an icon only
        /// </summary>
        IconOnly = 1,

        /// <summary>
        /// The item is shown with the icon, then the text below it
        /// </summary>
        IconWithTextBelow = 2,

        /// <summary>
        /// The item is shown with the icon then the text to the right
        /// </summary>
        IconWithTextAtRight = 4
    }
}
