using System;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Save as options, from <see cref="SolidWorks.Interop.swconst.swSaveAsOptions_e"/>
    /// </summary>
    /// <remarks>
    /// <para>
    ///     Options are bit-masked (flags) so you can specify one or more:
    ///     
    ///     <code>
    ///         model.SaveAs("name.sldprt", options: SaveAsOptions.Silent | SaveAsOptions.AvoidRebuildOnSave);
    ///     </code>
    /// </para>
    /// <para>
    ///     These options only apply to saving to native SolidWorks file formats. 
    /// </para>
    /// <para>
    ///     For example, to export a SolidWorks file to a VRML file format, use 
    ///     ISldWorks::GetUserPreferenceToggle and ISldWorks::SetUserPreferenceToggle 
    ///     with swExportVrmlAllComponentsInSingleFile
    /// </para>
    /// </remarks>
    [Flags]
    public enum SaveAsOptions
    {
        /// <summary>
        /// To specify no specific options
        /// </summary>
        None = 0,

        /// <summary>
        /// Saves without any user interaction
        /// </summary>
        Silent = 1,

        /// <summary>
        /// Saves the file as a copy
        /// </summary>
        Copy = 2,

        /// <summary>
        /// Supports parts, assemblies and drawings; this setting indicates to 
        /// save all components (sub-assemblies and parts) in both assemblies 
        /// and drawings. If a part has an external reference, then this setting
        /// indicates to save the external reference
        /// </summary>
        SaveReferenced = 4,

        /// <summary>
        /// Prevents rebuilding the model prior to saving
        /// </summary>
        AvoidRebuildOnSave = 8,

        /// <summary>
        /// Not a valid option for IPartDoc::SaveToFile2; this setting is only
        /// applicable for a drawing that has one or more sheets; this setting 
        /// updates the views on inactive sheets
        /// </summary>
        UpdateInactiveViews = 16,

        /// <summary>
        /// Saves eDrawings-related information into a section of the file being 
        /// saved; specifying this setting overrides the Tools, Options,
        /// System Options, General, Save eDrawings data in SolidWorks document 
        /// setting; not a valid option for IPartDoc::SaveToFile2
        /// </summary>
        OverrideSaveEmodel = 32,

        /// <summary>
        /// Obsolete. Do not use
        /// </summary>
        [Obsolete]
        SaveEmodelData = 64,

        /// <summary>
        /// Saves a drawing as a detached drawing.
        /// Not a valid option for IPartDoc::SaveToFile2
        /// </summary>
        DetachedDrawing = 128,

        /// <summary>
        /// Prune a SolidWorks file's revision history to just the current file name
        /// </summary>
        IgnoreBiography = 256
    }
}
