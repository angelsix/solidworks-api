using System;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents the mass information of a SolidWorks part
    /// </summary>
    public class MassProperties
    {
        #region Public Properties

        /// <summary>
        /// The center of mass X axis
        /// </summary>
        public double CenterOfMassX { get; set; }

        /// <summary>
        /// The center of mass Y axis
        /// </summary>
        public double CenterOfMassY { get; set; }

        /// <summary>
        /// The center of mass Z axis
        /// </summary>
        public double CenterOfMassZ { get; set; }

        /// <summary>
        /// The volume of the part
        /// </summary>
        public double Volume { get; set; }

        /// <summary>
        /// The area of the part
        /// </summary>
        public double Area { get; set; }

        /// <summary>
        /// The mass of the part in kilograms
        /// </summary>
        public double Mass { get; set; }

        /// <summary>
        /// The moment of inertia on XX
        /// </summary>
        public double MomentOfInertiaXX { get; set; }

        /// <summary>
        /// The moment of inertia on YY
        /// </summary>
        public double MomentOfInertiaYY { get; set; }

        /// <summary>
        /// The moment of inertia on ZZ
        /// </summary>
        public double MomentOfInertiaZZ { get; set; }

        /// <summary>
        /// The moment of inertia on XY
        /// </summary>
        public double MomentOfInertiaXY { get; set; }

        /// <summary>
        /// The moment of inertia on ZX
        /// </summary>
        public double MomentOfInertiaZX { get; set; }

        /// <summary>
        /// The moment of inertia on YZ
        /// </summary>
        public double MomentOfInertiaYZ { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MassProperties()
        {

        }

        /// <summary>
        /// Constructor accepting the SolidWorks double array containing the mass details
        /// </summary>
        /// <param name="properties">The properties from the SolidWorks GetMassProperties calls. The array should be at least 12 in length</param>
        public MassProperties(double[] properties)
        {
            CenterOfMassX = properties[0];
            CenterOfMassY = properties[1];
            CenterOfMassZ = properties[2];
            Volume = properties[3];
            Area = properties[4];
            Mass = properties[5];
            MomentOfInertiaXX = properties[6];
            MomentOfInertiaYY = properties[7];
            MomentOfInertiaZZ = properties[8];
            MomentOfInertiaXY = properties[9];
            MomentOfInertiaZX = properties[10];
            MomentOfInertiaYZ = properties[11];
        }

        #endregion

        /// <summary>
        /// The mass of the model displayed in grams or kilograms
        /// </summary>
        /// <param name="decimalPlaces">The precision to show the value in</param>
        /// <returns></returns>
        public string MassInMetric(int decimalPlaces = 2)
        {
            // If it's small, show grams
            if (Mass < 1)
                return $"{Math.Round(Mass * 1000, decimalPlaces)} grams";

            // Otherwise show kg
            return $"{Math.Round(Mass, decimalPlaces)} kg";
        }

        #region ToString

        /// <summary>
        /// Returns a user-friendly string with group properties.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Mass: {Mass} kg";
        }

        #endregion

    }
}
