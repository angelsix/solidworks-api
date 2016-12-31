using SolidWorks.Interop.sldworks;
using System;

namespace AngelSix.SolidDna
{
    /*
     *  NOTE: Outstanding question to SolidWorks...
     * 

        From the feature, I then need to get the specific feature. This is where the fun begins.
 
        I call feature. GetTypeName2 to get the type. Then from this list http://help.solidworks.com/2016/english/api/sldworksapi/SOLIDWORKS.Interop.sldworks~SOLIDWORKS.Interop.sldworks.IFeature~GetTypeName2.html I figure out what type of feature I should expect from GetSpecificFeature2.
 
        I’ve mapped everything then I come to check out the document for GetSpecificFeature2 http://help.solidworks.com/2016/english/api/sldworksapi/SOLIDWORKS.Interop.sldworks~SOLIDWORKS.Interop.sldworks.IFeature~GetSpecificFeature2.html
 
        It states it returns IAttribute, IBodyFolder etc... Now this list doesn’t match up. 
 
        GetTypeName2 never returns any possible name that would result in it being any of the following types:
 
        IComponent2, IConfiguration, IDimXpertAnnotation, IDimXpertFeature, IFlatPatternFolder, IHoleTable, IMateEntity2, IMateInPlace, IPunchTable, ISheet, ISheetMetalFolder and IView
 
        And on the opposite side, we have specific feature names that map to the following interfaces that are not mentioned in the GetSpecificFeature2 list
 
        IMotionStudyResults, ISketchBlockInstance,  apparently doesn’t return from the GetSpecificFeature2 call
 
        So my questions are:
 
        1. If I call GetSpecificFeature2 on a selected motion study result or sketch block instance, will it work or fail? If it fails, how do we get those 2 specific instances?
        2. What are the expected results for GetTypeName2 on the missing types that can be selected, like a Mate entity, hole table, flat pattern folder etc... in order to identify them as specific types? 
 
    */

    /// <summary>
    /// Represents a SolidWorks feature of any type
    /// NOTE: This is a SharedSolidDnaObject so the passed 
    /// in COM object should be final disposed by the <see cref="SelectedObject"/> parent
    /// 
    /// TODO: See if we can add support for non-interface types
    ///        
    ///       Bending (Flex), Deform, Imported, GridFeature (Grid), 
    ///       AEMTorsionalSpring (TorsionalSpring), FormToolInstance (FormTool), 
    ///       BlendRefSurface (Surface-Loft), RefSurface (Surface-Imported),
    ///       SweepRefSurface (Surface-Sweep), UnTrimRefSurf (Surface-Untrim)
    /// </summary>
    public class ModelFeature : SharedSolidDnaObject<Feature>
    {
        #region Protected Members

        /// <summary>
        /// The specific feature for this feature, if any
        /// </summary>
        protected SolidDnaObject<object> mSpecificFeature;

        /// <summary>
        /// The feature data for this feature, if any
        /// </summary>
        protected SolidDnaObject<object> mFeatureData;

        #endregion

        #region Public Properties

        /// <summary>
        /// The specific type of this feature
        /// </summary>
        public ModelFeatureType FeatureType { get { return this.SpecificFeatureType(); } }

        /// <summary>
        /// Gets the SolidWorks feature type name, such as RefSurface, CosmeticWeldBead, FeatSurfaceBodyFolder etc...
        /// </summary>
        /// <returns></returns>
        public string FeatureTypeName { get { return GetFeatureTypeName(); } }

        /// <summary>
        /// The specific feature for this feature, if any
        /// NOTE: This is a COM object. Set all instance variables of this to null once done if you set any
        /// </summary>
        public object SpecificFeature { get { return mSpecificFeature?.UnsafeObject; } }

        /// <summary>
        /// The feature data for this feature, if any
        /// NOTE: This is a COM object. Set all instance variables of this to null once done if you set any
        /// </summary>
        public object FeatureData { get { return mFeatureData?.UnsafeObject; } }

        #region Type Checks

