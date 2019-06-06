using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// A SolidWorks Note object
    /// </summary>
    public class Note : SolidDnaObject<INote>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Note(INote note) : base(note)
        {

        }

        #endregion

        #region Balloon Methods

        /// <summary>
        /// Gets the upper text of the selected BOM Balloon note
        /// </summary>
        /// <returns></returns>
        public string GetBOMBalloonUpperText() => BaseObject.GetBomBalloonText(true);

        /// <summary>
        /// Gets the upper text of the selected BOM Balloon note
        /// </summary>
        /// <returns></returns>
        public string GetBOMBalloonLowerText() => BaseObject.GetBomBalloonText(false);

        /// <summary>
        /// Sets the text for the selected BOM Balloon note
        /// </summary>
        /// <param name="upperTextStyle">The upper text style</param>
        /// <param name="upperText">The upper text</param>
        /// <param name="lowerTextStyle">The lower text style</param>
        /// <param name="lowerText">The lower text</param>
        /// <remarks>
        ///     If the upper or lower text style is <see cref="NoteTextContent.TextQuantity"/>
        ///     or <see cref="NoteTextContent.ItemNumber"/>, then SOLIDWORKS ignores the 
        ///     specified upper or lower text.
        /// </remarks>
        public void SetBOMBalloonText(
            NoteTextContent upperTextStyle, string upperText,
            NoteTextContent lowerTextStyle, string lowerText)
            => BaseObject.SetBomBalloonText((int)upperTextStyle, upperText, (int)lowerTextStyle, lowerText);

        #endregion
    }
}
