using System;
using System.Collections.Generic;
using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /*
     *  NOTE: Outstanding question to SolidWorks...
     * 

        From the feature, I then need to get the specific feature. This is where the fun begins.
 
        I call feature. GetTypeName2 to get the type. Then from this list http://help.solidworks.com/2020/english/api/sldworksapi/SOLIDWORKS.Interop.sldworks~SOLIDWORKS.Interop.sldworks.IFeature~GetTypeName2.html I figure out what type of feature I should expect from GetSpecificFeature2.
 
        I’ve mapped everything then I come to check out the document for GetSpecificFeature2 http://help.solidworks.com/2020/english/api/sldworksapi/SOLIDWORKS.Interop.sldworks~SOLIDWORKS.Interop.sldworks.IFeature~GetSpecificFeature2.html
 
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
    /// Represents a SolidWorks feature of any type.
    /// NOTE: This is a SharedSolidDnaObject so the passed 
    /// in COM object should be final disposed by the <see cref="SelectedObject"/> parent
    /// 
    /// TODO: See if we can add support for non-interface types
    ///        
    ///       AsmExploder (AssemblyExplodedView), CompExplodeStep (AssemblyExplodeStep), Bending (Flex),
    ///       BodyExplodeStep (MultiBodyPartExplodeStep), Deform, Imported, GridFeature (Grid),
    ///       AEMTorsionalSpring (TorsionalSpring), FormToolInstance (FormTool), BlendRefSurface (Surface-Loft),
    ///       PrtExploder (MultiBodyPartExplodedView), RefSurface (Surface-Imported),
    ///       SweepRefSurface (Surface-Sweep), UnTrimRefSurf (Surface-Untrim), MateGroup (MateGroup), Weldment (Weldment)
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
        public ModelFeatureType FeatureType => this.SpecificFeatureType();

        /// <summary>
        /// Gets the SolidWorks feature type name, such as RefSurface, CosmeticWeldBead, FeatSurfaceBodyFolder etc...
        /// </summary>
        /// <returns></returns>
        public string FeatureTypeName => GetFeatureTypeName();

        /// <summary>
        /// Gets the SolidWorks feature name, such as Sketch1
        /// </summary>
        /// <returns></returns>
        public string FeatureName => BaseObject.Name;

        /// <summary>
        /// The specific feature for this feature, if any.
        /// NOTE: This is a COM object. Set all instance variables of this to null once done if you set any
        /// </summary>
        public object SpecificFeature => mSpecificFeature?.UnsafeObject;

        /// <summary>
        /// The feature data for this feature, if any.
        /// NOTE: This is a COM object. Set all instance variables of this to null once done if you set any
        /// </summary>
        public object FeatureData => mFeatureData?.UnsafeObject;

        #region Type Checks

        #region Features

        /// <summary>
        /// Checks if this feature's specific type is an Attribute
        /// </summary>
        public bool IsAttribute => FeatureType == ModelFeatureType.Attribute;

        /// <summary>
        /// Checks if this feature's specific type is a Body Folder
        /// </summary>
        public bool IsBodyFolder => FeatureType == ModelFeatureType.BodyFolder;

        /// <summary>
        /// Checks if this feature's specific type is a BOM
        /// </summary>
        public bool IsBom => FeatureType == ModelFeatureType.Bom;

        /// <summary>
        /// Checks if this feature's specific type is a Camera
        /// </summary>
        public bool IsCamera => FeatureType == ModelFeatureType.Camera;

        /// <summary>
        /// Checks if this feature's specific type is a Comment Folder
        /// </summary>
        public bool IsCommentFolder => FeatureType == ModelFeatureType.CommentFolder;

        /// <summary>
        /// Checks if this feature's specific type is a Cosmetic Weld Bead Folder 
        /// </summary>
        public bool IsCosmeticWeldBeadFolder => FeatureType == ModelFeatureType.CosmeticWeldBeadFolder;

        /// <summary>
        /// Checks if this feature's specific type is a Detail Circle
        /// </summary>
        public bool IsDetailCircle => FeatureType == ModelFeatureType.DetailCircle;

        /// <summary>
        /// Checks if this feature's specific type is a Section view
        /// </summary>
        public bool IsDrSection => FeatureType == ModelFeatureType.DrSection;

        /// <summary>
        /// Checks if this feature's specific type is a Feature Folder
        /// </summary>
        public bool IsFeatureFolder => FeatureType == ModelFeatureType.FeatureFolder;

        /// <summary>
        /// Checks if this feature's specific type is a Flat Pattern Folder
        /// </summary>
        public bool IsFlatPatternFolder => FeatureType == ModelFeatureType.FlatPatternFolder;

        /// <summary>
        /// Checks if this feature's specific type is a Light
        /// </summary>
        public bool IsLight => FeatureType == ModelFeatureType.Light;

        /// <summary>
        /// Checks if this feature's specific type is a Mate 
        /// </summary>
        public bool IsMate => FeatureType == ModelFeatureType.Mate;

        /// <summary>
        /// Checks if this feature's specific type is a Mate Group
        /// </summary>
        public bool IsMateGroup => FeatureType == ModelFeatureType.MateGroup;

        /// <summary>
        /// Checks if this feature's specific type is a Mate Reference 
        /// </summary>
        public bool IsMateReference => FeatureType == ModelFeatureType.MateReference;

        /// <summary>
        /// Checks if this feature's specific type is a Motion Study Results
        /// </summary>
        public bool IsMotionStudyResults => FeatureType == ModelFeatureType.MotionStudyResults;

        /// <summary>
        /// Checks if this feature's specific type is a Ref Axis
        /// </summary>
        public bool IsReferenceAxis => FeatureType == ModelFeatureType.ReferenceAxis;

        /// <summary>
        /// Checks if this feature's specific type is a Reference Curve
        /// </summary>
        public bool IsReferenceCurve => FeatureType == ModelFeatureType.ReferenceCurve;

        /// <summary>
        /// Checks if this feature's specific type is a Reference Plane
        /// </summary>
        public bool IsReferencePlane => FeatureType == ModelFeatureType.ReferencePlane;

        /// <summary>
        /// Checks if this feature's specific type is a Reference Point
        /// </summary>
        public bool IsReferencePoint => FeatureType == ModelFeatureType.ReferencePoint;

        /// <summary>
        /// Checks if this feature's specific type is a Sensor 
        /// </summary>
        public bool IsSensor => FeatureType == ModelFeatureType.Sensor;

        /// <summary>
        /// Checks if this feature's specific type is a Sheet Metal Folder
        /// </summary>
        public bool IsSheetMetalFolder => FeatureType == ModelFeatureType.SheetMetalFolder;

        /// <summary>
        /// Checks if this feature's specific type is a Sketch
        /// </summary>
        public bool IsSketch => FeatureType == ModelFeatureType.Sketch;

        /// <summary>
        /// Checks if this feature's specific type is a Sketch Block Definition
        /// </summary>
        public bool IsSketchBlockDefinition => FeatureType == ModelFeatureType.SketchBlockDefinition;

        /// <summary>
        /// Checks if this feature's specific type is a Sketch Block Instance 
        /// </summary>
        public bool IsSketchBlockInstance => FeatureType == ModelFeatureType.SketchBlockInstance;

        /// <summary>
        /// Checks if this feature's specific type is a Sketch Picture
        /// </summary>
        public bool IsSketchPicture => FeatureType == ModelFeatureType.SketchPicture;

        /// <summary>
        /// Checks if this feature's specific type is a Surface Mid
        /// </summary>
        public bool IsSurfaceMid => FeatureType == ModelFeatureType.SurfaceMid;

        /// <summary>
        /// Checks if this feature's specific type is a Table Anchor
        /// </summary>
        public bool IsTableAnchor => FeatureType == ModelFeatureType.TableAnchor;

        /// <summary>
        /// Checks if this feature's specific type is a Weldment
        /// </summary>
        public bool IsWeldment => FeatureType == ModelFeatureType.Weldment;

        #endregion

        #region Feature Data

        /// <summary>
        /// Checks if this feature's specific type is Advanced Hole Wizard data
        /// </summary>
        public bool IsAdvancedHoleWizardData => FeatureType == ModelFeatureType.AdvancedHoleWizardData;

        /// <summary>
        /// Checks if this feature's specific type is  data
        /// </summary>
        public bool IsAngleMateData => FeatureType == ModelFeatureType.AngleMateData;

        /// <summary>
        /// Checks if this feature's specific type is Base Flange data
        /// </summary>
        public bool IsBaseFlangeData => FeatureType == ModelFeatureType.BaseFlangeData;

        /// <summary>
        /// Checks if this feature's specific type is Bends data
        /// </summary>
        public bool IsBendsData => FeatureType == ModelFeatureType.BendsData;

        /// <summary>
        /// Checks if this feature's specific type is Boundary Boss data
        /// </summary>
        public bool IsBoundaryBossData => FeatureType == ModelFeatureType.BoundaryBossData;

        /// <summary>
        /// Checks if this feature's specific type is Bounding Box data
        /// </summary>
        public bool IsBoundingBoxData => FeatureType == ModelFeatureType.BoundingBoxData;

        /// <summary>
        /// Checks if this feature's specific type is Break Corner data
        /// </summary>
        public bool IsBreakCornerData => FeatureType == ModelFeatureType.BreakCornerData;

        /// <summary>
        /// Checks if this feature's specific type is Broken Out Section data
        /// </summary>
        public bool IsBrokenOutSectionData => FeatureType == ModelFeatureType.BrokenOutSectionData;

        /// <summary>
        /// Checks if this feature's specific type is Cam Follower Mate data
        /// </summary>
        public bool IsCamFollowerMateData => FeatureType == ModelFeatureType.CamFollowerMateData;
        
        /// <summary>
        /// Checks if this feature's specific type is Cavity data
        /// </summary>
        public bool IsCavityData => FeatureType == ModelFeatureType.CavityData;

        /// <summary>
        /// Checks if this feature's specific type is Chain Pattern data
        /// </summary>
        public bool IsChainPatternData => FeatureType == ModelFeatureType.ChainPatternData;

        /// <summary>
        /// Checks if this feature's specific type is Chamfer data
        /// </summary>
        public bool IsChamferData => FeatureType == ModelFeatureType.ChamferData;

        /// <summary>
        /// Checks if this feature's specific type is Circular Pattern data
        /// </summary>
        public bool IsCircularPatternData => FeatureType == ModelFeatureType.CircularPatternData;

        /// <summary>
        /// Checks if this feature's specific type is Coincident Mate data
        /// </summary>
        public bool IsCoincidentMateData => FeatureType == ModelFeatureType.CoincidentMateData;

        /// <summary>
        /// Checks if this feature's specific type is Combine Bodies data
        /// </summary>
        public bool IsCombineBodiesData => FeatureType == ModelFeatureType.CombineBodiesData;

        /// <summary>
        /// Checks if this feature's specific type is Composite Curve data
        /// </summary>
        public bool IsCompositeCurveData => FeatureType == ModelFeatureType.CompositeCurveData;

        /// <summary>
        /// Checks if this feature's specific type is Concentric Mate data
        /// </summary>
        public bool IsConcentricMateData => FeatureType == ModelFeatureType.ConcentricMateData;

        /// <summary>
        /// Checks if this feature's specific type is Coordinate System data
        /// </summary>
        public bool IsCoordinateSystemData => FeatureType == ModelFeatureType.CoordinateSystemData;

        /// <summary>
        /// Checks if this feature's specific type is Core data
        /// </summary>
        public bool IsCoreData => FeatureType == ModelFeatureType.CoreData;

        /// <summary>
        /// Checks if this feature's specific type is Cosmetic Thread data
        /// </summary>
        public bool IsCosmeticThreadData => FeatureType == ModelFeatureType.CosmeticThreadData;

        /// <summary>
        /// Checks if this feature's specific type is Cosmetic Weld Bead data
        /// </summary>
        public bool IsCosmeticWeldBeadData => FeatureType == ModelFeatureType.CosmeticWeldBeadData;

        /// <summary>
        /// Checks if this feature's specific type is Cross Break data
        /// </summary>
        public bool IsCrossBreakData => FeatureType == ModelFeatureType.CrossBreakData;

        /// <summary>
        /// Checks if this feature's specific type is Curve Driven Pattern data
        /// </summary>
        public bool IsCurveDrivenPatternData => FeatureType == ModelFeatureType.CurveDrivenPatternData;

        /// <summary>
        /// Checks if this feature's specific type is Delete Body data
        /// </summary>
        public bool IsDeleteBodyData => FeatureType == ModelFeatureType.DeleteBodyData;

        /// <summary>
        /// Checks if this feature's specific type is Delete Face data
        /// </summary>
        public bool IsDeleteFaceData => FeatureType == ModelFeatureType.DeleteFaceData;

        /// <summary>
        /// Checks if this feature's specific type is Derived Part data
        /// </summary>
        public bool IsDerivedPartData => FeatureType == ModelFeatureType.DerivedPartData;

        /// <summary>
        /// Checks if this feature's specific type is Derived Pattern data
        /// </summary>
        public bool IsDerivedPatternData => FeatureType == ModelFeatureType.DerivedPatternData;

        /// <summary>
        /// Checks if this feature's specific type is Dim Pattern data
        /// </summary>
        public bool IsDimPatternData => FeatureType == ModelFeatureType.DimPatternData;

        /// <summary>
        /// Checks if this feature's specific type is Distance Mate data
        /// </summary>
        public bool IsDistanceMateData => FeatureType == ModelFeatureType.DistanceMateData;

        /// <summary>
        /// Checks if this feature's specific type is Dome data
        /// </summary>
        public bool IsDomeData => FeatureType == ModelFeatureType.DomeData;

        /// <summary>
        /// Checks if this feature's specific type is Draft data
        /// </summary>
        public bool IsDraftData => FeatureType == ModelFeatureType.DraftData;

        /// <summary>
        /// Checks if this feature's specific type is Edge Flange data
        /// </summary>
        public bool IsEdgeFlangeData => FeatureType == ModelFeatureType.EdgeFlangeData;

        /// <summary>
        /// Checks if this feature's specific type is End Cap data
        /// </summary>
        public bool IsEndCapData => FeatureType == ModelFeatureType.EndCapData;

        /// <summary>
        /// Checks if this feature's specific type is Extrude data
        /// </summary>
        public bool IsExtrudeData => FeatureType == ModelFeatureType.ExtrudeData;

        /// <summary>
        /// Checks if this feature's specific type is Fill Pattern data
        /// </summary>
        public bool IsFillPatternData => FeatureType == ModelFeatureType.FillPatternData;

        /// <summary>
        /// Checks if this feature's specific type is Flat Pattern data
        /// </summary>
        public bool IsFlatPatternData => FeatureType == ModelFeatureType.FlatPatternData;

        /// <summary>
        /// Checks if this feature's specific type is Folds data
        /// </summary>
        public bool IsFoldsData => FeatureType == ModelFeatureType.FoldsData;

        /// <summary>
        /// Checks if this feature's specific type is Free Point Curve data
        /// </summary>
        public bool IsFreePointCurveData => FeatureType == ModelFeatureType.FreePointCurveData;

        /// <summary>
        /// Checks if this feature's specific type is Gear Mate data
        /// </summary>
        public bool IsGearMateData => FeatureType == ModelFeatureType.GearMateData;

        /// <summary>
        /// Checks if this feature's specific type is Ground Plane data
        /// </summary>
        public bool IsGroundPlaneData => FeatureType == ModelFeatureType.GroundPlaneData;

        /// <summary>
        /// Checks if this feature's specific type is Gusset data
        /// </summary>
        public bool IsGussetData => FeatureType == ModelFeatureType.GussetData;

        /// <summary>
        /// Checks if this feature's specific type is Heal Edges data
        /// </summary>
        public bool IsHealEdgesData => FeatureType == ModelFeatureType.HealEdgesData;

        /// <summary>
        /// Checks if this feature's specific type is Helix data
        /// </summary>
        public bool IsHelixData => FeatureType == ModelFeatureType.HelixData;

        /// <summary>
        /// Checks if this feature's specific type is Hem data
        /// </summary>
        public bool IsHemData => FeatureType == ModelFeatureType.HemData;

        /// <summary>
        /// Checks if this feature's specific type is Hinge Mate data
        /// </summary>
        public bool IsHingeMateData => FeatureType == ModelFeatureType.HingeMateData;

        /// <summary>
        /// Checks if this feature's specific type is Hole Series data
        /// </summary>
        public bool IsHoleSeriesData => FeatureType == ModelFeatureType.HoleSeriesData;

        /// <summary>
        /// Checks if this feature's specific type is Hole Wizard data
        /// </summary>
        public bool IsHoleWizardData => FeatureType == ModelFeatureType.HoleWizardData;

        /// <summary>
        /// Checks if this feature's specific type is Import 3D Interconnect data
        /// </summary>
        public bool IsImport3DInterconnectData => FeatureType == ModelFeatureType.Import3DInterconnectData;

        /// <summary>
        /// Checks if this feature's specific type is Imported Curve data
        /// </summary>
        public bool IsImportedCurveData => FeatureType == ModelFeatureType.ImportedCurveData;

        /// <summary>
        /// Checks if this feature's specific type is Indent data
        /// </summary>
        public bool IsIndentData => FeatureType == ModelFeatureType.IndentData;

        /// <summary>
        /// Checks if this feature's specific type is Intersect data
        /// </summary>
        public bool IsIntersectData => FeatureType == ModelFeatureType.IntersectData;

        /// <summary>
        /// Checks if this feature's specific type is Jog data
        /// </summary>
        public bool IsJogData => FeatureType == ModelFeatureType.JogData;

        /// <summary>
        /// Checks if this feature's specific type is Library Feature data
        /// </summary>
        public bool IsLibraryFeatureData => FeatureType == ModelFeatureType.LibraryFeatureData;

        /// <summary>
        /// Checks if this feature's specific type is Linear Coupler Mate data
        /// </summary>
        public bool IsLinearCouplerMateData => FeatureType == ModelFeatureType.LinearCouplerMateData;

        /// <summary>
        /// Checks if this feature's specific type is Linear Pattern data
        /// </summary>
        public bool IsLinearPatternData => FeatureType == ModelFeatureType.LinearPatternData;

        /// <summary>
        /// Checks if this feature's specific type is Local Circular Pattern data
        /// </summary>
        public bool IsLocalCircularPatternData => FeatureType == ModelFeatureType.LocalCircularPatternData;

        /// <summary>
        /// Checks if this feature's specific type is Local Curve Pattern data
        /// </summary>
        public bool IsLocalCurvePatternData => FeatureType == ModelFeatureType.LocalCurvePatternData;

        /// <summary>
        /// Checks if this feature's specific type is Local Linear Pattern data
        /// </summary>
        public bool IsLocalLinearPatternData => FeatureType == ModelFeatureType.LocalLinearPatternData;

        /// <summary>
        /// Checks if this feature's specific type is Local Sketch Pattern data
        /// </summary>
        public bool IsLocalSketchPatternData => FeatureType == ModelFeatureType.LocalSketchPatternData;

        /// <summary>
        /// Checks if this feature's specific type is Lock Mate data
        /// </summary>
        public bool IsLockMateData => FeatureType == ModelFeatureType.LockMateData;

        /// <summary>
        /// Checks if this feature's specific type is Loft data
        /// </summary>
        public bool IsLoftData => FeatureType == ModelFeatureType.LoftData;

        /// <summary>
        /// Checks if this feature's specific type is Lofted Bend data
        /// </summary>
        public bool IsLoftedBendData => FeatureType == ModelFeatureType.LoftedBendsData;

        /// <summary>
        /// Checks if this feature's specific type is Macro data
        /// </summary>
        public bool IsMacroData => FeatureType == ModelFeatureType.MacroData;

        /// <summary>
        /// Checks if this feature's specific type is Mirror Component data
        /// </summary>
        public bool IsMirrorComponentData => FeatureType == ModelFeatureType.MirrorComponentData;

        /// <summary>
        /// Checks if this feature's specific type is Mirror Part data
        /// </summary>
        public bool IsMirrorPartData => FeatureType == ModelFeatureType.MirrorPartData;

        /// <summary>
        /// Checks if this feature's specific type is Mirror Pattern data
        /// </summary>
        public bool IsMirrorPatternData => FeatureType == ModelFeatureType.MirrorPatternData;

        /// <summary>
        /// Checks if this feature's specific type is Mirror Solid data
        /// </summary>
        public bool IsMirrorSolidData => FeatureType == ModelFeatureType.MirrorSolidData;

        /// <summary>
        /// Checks if this feature's specific type is Miter Flange data
        /// </summary>
        public bool IsMiterFlangeData => FeatureType == ModelFeatureType.MiterFlangeData;

        /// <summary>
        /// Checks if this feature's specific type is Motion Plot Axis data
        /// </summary>
        public bool IsMotionPlotAxisData => FeatureType == ModelFeatureType.MotionPlotAxisData;

        /// <summary>
        /// Checks if this feature's specific type is Motion Plot data
        /// </summary>
        public bool IsMotionPlotData => FeatureType == ModelFeatureType.MotionPlotData;

        /// <summary>
        /// Checks if this feature's specific type is Move Copy Body data
        /// </summary>
        public bool IsMoveCopyBodyData => FeatureType == ModelFeatureType.MoveCopyBodyData;

        /// <summary>
        /// Checks if this feature's specific type is Normal Cut data
        /// </summary>
        public bool IsNormalCutData => FeatureType == ModelFeatureType.NormalCutData;

        /// <summary>
        /// Checks if this feature's specific type is One Bend data
        /// </summary>
        public bool IsOneBendData => FeatureType == ModelFeatureType.OneBendData;

        /// <summary>
        /// Checks if this feature's specific type is Parallel Mate data
        /// </summary>
        public bool IsParallelMateData => FeatureType == ModelFeatureType.ParallelMateData;

        /// <summary>
        /// Checks if this feature's specific type is Parting Line data
        /// </summary>
        public bool IsPartingLineData => FeatureType == ModelFeatureType.PartingLineData;

        /// <summary>
        /// Checks if this feature's specific type is Parting Surface data
        /// </summary>
        public bool IsPartingSurfaceData => FeatureType == ModelFeatureType.PartingSurfaceData;

        /// <summary>
        /// Checks if this feature's specific type is Perpendicular Mate data
        /// </summary>
        public bool IsPerpendicularMateData => FeatureType == ModelFeatureType.PerpendicularMateData;

        /// <summary>
        /// Checks if this feature's specific type is Profile Center Mate data
        /// </summary>
        public bool IsProfileCenterData => FeatureType == ModelFeatureType.ProfileCenterMateData;

        /// <summary>
        /// Checks if this feature's specific type is Projection Curve data
        /// </summary>
        public bool IsProjectionCurveData => FeatureType == ModelFeatureType.ProjectionCurveData;

        /// <summary>
        /// Checks if this feature's specific type is Rack Pinion Mate data
        /// </summary>
        public bool IsRackPinionMateData => FeatureType == ModelFeatureType.RackPinionMateData;

        /// <summary>
        /// Checks if this feature's specific type is Reference Axis data
        /// </summary>
        public bool IsReferenceAxisData => FeatureType == ModelFeatureType.ReferenceAxisData;

        /// <summary>
        /// Checks if this feature's specific type is Reference Plane data
        /// </summary>
        public bool IsReferencePlaneData => FeatureType == ModelFeatureType.ReferencePlaneData;

        /// <summary>
        /// Checks if this feature's specific type is Reference Point Curve data
        /// </summary>
        public bool IsReferencePointCurveData => FeatureType == ModelFeatureType.ReferencePointCurveData;

        /// <summary>
        /// Checks if this feature's specific type is Reference Point data
        /// </summary>
        public bool IsReferencePointData => FeatureType == ModelFeatureType.ReferencePointData;

        /// <summary>
        /// Checks if this feature's specific type is Replace Face data
        /// </summary>
        public bool IsReplaceFaceData => FeatureType == ModelFeatureType.ReplaceFaceData;

        /// <summary>
        /// Checks if this feature's specific type is Revolve data
        /// </summary>
        public bool IsRevolveData => FeatureType == ModelFeatureType.RevolveData;

        /// <summary>
        /// Checks if this feature's specific type is Rib data
        /// </summary>
        public bool IsRibData => FeatureType == ModelFeatureType.RibData;

        /// <summary>
        /// Checks if this feature's specific type is Rip data
        /// </summary>
        public bool IsRipData => FeatureType == ModelFeatureType.RipData;

        /// <summary>
        /// Checks if this feature's specific type is Save Body data
        /// </summary>
        public bool IsSaveBodyData => FeatureType == ModelFeatureType.SaveBodyData;

        /// <summary>
        /// Checks if this feature's specific type is Scale data
        /// </summary>
        public bool IsScaleData => FeatureType == ModelFeatureType.ScaleData;

        /// <summary>
        /// Checks if this feature's specific type is Screw Mate data
        /// </summary>
        public bool IsScrewMateData => FeatureType == ModelFeatureType.ScrewMateData;

        /// <summary>
        /// Checks if this feature's specific type is Sheet Metal data
        /// </summary>
        public bool IsSheetMetalData => FeatureType == ModelFeatureType.SheetMetalData;

        /// <summary>
        /// Checks if this feature's specific type is Sheet Metal Gusset data
        /// </summary>
        public bool IsSheetMetalGussetData => FeatureType == ModelFeatureType.SheetMetalGussetData;

        /// <summary>
        /// Checks if this feature's specific type is Shell data
        /// </summary>
        public bool IsShellData => FeatureType == ModelFeatureType.ShellData;

        /// <summary>
        /// Checks if this feature's specific type is Shut Off Surface data
        /// </summary>
        public bool IsShutOffSurfaceData => FeatureType == ModelFeatureType.ShutOffSurfaceData;

        /// <summary>
        /// Checks if this feature's specific type is Simple Fillet data
        /// </summary>
        public bool IsSimpleFilletData => FeatureType == ModelFeatureType.SimpleFilletData;

        /// <summary>
        /// Checks if this feature's specific type is Simple Hole data
        /// </summary>
        public bool IsSimpleHoleData => FeatureType == ModelFeatureType.SimpleHoleData;

        /// <summary>
        /// Checks if this feature's specific type is Simulation 3D Contact data
        /// </summary>
        public bool IsSimulation3DContactData => FeatureType == ModelFeatureType.Simulation3DContactData;

        /// <summary>
        /// Checks if this feature's specific type is Simulation Damper data
        /// </summary>
        public bool IsSimulationDamperData => FeatureType == ModelFeatureType.SimulationDamperData;

        /// <summary>
        /// Checks if this feature's specific type is Simulation Force data
        /// </summary>
        public bool IsSimulationForceData => FeatureType == ModelFeatureType.SimulationForceData;

        /// <summary>
        /// Checks if this feature's specific type is Simulation Gravity data
        /// </summary>
        public bool IsSimulationGravityData => FeatureType == ModelFeatureType.SimulationGravityData;

        /// <summary>
        /// Checks if this feature's specific type is Simulation Linear Spring data
        /// </summary>
        public bool IsSimulationLinearSpringData => FeatureType == ModelFeatureType.SimulationLinearSpringData;

        /// <summary>
        /// Checks if this feature's specific type is Simulation Motor data
        /// </summary>
        public bool IsSimulationMotorData => FeatureType == ModelFeatureType.SimulationMotorData;

        /// <summary>
        /// Checks if this feature's specific type is Sketched Bend data
        /// </summary>
        public bool IsSketchedBendData => FeatureType == ModelFeatureType.SketchedBendData;

        /// <summary>
        /// Checks if this feature's specific type is Sketch Pattern data
        /// </summary>
        public bool IsSketchPatternData => FeatureType == ModelFeatureType.SketchPatternData;

        /// <summary>
        /// Checks if this feature's specific type is Slot Mate data
        /// </summary>
        public bool IsSlotMateData => FeatureType == ModelFeatureType.SlotMateData;

        /// <summary>
        /// Checks if this feature's specific type is Smart Component data
        /// </summary>
        public bool IsSmartComponentData => FeatureType == ModelFeatureType.SmartComponentFeatureData;

        /// <summary>
        /// Checks if this feature's specific type is Split Body data
        /// </summary>
        public bool IsSplitBodyData => FeatureType == ModelFeatureType.SplitBodyData;

        /// <summary>
        /// Checks if this feature's specific type is Split Line data
        /// </summary>
        public bool IsSplitLineData => FeatureType == ModelFeatureType.SplitLineData;

        /// <summary>
        /// Checks if this feature's specific type is Surface Cut data
        /// </summary>
        public bool IsSurfaceCutData => FeatureType == ModelFeatureType.SurfaceCutData;

        /// <summary>
        /// Checks if this feature's specific type is Surface Extend data
        /// </summary>
        public bool IsSurfaceExtendData => FeatureType == ModelFeatureType.SurfaceExtendData;

        /// <summary>
        /// Checks if this feature's specific type is Surface Extrude data
        /// </summary>
        public bool IsSurfaceExtrudeData => FeatureType == ModelFeatureType.SurfaceExtrudeData;

        /// <summary>
        /// Checks if this feature's specific type is Surface Fill data
        /// </summary>
        public bool IsSurfaceFillData => FeatureType == ModelFeatureType.SurfaceFillData;

        /// <summary>
        /// Checks if this feature's specific type is Surface Flatten data
        /// </summary>
        public bool IsSurfaceFlattenData => FeatureType == ModelFeatureType.SurfaceFlattenData;

        /// <summary>
        /// Checks if this feature's specific type is Surface Knit data
        /// </summary>
        public bool IsSurfaceKnitData => FeatureType == ModelFeatureType.SurfaceKnitData;

        /// <summary>
        /// Checks if this feature's specific type is Surface Offset data
        /// </summary>
        public bool IsSurfaceOffsetData => FeatureType == ModelFeatureType.SurfaceOffsetData;

        /// <summary>
        /// Checks if this feature's specific type is Surface Planar data
        /// </summary>
        public bool IsSurfacePlanarData => FeatureType == ModelFeatureType.SurfacePlanarData;

        /// <summary>
        /// Checks if this feature's specific type is Surface Radiate data
        /// </summary>
        public bool IsSurfaceRadiateData => FeatureType == ModelFeatureType.SurfaceRadiateData;

        /// <summary>
        /// Checks if this feature's specific type is Surface Revolve data
        /// </summary>
        public bool IsSurfaceRevolveData => FeatureType == ModelFeatureType.SurfaceRevolveData;

        /// <summary>
        /// Checks if this feature's specific type is Surface Ruled data
        /// </summary>
        public bool IsSurfaceRuledData => FeatureType == ModelFeatureType.SurfaceRuledData;

        /// <summary>
        /// Checks if this feature's specific type is Surface Sweep data
        /// </summary>
        public bool IsSurfaceSweepData => FeatureType == ModelFeatureType.SurfaceSweepData;

        /// <summary>
        /// Checks if this feature's specific type is Surface Trim data
        /// </summary>
        public bool IsSurfaceTrimData => FeatureType == ModelFeatureType.SurfaceTrimData;

        /// <summary>
        /// Checks if this feature's specific type is Sweep data
        /// </summary>
        public bool IsSweepData => FeatureType == ModelFeatureType.SweepData;

        /// <summary>
        /// Checks if this feature's specific type is Symmetric Mate data
        /// </summary>
        public bool IsSymmetricMateData => FeatureType == ModelFeatureType.SymmetricMateData;

        /// <summary>
        /// Checks if this feature's specific type is Table Pattern data
        /// </summary>
        public bool IsTablePatternData => FeatureType == ModelFeatureType.TablePatternData;

        /// <summary>
        /// Checks if this feature's specific type is Tangent Mate data
        /// </summary>
        public bool IsTangentMateData => FeatureType == ModelFeatureType.TangentMateData;

        /// <summary>
        /// Checks if this feature's specific type is Thicken data
        /// </summary>
        public bool IsThickenData => FeatureType == ModelFeatureType.ThickenData;

        /// <summary>
        /// Checks if this feature's specific type is Thread data
        /// </summary>
        public bool IsThreadData => FeatureType == ModelFeatureType.ThreadData;

        /// <summary>
        /// Checks if this feature's specific type is Tooling Split data
        /// </summary>
        public bool IsToolingSplitData => FeatureType == ModelFeatureType.ToolingSplitData;

        /// <summary>
        /// Checks if this feature's specific type is Universal Joint Mate data
        /// </summary>
        public bool IsUniversalJointMateData => FeatureType == ModelFeatureType.UniversalJointMateData;

        /// <summary>
        /// Checks if this feature's specific type is Variable Fillet data
        /// </summary>
        public bool IsVariableFilletData => FeatureType == ModelFeatureType.VariableFilletData;

        /// <summary>
        /// Checks if this feature's specific type is Weldment Bead data
        /// </summary>
        public bool IsWeldmentBeadData => FeatureType == ModelFeatureType.WeldmentBeadData;

        /// <summary>
        /// Checks if this feature's specific type is Weldment Cut List data
        /// </summary>
        public bool IsWeldmentCutListData => FeatureType == ModelFeatureType.WeldmentCutListData;

        /// <summary>
        /// Checks if this feature's specific type is Weldment Member data
        /// </summary>
        public bool IsWeldmentMemberData => FeatureType == ModelFeatureType.WeldmentMemberData;

        /// <summary>
        /// Checks if this feature's specific type is Weldment Trim Extend data
        /// </summary>
        public bool IsWeldmentTrimExtendData => FeatureType == ModelFeatureType.WeldmentTrimExtendData;

        /// <summary>
        /// Checks if this feature's specific type is Width Mate data
        /// </summary>
        public bool IsWidthMateData => FeatureType == ModelFeatureType.WidthMateData;

        /// <summary>
        /// Checks if this feature's specific type is Wrap Sketch data
        /// </summary>
        public bool IsWrapSketchData => FeatureType == ModelFeatureType.WrapSketchData;

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

        #region Public Methods

        /// <summary>
        /// Sets the suppression state of this feature
        /// </summary>
        /// <remarks>SolidWorks does not allow suppressing features while a PropertyManager page is open.</remarks>
        /// <param name="state">Suppression state as defined in <see cref="ModelFeatureSuppressionState"/></param>
        /// <param name="configurationOption">Configuration option as defined in <see cref="ModelConfigurationOptions"/></param>
        /// <param name="configurationNames">Array of configuration names; valid only if configurationOption set to <see cref="ModelConfigurationOptions.SpecificConfiguration"/></param>
        /// <returns>True if operation was successful</returns>
        public bool SetSuppressionState(ModelFeatureSuppressionState state, ModelConfigurationOptions configurationOption, string[] configurationNames = null)
        {
            return BaseObject.SetSuppression2((int)state, (int)configurationOption, configurationNames);
        }

        /// <summary>
        /// Gets whether the feature in the specified configurations is suppressed
        /// </summary>
        /// <param name="configurationOption">Configuration option as defined in <see cref="ModelConfigurationOptions"/></param>
        /// <param name="configurationNames">Array of configuration names</param>
        /// <returns>Array of Booleans indicating the suppression states for the feature in the specified configurations</returns>
        public bool[] IsSuppressed(ModelConfigurationOptions configurationOption, string[] configurationNames = null)
        {
            return (bool[])BaseObject.IsSuppressed2((int)configurationOption, configurationNames);
        }

        #region Custom Properties

        /// <summary>
        /// Gets a custom property editor for this feature.
        /// Throws an error when the feature is not a cut list folder or the Weldment feature.
        /// 
        /// NOTE: Custom Property Editor must be disposed of once finished
        /// </summary>
        public CustomPropertyEditor GetCustomPropertyEditor()
        {
            if (FeatureTypeName == "CutListFolder" || FeatureTypeName == "Weldment")
                return new CustomPropertyEditor(UnsafeObject.CustomPropertyManager);

            throw new SolidDnaException(
                SolidDnaErrors.CreateError(
                    SolidDnaErrorTypeCode.SolidWorksModel,
                    SolidDnaErrorCode.SolidWorksModelFeatureGetCustomPropertyEditor,
                    Localization.GetString("SolidWorksModelFeatureGetCustomPropertyEditor")));
        }

        /// <summary>
        /// Sets a custom property to the given value.
        /// Only works for Cut List Folders and the Weldment feature.
        /// </summary>
        /// <param name="name">The name of the property</param>
        /// <param name="value">The value of the property</param>
        public void SetCustomProperty(string name, string value)
        {
            // Get the custom property editor
            using (var editor = GetCustomPropertyEditor())
            {
                // Set the property
                editor.SetCustomProperty(name, value);
            }
        }

        /// <summary>
        /// Gets a custom property by the given name. 
        /// Only works for Cut List Folders and the Weldment feature.
        /// </summary>
        /// <param name="name">The name of the custom property</param>
        ///<param name="resolved">True to get the resolved value of the property, false to get the actual text</param>
        /// <returns></returns>
        public string GetCustomProperty(string name, bool resolved = false)
        {
            // Get the custom property editor
            using (var editor = GetCustomPropertyEditor())
            {
                // Get the property
                return editor.GetCustomProperty(name, resolve: resolved);
            }
        }

        /// <summary>
        /// Gets all of the custom properties in this feature.
        /// Only works for Cut List Folders and the Weldment feature.
        /// </summary>
        /// <param name="action">The custom properties list to be worked on inside the action. NOTE: Do not store references to them outside of this action</param>
        /// <returns></returns>
        public void CustomProperties(Action<List<CustomProperty>> action)
        {
            // Get the custom property editor
            using (var editor = GetCustomPropertyEditor())
            {
                // Get the properties
                var properties = editor.GetCustomProperties();

                // Let the action use them
                action(properties);
            }
        }

        #endregion

        #endregion

        #region Protected Helpers

        /// <summary>
        /// Gets the SolidWorks feature type name, such as RefSurface, CosmeticWeldBead, FeatSurfaceBodyFolder etc...
        /// </summary>
        /// <returns></returns>
        protected string GetFeatureTypeName()
        {
            // TODO: Handle Intant3D feature, then call GetTypeName instead of 2
            return BaseObject.GetTypeName2();
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns a user-friendly string with feature properties.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Name: {FeatureName}. Type: {FeatureTypeName}";
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