        #region Features

        /// <summary>
        /// Checks if this features specify type is an Attribute
        /// </summary>
        public bool IsAttribute { get { return FeatureType == ModelFeatureType.Attribute; } }

        /// <summary>
        /// Checks if this features specify type is a Body Folder
        /// </summary>
        public bool IsBodyFolder { get { return FeatureType == ModelFeatureType.BodyFolder; } }

        /// <summary>
        /// Checks if this features specify type is a Bom
        /// </summary>
        public bool IsBom { get { return FeatureType == ModelFeatureType.Bom; } }

        /// <summary>
        /// Checks if this features specify type is a Camera
        /// </summary>
        public bool IsCamera { get { return FeatureType == ModelFeatureType.Camera; } }

        /// <summary>
        /// Checks if this features specify type is a Comment Folder
        /// </summary>
        public bool IsCommentFolder { get { return FeatureType == ModelFeatureType.CommentFolder; } }

        /// <summary>
        /// Checks if this features specify type is a Cosmetic Weld Bead Folder 
        /// </summary>
        public bool IsCosmeticWeldBeadFolder { get { return FeatureType == ModelFeatureType.CosmeticWeldBeadFolder; } }

        /// <summary>
        /// Checks if this features specify type is a Detail Circle
        /// </summary>
        public bool IsDetailCircle { get { return FeatureType == ModelFeatureType.DetailCircle; } }

        /// <summary>
        /// Checks if this features specify type is an 
        /// </summary>
        public bool IsDrSection { get { return FeatureType == ModelFeatureType.DrSection; } }

        /// <summary>
        /// Checks if this features specify type is a Feature Folder
        /// </summary>
        public bool IsFeatureFolder { get { return FeatureType == ModelFeatureType.FeatureFolder; } }

        /// <summary>
        /// Checks if this features specify type is a Light
        /// </summary>
        public bool IsLight { get { return FeatureType == ModelFeatureType.Light; } }

        /// <summary>
        /// Checks if this features specify type is a Mate 
        /// </summary>
        public bool IsMate { get { return FeatureType == ModelFeatureType.Mate; } }

        /// <summary>
        /// Checks if this features specify type is a Mate Reference 
        /// </summary>
        public bool IsMateReference { get { return FeatureType == ModelFeatureType.MateReference; } }

        /// <summary>
        /// Checks if this features specify type is a Motion Study Results
        /// </summary>
        public bool IsMotionStudyResults { get { return FeatureType == ModelFeatureType.MotionStudyResults; } }

        /// <summary>
        /// Checks if this features specify type is a Ref Axis
        /// </summary>
        public bool IsReferenceAxis { get { return FeatureType == ModelFeatureType.ReferenceAxis; } }

        /// <summary>
        /// Checks if this features specify type is a Reference Curve
        /// </summary>
        public bool IsReferenceCruve { get { return FeatureType == ModelFeatureType.ReferenceCurve; } }

        /// <summary>
        /// Checks if this features specify type is a Reference Plane
        /// </summary>
        public bool IsReferencePlane { get { return FeatureType == ModelFeatureType.ReferencePlane; } }

        /// <summary>
        /// Checks if this features specify type is a Reference Point
        /// </summary>
        public bool IsReferencePoint { get { return FeatureType == ModelFeatureType.ReferencePoint; } }

        /// <summary>
        /// Checks if this features specify type is a Sensor 
        /// </summary>
        public bool IsSensor { get { return FeatureType == ModelFeatureType.Sensor; } }

        /// <summary>
        /// Checks if this features specify type is a Sketch
        /// </summary>
        public bool IsSketch { get { return FeatureType == ModelFeatureType.Sketch; } }

        /// <summary>
        /// Checks if this features specify type is a Sketch Block Definition
        /// </summary>
        public bool IsSketchBlockDefinition { get { return FeatureType == ModelFeatureType.SketchBlockDefinition; } }

