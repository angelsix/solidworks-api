using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A command flyout for the top command bar in SolidWorks
    /// </summary>
    public class CommandManagerFlyout : SolidDnaObject<IFlyoutGroup>
    {
        #region Public Properties

        /// <summary>
        /// The Id used when this command flyout was created
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// The unique Callback ID (set by creator)
        /// </summary>
        public string CallbackId { get; }

        /// <summary>
        /// The title of this command group
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The hint of this command group
        /// </summary>
        public string Hint { get; set; }

        /// <summary>
        /// The tooltip of this command group
        /// </summary>
        public string Tooltip { get; set; }

        /// <summary>
        /// The command items to add to this flyout
        /// </summary>
        public List<CommandManagerItem> Items { get; set; }

        /// <summary>
        /// The command Id for this flyout item
        /// </summary>
        public int CommandId => BaseObject.CmdID;

        /// <summary>
        /// The tab view style (whether and how to show in the large icon tab bar view)
        /// </summary>
        public CommandManagerItemTabView TabView { get; set; } = CommandManagerItemTabView.IconWithTextBelow;

        /// <summary>
        /// True to show this item in the command tab when a part is open
        /// </summary>
        public bool VisibleForParts { get; set; } = true;

        /// <summary>
        /// True to show this item in the command tab when an assembly is open
        /// </summary>
        public bool VisibleForAssemblies { get; set; } = true;

        /// <summary>
        /// True to show this item in the command tab when a drawing is open
        /// </summary>
        public bool VisibleForDrawings { get; set; } = true;

        /// <summary>
        /// The action to call when the item is clicked
        /// </summary>
        public Action OnClick { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="flyoutGroup">The SolidWorks command manager flyout group</param>
        /// <param name="callbackId">The unique callback ID</param>
        /// <param name="items">The command items to add</param>
        /// <param name="title">The title</param>
        /// <param name="hint">The hint</param>
        /// <param name="tooltip">The tool tip</param>
        public CommandManagerFlyout(IFlyoutGroup flyoutGroup, int userId, string callbackId, List<CommandManagerItem> items, string title, string hint, string tooltip) : base(flyoutGroup)
        {
            // Set user Id
            UserId = userId;

            // Callback Id
            CallbackId = callbackId;

            // Set items
            Items = items;

            // Set title
            Title = title;

            // Set hint
            Hint = hint;

            // Set tooltip
            Tooltip = tooltip;

            // Listen out for callbacks
            PlugInIntegration.CallbackFired += PlugInIntegration_CallbackFired;

            // Add items
            Items?.ForEach(AddCommandItem);
    
            // NOTE: No need to set items command IDs as they are only needed when 
            //       Calling AddItemToTab and the flyout itself gets added, not
            //       the flyouts inner commands

        }

        #endregion

        #region Callbacks

        /// <summary>
        /// Fired when a SolidWorks callback is fired
        /// </summary>
        /// <param name="name">The name of the callback that was fired</param>
        private void PlugInIntegration_CallbackFired(string name)
        {
            // Find the item, if any
            var item = Items?.FirstOrDefault(f => f.CallbackId == name);

            // Call the action
            item?.OnClick?.Invoke();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a command item to the group
        /// </summary>
        /// <param name="item">The item to add</param>
        public void AddCommandItem(CommandManagerItem item)
        {
            // Add the item
            var id = BaseObject.AddCommandItem(item.Name, item.Hint, item.ImageIndex, $"Callback({item.CallbackId})", null);

            // Set the Id we got
            item.UniqueId = id;
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Disposing
        /// </summary>
        public override void Dispose()
        {
            // Stop listening out for callbacks
            PlugInIntegration.CallbackFired -= PlugInIntegration_CallbackFired;

            base.Dispose();
        }

        #endregion
    }
}
