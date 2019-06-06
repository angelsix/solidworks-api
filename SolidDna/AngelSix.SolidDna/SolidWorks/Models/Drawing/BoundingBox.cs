namespace AngelSix.SolidDna
{
    /// <summary>
    /// A bounding box for a view or similar object
    /// </summary>
    public class BoundingBox
    {
        #region Public Properties

        /// <summary>
        /// The left edge
        /// </summary>
        public double Left { get; }

        /// <summary>
        /// The right edge
        /// </summary>
        public double Right { get; }

        /// <summary>
        /// The bottom edge
        /// </summary>
        public double Bottom { get; }

        /// <summary>
        /// The top edge
        /// </summary>
        public double Top { get; }

        /// <summary>
        /// The width
        /// </summary>
        public double Width => Right - Left;

        /// <summary>
        /// The height
        /// </summary>
        public double Height => Top - Bottom;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="left">Left edge</param>
        /// <param name="bottom">Bottom edge</param>
        /// <param name="right">Right edge</param>
        /// <param name="top">Top edge</param>
        public BoundingBox(double left, double bottom, double right, double top)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        #endregion
    }
}