        /// <summary>
        /// Checks if this features specify type is a Sketch Block Instance 
        /// </summary>
        public bool IsSketchBlockInstance { get { return FeatureType == ModelFeatureType.SketchBlockInstance; } }

        /// <summary>
        /// Checks if this features specify type is a Sketch Picture
        /// </summary>
        public bool IsSketchPicture { get { return FeatureType == ModelFeatureType.SketchPicture; } }

        /// <summary>
        /// Checks if this features specify type is a Surface Mid
        /// </summary>
        public bool IsSurfaceMid { get { return FeatureType == ModelFeatureType.SurfaceMid; } }

        /// <summary>
        /// Checks if this features specify type is a Table Anchor
        /// </summary>
        public bool IsTableAnchor { get { return FeatureType == ModelFeatureType.TableAnchor; } }

        #endregion

        #region Feature Data

        /// <summary>
        /// Checks if this features specify type is Base Flange data
        /// </summary>
        public bool IsBaseFlangeData { get { return FeatureType == ModelFeatureType.BaseFlangeData; } }

        /// <summary>
        /// Checks if this features specify type is Bends data
        /// </summary>
        public bool IsBendsData { get { return FeatureType == ModelFeatureType.BendsData; } }

        /// <summary>
        /// Checks if this features specify type is Boundary Boss data
        /// </summary>
        public bool IsBoundaryBossData { get { return FeatureType == ModelFeatureType.BoundaryBossData; } }

        /// <summary>
        /// Checks if this features specify type is Break Corner data
        /// </summary>
        public bool IsBreakCornerData { get { return FeatureType == ModelFeatureType.BreakCornerData; } }

        /// <summary>
        /// Checks if this features specify type is Broken Out Section data
        /// </summary>
        public bool IsBrokenOutSectionData { get { return FeatureType == ModelFeatureType.BrokenOutSectionData; } }

        /// <summary>
        /// Checks if this features specify type is Cavity data
        /// </summary>
        public bool IsCavityData { get { return FeatureType == ModelFeatureType.CavityData; } }

        /// <summary>
        /// Checks if this features specify type is Chain Pattern data
        /// </summary>
        public bool IsChainPatternData { get { return FeatureType == ModelFeatureType.ChainPatternData; } }

        /// <summary>
        /// Checks if this features specify type is Chamfer data
        /// </summary>
        public bool IsChamferData { get { return FeatureType == ModelFeatureType.ChamferData; } }

        /// <summary>
        /// Checks if this features specify type is Circular Pattern data
        /// </summary>
        public bool IsCircularPatternData { get { return FeatureType == ModelFeatureType.CircularPatternData; } }

        /// <summary>
        /// Checks if this features specify type is Combine Bodies data
        /// </summary>
        public bool IsCombineBodiesData { get { return FeatureType == ModelFeatureType.CombineBodiesData; } }

        /// <summary>
        /// Checks if this features specify type is Composite Curve data
        /// </summary>
        public bool IsCompositeCurveData { get { return FeatureType == ModelFeatureType.CompositeCurveData; } }

        /// <summary>
        /// Checks if this features specify type is Coordinate System data
        /// </summary>
        public bool IsCoordinateSystemData { get { return FeatureType == ModelFeatureType.CoordinateSystemData; } }

        /// <summary>
        /// Checks if this features specify type is Core data
        /// </summary>
        public bool IsCoreData { get { return FeatureType == ModelFeatureType.CoreData; } }

        /// <summary>
        /// Checks if this features specify type is Cosmetic Thread data
        /// </summary>
        public bool IsCosmeticThreadData { get { return FeatureType == ModelFeatureType.CosmeticThreadData; } }

        /// <summary>
        /// Checks if this features specify type is Cosmetic Weld Bead data
        /// </summary>
        public bool IsCosmeticWeldBeadData { get { return FeatureType == ModelFeatureType.CosmeticWeldBeadData; } }

        /// <summary>
        /// Checks if this features specify type is Cross Break data
        /// </summary>
        public bool IsCrossBreakData { get { return FeatureType == ModelFeatureType.CrossBreakData; } }

