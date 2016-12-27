using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a core SolidDNA object, that is disposable
    /// and needs a COM object disposing cleanly on disposal
    /// 
    /// NOTE: Use this shared type if another part of the application may have access to this
    ///       same COM object and the life-cycle for each reference is managed independently
    /// </summary>
    public class SharedSolidDnaObject<T> : IDisposable
    {
        #region Protected Members

        /// <summary>
        /// A list of COM objects that should be cleanly disposed on disposing
        /// </summary>
        T mBaseObject;

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="comObject">The COM object to wrap</param>
        public SharedSolidDnaObject(T comObject)
        {
            mBaseObject = comObject;
        }

        #region Dispose

        /// <summary>
        /// Disposal
        /// </summary>
        public void Dispose()
        {
            if (mBaseObject == null)
                return;

            // Release object
            Marshal.ReleaseComObject(mBaseObject);

            // Clear reference
            mBaseObject = default(T);
        }

        #endregion
    }
}
