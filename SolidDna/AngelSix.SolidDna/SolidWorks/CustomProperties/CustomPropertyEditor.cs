using SolidWorks.Interop.sldworks;
using System.Collections.Generic;
using System.Linq;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents a SolidWorks custom property manager for a model
    /// </summary>
    public class CustomPropertyEditor : SolidDnaObject<CustomPropertyManager>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomPropertyEditor(CustomPropertyManager model) : base(model)
        {

        }

        #endregion

        /// <summary>
        /// Gets the value of a custom property by name
        /// </summary>
        /// <param name="name">The name of the custom property</param>
        /// <param name="resolve">True to resolve the custom property value</param>
        /// <returns></returns>
        public string GetCustomProperty(string name, bool resolve = false)
        {
            // TODO: Add error checking and exception catching

            string val;
            string resolvedVal;
            bool wasResolved;

            // Get custom property
            mBaseObject.Get5(name, false, out val, out resolvedVal, out wasResolved);

            // Return desired result
            return resolve ? resolvedVal : val;
        }

        /// <summary>
        /// Sets the value of a custom property by name
        /// </summary>
        /// <param name="name">The name of the custom property</param>
        /// <param name="value">The value of the custom property</param>
        /// <returns></returns>
        public void SetCustomProperty(string name, string value)
        {
            // TODO: Add error checking and exception catching

            // Set custom property
            mBaseObject.Set2(name, value);
        }

        /// <summary>
        /// Deletes a custom property by name
        /// </summary>
        /// <param name="name">The name of the custom property</param>
        public void DeleteCustomProperty(string name)
        {
            // TODO: Add error checking and exception catching

            mBaseObject.Delete2(name);
        }

        /// <summary>
        /// Gets a list of all custom properties
        /// </summary>
        /// <returns></returns>
        public List<CustomProperty> GetCustomProperties()
        {
            // TODO: Add error checking and exception catching

            // Create an empty list
            var list = new List<CustomProperty>();

            // Get all properties
            var names = (string[])mBaseObject.GetNames();

            // Create custom property objects for each
            if (names?.Length > 0)
                list.AddRange(names.Select(name => new CustomProperty(this, name)).ToList());

            // Return the list
            return list;
        }
    }
}