        /// <summary>
        /// Checks if this features specify type is Curve Driven Pattern data
        /// </summary>
        public bool IsCurveDrivenPatternData { get { return FeatureType == ModelFeatureType.CurveDrivenPatternData; } }

        /// <summary>
        /// Checks if this features specify type is Delete Body data
        /// </summary>
        public bool IsDeleteBodyData { get { return FeatureType == ModelFeatureType.DeleteBodyData; } }

        /// <summary>
        /// Checks if this features specify type is Delete Face data
        /// </summary>
        public bool IsDeleteFaceData { get { return FeatureType == ModelFeatureType.DeleteFaceData; } }

        /// <summary>
        /// Checks if this features specify type is Derived Part data
        /// </summary>
        public bool IsDerivedPartData { get { return FeatureType == ModelFeatureType.DerivedPartData; } }

        /// <summary>
        /// Checks if this features specify type is Derived Pattern data
        /// </summary>
        public bool IsDerivedPatternData { get { return FeatureType == ModelFeatureType.DerivedPatternData; } }

        /// <summary>
        /// Checks if this features specify type is Dim Pattern data
        /// </summary>
        public bool IsDimPatternData { get { return FeatureType == ModelFeatureType.DimPatternData; } }

        /// <summary>
        /// Checks if this features specify type is Dome data
        /// </summary>
        public bool IsDomeData { get { return FeatureType == ModelFeatureType.DomeData; } }

        /// <summary>
        /// Checks if this features specify type is Draft data
        /// </summary>
        public bool IsDraftData { get { return FeatureType == ModelFeatureType.DraftData; } }

        /// <summary>
        /// Checks if this features specify type is Edge Flange data
        /// </summary>
        public bool IsEdgeFlangeData { get { return FeatureType == ModelFeatureType.EdgeFlangeData; } }

        /// <summary>
        /// Checks if this features specify type is End Cap data
        /// </summary>
        public bool IsEndCapData { get { return FeatureType == ModelFeatureType.EndCapData; } }

        /// <summary>
        /// Checks if this features specify type is Extrude data
        /// </summary>
        public bool IsExtrudeData { get { return FeatureType == ModelFeatureType.ExtrudeData; } }

        /// <summary>
        /// Checks if this features specify type is Fill Pattern data
        /// </summary>
        public bool IsFillPatternData { get { return FeatureType == ModelFeatureType.FillPatternData ; } }

        /// <summary>
        /// Checks if this features specify type is Flat Pattern data
        /// </summary>
        public bool IsFlatPatternData { get { return FeatureType == ModelFeatureType.FlatPatternData; } }

        /// <summary>
        /// Checks if this features specify type is Folds data
        /// </summary>
        public bool IsFoldsData { get { return FeatureType == ModelFeatureType.FoldsData; } }

        /// <summary>
        /// Checks if this features specify type is Free Point Curve data
        /// </summary>
        public bool IsFreePointCurveData { get { return FeatureType == ModelFeatureType.FreePointCurveData; } }

        /// <summary>
        /// Checks if this features specify type is Gusset data
        /// </summary>
        public bool IsGussetData { get { return FeatureType == ModelFeatureType.GussetData; } }

        /// <summary>
        /// Checks if this features specify type is Heal Edges data
        /// </summary>
        public bool IsHealEdgesData { get { return FeatureType == ModelFeatureType.HealEdgesData; } }

        /// <summary>
        /// Checks if this features specify type is Helix data
        /// </summary>
        public bool IsHelixData { get { return FeatureType == ModelFeatureType.HelixData; } }

        /// <summary>
        /// Checks if this features specify type is Hem data
        /// </summary>
        public bool IsHemData { get { return FeatureType == ModelFeatureType.HemData; } }

        /// <summary>
        /// Checks if this features specify type is Hole Series data
        /// </summary>
        public bool IsHoleSeriesData { get { return FeatureType == ModelFeatureType.HoleSeriesData; } }

