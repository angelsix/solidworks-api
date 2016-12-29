namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents the SolidWorks version and build information
    /// </summary>
    public class SolidWorksVersion
    {
        /// <summary>
        /// The version, such as 2017
        /// If unknown will return -1
        /// </summary>
        public int Version
        {
            get
            {
                // So far from all previous versions it is safe to assume that
                // the year (SolidWorks 20XX) of the product is the revision number
                // - 8 + 2000 so revision 23 is 2015
                int result;
                if (string.IsNullOrEmpty(RevisionNumber) || !RevisionNumber.Contains(".") || !int.TryParse(RevisionNumber.Split('.')[0], out result))
                    return -1;

                return result - 8 + 2000;
            }
        }

        /// <summary>
        /// The major service pack number, such as 2 for SP2.0
        /// If unknown will return -1
        /// </summary>
        public int ServicePackMajor
        {
            get
            {
                // Extract the second part of the revision number for the service pack
                int result;
                if (string.IsNullOrEmpty(RevisionNumber) || !RevisionNumber.Contains(".") || RevisionNumber.Split('.').Length < 2 || !int.TryParse(RevisionNumber.Split('.')[1], out result))
                    return -1;

                return result;
            }
        }

        /// <summary>
        /// The minor service pack number, such as 0 for SP2.0
        /// If unknown will return -1
        /// </summary>
        public int ServicePackMinor
        {
            get
            {
                // Extract the second part of the revision number for the service pack
                int result;
                if (string.IsNullOrEmpty(RevisionNumber) || !RevisionNumber.Contains(".") || RevisionNumber.Split('.').Length < 3 || !int.TryParse(RevisionNumber.Split('.')[2], out result))
                    return -1;

                return result;
            }
        }

        /// <summary>
        /// The raw revision, for example where SolidWorks 2015 SP2.0 is 23.2.0
        /// </summary>
        public string RevisionNumber { get; set; }

        /// <summary>
        /// The raw revision, for example where SolidWorks 2015 SP2.0 is 23.2.0
        /// </summary>
        public string Revision { get; set; }

        /// <summary>
        /// The raw build number, for example where SolidWorks 2015 SP2.0 it is d150130.002
        /// </summary>
        public string BuildNumber { get; set; }

        /// <summary>
        /// The raw hotfix string
        /// </summary>
        public string Hotfix { get; set; }
    }
}
