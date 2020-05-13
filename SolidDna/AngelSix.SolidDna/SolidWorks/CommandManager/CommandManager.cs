using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace AngelSix.SolidDna
{
    public class CommandManager : SolidDnaObject<ICommandManager>
    {
        #region Private Members

        /// <summary>
        /// A list of all created command groups
        /// </summary>
        private readonly List<CommandManagerGroup> mCommandGroups = new List<CommandManagerGroup>();

        /// <summary>
        /// A list of all created command flyouts
        /// </summary>
        private List<CommandManagerFlyout> mCommandFlyouts = new List<CommandManagerFlyout>();

        /// <summary>
        /// Unique Id for flyouts (just increment every time we add one)
        /// </summary>
        private int mFlyoutIdCount = 1000;

        /// <summary>
        /// List of icon sizes used by SOLIDWORKS. Icons are square, so these values are both width and height.
        /// </summary>
        private readonly int[] mIconSizes = { 20, 32, 40, 64, 96, 128 };

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CommandManager(ICommandManager commandManager) : base(commandManager)
        {

        }

        #endregion

        #region Command Group

        /// <summary>
        /// Create a command group from a list of <see cref="CommandManagerItem"/> items
        /// </summary>
        /// <param name="title">Name of the CommandGroup to create (see Remarks)</param>
        /// <param name="items">The command items to add</param>
        /// <param name="flyoutItems">The flyout command items that contain a list of other commands</param>
        /// <param name="iconListsPath">The icon list absolute path based on a string format of the absolute path to the icon list images, replacing {0} with the size. 
        ///     For example C:\Folder\myiconlist{0}.png</param>
        /// <param name="tooltip">Tool tip for the CommandGroup</param>
        /// <param name="hint">Text displayed in SOLIDWORKS status bar when a user's mouse pointer is over the CommandGroup</param>
        /// <param name="position">Position of the CommandGroup in the CommandManager for all document templates.
        /// NOTE: Specify 0 to add the CommandGroup to the beginning of the CommandManager, or specify -1 to add it to the end of the CommandManager.
        /// NOTE: You can also use ICommandGroup::MenuPosition to control the position of the CommandGroup in specific document templates.</param>
        /// <param name="ignorePreviousVersion">True to remove all previously saved customization and toolbar information before creating a new CommandGroup, false to not.
        /// Call CommandManager.GetGroupDataFromRegistry before calling this method to determine how to set IgnorePreviousVersion. Set IgnorePreviousVersion to true to prevent SOLIDWORKS from saving the current toolbar setting to the registry, even if there is no previous version.</param>
        /// <param name="hasMenu">Whether the CommandGroup should appear in the Tools dropdown menu.</param>
        /// <param name="hasToolbar">Whether the CommandGroup should appear in the Command Manager and as a separate toolbar.</param>
        /// <param name="addDropdownBoxForParts">If true, adds a command box to the toolbar for parts that has a dropdown of all commands that are part of this group. The tooltip of the command group is used as the name.</param>
        /// <param name="addDropdownBoxForAssemblies">If true, adds a command box to the toolbar for assemblies that has a dropdown of all commands that are part of this group. The tooltip of the command group is used as the name.</param>
        /// <param name="addDropdownBoxForDrawings">If true, adds a command box to the toolbar for drawings that has a dropdown of all commands that are part of this group. The tooltip of the command group is used as the name.</param>
        /// <param name="documentTypes">The document types where this menu/toolbar is visible.</param>
        /// <returns></returns>
        public CommandManagerGroup CreateCommandGroupAndTabs(
            string title,
            List<CommandManagerItem> items,
            List<CommandManagerFlyout> flyoutItems,
            string iconListsPath = "",
            string tooltip = "",
            string hint = "",
            int position = -1,
            bool ignorePreviousVersion = true,
            bool hasMenu = true,
            bool hasToolbar = true,
            bool addDropdownBoxForParts = false,
            bool addDropdownBoxForAssemblies = false,
            bool addDropdownBoxForDrawings = false,
            ModelTemplateType documentTypes = ModelTemplateType.Part | ModelTemplateType.Assembly | ModelTemplateType.Drawing)
        {
            // Wrap any error creating the taskpane in a SolidDna exception
            return SolidDnaErrors.Wrap(() =>
            {
                // Lock the list
                lock (mCommandGroups)
                {
                    // Create the command group
                    var group = CreateCommandGroup(
                        title,
                        items,
                        flyoutItems,
                        tooltip,
                        hint,
                        position,
                        ignorePreviousVersion,
                        hasMenu,
                        hasToolbar,
                        addDropdownBoxForParts,
                        addDropdownBoxForAssemblies,
                        addDropdownBoxForDrawings,
                        documentTypes);

                    // Track all flyouts
                    mCommandFlyouts = flyoutItems;

                    // Set icon list
                    group.SetIconLists(iconListsPath);

                    // Create the group
                    group.Create(this);

                    // Add this group to the list
                    mCommandGroups.Add(group);

                    // Return the group
                    return group;
                }
            },
                SolidDnaErrorTypeCode.SolidWorksCommandManager,
                SolidDnaErrorCode.SolidWorksCommandGroupCreateError,
                Localization.GetString("ErrorSolidWorksCommandGroupAddError"));
        }

        /// <summary>
        /// Create a command group flyout containing a list of <see cref="CommandManagerItem"/> items
        /// </summary>
        /// <param name="title">Name of the flyout to create</param>
        /// <param name="items">The command items to add</param>
        /// <param name="pathFormat">The icon list absolute path based on a string format of the absolute path to the icon list images, replacing {0} with the size. 
        ///     For example C:\Folder\myiconlist{0}.png</param>
        /// <param name="tooltip">Tool tip for the new flyout</param>
        /// <param name="hint">Text displayed in SOLIDWORKS status bar when a user's mouse pointer is over the flyout</param>
        /// <returns></returns>
        public CommandManagerFlyout CreateFlyoutGroup(string title, List<CommandManagerItem> items, string pathFormat, string tooltip = "", string hint = "")
        {
            #region Icons 

            // Make sure the path format contains "{0}"
            if (pathFormat == null || !pathFormat.Contains("{0}"))
                throw new SolidDnaException(SolidDnaErrors.CreateError(
                    SolidDnaErrorTypeCode.SolidWorksCommandManager,
                    SolidDnaErrorCode.SolidWorksCommandGroupInvalidPathFormatError,
                    Localization.GetString("ErrorSolidWorksCommandGroupIconListInvalidPathError")));

            var iconListPaths = new Dictionary<int, string>();

            // Fill the dictionary with all paths that exist
            foreach (var iconSize in mIconSizes)
            {
                var path = string.Format(pathFormat, iconSize);
                if (File.Exists(path))
                {
                    iconListPaths.Add(iconSize, path);
                }
            }

            // Get icon paths
            var icons = iconListPaths.Values.ToArray();

            #endregion

            // Create unique callback Id
            var callbackId = Guid.NewGuid().ToString("N");

            // Attempt to create the command flyout
            var unsafeCommandFlyout = BaseObject.CreateFlyoutGroup2(
                mFlyoutIdCount,
                title,
                tooltip,
                hint,
                icons,
                icons,
                $"Callback({callbackId})",
                null);

            // Create managed object
            var flyout = new CommandManagerFlyout(
                unsafeCommandFlyout,
                mFlyoutIdCount++,
                callbackId,
                items,
                title,
                hint, tooltip);

            // Return it
            return flyout;
        }

        /// <summary>
        /// Creates a command group
        /// </summary>
        /// <param name="title">The title</param>
        /// <param name="items">The command items to add</param>
        /// <param name="flyoutItems">The flyout command items that contain a list of other commands</param>
        /// <param name="tooltip">The tool tip</param>
        /// <param name="hint">The hint</param>
        /// <param name="position">Position of the CommandGroup in the CommandManager for all document templates.
        /// NOTE: Specify 0 to add the CommandGroup to the beginning of the CommandManager, or specify -1 to add it to the end of the CommandManager.
        /// NOTE: You can also use ICommandGroup::MenuPosition to control the position of the CommandGroup in specific document templates.</param>
        /// <param name="ignorePreviousVersion">True to remove all previously saved customization and toolbar information before creating a new CommandGroup, false to not.
        ///     Call CommandManager.GetGroupDataFromRegistry before calling this method to determine how to set IgnorePreviousVersion.
        ///     Set IgnorePreviousVersion to true to prevent SOLIDWORKS from saving the current toolbar setting to the registry, even if there is no previous version.</param>
        /// <param name="hasMenu">Whether the CommandGroup should appear in the Tools dropdown menu.</param>
        /// <param name="hasToolbar">Whether the CommandGroup should appear in the Command Manager and as a separate toolbar.</param>
        /// <param name="addDropdownBoxForParts">If true, adds a command box to the toolbar for parts that has a dropdown of all commands that are part of this group. The tooltip of the command group is used as the name.</param>
        /// <param name="addDropdownBoxForAssemblies">If true, adds a command box to the toolbar for assemblies that has a dropdown of all commands that are part of this group. The tooltip of the command group is used as the name.</param>
        /// <param name="addDropdownBoxForDrawings">If true, adds a command box to the toolbar for drawings that has a dropdown of all commands that are part of this group. The tooltip of the command group is used as the name.</param>
        /// <param name="documentTypes">The document types where this menu/toolbar is visible.</param>
        /// <returns></returns>
        private CommandManagerGroup CreateCommandGroup(
            string title,
            List<CommandManagerItem> items,
            List<CommandManagerFlyout> flyoutItems,
            string tooltip = "",
            string hint = "",
            int position = -1,
            bool ignorePreviousVersion = true,
            bool hasMenu = true,
            bool hasToolbar = true,
            bool addDropdownBoxForParts = false,
            bool addDropdownBoxForAssemblies = false,
            bool addDropdownBoxForDrawings = false, 
            ModelTemplateType documentTypes = ModelTemplateType.Part | ModelTemplateType.Assembly | ModelTemplateType.Drawing)
        {
            // NOTE: We may need to look carefully at this Id if things get removed and re-added based on this SolidWorks note:
            //     
            //       If you change the definition of an existing CommandGroup (i.e., add or remove toolbar buttons), you must assign a 
            //       new unique user-defined UserID to that CommandGroup. You must perform this action to avoid conflicts with any 
            //       previously existing CommandGroups and to allow for backward and forward compatibility of the CommandGroups in your application.
            // 

            // Get the next Id
            var id = mCommandGroups.Count == 0 ? 1 : mCommandGroups.Max(f => f.UserId) + 1;

            // Store error code
            var errors = -1;

            // Attempt to create the command group
            var unsafeCommandGroup = BaseObject.CreateCommandGroup2(id, title, tooltip, hint, position, ignorePreviousVersion, ref errors);

            // Check for errors
            if (errors != (int)swCreateCommandGroupErrors.swCreateCommandGroup_Success)
            {
                // Get enum name
                var errorEnumString = ((swCreateCommandGroupErrors)errors).ToString();

                // Throw error
                throw new SolidDnaException(SolidDnaErrors.CreateError(
                    SolidDnaErrorTypeCode.SolidWorksCommandManager,
                    SolidDnaErrorCode.SolidWorksCommandGroupCreateError,
                    Localization.GetString("ErrorSolidWorksCommandGroupAddError") + $". {errorEnumString}"));
            }

            // Otherwise we got the command group
            var group = new CommandManagerGroup(
                unsafeCommandGroup,
                items,
                flyoutItems,
                id,
                title,
                tooltip,
                hint,
                hasMenu,
                hasToolbar,
                addDropdownBoxForParts,
                addDropdownBoxForAssemblies,
                addDropdownBoxForDrawings,
                documentTypes);

            // Return it
            return group;
        }

        /// <summary>
        /// Removes the specific command flyout
        /// </summary>
        /// <param name="flyout">The command flyout to remove</param>
        public void RemoveCommandFlyout(CommandManagerFlyout flyout)
        {
            lock (mCommandFlyouts)
            {
                BaseObject.RemoveFlyoutGroup(flyout.UserId);
            }
        }
        /// <summary>
        /// Removes the specific command group
        /// </summary>
        /// <param name="group">The command group to remove</param>
        /// <param name="runtimeOnly">True to remove the CommandGroup, saving its toolbar information in the registry. False to remove both the CommandGroup and its toolbar information in the registry</param>
        public void RemoveCommandGroup(CommandManagerGroup group, bool runtimeOnly = false)
        {
            lock (mCommandGroups)
            {
                BaseObject.RemoveCommandGroup2(group.UserId, runtimeOnly);
            }
        }

        #endregion

        #region Command Tabs

        /// <summary>
        /// Get's the command tab for this 
        /// </summary>
        /// <param name="type">The type of document to get the tab for. Use only Part, Assembly or Drawing one at a time, otherwise the first found tab gets returned</param>
        /// <param name="title">The title of the command tab to get</param>
        /// <param name="createIfNotExist">True to create the tab if it does not exist</param>
        /// <param name="clearExistingItems">Removes any existing items on the tab if true</param>
        /// <returns></returns>
        public CommandManagerTab GetCommandTab(ModelType type, string title, bool createIfNotExist = true, bool clearExistingItems = true)
        {
            // Try and get the tab
            var unsafeTab = BaseObject.GetCommandTab((int)type, title);

            // If we did not get it, just ignore
            if (unsafeTab == null && !createIfNotExist)
                return null;

            // If we want to remove any previous tabs...
            while (clearExistingItems && unsafeTab != null)
            {
                // Remove it
                BaseObject.RemoveCommandTab(unsafeTab);

                // Clean COM object
                Marshal.ReleaseComObject(unsafeTab);

                // Try and get another
                unsafeTab = BaseObject.GetCommandTab((int)type, title);
            }

            // Create it if it doesn't exist
            if (unsafeTab == null)
                unsafeTab = BaseObject.AddCommandTab((int)type, title);

            // If it's still null, we failed
            if (unsafeTab == null)
                // Throw error
                throw new SolidDnaException(SolidDnaErrors.CreateError(
                    SolidDnaErrorTypeCode.SolidWorksCommandManager,
                    SolidDnaErrorCode.SolidWorksCommandGroupCreateTabError,
                    Localization.GetString("ErrorSolidWorksCommandCreateTabError")));

            // Return tab
            return new CommandManagerTab(unsafeTab);
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            // Remove all command groups
            mCommandGroups?.ForEach(f => RemoveCommandGroup(f));

            // Remove all command flyouts
            mCommandFlyouts?.ForEach(f => RemoveCommandFlyout(f));

            base.Dispose();
        }

        #endregion
    }
}