        /// <summary>
        /// Checks if this features specify type is Hole Wizard data
        /// </summary>
        public bool IsHoleWizardData { get { return FeatureType == ModelFeatureType.HoleWizardData; } }

        /// <summary>
        /// Checks if this features specify type is Imported Curve data
        /// </summary>
        public bool IsImportedCurveData { get { return FeatureType == ModelFeatureType.ImportedCurveData; } }

        /// <summary>
        /// Checks if this features specify type is Indent data
        /// </summary>
        public bool IsIndentData { get { return FeatureType == ModelFeatureType.IndentData; } }

        /// <summary>
        /// Checks if this features specify type is Intersect data
        /// </summary>
        public bool IsIntersectData { get { return FeatureType == ModelFeatureType.IntersectData; } }

        /// <summary>
        /// Checks if this features specify type is Jog data
        /// </summary>
        public bool IsJogData { get { return FeatureType == ModelFeatureType.JogData; } }

        /// <summary>
        /// Checks if this features specify type is Library Feature data
        /// </summary>
        public bool IsLibraryFeatureData { get { return FeatureType == ModelFeatureType.LibraryFeatureData; } }

        /// <summary>
        /// Checks if this features specify type is Linear Pattern data
        /// </summary>
        public bool IsLinearPatternData { get { return FeatureType == ModelFeatureType.LinearPatternData; } }

        /// <summary>
        /// Checks if this features specify type is Local Circular Pattern data
        /// </summary>
        public bool IsLocalCircularPatternData { get { return FeatureType == ModelFeatureType.LocalCircularPatternData; } }

        /// <summary>
        /// Checks if this features specify type is Local Curve Pattern data
        /// </summary>
        public bool IsLocalCurvePatternData { get { return FeatureType == ModelFeatureType.LocalCurvePatternData; } }

        /// <summary>
        /// Checks if this features specify type is Local Linear Pattern data
        /// </summary>
        public bool IsLocalLinearPatternData{ get { return FeatureType == ModelFeatureType.LocalLinearPatternData; } }

        /// <summary>
        /// Checks if this features specify type is Loft data
        /// </summary>
        public bool IsLoftData { get { return FeatureType == ModelFeatureType.LoftData; } }

        /// <summary>
        /// Checks if this features specify type is Lofted Bend data
        /// </summary>
        public bool IsLoftedBendData { get { return FeatureType == ModelFeatureType.LoftedBendsData; } }

        /// <summary>
        /// Checks if this features specify type is Macro data
        /// </summary>
        public bool IsMacroData { get { return FeatureType == ModelFeatureType.MacroData; } }

        /// <summary>
        /// Checks if this features specify type is Mirror Part data
        /// </summary>
        public bool IsMirrorPartData { get { return FeatureType == ModelFeatureType.MirrorPartData; } }

        /// <summary>
        /// Checks if this features specify type is Mirror Pattern data
        /// </summary>
        public bool IsMirrorPatternData { get { return FeatureType == ModelFeatureType.MirrorPatternData; } }

        /// <summary>
        /// Checks if this features specify type is Mirror Solid data
        /// </summary>
        public bool IsMirrorSolidData { get { return FeatureType == ModelFeatureType.MirrorSolidData; } }

        /// <summary>
        /// Checks if this features specify type is Miter Flange data
        /// </summary>
        public bool IsMiterFlangeData { get { return FeatureType == ModelFeatureType.MiterFlangeData; } }

        /// <summary>
        /// Checks if this features specify type is Motion Plot Axis data
        /// </summary>
        public bool IsMotionPlotAxisData { get { return FeatureType == ModelFeatureType.MotionPlotAxisData; } }

        /// <summary>
        /// Checks if this features specify type is Motion Plot data
        /// </summary>
        public bool IsMotionPlotData { get { return FeatureType == ModelFeatureType.MotionPlotData; } }

        /// <summary>
        /// Checks if this features specify type is Move Copy Body data
        /// </summary>
        public bool IsMoveCopyBodyData { get { return FeatureType == ModelFeatureType.MoveCopyBodyData; } }

