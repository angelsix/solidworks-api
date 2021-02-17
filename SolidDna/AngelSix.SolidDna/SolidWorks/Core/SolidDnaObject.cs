using System;
using System.Runtime.InteropServices;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a core SolidDNA object, that is disposable
    /// and needs a COM object disposing cleanly on disposal
    /// </summary>
    public class SolidDnaObject
    {
        #region Protected Members

        /// <summary>
        /// A COM objects that should be cleanly disposed on disposing
        /// </summary>
        protected object mBaseObject;

        #endregion

        #region Public Properties

        /// <summary>
        /// The raw underlying COM object
        /// WARNING: Use with caution. You must handle all disposal from this point on
        /// </summary>
        public object UnsafeObject => mBaseObject;

        #endregion
    }

    /// <summary>
    /// Represents a core SolidDNA object, that is disposable
    /// and needs a COM object disposing cleanly on disposal
    /// </summary>
    public class SolidDnaObject<T> : SolidDnaObject, IDisposable
    {
        #region Protected Properties

        /// <summary>
        /// A COM objects that should be cleanly disposed on disposing
        /// </summary>
        protected T BaseObject
        {
            get => (T)mBaseObject;
            set => mBaseObject = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The raw underlying COM object
        /// WARNING: Use with caution. You must handle all disposal from this point on
        /// </summary>
        public new T UnsafeObject => BaseObject;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="comObject">The COM object to wrap</param>
        public SolidDnaObject(T comObject)
        {
            BaseObject = comObject;
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Disposal
        /// </summary>
        public virtual void Dispose()
        {
            if (BaseObject == null)
                return;

            // Do any specific disposal
            SolidDnaObjectDisposal.Dispose<T>(BaseObject);

            // COM release object. Calling Marshal.FinalReleaseComObject caused other add-ins to malfunction, so we use the less aggressive option
            Marshal.ReleaseComObject(BaseObject);

            // Clear reference
            BaseObject = default(T);
        }

        #endregion
    }

    /// <summary>
    /// Extension method helpers for <see cref="SolidDnaObject{T}"/>
    /// </summary>
    public static class SolidDnaObjectExtensions
    {
        #region Creation Methods

        /// <summary>
        /// Checks if the inner COM object is null. If so, returns null instead of 
        /// the created safe <see cref="SolidDnaObject"/> object
        /// </summary>
        /// <typeparam name="T">The type of SolidDnaObject object being created</typeparam>
        /// <param name="createdObject">The instance that was created</param>
        /// <returns></returns>
        public static T CreateOrNull<T>(this T createdObject)
            where T : SolidDnaObject
        {
            return createdObject?.UnsafeObject == null ? null : createdObject;
        }


        #endregion

    }
}
