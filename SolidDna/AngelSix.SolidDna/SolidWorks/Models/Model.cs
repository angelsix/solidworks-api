using System;
using SolidWorks.Interop.sldworks;
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
        public bool HasBeenSaved { get; protected set; }

        /// <summary>
        /// The type of document such as a part, assembly or drawing
        /// </summary>
        public ModelType ModelType { get; protected set; }

        /// <summary>
        /// True if this model is a part
        /// </summary>
        public bool IsPart { get { return ModelType == ModelType.Part; } }

        /// <summary>
        /// True if this model is an assembly
        /// </summary>
        public bool IsAssembly { get { return ModelType == ModelType.Assembly; } }

        /// <summary>
        /// True if this model is a drawing
        /// </summary>
        public bool IsDrawing { get { return ModelType == ModelType.Drawing; } }

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
        public int ConfigurationCount { get { return mBaseObject.GetConfigurationCount(); } }

        /// <summary>
        /// The mass properties of the part
        /// </summary>
        public MassProperties MassProperties {  get { return Extension.GetMassProperties(); } }

        #endregion

        #region Public Events

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

            // If no path is retrieved, the file hasn't been saved
            HasBeenSaved = !string.IsNullOrEmpty(FilePath);

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
                    AsDrawing().DestroyNotify += FileDestroyedNotify;
                    AsDrawing().FileSavePostNotify += FileSaveNotify;
                    AsDrawing().UserSelectionPostNotify += UserSelectionPostNotify;
                    AsDrawing().ClearSelectionsNotify += UserSelectionPostNotify;
                    break;
            }
        }

        private int Model_ClearSelectionsNotify()
        {
            throw new NotImplementedException();
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
            // If the current model was not saved before this event, update the state
            if (!HasBeenSaved)
                ReloadModelData();

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