        /// <summary>
        /// Checks if this features specify type is One Bend data
        /// </summary>
        public bool IsOneBendData { get { return FeatureType == ModelFeatureType.OneBendData; } }

        /// <summary>
        /// Checks if this features specify type is Parting Line data
        /// </summary>
        public bool IsPartingLineData { get { return FeatureType == ModelFeatureType.PartingLineData; } }

        /// <summary>
        /// Checks if this features specify type is Parting Surface data
        /// </summary>
        public bool IsPartingSurfaceData { get { return FeatureType == ModelFeatureType.PartingSurfaceData; } }

        /// <summary>
        /// Checks if this features specify type is Projection Curve data
        /// </summary>
        public bool IsProjectionCurveData { get { return FeatureType == ModelFeatureType.ProjectionCurveData; } }

        /// <summary>
        /// Checks if this features specify type is Reference Axis data
        /// </summary>
        public bool IsReferenceAxisData { get { return FeatureType == ModelFeatureType.ReferenceAxisData; } }

        /// <summary>
        /// Checks if this features specify type is Reference Plane data
        /// </summary>
        public bool IsReferencePlaneData { get { return FeatureType == ModelFeatureType.ReferencePlaneData; } }

        /// <summary>
        /// Checks if this features specify type is Reference Point Curve data
        /// </summary>
        public bool IsReferencePointCurveData { get { return FeatureType == ModelFeatureType.ReferencePointCurveData; } }

        /// <summary>
        /// Checks if this features specify type is Reference Point data
        /// </summary>
        public bool IsReferencePointData { get { return FeatureType == ModelFeatureType.ReferencePointData; } }

        /// <summary>
        /// Checks if this features specify type is Replace Face data
        /// </summary>
        public bool IsReplaceFaceData { get { return FeatureType == ModelFeatureType.ReplaceFaceData; } }

        /// <summary>
        /// Checks if this features specify type is Revolve data
        /// </summary>
        public bool IsRevolveData { get { return FeatureType == ModelFeatureType.RevolveData; } }

        /// <summary>
        /// Checks if this features specify type is Rib data
        /// </summary>
        public bool IsRibData { get { return FeatureType == ModelFeatureType.RibData; } }

        /// <summary>
        /// Checks if this features specify type is Rip data data
        /// </summary>
        public bool IsRipData { get { return FeatureType == ModelFeatureType.RipData; } }

        /// <summary>
        /// Checks if this features specify type is Save Body data
        /// </summary>
        public bool IsSaveBodyData { get { return FeatureType == ModelFeatureType.SaveBodyData; } }

        /// <summary>
        /// Checks if this features specify type is Scale data
        /// </summary>
        public bool IsScaleData { get { return FeatureType == ModelFeatureType.ScaleData; } }

        /// <summary>
        /// Checks if this features specify type is Sheet Metal data
        /// </summary>
        public bool IsSheetMetalData { get { return FeatureType == ModelFeatureType.SheetMetalData; } }

        /// <summary>
        /// Checks if this features specify type is Sheet Metal Gusset data
        /// </summary>
        public bool IsSheetMetalGussetData { get { return FeatureType == ModelFeatureType.SheetMetalGussetData; } }

        /// <summary>
        /// Checks if this features specify type is Shell data
        /// </summary>
        public bool IsShellData { get { return FeatureType == ModelFeatureType.ShellData; } }

        /// <summary>
        /// Checks if this features specify type is Shut Off Surface data
        /// </summary>
        public bool IsShutOffSurfaceData { get { return FeatureType == ModelFeatureType.ShutOffSurfaceData; } }

        /// <summary>
        /// Checks if this features specify type is Simple Fillet data
        /// </summary>
        public bool IsSimpleFilletData { get { return FeatureType == ModelFeatureType.SimpleFilletData; } }

        /// <summary>
        /// Checks if this features specify type is Simple Hole data
        /// </summary>
        public bool IsSimpleHoleData { get { return FeatureType == ModelFeatureType.SimpleHoleData; } }

