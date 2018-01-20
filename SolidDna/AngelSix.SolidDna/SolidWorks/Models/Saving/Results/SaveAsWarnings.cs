using System;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Any warnings of a model save operation. 
    /// Warnings mean the save was successful, but it had some warnings.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     Warnings are bit-masked (flags) so you can get each warning via:
    ///     
    ///     <code>
    ///         warnings.GetFlags();
    ///     </code>
    /// </para>
    /// </remarks>
    [Flags]
    public enum SaveAsWarnings
    {
        /// <summary>
        /// No warnings
        /// </summary>
        None = 0,

        /// <summary>
        /// There was a problem rebuilding the model
        /// </summary>
        RebuildError = 1,

        /// <summary>
        /// The model needs to be rebuilt
        /// </summary>
        NeedsRebuild = 2,

        /// <summary>
        /// The drawing views need updating
        /// </summary>
        ViewsNeedUpdate = 4,

        /// <summary>
        /// The animator needs to be solved
        /// </summary>
        AnimatorNeedToSolve = 8,

        /// <summary>
        /// The animator feature has edits
        /// </summary>
        AnimatorFeatureEdits = 16,

        /// <summary>
        /// The eDrawing file has a bad selection
        /// </summary>
        EdrwingsBadSelection = 32,

        /// <summary>
        /// The animator lights has edits
        /// </summary>
        AnimatorLightEdits = 64,

        /// <summary>
        /// The animator camera views have issues
        /// </summary>
        AnimatorCameraViews = 128,

        /// <summary>
        /// The animator section views have issues
        /// </summary>
        AnimatorSectionViews = 256,

        /// <summary>
        /// The file is missing OLE objects
        /// </summary>
        MissingOLEObjects = 512,

        /// <summary>
        /// The file is using the opened view only
        /// </summary>
        OpenedViewOnly = 1024
    }
}
