using System;
using System.Runtime.InteropServices;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a core SolidDNA object, that is disposable
    /// and needs a COM object disposing cleanly on disposal
    /// </summary>
    public class SolidDnaObject<T> : IDisposable
    {
        #region Protected Members

        /// <summary>
        /// A COM objects that should be cleanly disposed on disposing
        /// </summary>
        protected T mBaseObject;

        #endregion

        #region Public Properties

        /// <summary>
        /// The raw underlying COM object
        /// WARNING: Use with caution. You must handle all disposal from this point on
        /// </summary>
        public T UnsafeObject => mBaseObject;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="comObject">The COM object to wrap</param>
        public SolidDnaObject(T comObject)
        {
            mBaseObject = comObject;
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Disposal
        /// </summary>
        public virtual void Dispose()
        {
            if (mBaseObject == null)
                return;

            // Do any specific disposal
            SolidDnaObjectDisposal.Dispose<T>(mBaseObject);

            // COM release object
            Marshal.FinalReleaseComObject(mBaseObject);

            // Clear reference
            mBaseObject = default(T);
        }

        #endregion
    }
}
