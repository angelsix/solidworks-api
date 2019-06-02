namespace AngelSix.SolidDna
{
    /// <summary>
    /// Options for the <see cref="DrawingDocument.AutoBalloon"/> call
    /// </summary>
    public class AutoBalloonOptions
    {
        /// <summary>
        /// User-defined size of the balloons.
        /// Valid only when  is set to swBalloonFit_e.swBF_UserDef
        /// </summary>
        public double CustomSize { get; set; }

        /// <summary>
        /// The edit balloon options. 
        /// Only valid only if <see cref="EditBalloons"/> is true
        /// </summary>
        public EditBalloonOptions EditBalloonOptions { get; set; }

        /// <summary>
        /// Whether to use edit balloon options when creating the auto balloon
        /// </summary>
        public bool EditBalloons { get; set; }

        /// <summary>
        /// Sets the first balloon item.
        /// </summary>
        public Component FirstItem { get; set; }

        /// <summary>
        /// True to create a balloon for only one instance of an item.
        /// False to create multiple balloons for multiple instances of an item
        /// </summary>
        public bool IgnoreMultiple { get; set; }

        /// <summary>
        /// True to insert magnetic lines, false to not.
        /// Only valid when <see cref="Layout"/> is not set to <see cref="BalloonLayoutType.Circle"/>
        /// </summary>
        public bool InsertMagnaticLine { get; set; }

        /// <summary>
        /// The item number increment
        /// </summary>
        public int ItemNumberIncrement { get; set; }

        /// <summary>
        /// The starting item number
        /// </summary>
        public int ItemNumberStart { get; set; }

        /// <summary>
        /// The item ordering
        /// </summary>
        public BalloonItemNumberOrder ItemOrder { get; set; }

        /// <summary>
        /// The name of the layer on which to create balloons
        /// </summary>
        public string LayerName { get; set; }

        /// <summary>
        /// Style of the balloon layout
        /// </summary>
        public BalloonLayoutType Layout { get; set; }

        /// <summary>
        /// True to attach balloons to faces. 
        /// False to attach balloons to edges
        /// </summary>
        public bool LeaderAttachmentToFaces { get; set; }

        /// <summary>
        /// The lower text of the balloons
        /// </summary>
        /// <remarks>
        ///     This property is valid only when <see cref="Style"/> is set
        ///     to <see cref="BalloonStyle.Circular"/>.
        ///     You can only get or set the lower text in a BOM balloon 
        ///     using <see cref="Note.GetBOMBalloonLowerText"/> or 
        ///     <see cref="Note.SetBOMBalloonText(NoteTextContent, string, NoteTextContent, string)"/> 
        ///     after inserting a BOM balloon.
        /// </remarks>
        public string LowerText { get; set; }

        /// <summary>
        /// Style of the lower text
        /// </summary>
        /// <remarks>
        ///     This property is valid only when <see cref="Style"/> is set 
        ///     to <see cref="BalloonStyle.SplitCircle"/>
        /// </remarks>
        public BalloonTextContent LowerTextContent { get; set; }

        /// <summary>
        /// True to reverse the item ordering, false to not.
        /// Only valid when <see cref="ItemOrder"/> is set to 
        /// <see cref="BalloonItemNumberOrder.OrderSequentially"/>
        /// </summary>
        public bool ReverseDirection { get; set; }

        /// <summary>
        /// The size of the balloon
        /// </summary>
        public BalloonFitSize Size { get; set; }

        /// <summary>
        /// Style of the balloon
        /// </summary>
        public BalloonStyle Style { get; set; }

        /// <summary>
        /// The upper text of the balloons
        /// </summary>
        /// <remarks>
        ///     You can only get or set the upper text in a BOM balloon using 
        ///     <see cref="Note.GetBOMBalloonUpperText"/> or 
        ///     <see cref="Note.SetBOMBalloonText(NoteTextContent, string, NoteTextContent, string)"/> 
        ///     after inserting a BOM balloon.
        /// </remarks>
        public string UpperText { get; set; }

        /// <summary>
        /// Style of the upper text
        /// </summary>
        public BalloonTextContent UpperTextContent { get; set; }
    }
}
