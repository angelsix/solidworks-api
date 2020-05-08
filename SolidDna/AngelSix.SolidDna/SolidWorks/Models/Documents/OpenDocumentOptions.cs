using SolidWorks.Interop.swconst;
using System;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Options for opening a document, from <see cref="swOpenDocOptions_e"/>
    /// </summary>
    [Flags]
    public enum OpenDocumentOptions
    {
        /// <summary>
        /// No specific options
        /// </summary>
        None = 0,

        /// <summary>
        /// The software automatically uses the last-used configuration of a model when it
        /// discovers missing configurations or component references as it silently 
        /// opens drawings and assemblies.
        /// </summary>
        [Obsolete]
        AutoMissingConfiguration = 32,

        /// <summary>
        ///  By default, hidden components are loaded when you open an assembly document. 
        ///  Set to not load hidden components when opening an assembly document.
        /// </summary>
        DontLoadHiddenComponents = 256,

        /// <summary>
        /// Open external references in memory only; this setting is valid only if 
        /// swUserPreferenceIntegerValue_e.swLoadExternalReferences is not set to 
        /// swLoadExternalReferences_e.swLoadExternalReferences_None.
        /// 
        /// swUserPreferenceToggle_e.swExtRefLoadRefDocsInMemory 
        /// (System Options > External References > Load documents in memory only) 
        /// is ignored when opening documents through the API because 
        /// IDocumentSpecification::LoadExternalReferencesInMemory and 
        /// ISldWorks::OpenDoc6 (swOpenDocOptions_e.swOpenDocOptions_LoadExternalReferencesInMemory) 
        /// have sole control over reference loading
        /// </summary>
        LoadExternalReferencesInMemory = 512,

        /// <summary>
        /// Open assembly document as lightweight.
        /// 
        /// NOTE: The default for whether an assembly document is opened lightweight is based on 
        /// a registry setting accessed via Tools, Options, Assemblies or with the user preference 
        /// setting swAutoLoadPartsLightweight.
        /// 
        /// To override the default and specify a value with 
        /// <see cref="SolidDna.SolidWorksApplication.OpenFile(string, bool)"/>, set <see cref="LoadLightweight"/>. 
        /// 
        /// If set, then you can set <see cref="LoadLightweight"/> to open an assembly document as lightweight
        /// </summary>
        LoadLightweight = 128,

        /// <summary>
        /// Load Detached model upon opening document (drawings only).
        /// </summary>
        LoadModel = 16,

        /// <summary>
        /// Override default setting whether to open an assembly document as lightweight
        /// </summary>
        OverrideDefaultLoadLightweight = 64,

        /// <summary>
        /// Convert document to Detached format (drawings only)
        /// </summary>
        RapidDraft = 8,

        /// <summary>
        /// Open document read only
        /// </summary>
        ReadOnly = 2,

        /// <summary>
        /// Open document silently
        /// </summary>
        Silent = 1,

        /// <summary>
        /// Open document in Large Design Review mode only (assemblies only).
        /// </summary>
        ViewOnly = 4,
    }
}
