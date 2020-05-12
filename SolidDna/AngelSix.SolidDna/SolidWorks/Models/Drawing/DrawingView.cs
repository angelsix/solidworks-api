using SolidWorks.Interop.sldworks;
using System.Drawing;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A view of a drawing
    /// </summary>
    public class DrawingView : SolidDnaObject<View>
    {
        #region Public Properties

        /// <summary>
        /// The drawing view type
        /// </summary>
        public DrawingViewType ViewType => (DrawingViewType)BaseObject.Type;

        /// <summary>
        /// The name of the view
        /// </summary>
        public string Name => BaseObject.Name;

        /// <summary>
        /// The X position of the view origin with respect to the drawing sheet origin
        /// </summary>
        public double PositionX => ((double[])BaseObject.Position)[0];

        /// <summary>
        /// The Y position of the view origin with respect to the drawing sheet origin
        /// </summary>
        public double PositionY => ((double[])BaseObject.Position)[1];

        /// <summary>
        /// The bounding box of the view
        /// </summary>
        public BoundingBox BoundingBox
        {
            get
            {
                var box = (double[])BaseObject.GetOutline();
                return new BoundingBox(box[0], box[1], box[2], box[3]);
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="comObject">The underlying COM object</param>
        public DrawingView(View comObject) : base(comObject)
        {
        }

        #endregion
    }
}
