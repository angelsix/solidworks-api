using System;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks model of any type (Drawing, Part or Assembly)
    /// </summary>
    public class Model : SolidDnaObject<ModelDoc2>
    {
        #region Public Properties

        /// <summary>
        /// The absolute file path of this model if it has been saved
        /// </summary>
        public string FilePath { get; protected set; }

        /// <summary>
        /// Indicates if this file has been saved (so exists on disk)
        /// If not, it's a new model currently only in-memory and will not have a file path
        /// </summary>
        public bool HasBeenSaved => !string.IsNullOrEmpty(UnsafeObject?.GetPathName());

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
        public ModelSelectionManager SelectionManager { get; protected set; }

        /// <summary>
        /// Get the number of configurations
        /// </summary>
        public int ConfigurationCount => mBaseObject.GetConfigurationCount();

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
        /// Called as the model has been saved
        /// </summary>
        public event Action ModelSaved = () => { };

        /// <summary>
        /// Called when any of the model information changes
        /// </summary>
        public event Action ModelInformationChanged = () => { };

        /// <summary>
        /// Called when the selected objects in the model has changed
        /// </summary>
        public event Action SelectionChanged = () => { };

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Model(ModelDoc2 model) : base(model)
        {
            ReloadModelData();
        }

        #endregion

        #region Model Data

        /// <summary>
        /// Reloads all variables and data about this model
        /// </summary>
        protected void ReloadModelData()
        {
            // Clean up any previous data
            DisposeChildren();

            // Can't do much if there is no document
            if (mBaseObject == null)
                return;

            // Get the file path
            FilePath = mBaseObject.GetPathName();

            // Get the models type
            ModelType = (ModelType)mBaseObject.GetType();

            // Get the extension
            Extension = new ModelExtension(mBaseObject.Extension, this);

            // Get the active configuration
            ActiveConfiguration = new ModelConfiguration(mBaseObject.IGetActiveConfiguration());

            // Get the selection manager
            SelectionManager = new ModelSelectionManager(mBaseObject.ISelectionManager);

            // Re-attach event handlers
            SetupModelEventHandlers();

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
                    AsAssembly().FileSavePostNotify += FileSaveNotify;
                    AsAssembly().UserSelectionPostNotify += UserSelectionPostNotify;
                    AsAssembly().ClearSelectionsNotify += UserSelectionPostNotify;
                    break;
                case ModelType.Part:
                    AsPart().ActiveConfigChangePostNotify += ActiveConfigChangePostNotify;
                    AsPart().DestroyNotify += FileDestroyedNotify;
                    AsPart().FileSavePostNotify += FileSaveNotify;
                    AsPart().UserSelectionPostNotify += UserSelectionPostNotify;
                    AsPart().ClearSelectionsNotify += UserSelectionPostNotify;
                    break;
                case ModelType.Drawing:
                    AsDrawing().ActivateSheetPostNotify += SheetActivatePostNotify;
                    AsDrawing().ActivateSheetPreNotify += SheetActivatePreNotify;
                    AsDrawing().AddItemNotify += DrawingItemAddNotify;
                    AsDrawing().DeleteItemNotify += DrawingDeleteItemNotify;
                    AsDrawing().DestroyNotify += FileDestroyedNotify;
                    AsDrawing().FileSavePostNotify += FileSaveNotify;
                    AsDrawing().UserSelectionPostNotify += UserSelectionPostNotify;
                    AsDrawing().ClearSelectionsNotify += UserSelectionPostNotify;
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
        /// Called when the user changes the selected objects
        /// </summary>
        /// <returns></returns>
        protected int UserSelectionPostNotify()
        {
            // Inform Listenes
            SelectionChanged();

            return 0;
        }

        /// <summary>
        /// Called when a model has been saved
        /// </summary>
        /// <param name="filename">The name of the file that has been saved</param>
        /// <param param name="saveType">The type of file that has been saved</param>
        /// <returns></returns>
        protected int FileSaveNotify(int saveType, string filename)
        {
            // Inform listeners
            ModelSaved();

            // NOTE: 0 is success, anything else is an error
            return 0;
        }

        /// <summary>
        /// Called when a model is about to be destroyed
        /// This is a pre-notify so just clean up data don't reload for new data yet
        /// </summary>
        /// <returns></returns>
        protected int FileDestroyedNotify()
        {
            // This is a pre-notify so just clear our data don't reload state
            DisposeChildren();

            // Inform listeners
            ModelClosing();

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
        public AssemblyDoc AsAssembly() { return ((AssemblyDoc)mBaseObject); }

        /// <summary>
        /// Casts the current model to a part
        /// NOTE: Check the <see cref="ModelType"/> to confirm this model is of the correct type before casting
        /// </summary>
        /// <returns></returns>
        public PartDoc AsPart() { return ((PartDoc)mBaseObject); }


        /// <summary>
        /// Casts the current model to a drawing
        /// NOTE: Check the <see cref="ModelType"/> to confirm this model is of the correct type before casting
        /// </summary>
        /// <returns></returns>
        public DrawingDoc AsDrawing() { return ((DrawingDoc)mBaseObject); }

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
        /// Get's all of the custom properties in this model
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
                var idString = mBaseObject.MaterialIdName;

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
                var fullPath = Dna.Application.GetMaterials()?.FirstOrDefault(f => string.Equals(databaseName, Path.GetFileNameWithoutExtension(f.Database), StringComparison.InvariantCultureIgnoreCase));
                var found = fullPath != null;

                // Now we have the file, try and find the material from it
                if (found)
                {
                    var foundMaterial = Dna.Application.FindMaterial(fullPath.Database, materialName);
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
        /// Get's all of the selected objects in the model
        /// </summary>
        /// <param name="action">The selected objects list to be worked on inside the action. NOTE: Do not store references to them outside of this action</param>
        /// <returns></returns>
        public void SelectedObjects(Action<List<SelectedObject>> action)
        {
             SelectionManager?.SelectedObjects(action);
        }

        #endregion

        #region Saving

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
                mBaseObject.Extension.SaveAs(savePath, (int)version, (int)options, pdfExportData?.ExportData, ref errors, ref warnings);

                // If this fails, try another way
                if (errors != 0)
                    mBaseObject.SaveAs4(savePath, (int)version, (int)options, ref errors, ref warnings);

                // Add any warnings
                results.Warnings = (SaveAsWarnings)warnings;

                // Add any errors
                results.Errors = (SaveAsErrors)errors;

                // If successful...
                if (results.Successful)
                    // Reload model data
                    ReloadModelData();

                // Return result
                return results;
            },
                SolidDnaErrorTypeCode.SolidWorksModel,
                SolidDnaErrorCode.SolidWorksModelSaveAsError,
                Localization.GetString("SolidWorksModelGetMaterialError"));
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Clean up any embedded models
        /// </summary>
        protected void DisposeChildren()
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
        }

        public override void Dispose()
        {
            // Clean up embedded objects
            DisposeChildren();

            // Dispose self
            base.Dispose();
        }

        #endregion
    }
}
