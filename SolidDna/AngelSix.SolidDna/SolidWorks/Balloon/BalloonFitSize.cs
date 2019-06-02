namespace AngelSix.SolidDna
{
    /// <summary>
    /// Types of balloon and label location fits
    /// </summary>
    public enum BalloonFitSize
    {
        /// <summary>
        /// Tightest fit (not available for a label location)
        /// </summary>
        Tightest = 0,

        /// <summary>
        /// Fits a single character
        /// </summary>
        Character1 = 1,

        /// <summary>
        /// Fits 2 characters
        /// </summary>
        Character2 = 2,

        /// <summary>
        /// Fits 3 characters
        /// </summary>
        Character3 = 3,

        /// <summary>
        /// Fits 4 characters
        /// </summary>
        Character4 = 4,

        /// <summary>
        /// Fits 5 characters
        /// </summary>
        Character5 = 5,

        /// <summary>
        /// Size defined by <see cref="AutoBalloonOptions.CustomSize"/>
        /// </summary>
        UserDefined = 6
    }
}