        /// <summary>
        /// Checks if this features specify type is Simulation 3D Contact data
        /// </summary>
        public bool IsSimulation3DContactData { get { return FeatureType == ModelFeatureType.Simulation3DContactData; } }

        /// <summary>
        /// Checks if this features specify type is Simulation Damper data
        /// </summary>
        public bool IsSimulationDamperData { get { return FeatureType == ModelFeatureType.SimulationDamperData; } }

        /// <summary>
        /// Checks if this features specify type is Simulation Force data
        /// </summary>
        public bool IsSimulationForceData { get { return FeatureType == ModelFeatureType.SimulationForceData; } }

        /// <summary>
        /// Checks if this features specify type is Simulation Gravity data
        /// </summary>
        public bool IsSimulationGravityData { get { return FeatureType == ModelFeatureType.SimulationGravityData; } }

        /// <summary>
        /// Checks if this features specify type is Simulation Linear Spring data
        /// </summary>
        public bool IsSimulationLinearSpringData { get { return FeatureType == ModelFeatureType.SimulationLinearSpringData; } }

        /// <summary>
        /// Checks if this features specify type is Simulation Motor data
        /// </summary>
        public bool IsSimulationMotorData { get { return FeatureType == ModelFeatureType.SimulationMotorData; } }

        /// <summary>
        /// Checks if this features specify type is Sketched Bend data
        /// </summary>
        public bool IsSketchedBendData { get { return FeatureType == ModelFeatureType.SketchedBendData; } }

        /// <summary>
        /// Checks if this features specify type is Sketch Pattern data
        /// </summary>
        public bool IsSketchPatternData { get { return FeatureType == ModelFeatureType.SketchPatternData; } }

        /// <summary>
        /// Checks if this features specify type is Smart Component data
        /// </summary>
        public bool IsSmartComponentData { get { return FeatureType == ModelFeatureType.SmartComponentFeatureData; } }

        /// <summary>
        /// Checks if this features specify type is Split Body data
        /// </summary>
        public bool IsSplitBodyData { get { return FeatureType == ModelFeatureType.SplitBodyData; } }

        /// <summary>
        /// Checks if this features specify type is Split Line data
        /// </summary>
        public bool IsSplitLineData { get { return FeatureType == ModelFeatureType.SplitLineData; } }

        /// <summary>
        /// Checks if this features specify type is Surface Cut data
        /// </summary>
        public bool IsSurfaceCutData { get { return FeatureType == ModelFeatureType.SurfaceCutData; } }

        /// <summary>
        /// Checks if this features specify type is Surface Extend data
        /// </summary>
        public bool IsSurfaceExtendData { get { return FeatureType == ModelFeatureType.SurfaceExtendData; } }

        /// <summary>
        /// Checks if this features specify type is Surface Extrude data
        /// </summary>
        public bool IsSurfaceExtrudeData { get { return FeatureType == ModelFeatureType.SurfaceExtrudeData; } }

        /// <summary>
        /// Checks if this features specify type is Surface Fill data
        /// </summary>
        public bool IsSurfaceFillData { get { return FeatureType == ModelFeatureType.SurfaceFillData; } }

        /// <summary>
        /// Checks if this features specify type is Surface Flatten data
        /// </summary>
        public bool IsSurfaceFlattenData { get { return FeatureType == ModelFeatureType.SurfaceFlattenData; } }

        /// <summary>
        /// Checks if this features specify type is Surface Knit data
        /// </summary>
        public bool IsSurfaceKnitData { get { return FeatureType == ModelFeatureType.SurfaceKnitData; } }

        /// <summary>
        /// Checks if this features specify type is Surface Offset data
        /// </summary>
        public bool IsSurfaceOffsetData { get { return FeatureType == ModelFeatureType.SurfaceOffsetData; } }

        /// <summary>
        /// Checks if this features specify type is Surface Planar data
        /// </summary>
        public bool IsSurfacePlanarData { get { return FeatureType == ModelFeatureType.SurfacePlanarData; } }

