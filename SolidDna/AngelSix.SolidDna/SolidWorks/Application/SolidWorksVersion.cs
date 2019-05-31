namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents the SolidWorks version and build information
    /// </summary>
    public class SolidWorksVersion
    {
        #region Public properties

        /// <summary>
        /// The version, such as 2017.
        /// If unknown will return -1
        /// </summary>
        public int Version { get; }

        /// <summary>
        /// The major service pack number, such as 2 for SP2.0.
        /// If unknown will return -1
        /// </summary>
        public int ServicePackMajor { get; }

        /// <summary>
        /// The minor service pack number, such as 0 for SP2.0.
        /// If unknown will return -1
        /// </summary>
        public int ServicePackMinor { get; }

        /// <summary>
        /// The raw revision, for example where SolidWorks 2015 SP2.0 is 23.2.0
        /// </summary>
        public string RevisionNumber { get; }

        /// <summary>
        /// The raw revision, for example where SolidWorks 2015 SP2.0 is 23.2.0
        /// </summary>
        public string Revision { get; }

        /// <summary>
        /// The raw build number, for example where SolidWorks 2015 SP2.0 it is d150130.002
        /// </summary>
        public string BuildNumber { get; }

        /// <summary>
        /// The raw hotfix string
        /// </summary>
        public string Hotfix { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// SolidWorks version information
        /// </summary>
        /// <param name="revisionNumber"></param>
        /// <param name="revision"></param>
        /// <param name="buildNumber"></param>
        /// <param name="hotfix"></param>
        public SolidWorksVersion(string revisionNumber, string revision, string buildNumber, string hotfix)
        {
            RevisionNumber = revisionNumber;
            Revision = revision;
            BuildNumber = buildNumber;
            Hotfix = hotfix;

            const int versionUnknown = -1;
            if (string.IsNullOrEmpty(revisionNumber) || !revisionNumber.Contains("."))
            {
                Version = ServicePackMajor = ServicePackMinor = versionUnknown;
                return;
            }

            var revisionParts = revisionNumber.Split('.');

            // So far from all previous versions it is safe to assume that
            // the year (SolidWorks 20XX) of the product is the revision number
            // - 8 + 2000 so revision 23 is 2015
            Version = int.TryParse(revisionParts[0], out var version) ? version - 8 + 2000 : versionUnknown;

            // Extract the first part of the revision number for the service pack
            ServicePackMajor = revisionParts.Length >= 2 && int.TryParse(revisionParts[1], out var major) ? major : versionUnknown;

            // Extract the second part of the revision number for the service pack
            ServicePackMinor = revisionParts.Length >= 3 && int.TryParse(revisionParts[2], out var minor) ? minor : versionUnknown;
        }

        #endregion
    }
}
