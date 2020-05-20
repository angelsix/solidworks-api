using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks model of any type (Drawing, Part or Assembly)
    /// </summary>
    public class Model : SharedSolidDnaObject<ModelDoc2>
    {
        #region Public Properties

        /// <summary>
        /// The absolute file path of this model if it has been saved
        /// </summary>
        public string FilePath { get; protected set; }

        /// <summary>
        /// Indicates if this file has been saved (so exists on disk).
        /// If not, it's a new model currently only in-memory and will not have a file path
        /// </summary>
        public bool HasBeenSaved => !string.IsNullOrEmpty(FilePath);

        /// <summary>
        /// Indicates if this file needs saving (has file changes).
        /// </summary>
        public bool NeedsSaving => BaseObject.GetSaveFlag();

        /// <summary>
        /// The type of document such as a part, assembly or drawing
        /// </summary>
        public ModelType ModelType { get; protected set; }

        /// <summary>
        /// True if this model is a part
        /// </summary>
        public bool IsPart => ModelType == ModelType.Part;

        /// <summary>
        /// True if this model is an assembly
        /// </summary>
        public bool IsAssembly => ModelType == ModelType.Assembly;

        /// <summary>
        /// True if this model is a drawing
        /// </summary>
        public bool IsDrawing => ModelType == ModelType.Drawing;

        /// <summary>
        /// Contains extended information about the model
        /// </summary>
        public ModelExtension Extension { get; protected set; }

        /// <summary>
        /// Contains the current active configuration information
        /// </summary>
        public ModelConfiguration ActiveConfiguration { get; protected set; }

        /// <summary>
        /// The selection manager for this model
        /// </summary>
        public SelectionManager SelectionManager { get; protected set; }

        /// <summary>
        /// Get the number of configurations
        /// </summary>
        public int ConfigurationCount => BaseObject.GetConfigurationCount();

        /// <summary>
        /// Gets the configuration names
        /// </summary>
        public List<string> ConfigurationNames => new List<string>((string[])BaseObject.GetConfigurationNames());

        /// <summary>
        /// The mass properties of the part
        /// </summary>
        public MassProperties MassProperties => Extension.GetMassProperties();

        #endregion

        #region Public Events

        /// <summary>
        /// Called after the a drawing sheet was added
        /// </summary>
        public event Action<string> DrawingSheetAdded = (sheetName) => { };

        /// <summary>
        /// Called after the active drawing sheet has changed
        /// </summary>
        public event Action<string> DrawingActiveSheetChanged = (sheetName) => { };

        /// <summary>
        /// Called before the active drawing sheet changes
        /// </summary>
        public event Action<string> DrawingActiveSheetChanging = (sheetName) => { };

        /// <summary>
        /// Called after the a drawing sheet was deleted
        /// </summary>
        public event Action<string> DrawingSheetDeleted = (sheetName) => { };

        /// <summary>
        /// Called as the model is about to be closed
        /// </summary>
        public event Action ModelClosing = () => { };

        /// <summary>
        /// Called when the model is first modified since it was last saved.
        /// SOLIDWORKS marks the file as Dirty and sets <see cref="IModelDoc2.GetSaveFlag"/>
        /// </summary>
        public event Action ModelModified = () => { };

        /// <summary>
        /// Called after a model was rebuilt (any model type) or if the rollback bar position changed (for parts and assemblies).
        /// NOTE: Does not always fire on normal rebuild (Ctrl+B) on assemblies.
        /// </summary>
        public event Action ModelRebuilt = () => { };

        /// <summary>
        /// Called as the model has been saved
        /// </summary>
        public event Action ModelSaved = () => { };

        /// <summary>
        /// Called when the user cancels the save action and <see cref="ModelSaved"/> will not be fired.
        /// </summary>
        public event Action ModelSaveCanceled = () => { };

        /// <summary>
        /// Called before a model is saved with a new file name.
        /// Called before the Save As dialog is shown.
        /// Allows you to make changes that need to be included in the save. 
        /// </summary>
        public event Action<string> ModelSavingAs = (fileName) => { };

        /// <summary>
        /// Called when any of the model properties changes
        /// </summary>
        public event Action ModelInformationChanged = () => { };

        /// <summary>
        /// Called when the active configuration has changed
        /// </summary>
        public event Action ActiveConfigurationChanged = () => { };

        /// <summary>
        /// Called when the selected objects in the model have changed
        /// </summary>
        public event Action SelectionChanged = () => { };

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Model(ModelDoc2 model) : base(model)
        {
            // Update information about this model
            ReloadModelData();

            // Attach event handlers
            SetupModelEventHandlers();
        }

        #endregion

        #region Model Data

        /// <summary>
        /// Reloads all variables and data about this model
        /// </summary>
        protected void ReloadModelData()
        {
            // Clean up any previous data
            DisposeAllReferences();

            // Can't do much if there is no document
            if (BaseObject == null)
                return;

            // Get the file path
            FilePath = BaseObject.GetPathName();

            // Get the models type
            ModelType = (ModelType)BaseObject.GetType();

            // Get the extension
            Extension = new ModelExtension(BaseObject.Extension, this);

            // Get the active configuration
            ActiveConfiguration = new ModelConfiguration(BaseObject.IGetActiveConfiguration());

            // Get the selection manager
            SelectionManager = new SelectionManager(BaseObject.ISelectionManager);

            // Set drawing access
            Drawing = IsDrawing ? new DrawingDocument((DrawingDoc)BaseObject) : null;

            // Set part access
            Part = IsPart ? new PartDocument((PartDoc)BaseObject) : null;

            // Set assembly access
            Assembly = IsAssembly ? new AssemblyDocument((AssemblyDoc)BaseObject) : null;

            // Inform listeners
            ModelInformationChanged();
        }

        /// <summary>
        /// Hooks into model-specific events for keeping track of up-to-date information
        /// </summary>
        protected void SetupModelEventHandlers()
        {
            // Based on the type of model this is...
            switch (ModelType)
            {
                // Hook into the save and destroy events to keep data fresh
                case ModelType.Assembly:
                    AsAssembly().ActiveConfigChangePostNotify += ActiveConfigChangePostNotify;
                    AsAssembly().DestroyNotify += FileDestroyedNotify;
                    AsAssembly().FileSaveAsNotify2 += FileSaveAsPreNotify;
                    AsAssembly().FileSavePostCancelNotify += FileSaveCanceled;
                    AsAssembly().FileSavePostNotify += FileSavePostNotify;
                    AsAssembly().ModifyNotify += FileModified;
                    AsAssembly().RegenPostNotify2 += AssemblyOrPartRebuilt;
                    AsAssembly().UserSelectionPostNotify += UserSelectionPostNotify;
                    AsAssembly().ClearSelectionsNotify += UserSelectionPostNotify;
                    break;
                case ModelType.Part:
                    AsPart().ActiveConfigChangePostNotify += ActiveConfigChangePostNotify;
                    AsPart().DestroyNotify += FileDestroyedNotify;
                    AsPart().FileSaveAsNotify2 += FileSaveAsPreNotify;
                    AsPart().FileSavePostCancelNotify += FileSaveCanceled;
                    AsPart().FileSavePostNotify += FileSavePostNotify;
                    AsPart().ModifyNotify += FileModified;
                    AsPart().RegenPostNotify2 += AssemblyOrPartRebuilt;
                    AsPart().UserSelectionPostNotify += UserSelectionPostNotify;
                    AsPart().ClearSelectionsNotify += UserSelectionPostNotify;
                    break;
                case ModelType.Drawing:
                    AsDrawing().ActivateSheetPostNotify += SheetActivatePostNotify;
                    AsDrawing().ActivateSheetPreNotify += SheetActivatePreNotify;
                    AsDrawing().AddItemNotify += DrawingItemAddNotify;
                    AsDrawing().DeleteItemNotify += DrawingDeleteItemNotify;
                    AsDrawing().DestroyNotify += FileDestroyedNotify;
                    AsDrawing().FileSaveAsNotify2 += FileSaveAsPreNotify;
                    AsDrawing().FileSavePostCancelNotify += FileSaveCanceled;
                    AsDrawing().FileSavePostNotify += FileSavePostNotify;
                    AsDrawing().ModifyNotify += FileModified;
                    AsDrawing().RegenPostNotify += DrawingRebuilt;
                    AsDrawing().UserSelectionPostNotify += UserSelectionPostNotify;
                    AsDrawing().ClearSelectionsNotify += UserSelectionPostNotify;
                    break;
            }
        }

        /// <summary>
        /// Packs up the current model into a flattened structure to a new location
        /// </summary>
        /// <param name="outputFolder">The output folder. If left blank will go to Local App Data folder under a unique name</param>
        /// <param name="filenamePrefix">A prefix to add to all files once packed</param>
        /// <returns></returns>
        public string PackAndGo(string outputFolder = null, string filenamePrefix = "")
        {
            // Wrap any error
            return SolidDnaErrors.Wrap(() =>
            {
                // If no output path specified...
                if (string.IsNullOrEmpty(outputFolder))
                    // Set it to app data folder
                    outputFolder = Path.Combine(
                        System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),
                        "SolidDna",
                        "PackAndGo",
                        // Unique folder name of current time
                        DateTime.UtcNow.ToString("MM-dd-yyyy-HH-mm-ss"));

                // Create output folder
                Directory.CreateDirectory(outputFolder);

                // If folder is not empty
                if (Directory.GetFiles(outputFolder)?.Length > 0)
                    throw new ArgumentException("Output folder is not empty");

                // Get pack and go object
                var packAndGo = BaseObject.Extension.GetPackAndGo();

                // Include any drawings, SOLIDWORKS Simulation results, and SOLIDWORKS Toolbox components
                packAndGo.IncludeDrawings = true;

                // NOTE: We could include more files...
                // packAndGo.IncludeSimulationResults = true;
                // packAndGo.IncludeToolboxComponents = true;

                // Add prefix to all files
                packAndGo.AddPrefix = filenamePrefix;

                // Get current paths and filenames of the assembly's documents
                if (!packAndGo.GetDocumentNames(out var filesArray))
                    // Throw error
                    throw new ArgumentException("Failed to get document names");

                // Cast filenames
                var filenames = (string[])filesArray;

                // If fails to set folder where to save the files
                if (!packAndGo.SetSaveToName(true, outputFolder))
                    // Throw error
                    throw new ArgumentException("Failed to set save to folder");

                // Flatten the Pack and Go folder so all files are in single folder
                packAndGo.FlattenToSingleFolder = true;

                // Save all files
                var results = (PackAndGoSaveStatus[])BaseObject.Extension.SavePackAndGo(packAndGo);

                // There is a result per file, so all must be successful
                if (!results.All(f => f == PackAndGoSaveStatus.Success))
                    // Throw error
                    throw new ArgumentException("Failed to save pack and go");

                // Return the output folder
                return outputFolder;
            },
                SolidDnaErrorTypeCode.SolidWorksModel,
                SolidDnaErrorCode.SolidWorksModelPackAndGoError,
                Localization.GetString("SolidWorksModelPackAndGoError"));

        }

        /// <summary>
        /// Unhooks model-specific events when model becomes inactive.
        /// Model becomes inactive when it is closed or when another model becomes the active model.
        /// </summary>
        protected void ClearModelEventHandlers()
        {
            switch (ModelType)
            {
                case ModelType.Part when AsPart() == null:
                case ModelType.Assembly when AsAssembly() == null:
                case ModelType.Drawing when AsDrawing() == null:
                {
                    // Happens in multiple cases:
                    // 1: When SolidWorks is being closed
                    // 1: When the non-last model is being closed
                    // 2: When the first model is opened after all models were closed.
                    return;
                }
            }

            // Based on the type of model this is...
            switch (ModelType)
            {
                // Hook into the save and destroy events to keep data fresh
                case ModelType.Assembly:
                    AsAssembly().ActiveConfigChangePostNotify -= ActiveConfigChangePostNotify;
                    AsAssembly().DestroyNotify -= FileDestroyedNotify;
                    AsAssembly().FileSaveAsNotify2 -= FileSaveAsPreNotify;
                    AsAssembly().FileSavePostCancelNotify -= FileSaveCanceled;
                    AsAssembly().FileSavePostNotify -= FileSavePostNotify;
                    AsAssembly().ModifyNotify -= FileModified;
                    AsAssembly().RegenPostNotify2 -= AssemblyOrPartRebuilt;
                    AsAssembly().UserSelectionPostNotify -= UserSelectionPostNotify;
                    AsAssembly().ClearSelectionsNotify -= UserSelectionPostNotify;
                    break;
                case ModelType.Part:
                    AsPart().ActiveConfigChangePostNotify -= ActiveConfigChangePostNotify;
                    AsPart().DestroyNotify -= FileDestroyedNotify;
                    AsPart().FileSaveAsNotify2 -= FileSaveAsPreNotify;
                    AsPart().FileSavePostCancelNotify -= FileSaveCanceled;
                    AsPart().FileSavePostNotify -= FileSavePostNotify;
                    AsPart().ModifyNotify -= FileModified;
                    AsPart().RegenPostNotify2 -= AssemblyOrPartRebuilt;
                    AsPart().UserSelectionPostNotify -= UserSelectionPostNotify;
                    AsPart().ClearSelectionsNotify -= UserSelectionPostNotify;
                    break;
                case ModelType.Drawing:
                    AsDrawing().ActivateSheetPostNotify -= SheetActivatePostNotify;
                    AsDrawing().ActivateSheetPreNotify -= SheetActivatePreNotify;
                    AsDrawing().AddItemNotify -= DrawingItemAddNotify;
                    AsDrawing().DeleteItemNotify -= DrawingDeleteItemNotify;
                    AsDrawing().DestroyNotify -= FileDestroyedNotify;
                    AsDrawing().FileSaveAsNotify2 -= FileSaveAsPreNotify;
                    AsDrawing().FileSavePostCancelNotify -= FileSaveCanceled;
                    AsDrawing().FileSavePostNotify -= FileSavePostNotify;
                    AsDrawing().ModifyNotify -= FileModified;
                    AsDrawing().RegenPostNotify -= DrawingRebuilt;
                    AsDrawing().UserSelectionPostNotify -= UserSelectionPostNotify;
                    AsDrawing().ClearSelectionsNotify -= UserSelectionPostNotify;
                    break;
            }
        }

        #endregion

        #region Model Event Methods

        /// <summary>
        /// Called when a part or assembly has its active configuration changed
        /// </summary>
        /// <returns></returns>
        protected int ActiveConfigChangePostNotify()
        {
            // Refresh data
            ReloadModelData();

            // Inform listeners
            ActiveConfigurationChanged();

            // NOTE: 0 is success, anything else is an error
            return 0;
        }

        /// <summary>
        /// Called after an assembly or part was rebuilt or if the rollback bar position changed.
        /// </summary>
        /// <param name="firstFeatureBelowRollbackBar"></param>
        /// <returns></returns>
        protected int AssemblyOrPartRebuilt(object firstFeatureBelowRollbackBar)
        {
            // Inform listeners
            ModelRebuilt();

            // NOTE: 0 is success, anything else is an error
            return 0;
        }

        /// <summary>
        /// Called when a drawing item is added to the feature tree
        /// </summary>
        /// <param name="entityType">Type of entity that is changed</param>
        /// <param name="itemName">Name of entity that is changed</param>
        /// <returns></returns>
        protected int DrawingItemAddNotify(int entityType, string itemName)
        {
            // Check if a sheet is added.
            // SolidWorks always activates the new sheet, but the sheet activate events aren't fired.
            if (EntityIsDrawingSheet(entityType))
            {
                SheetAddedNotify(itemName);
                SheetActivatePostNotify(itemName);
            }

            // NOTE: 0 is success, anything else is an error
            return 0;
        }

        /// <summary>
        /// Called when a drawing item is removed from the feature tree
        /// </summary>
        /// <param name="entityType">Type of entity that is changed</param>
        /// <param name="itemName">Name of entity that is changed</param>
        /// <returns></returns>
        protected int DrawingDeleteItemNotify(int entityType, string itemName)
        {
            // Check if the removed items is a sheet
            if (EntityIsDrawingSheet(entityType))
            {
                // Inform listeners
                DrawingSheetDeleted(itemName);
            }

            // NOTE: 0 is success, anything else is an error
            return 0;
        }

        /// <summary>
        /// Called after a drawing was rebuilt.
        /// </summary>
        /// <returns></returns>
        protected int DrawingRebuilt()
        {
            // Inform listeners
            ModelRebuilt();

            // NOTE: 0 is success, anything else is an error
            return 0;
        }

        /// <summary>
        /// Called when the user changes the selected objects
        /// </summary>
        /// <returns></returns>
        protected int UserSelectionPostNotify()
        {
            // Inform Listeners
            SelectionChanged();

            return 0;
        }

        /// <summary>
        /// Called when the user cancels the save action and <see cref="FileSavePostNotify"/> will not be fired.
        /// </summary>
        /// <returns></returns>
        private int FileSaveCanceled()
        {
            // Inform listeners
            ModelSaveCanceled();

            // NOTE: 0 is success, anything else is an error
            return 0;
        }

        /// <summary>
        /// Called when a model has been saved
        /// </summary>
        /// <param name="fileName">The name of the file that has been saved</param>
        /// <param name="saveType">The type of file that has been saved</param>
        /// <returns></returns>
        protected int FileSavePostNotify(int saveType, string fileName)
        {
            // Were we saved or is this a new file?
            var wasNewFile = !HasBeenSaved;

            // Update filepath
            FilePath = BaseObject.GetPathName();

            // Inform listeners
            ModelSaved();

            // NOTE: Due to bug in SolidWorks, saving new files refreshes the COM reference
            //       without it ever being so kind as to inform us via ANY callback in 
            //       the SolidWorksApplication or this model
            //      
            //       So to fix, wait for an idle moment and refresh our info.
            //       Best fix I can think of for now.
            //
            //       We could keep checking the COM instance BaseObject doesn't throw
            //       an error to detect when it got disposed but I think the idle
            //       is less intensive and works fine so far
            if (wasNewFile)
            {
                void refreshEvent()
                {
                    SolidWorksEnvironment.Application.RequestActiveModelChanged();
                    SolidWorksEnvironment.Application.Idle -= refreshEvent;
                }

                SolidWorksEnvironment.Application.Idle += refreshEvent;
            }

            // NOTE: 0 is success, anything else is an error
            return 0;
        }

        /// <summary>
        /// Called when a model is about to be saved with a new file name.
        /// Called before the Save As dialog is shown.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private int FileSaveAsPreNotify(string fileName)
        {
            // Inform listeners
            ModelSavingAs(fileName);

            // NOTE: 0 is success, anything else is an error
            return 0;
        }

        /// <summary>
        /// Called when a model is about to be destroyed.
        /// This is a pre-notify so just clean up data, don't reload for new data yet
        /// </summary>
        /// <returns></returns>
        protected int FileDestroyedNotify()
        {
            // Inform listeners
            ModelClosing();

            // This is a pre-notify but we are going to be dead
            // so dispose ourselves (our underlying COM objects)
            Dispose();

            // NOTE: 0 is success, anything else is an error
            return 0;
        }

        /// <summary>
        /// Called when the model is first modified since it was last saved.
        /// </summary>
        /// <returns></returns>
        protected int FileModified()
        {
            // Inform listeners
            ModelModified();

            // NOTE: 0 is success, anything else is an error
            return 0;
        }

        /// <summary>
        /// Called after the active drawing sheet is changed.
        /// </summary>
        /// <param name="sheetName">Name of the sheet that is activated</param>
        /// <returns></returns>
        protected int SheetActivatePostNotify(string sheetName)
        {
            // Inform listeners
            DrawingActiveSheetChanged(sheetName);

            // NOTE: 0 is success, anything else is an error
            return 0;
        }

        /// <summary>
        /// Called before the active drawing sheet changes
        /// </summary>
        protected int SheetActivatePreNotify(string sheetName)
        {
            // Inform listeners
            DrawingActiveSheetChanging(sheetName);

            // NOTE: 0 is success, anything else is an error
            return 0;

        }

        /// <summary>
        /// Called after a sheet is added.
        /// </summary>
        /// <param name="sheetName">Name of the new sheet</param>
        /// <returns></returns>
        protected int SheetAddedNotify(string sheetName)
        {
            // Inform listeners
            DrawingSheetAdded(sheetName);

            // NOTE: 0 is success, anything else is an error
            return 0;
        }

        #endregion

        #region Specific Model

        /// <summary>
        /// Casts the current model to an assembly
        /// NOTE: Check the <see cref="ModelType"/> to confirm this model is of the correct type before casting
        /// </summary>
        /// <returns></returns>
        public AssemblyDoc AsAssembly() => (AssemblyDoc)BaseObject;

        /// <summary>
        /// Casts the current model to a part
        /// NOTE: Check the <see cref="ModelType"/> to confirm this model is of the correct type before casting
        /// </summary>
        /// <returns></returns>
        public PartDoc AsPart() => (PartDoc)BaseObject;

        /// <summary>
        /// Casts the current model to a drawing
        /// NOTE: Check the <see cref="ModelType"/> to confirm this model is of the correct type before casting
        /// </summary>
        /// <returns></returns>
        public DrawingDoc AsDrawing() => (DrawingDoc)BaseObject;

        /// <summary>
        /// Accesses the current model as a drawing to expose all Drawing API calls.
        /// Check <see cref="IsDrawing"/> before calling into this.
        /// </summary>
        public DrawingDocument Drawing { get; private set; }

        /// <summary>
        /// Accesses the current model as a part to expose all Part API calls.
        /// Check <see cref="IsPart"/> before calling into this.
        /// </summary>
        public PartDocument Part { get; private set; }

        /// <summary>
        /// Accesses the current model as an assembly to expose all Assembly API calls.
        /// Check <see cref="IsAssembly"/> before calling into this.
        /// </summary>
        public AssemblyDocument Assembly { get; private set; }

        #endregion

        #region Custom Properties

        /// <summary>
        /// Sets a custom property to the given value.
        /// If a configuration is specified then the configuration-specific property is set
        /// </summary>
        /// <param name="name">The name of the property</param>
        /// <param name="value">The value of the property</param>
        /// <param name="configuration">The configuration to set the properties from, otherwise set custom property</param>
        public void SetCustomProperty(string name, string value, string configuration = null)
        {
            // Get the custom property editor
            using (var editor = Extension.CustomPropertyEditor(configuration))
            {
                // Set the property
                editor.SetCustomProperty(name, value);
            }
        }

        /// <summary>
        /// Gets a custom property by the given name
        /// </summary>
        /// <param name="name">The name of the custom property</param>
        /// <param name="configuration">The configuration to get the properties from, otherwise get custom property</param>
        ///<param name="resolved">True to get the resolved value of the property, false to get the actual text</param>
        /// <returns></returns>
        public string GetCustomProperty(string name, string configuration = null, bool resolved = false)
        {
            // Get the custom property editor
            using (var editor = Extension.CustomPropertyEditor(configuration))
            {
                // Get the property
                return editor.GetCustomProperty(name, resolve: resolved);
            }
        }

        /// <summary>
        /// Gets all of the custom properties in this model.
        /// Simply set the Value of the custom property to edit it
        /// </summary>
        /// <param name="action">The custom properties list to be worked on inside the action. NOTE: Do not store references to them outside of this action</param>
        /// <param name="configuration">Specify a configuration to get configuration-specific properties</param>
        /// <returns></returns>
        public void CustomProperties(Action<List<CustomProperty>> action, string configuration = null)
        {
            // Get the custom property editor
            using (var editor = Extension.CustomPropertyEditor(configuration))
            {
                // Get the properties
                var properties = editor.GetCustomProperties();

                // Let the action use them
                action(properties);
            }
        }

        /// <summary>
        /// Gets all of the custom properties in this model including any configuration specific properties
        /// </summary>
        /// <param name="action">The custom properties list to be worked on inside the action. NOTE: Do not store references to them outside of this action</param>
        /// <returns>Custom property and the configuration name it belongs to (or null if none)</returns>
        public IEnumerable<(string configuration, CustomProperty property)> AllCustomProperties()
        {
            // Custom properties
            using (var editor = Extension.CustomPropertyEditor(null))
            {
                // Get the properties
                var properties = editor.GetCustomProperties();

                // Loop each property
                foreach (var property in properties)
                    // Return result
                    yield return (null, property);
            }

            // Configuration specific properties
            foreach (var configuration in ConfigurationNames)
            {
                // Get the custom property editor
                using (var editor = Extension.CustomPropertyEditor(configuration))
                {
                    // Get the properties
                    var properties = editor.GetCustomProperties();

                    // Loop each property
                    foreach (var property in properties)
                        // Return result
                        yield return (configuration, property);
                }
            }
        }

        #endregion

        #region Drawings

        /// <summary>
        /// Check if an entity that was added, changed or removed is a drawing sheet.
        /// </summary>
        /// <param name="entityType">Type of the entity</param>
        /// <returns></returns>
        private static bool EntityIsDrawingSheet(int entityType)
        {
            return entityType == (int)swNotifyEntityType_e.swNotifyDrawingSheet;
        }

        #endregion

        #region Material

        /// <summary>
        /// Read the material from the model
        /// </summary>
        /// <returns></returns>
        public Material GetMaterial()
        {
            // Wrap any error
            return SolidDnaErrors.Wrap(() =>
            {
                // Get the Id's
                var idString = BaseObject.MaterialIdName;

                // Make sure we have some data
                if (idString == null || !idString.Contains("|"))
                    return null;

                // The Id string is split by pipes |
                var ids = idString.Split('|');

                // We need at least the first and second 
                // (first is database file name, second is material name)
                if (ids.Length < 2)
                    throw new ArgumentOutOfRangeException(Localization.GetString("SolidWorksModelGetMaterialIdMissingError"));

                // Extract data
                var databaseName = ids[0];
                var materialName = ids[1];

                // See if we have a database file with the same name
                var fullPath = SolidWorksEnvironment.Application.GetMaterials()?.FirstOrDefault(f => string.Equals(databaseName, Path.GetFileNameWithoutExtension(f.Database), StringComparison.InvariantCultureIgnoreCase));
                var found = fullPath != null;

                // Now we have the file, try and find the material from it
                if (found)
                {
                    var foundMaterial = SolidWorksEnvironment.Application.FindMaterial(fullPath.Database, materialName);
                    if (foundMaterial != null)
                        return foundMaterial;
                }

                // If we got here, the material was not found
                // So fill in as much information as we have
                return new Material
                {
                    Database = databaseName,
                    Name = materialName
                };
            },
                SolidDnaErrorTypeCode.SolidWorksModel,
                SolidDnaErrorCode.SolidWorksModelGetMaterialError,
                Localization.GetString("SolidWorksModelGetMaterialError"));
        }

        /// <summary>
        /// Sets the material for the model
        /// </summary>
        /// <param name="material">The material</param>
        /// <param name="configuration">The configuration to set the material on, null for the default</param>
        /// 
        public void SetMaterial(Material material, string configuration = null)
        {
            // Wrap any error
            SolidDnaErrors.Wrap(() =>
            {
                // Make sure we are a part
                if (!IsPart)
                    throw new InvalidOperationException(Localization.GetString("SolidWorksModelSetMaterialModelNotPartError"));

                // If the material is null, remove the material
                if (material == null || !material.DatabaseFileFound)
                    AsPart().SetMaterialPropertyName2(string.Empty, string.Empty, string.Empty);
                // Otherwise set the material
                else
                    AsPart().SetMaterialPropertyName2(configuration, material.Database, material.Name);
            },
                SolidDnaErrorTypeCode.SolidWorksModel,
                SolidDnaErrorCode.SolidWorksModelSetMaterialError,
                Localization.GetString("SolidWorksModelSetMaterialError"));
        }

        #endregion

        #region Selected Entities

        /// <summary>
        /// Gets all of the selected objects in the model
        /// </summary>
        /// <param name="action">The selected objects list to be worked on inside the action. NOTE: Do not store references to them outside of this action</param>
        /// <returns></returns>
        public void SelectedObjects(Action<List<SelectedObject>> action)
        {
            SelectionManager?.SelectedObjects(action);
        }

        #endregion

        #region Features

        /// <summary>
        /// Recurses the model for all of it's features and sub-features
        /// </summary>
        /// <param name="featureAction">The callback action that is called for each feature in the model</param>
        public void Features(Action<ModelFeature, int> featureAction)
        {
            RecurseFeatures(featureAction, UnsafeObject.FirstFeature() as Feature);
        }

        #region Private Feature Helpers

        /// <summary>
        /// Recurses features and sub-features and provides a callback action to process and work with each feature
        /// </summary>
        /// <param name="featureAction">The callback action that is called for each feature in the model</param>
        /// <param name="startFeature">The feature to start at</param>
        /// <param name="featureDepth">The current depth of the sub-features based on the original calling feature</param>
        private static void RecurseFeatures(Action<ModelFeature, int> featureAction, Feature startFeature = null, int featureDepth = 0)
        {
            // Get the current feature
            var currentFeature = startFeature;

            // While that feature is not null...
            while (currentFeature != null)
            {
                // Now get the first sub-feature
                var subFeature = currentFeature.GetFirstSubFeature() as Feature;

                // Get the next feature if we should
                var nextFeature = default(Feature);
                if (featureDepth == 0)
                    nextFeature = currentFeature.GetNextFeature() as Feature;

                // Create model feature
                using (var modelFeature = new ModelFeature((Feature)currentFeature))
                {
                    // Inform callback of the feature
                    featureAction(modelFeature, featureDepth);
                }

                // While we have a sub-feature...
                while (subFeature != null)
                {
                    // Get its next sub-feature
                    var nextSubFeature = subFeature.GetNextSubFeature() as Feature;

                    // Recurse all of the sub-features
                    RecurseFeatures(featureAction, subFeature, featureDepth + 1);

                    // And once back up out of the recursive dive
                    // Move to the next sub-feature and process that
                    subFeature = nextSubFeature;
                }

                // If we are at the top-level...
                if (featureDepth == 0)
                {
                    // And update the current feature reference to the next feature
                    // to carry on the loop
                    currentFeature = nextFeature;
                }
                // Otherwise...
                else
                {
                    // Remove the current feature as it is a sub-feature
                    // and is processed in the `while (subFeature != null)` loop
                    currentFeature = null;
                }
            }
        }

        #endregion

        #endregion

        #region Components

        /// <summary>
        /// Recurses the model for all of it's components and sub-components
        /// </summary>
        public IEnumerable<(Component component, int depth)> Components()
        {
            // Component to be set
            Component component;

            try
            {
                // Try and create component object from active configuration
                component = new Component(ActiveConfiguration.UnsafeObject?.GetRootComponent3(true));
            }
            // If COM failure...
            catch (InvalidComObjectException)
            {
                // Re-get configuration
                ActiveConfiguration = new ModelConfiguration(BaseObject.IGetActiveConfiguration());

                // Try once more
                component = new Component(ActiveConfiguration.UnsafeObject?.GetRootComponent3(true));
            }

            // Return components
            return RecurseComponents(component);
        }

        #region Private Component Helpers

        /// <summary>
        /// Recurses components and sub-components and provides a callback action to process and work with each components
        /// </summary>
        /// <param name="componentAction">The callback action that is called for each components in the component</param>
        /// <param name="startComponent">The components to start at</param>
        /// <param name="componentDepth">The current depth of the sub-components based on the original calling components</param>
        private IEnumerable<(Component, int)> RecurseComponents(Component startComponent = null, int componentDepth = 0)
        {
            // While that component is not null...
            if (startComponent != null)
                // Inform callback of the feature
                yield return (startComponent, componentDepth);

            // Loop each child
            if (startComponent.Children != null)
                foreach (Component2 childComponent in startComponent.Children)
                {
                    // Get the current component
                    using (var currentComponent = new Component(childComponent))
                    {
                        // If we have a component
                        if (currentComponent != null)
                            // Recurse into it
                            foreach (var component in RecurseComponents(currentComponent, componentDepth + 1))
                                // Return component
                                yield return component;
                    }
                }
        }

        #endregion

        #endregion

        #region Dependencies

        /// <summary>
        /// Returns a list of full file paths for all dependencies of this model
        /// </summary>
        /// <param name="includeSelf">True to include this file as part of the dependency list</param>
        /// <param name="includeDrawings">True to look for drawings with the same name as their models</param>
        /// <returns>Returns a list of full filepaths of all dependencies of this model</returns>
        public List<string> Dependencies(bool includeSelf = true, bool includeDrawings = true)
        {
            // New list
            var dependencies = new List<string>();

            // If we should add ourselves...
            if (includeSelf)
                // Add main model
                dependencies.Add(FilePath);

            // Get array of dependencies of currently active swModel
            var modelDependencies = (string[])BaseObject.GetDependencies2(true, true, false);

            // Add all dependencies (Format {"Name1", "Path1+Name1", "Name2", "Path2+Name2", ...})
            // Take every other element, starting at second one
            foreach (var dependant in modelDependencies.Where((f, i) => (i + 1) % 2 == 0))
                dependencies.Add(dependant);

            // Find any drawings that exist...
            foreach (var drawing in dependencies.Where(f => !f.ToLower().EndsWith(".slddrw") && File.Exists(Path.ChangeExtension(f, ".slddrw")))
                // Clone list so we can add new items to same list
                .ToList())
            {
                // Add them to list
                dependencies.Add(drawing);
            }

            // Return the list (filtering our duplicates)
            return dependencies.Distinct().ToList();
        }

        #endregion

        #region Saving

        /// <summary>
        /// Saves the current model, with the specified options
        /// </summary>
        /// <param name="options">Any save as options</param>
        /// <returns></returns>
        public ModelSaveResult Save(SaveAsOptions options = SaveAsOptions.None)
        {
            // Start with a successful result
            var results = new ModelSaveResult();

            // Set errors and warnings to none to start with
            var errors = 0;
            var warnings = 0;

            // Wrap any error
            return SolidDnaErrors.Wrap(() =>
            {
                // Try and save the model using the Save3 method
                BaseObject.Save3((int)options, ref errors, ref warnings);

                // Add any warnings
                results.Warnings = (SaveAsWarnings)warnings;

                // Add any errors
                results.Errors = (SaveAsErrors)errors;

                // If successful, and this is not a new file 
                // (otherwise the RCW changes and SolidWorksApplication has to reload ActiveModel)...
                if (results.Successful && HasBeenSaved)
                    // Reload model data
                    ReloadModelData();

                // If we have not been saved, SolidWorks never fires any FileSave events at all
                // so request a refresh of the ActiveModel. That is the best we can do
                // as this RCW is now invalid. If this model is not active when saved then 
                // it will simply reload the active models information
                if (!HasBeenSaved)
                    SolidWorksEnvironment.Application.RequestActiveModelChanged();

                // Return result
                return results;
            },
                SolidDnaErrorTypeCode.SolidWorksModel,
                SolidDnaErrorCode.SolidWorksModelSaveError,
                Localization.GetString("SolidWorksModelSaveError"));
        }

        /// <summary>
        /// Saves a file to the specified path, with the specified options
        /// </summary>
        /// <param name="savePath">The path of the file to save as</param>
        /// <param name="version">The version</param>
        /// <param name="options">Any save as options</param>
        /// <param name="pdfExportData">The PDF Export data if the save as type is a PDF</param>
        /// <returns></returns>
        public ModelSaveResult SaveAs(string savePath, SaveAsVersion version = SaveAsVersion.CurrentVersion, SaveAsOptions options = SaveAsOptions.None, PdfExportData pdfExportData = null)
        {
            // Start with a successful result
            var results = new ModelSaveResult();

            // Set errors and warnings to none to start with
            var errors = 0;
            var warnings = 0;

            // Wrap any error
            return SolidDnaErrors.Wrap(() =>
            {
                // Try and save the model using the SaveAs method
                BaseObject.Extension.SaveAs(savePath, (int)version, (int)options, pdfExportData?.ExportData, ref errors, ref warnings);

                // If this fails, try another way
                if (errors != 0)
                    BaseObject.SaveAs4(savePath, (int)version, (int)options, ref errors, ref warnings);

                // Add any warnings
                results.Warnings = (SaveAsWarnings)warnings;

                // Add any errors
                results.Errors = (SaveAsErrors)errors;

                // If successful, and this is not a new file 
                // (otherwise the RCW changes and SolidWorksApplication has to reload ActiveModel)...
                if (results.Successful && HasBeenSaved)
                    // Reload model data
                    ReloadModelData();

                // If we have not been saved, SolidWorks never fires any FileSave events at all
                // so request a refresh of the ActiveModel. That is the best we can do
                // as this RCW is now invalid. If this model is not active when saved then 
                // it will simply reload the active models information
                if (!HasBeenSaved)
                    SolidWorksEnvironment.Application.RequestActiveModelChanged();

                // Return result
                return results;
            },
                SolidDnaErrorTypeCode.SolidWorksModel,
                SolidDnaErrorCode.SolidWorksModelSaveAsError,
                Localization.GetString("SolidWorksModelSaveAsError"));
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns a user-friendly string with model properties.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Model type: {ModelType}. File path: {FilePath}";
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Clean up all COM references for this model, its children and anything that was used by this model
        /// </summary>
        protected void DisposeAllReferences()
        {
            // Tidy up embedded SolidDNA objects
            Extension?.Dispose();
            Extension = null;

            // Release the active configuration
            ActiveConfiguration?.Dispose();
            ActiveConfiguration = null;

            // Selection manager
            SelectionManager?.Dispose();
            SelectionManager = null;

            // Unhook all events
            ClearModelEventHandlers();
        }

        public override void Dispose()
        {
            // Clean up embedded objects
            DisposeAllReferences();

            // Dispose self
            base.Dispose();
        }

        #endregion
    }
}