        /// <summary>
        /// Checks if this features specify type is Surface Radiate data
        /// </summary>
        public bool IsSurfaceRadiateData { get { return FeatureType == ModelFeatureType.SurfaceRadiateData; } }

        /// <summary>
        /// Checks if this features specify type is Surface Revolve data
        /// </summary>
        public bool IsSurfaceRevolveData { get { return FeatureType == ModelFeatureType.SurfaceRevolveData; } }

        /// <summary>
        /// Checks if this features specify type is Surface Ruled data
        /// </summary>
        public bool IsSurfaceRuledData { get { return FeatureType == ModelFeatureType.SurfaceRuledData; } }

        /// <summary>
        /// Checks if this features specify type is Surface Trim data
        /// </summary>
        public bool IsSurfaceTrimData { get { return FeatureType == ModelFeatureType.SurfaceTrimData; } }

        /// <summary>
        /// Checks if this features specify type is Sweep data
        /// </summary>
        public bool IsSweepData { get { return FeatureType == ModelFeatureType.SweepData; } }

        /// <summary>
        /// Checks if this features specify type is Table Pattern data
        /// </summary>
        public bool IsTablePatternData { get { return FeatureType == ModelFeatureType.TablePatternData; } }

        /// <summary>
        /// Checks if this features specify type is Thicken data
        /// </summary>
        public bool IsThickenData { get { return FeatureType == ModelFeatureType.ThickenData; } }

        /// <summary>
        /// Checks if this features specify type is Tooling Split data
        /// </summary>
        public bool IsToolingSplitData { get { return FeatureType == ModelFeatureType.ToolingSplitData; } }

        /// <summary>
        /// Checks if this features specify type is Variable Fillet data
        /// </summary>
        public bool IsVariableFilletData { get { return FeatureType == ModelFeatureType.VariableFilletData; } }

        /// <summary>
        /// Checks if this features specify type is Weldment Bead data
        /// </summary>
        public bool IsWeldmentBeadData { get { return FeatureType == ModelFeatureType.WeldmentBeadData; } }

        /// <summary>
        /// Checks if this features specify type is Weldment Cut List data
        /// </summary>
        public bool IsWeldmentCutListData { get { return FeatureType == ModelFeatureType.WeldmentCutListData; } }

        /// <summary>
        /// Checks if this features specify type is Weldment Member data
        /// </summary>
        public bool IsWeldmentMemberData { get { return FeatureType == ModelFeatureType.WeldmentMemberData; } }

        /// <summary>
        /// Checks if this features specify type is Weldment Trim Extend data
        /// </summary>
        public bool IsWeldmentTrimExtendData { get { return FeatureType == ModelFeatureType.WeldmentTrimExtendData; } }

        /// <summary>
        /// Checks if this features specify type is Wrap Sketch data
        /// </summary>
        public bool IsWrapSketchData { get { return FeatureType == ModelFeatureType.WrapSketchData; } }

        #endregion

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ModelFeature(Feature model) : base(model)
        {
            // Get the specific feature
            mSpecificFeature = new SolidDnaObject<object>(model?.GetSpecificFeature2());

            // Get the definition
            mFeatureData = new SolidDnaObject<object>(model?.GetDefinition());
        }

        #endregion

        #region Protected Helpers

        /// <summary>
        /// Gets the SolidWorks feature type name, such as RefSurface, CosmeticWeldBead, FeatSurfaceBodyFolder etc...
        /// </summary>
        /// <returns></returns>
        protected string GetFeatureTypeName()
        {
            // TODO: Handle Intant3D feature, then call GetTypeName instead of 2
            return mBaseObject.GetTypeName2();
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Disposing
        /// </summary>
        public override void Dispose()
        {
            // Clean up feature and data
            mSpecificFeature?.Dispose();
            mSpecificFeature = null;

            mFeatureData?.Dispose();
            mFeatureData = null;

            base.Dispose();
        }

        #endregion
    }
}
