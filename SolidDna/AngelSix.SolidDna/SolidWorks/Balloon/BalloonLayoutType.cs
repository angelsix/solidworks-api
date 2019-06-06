namespace AngelSix.SolidDna
{
    /// <summary>
    /// Arrangements for automatic BOM balloons in relation to the drawing views
    /// with which they are associated
    /// </summary>
    public enum BalloonLayoutType
    {
        /// <summary>
        /// Use the document layout default
        /// </summary>
        DocumentDefault = -1,

        /// <summary>
        /// In a box around the drawing view
        /// </summary>
        Square = 1,

        /// <summary>
        /// In a circle around the drawing view
        /// </summary>
        Circle = 2,

        /// <summary>
        /// Along the top edge of the drawing view
        /// </summary>
        Top = 3,

        /// <summary>
        /// Along the bottom edge of the drawing view
        /// </summary>
        Bottom = 4,

        /// <summary>
        /// Along the right edge of the drawing view
        /// </summary>
        Right = 5,

        /// <summary>
        /// Along the left edge of the drawing view
        /// </summary>
        Left = 6
    }
}
