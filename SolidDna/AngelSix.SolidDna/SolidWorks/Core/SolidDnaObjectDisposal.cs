using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Handles SolidWorks-specific COM disposal based on the type of object
    /// </summary>
    public static class SolidDnaObjectDisposal
    {
        public static void Dispose<T>(object comObject)
        {
            // Taskpane View
            if (typeof(T).IsInterfaceOrHasInterface<ITaskpaneView>())
            {
                ((ITaskpaneView)comObject).DeleteView();
            }  
        }
    }
}
