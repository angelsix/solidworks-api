﻿using SolidWorks.Interop.sldworks;

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
        public string FeatureName => mBaseObject.Name;

        /// <summary>
        /// The specific feature for this feature, if any
        /// NOTE: This is a COM object. Set all instance variables of this to null once done if you set any
        /// </summary>
        public object SpecificFeature => mSpecificFeature?.UnsafeObject;

        /// <summary>
        /// The feature data for this feature, if any
        /// NOTE: This is a COM object. Set all instance variables of this to null once done if you set any
        /// </summary>
        public object FeatureData => mFeatureData?.UnsafeObject;

        #region Type Checks

        #region Features

        /// <summary>
        /// Checks if this features specify type is an Attribute
        /// </summary>
        public bool IsAttribute => FeatureType == ModelFeatureType.Attribute;

        /// <summary>
        /// Checks if this features specify type is a Body Folder
        /// </summary>
        public bool IsBodyFolder => FeatureType == ModelFeatureType.BodyFolder;

        /// <summary>
        /// Checks if this features specify type is a BOM
        /// </summary>
        public bool IsBom => FeatureType == ModelFeatureType.Bom;

        /// <summary>
        /// Checks if this features specify type is a Camera
        /// </summary>
        public bool IsCamera => FeatureType == ModelFeatureType.Camera;

        /// <summary>
        /// Checks if this features specify type is a Comment Folder
        /// </summary>
        public bool IsCommentFolder => FeatureType == ModelFeatureType.CommentFolder;

        /// <summary>
        /// Checks if this features specify type is a Cosmetic Weld Bead Folder 
        /// </summary>
        public bool IsCosmeticWeldBeadFolder => FeatureType == ModelFeatureType.CosmeticWeldBeadFolder;

        /// <summary>
        /// Checks if this features specify type is a Detail Circle
        /// </summary>
        public bool IsDetailCircle => FeatureType == ModelFeatureType.DetailCircle;

        /// <summary>
        /// Checks if this features specify type is an 
        /// </summary>
        public bool IsDrSection => FeatureType == ModelFeatureType.DrSection;

        /// <summary>
        /// Checks if this features specify type is a Feature Folder
        /// </summary>
        public bool IsFeatureFolder => FeatureType == ModelFeatureType.FeatureFolder;

        /// <summary>
        /// Checks if this features specify type is a Light
        /// </summary>
        public bool IsLight => FeatureType == ModelFeatureType.Light;

        /// <summary>
        /// Checks if this features specify type is a Mate 
        /// </summary>
        public bool IsMate => FeatureType == ModelFeatureType.Mate;

        /// <summary>
        /// Checks if this features specify type is a Mate Reference 
        /// </summary>
        public bool IsMateReference => FeatureType == ModelFeatureType.MateReference;

        /// <summary>
        /// Checks if this features specify type is a Motion Study Results
        /// </summary>
        public bool IsMotionStudyResults => FeatureType == ModelFeatureType.MotionStudyResults;

        /// <summary>
        /// Checks if this features specify type is a Ref Axis
        /// </summary>
        public bool IsReferenceAxis => FeatureType == ModelFeatureType.ReferenceAxis;

        /// <summary>
        /// Checks if this features specify type is a Reference Curve
        /// </summary>
        public bool IsReferenceCurve => FeatureType == ModelFeatureType.ReferenceCurve;

        /// <summary>
        /// Checks if this features specify type is a Reference Plane
        /// </summary>
        public bool IsReferencePlane => FeatureType == ModelFeatureType.ReferencePlane;

        /// <summary>
        /// Checks if this features specify type is a Reference Point
        /// </summary>
        public bool IsReferencePoint => FeatureType == ModelFeatureType.ReferencePoint;

        /// <summary>
        /// Checks if this features specify type is a Sensor 
        /// </summary>
        public bool IsSensor => FeatureType == ModelFeatureType.Sensor;

        /// <summary>
        /// Checks if this features specify type is a Sketch
        /// </summary>
        public bool IsSketch => FeatureType == ModelFeatureType.Sketch;

        /// <summary>
        /// Checks if this features specify type is a Sketch Block Definition
        /// </summary>
        public bool IsSketchBlockDefinition => FeatureType == ModelFeatureType.SketchBlockDefinition;

        /// <summary>
        /// Checks if this features specify type is a Sketch Block Instance 
        /// </summary>
        public bool IsSketchBlockInstance => FeatureType == ModelFeatureType.SketchBlockInstance;

        /// <summary>
        /// Checks if this features specify type is a Sketch Picture
        /// </summary>
        public bool IsSketchPicture => FeatureType == ModelFeatureType.SketchPicture;

        /// <summary>
        /// Checks if this features specify type is a Surface Mid
        /// </summary>
        public bool IsSurfaceMid => FeatureType == ModelFeatureType.SurfaceMid;

        /// <summary>
        /// Checks if this features specify type is a Table Anchor
        /// </summary>
        public bool IsTableAnchor => FeatureType == ModelFeatureType.TableAnchor;

        #endregion

        #region Feature Data

        /// <summary>
        /// Checks if this features specify type is Base Flange data
        /// </summary>
        public bool IsBaseFlangeData => FeatureType == ModelFeatureType.BaseFlangeData;

        /// <summary>
        /// Checks if this features specify type is Bends data
        /// </summary>
        public bool IsBendsData => FeatureType == ModelFeatureType.BendsData;

        /// <summary>
        /// Checks if this features specify type is Boundary Boss data
        /// </summary>
        public bool IsBoundaryBossData => FeatureType == ModelFeatureType.BoundaryBossData;

        /// <summary>
        /// Checks if this features specify type is Break Corner data
        /// </summary>
        public bool IsBreakCornerData => FeatureType == ModelFeatureType.BreakCornerData;

        /// <summary>
        /// Checks if this features specify type is Broken Out Section data
        /// </summary>
        public bool IsBrokenOutSectionData => FeatureType == ModelFeatureType.BrokenOutSectionData;

        /// <summary>
        /// Checks if this features specify type is Cavity data
        /// </summary>
        public bool IsCavityData => FeatureType == ModelFeatureType.CavityData;

        /// <summary>
        /// Checks if this features specify type is Chain Pattern data
        /// </summary>
        public bool IsChainPatternData => FeatureType == ModelFeatureType.ChainPatternData;

        /// <summary>
        /// Checks if this features specify type is Chamfer data
        /// </summary>
        public bool IsChamferData => FeatureType == ModelFeatureType.ChamferData;

        /// <summary>
        /// Checks if this features specify type is Circular Pattern data
        /// </summary>
        public bool IsCircularPatternData => FeatureType == ModelFeatureType.CircularPatternData;

        /// <summary>
        /// Checks if this features specify type is Combine Bodies data
        /// </summary>
        public bool IsCombineBodiesData => FeatureType == ModelFeatureType.CombineBodiesData;

        /// <summary>
        /// Checks if this features specify type is Composite Curve data
        /// </summary>
        public bool IsCompositeCurveData => FeatureType == ModelFeatureType.CompositeCurveData;

        /// <summary>
        /// Checks if this features specify type is Coordinate System data
        /// </summary>
        public bool IsCoordinateSystemData => FeatureType == ModelFeatureType.CoordinateSystemData;

        /// <summary>
        /// Checks if this features specify type is Core data
        /// </summary>
        public bool IsCoreData => FeatureType == ModelFeatureType.CoreData;

        /// <summary>
        /// Checks if this features specify type is Cosmetic Thread data
        /// </summary>
        public bool IsCosmeticThreadData => FeatureType == ModelFeatureType.CosmeticThreadData;

        /// <summary>
        /// Checks if this features specify type is Cosmetic Weld Bead data
        /// </summary>
        public bool IsCosmeticWeldBeadData => FeatureType == ModelFeatureType.CosmeticWeldBeadData;

        /// <summary>
        /// Checks if this features specify type is Cross Break data
        /// </summary>
        public bool IsCrossBreakData => FeatureType == ModelFeatureType.CrossBreakData;

        /// <summary>
        /// Checks if this features specify type is Curve Driven Pattern data
        /// </summary>
        public bool IsCurveDrivenPatternData => FeatureType == ModelFeatureType.CurveDrivenPatternData;

        /// <summary>
        /// Checks if this features specify type is Delete Body data
        /// </summary>
        public bool IsDeleteBodyData => FeatureType == ModelFeatureType.DeleteBodyData;

        /// <summary>
        /// Checks if this features specify type is Delete Face data
        /// </summary>
        public bool IsDeleteFaceData => FeatureType == ModelFeatureType.DeleteFaceData;

        /// <summary>
        /// Checks if this features specify type is Derived Part data
        /// </summary>
        public bool IsDerivedPartData => FeatureType == ModelFeatureType.DerivedPartData;

        /// <summary>
        /// Checks if this features specify type is Derived Pattern data
        /// </summary>
        public bool IsDerivedPatternData => FeatureType == ModelFeatureType.DerivedPatternData;

        /// <summary>
        /// Checks if this features specify type is Dim Pattern data
        /// </summary>
        public bool IsDimPatternData => FeatureType == ModelFeatureType.DimPatternData;

        /// <summary>
        /// Checks if this features specify type is Dome data
        /// </summary>
        public bool IsDomeData => FeatureType == ModelFeatureType.DomeData;

        /// <summary>
        /// Checks if this features specify type is Draft data
        /// </summary>
        public bool IsDraftData => FeatureType == ModelFeatureType.DraftData;

        /// <summary>
        /// Checks if this features specify type is Edge Flange data
        /// </summary>
        public bool IsEdgeFlangeData => FeatureType == ModelFeatureType.EdgeFlangeData;

        /// <summary>
        /// Checks if this features specify type is End Cap data
        /// </summary>
        public bool IsEndCapData => FeatureType == ModelFeatureType.EndCapData;

        /// <summary>
        /// Checks if this features specify type is Extrude data
        /// </summary>
        public bool IsExtrudeData => FeatureType == ModelFeatureType.ExtrudeData;

        /// <summary>
        /// Checks if this features specify type is Fill Pattern data
        /// </summary>
        public bool IsFillPatternData => FeatureType == ModelFeatureType.FillPatternData;

        /// <summary>
        /// Checks if this features specify type is Flat Pattern data
        /// </summary>
        public bool IsFlatPatternData => FeatureType == ModelFeatureType.FlatPatternData;

        /// <summary>
        /// Checks if this features specify type is Folds data
        /// </summary>
        public bool IsFoldsData => FeatureType == ModelFeatureType.FoldsData;

        /// <summary>
        /// Checks if this features specify type is Free Point Curve data
        /// </summary>
        public bool IsFreePointCurveData => FeatureType == ModelFeatureType.FreePointCurveData;

        /// <summary>
        /// Checks if this features specify type is Gusset data
        /// </summary>
        public bool IsGussetData => FeatureType == ModelFeatureType.GussetData;

        /// <summary>
        /// Checks if this features specify type is Heal Edges data
        /// </summary>
        public bool IsHealEdgesData => FeatureType == ModelFeatureType.HealEdgesData;

        /// <summary>
        /// Checks if this features specify type is Helix data
        /// </summary>
        public bool IsHelixData => FeatureType == ModelFeatureType.HelixData;

        /// <summary>
        /// Checks if this features specify type is Hem data
        /// </summary>
        public bool IsHemData => FeatureType == ModelFeatureType.HemData;

        /// <summary>
        /// Checks if this features specify type is Hole Series data
        /// </summary>
        public bool IsHoleSeriesData => FeatureType == ModelFeatureType.HoleSeriesData;

        /// <summary>
        /// Checks if this features specify type is Hole Wizard data
        /// </summary>
        public bool IsHoleWizardData => FeatureType == ModelFeatureType.HoleWizardData;

        /// <summary>
        /// Checks if this features specify type is Imported Curve data
        /// </summary>
        public bool IsImportedCurveData => FeatureType == ModelFeatureType.ImportedCurveData;

        /// <summary>
        /// Checks if this features specify type is Indent data
        /// </summary>
        public bool IsIndentData => FeatureType == ModelFeatureType.IndentData;

        /// <summary>
        /// Checks if this features specify type is Intersect data
        /// </summary>
        public bool IsIntersectData => FeatureType == ModelFeatureType.IntersectData;

        /// <summary>
        /// Checks if this features specify type is Jog data
        /// </summary>
        public bool IsJogData => FeatureType == ModelFeatureType.JogData;

        /// <summary>
        /// Checks if this features specify type is Library Feature data
        /// </summary>
        public bool IsLibraryFeatureData => FeatureType == ModelFeatureType.LibraryFeatureData;

        /// <summary>
        /// Checks if this features specify type is Linear Pattern data
        /// </summary>
        public bool IsLinearPatternData => FeatureType == ModelFeatureType.LinearPatternData;

        /// <summary>
        /// Checks if this features specify type is Local Circular Pattern data
        /// </summary>
        public bool IsLocalCircularPatternData => FeatureType == ModelFeatureType.LocalCircularPatternData;

        /// <summary>
        /// Checks if this features specify type is Local Curve Pattern data
        /// </summary>
        public bool IsLocalCurvePatternData => FeatureType == ModelFeatureType.LocalCurvePatternData;

        /// <summary>
        /// Checks if this features specify type is Local Linear Pattern data
        /// </summary>
        public bool IsLocalLinearPatternData => FeatureType == ModelFeatureType.LocalLinearPatternData;

        /// <summary>
        /// Checks if this features specify type is Loft data
        /// </summary>
        public bool IsLoftData => FeatureType == ModelFeatureType.LoftData;

        /// <summary>
        /// Checks if this features specify type is Lofted Bend data
        /// </summary>
        public bool IsLoftedBendData => FeatureType == ModelFeatureType.LoftedBendsData;

        /// <summary>
        /// Checks if this features specify type is Macro data
        /// </summary>
        public bool IsMacroData => FeatureType == ModelFeatureType.MacroData;

        /// <summary>
        /// Checks if this features specify type is Mirror Part data
        /// </summary>
        public bool IsMirrorPartData => FeatureType == ModelFeatureType.MirrorPartData;

        /// <summary>
        /// Checks if this features specify type is Mirror Pattern data
        /// </summary>
        public bool IsMirrorPatternData => FeatureType == ModelFeatureType.MirrorPatternData;

        /// <summary>
        /// Checks if this features specify type is Mirror Solid data
        /// </summary>
        public bool IsMirrorSolidData => FeatureType == ModelFeatureType.MirrorSolidData;

        /// <summary>
        /// Checks if this features specify type is Miter Flange data
        /// </summary>
        public bool IsMiterFlangeData => FeatureType == ModelFeatureType.MiterFlangeData;

        /// <summary>
        /// Checks if this features specify type is Motion Plot Axis data
        /// </summary>
        public bool IsMotionPlotAxisData => FeatureType == ModelFeatureType.MotionPlotAxisData;

        /// <summary>
        /// Checks if this features specify type is Motion Plot data
        /// </summary>
        public bool IsMotionPlotData => FeatureType == ModelFeatureType.MotionPlotData;

        /// <summary>
        /// Checks if this features specify type is Move Copy Body data
        /// </summary>
        public bool IsMoveCopyBodyData => FeatureType == ModelFeatureType.MoveCopyBodyData;

        /// <summary>
        /// Checks if this features specify type is One Bend data
        /// </summary>
        public bool IsOneBendData => FeatureType == ModelFeatureType.OneBendData;

        /// <summary>
        /// Checks if this features specify type is Parting Line data
        /// </summary>
        public bool IsPartingLineData => FeatureType == ModelFeatureType.PartingLineData;

        /// <summary>
        /// Checks if this features specify type is Parting Surface data
        /// </summary>
        public bool IsPartingSurfaceData => FeatureType == ModelFeatureType.PartingSurfaceData;

        /// <summary>
        /// Checks if this features specify type is Projection Curve data
        /// </summary>
        public bool IsProjectionCurveData => FeatureType == ModelFeatureType.ProjectionCurveData;

        /// <summary>
        /// Checks if this features specify type is Reference Axis data
        /// </summary>
        public bool IsReferenceAxisData => FeatureType == ModelFeatureType.ReferenceAxisData;

        /// <summary>
        /// Checks if this features specify type is Reference Plane data
        /// </summary>
        public bool IsReferencePlaneData => FeatureType == ModelFeatureType.ReferencePlaneData;

        /// <summary>
        /// Checks if this features specify type is Reference Point Curve data
        /// </summary>
        public bool IsReferencePointCurveData => FeatureType == ModelFeatureType.ReferencePointCurveData;

        /// <summary>
        /// Checks if this features specify type is Reference Point data
        /// </summary>
        public bool IsReferencePointData => FeatureType == ModelFeatureType.ReferencePointData;

        /// <summary>
        /// Checks if this features specify type is Replace Face data
        /// </summary>
        public bool IsReplaceFaceData => FeatureType == ModelFeatureType.ReplaceFaceData;

        /// <summary>
        /// Checks if this features specify type is Revolve data
        /// </summary>
        public bool IsRevolveData => FeatureType == ModelFeatureType.RevolveData;

        /// <summary>
        /// Checks if this features specify type is Rib data
        /// </summary>
        public bool IsRibData => FeatureType == ModelFeatureType.RibData;

        /// <summary>
        /// Checks if this features specify type is Rip data
        /// </summary>
        public bool IsRipData => FeatureType == ModelFeatureType.RipData;

        /// <summary>
        /// Checks if this features specify type is Save Body data
        /// </summary>
        public bool IsSaveBodyData => FeatureType == ModelFeatureType.SaveBodyData;

        /// <summary>
        /// Checks if this features specify type is Scale data
        /// </summary>
        public bool IsScaleData => FeatureType == ModelFeatureType.ScaleData;

        /// <summary>
        /// Checks if this features specify type is Sheet Metal data
        /// </summary>
        public bool IsSheetMetalData => FeatureType == ModelFeatureType.SheetMetalData;

        /// <summary>
        /// Checks if this features specify type is Sheet Metal Gusset data
        /// </summary>
        public bool IsSheetMetalGussetData => FeatureType == ModelFeatureType.SheetMetalGussetData;

        /// <summary>
        /// Checks if this features specify type is Shell data
        /// </summary>
        public bool IsShellData => FeatureType == ModelFeatureType.ShellData;

        /// <summary>
        /// Checks if this features specify type is Shut Off Surface data
        /// </summary>
        public bool IsShutOffSurfaceData => FeatureType == ModelFeatureType.ShutOffSurfaceData;

        /// <summary>
        /// Checks if this features specify type is Simple Fillet data
        /// </summary>
        public bool IsSimpleFilletData => FeatureType == ModelFeatureType.SimpleFilletData;

        /// <summary>
        /// Checks if this features specify type is Simple Hole data
        /// </summary>
        public bool IsSimpleHoleData => FeatureType == ModelFeatureType.SimpleHoleData;

        /// <summary>
        /// Checks if this features specify type is Simulation 3D Contact data
        /// </summary>
        public bool IsSimulation3DContactData => FeatureType == ModelFeatureType.Simulation3DContactData;

        /// <summary>
        /// Checks if this features specify type is Simulation Damper data
        /// </summary>
        public bool IsSimulationDamperData => FeatureType == ModelFeatureType.SimulationDamperData;

        /// <summary>
        /// Checks if this features specify type is Simulation Force data
        /// </summary>
        public bool IsSimulationForceData => FeatureType == ModelFeatureType.SimulationForceData;

        /// <summary>
        /// Checks if this features specify type is Simulation Gravity data
        /// </summary>
        public bool IsSimulationGravityData => FeatureType == ModelFeatureType.SimulationGravityData;

        /// <summary>
        /// Checks if this features specify type is Simulation Linear Spring data
        /// </summary>
        public bool IsSimulationLinearSpringData => FeatureType == ModelFeatureType.SimulationLinearSpringData;

        /// <summary>
        /// Checks if this features specify type is Simulation Motor data
        /// </summary>
        public bool IsSimulationMotorData => FeatureType == ModelFeatureType.SimulationMotorData;

        /// <summary>
        /// Checks if this features specify type is Sketched Bend data
        /// </summary>
        public bool IsSketchedBendData => FeatureType == ModelFeatureType.SketchedBendData;

        /// <summary>
        /// Checks if this features specify type is Sketch Pattern data
        /// </summary>
        public bool IsSketchPatternData => FeatureType == ModelFeatureType.SketchPatternData;

        /// <summary>
        /// Checks if this features specify type is Smart Component data
        /// </summary>
        public bool IsSmartComponentData => FeatureType == ModelFeatureType.SmartComponentFeatureData;

        /// <summary>
        /// Checks if this features specify type is Split Body data
        /// </summary>
        public bool IsSplitBodyData => FeatureType == ModelFeatureType.SplitBodyData;

        /// <summary>
        /// Checks if this features specify type is Split Line data
        /// </summary>
        public bool IsSplitLineData => FeatureType == ModelFeatureType.SplitLineData;

        /// <summary>
        /// Checks if this features specify type is Surface Cut data
        /// </summary>
        public bool IsSurfaceCutData => FeatureType == ModelFeatureType.SurfaceCutData;

        /// <summary>
        /// Checks if this features specify type is Surface Extend data
        /// </summary>
        public bool IsSurfaceExtendData => FeatureType == ModelFeatureType.SurfaceExtendData;

        /// <summary>
        /// Checks if this features specify type is Surface Extrude data
        /// </summary>
        public bool IsSurfaceExtrudeData => FeatureType == ModelFeatureType.SurfaceExtrudeData;

        /// <summary>
        /// Checks if this features specify type is Surface Fill data
        /// </summary>
        public bool IsSurfaceFillData => FeatureType == ModelFeatureType.SurfaceFillData;

        /// <summary>
        /// Checks if this features specify type is Surface Flatten data
        /// </summary>
        public bool IsSurfaceFlattenData => FeatureType == ModelFeatureType.SurfaceFlattenData;

        /// <summary>
        /// Checks if this features specify type is Surface Knit data
        /// </summary>
        public bool IsSurfaceKnitData => FeatureType == ModelFeatureType.SurfaceKnitData;

        /// <summary>
        /// Checks if this features specify type is Surface Offset data
        /// </summary>
        public bool IsSurfaceOffsetData => FeatureType == ModelFeatureType.SurfaceOffsetData;

        /// <summary>
        /// Checks if this features specify type is Surface Planar data
        /// </summary>
        public bool IsSurfacePlanarData => FeatureType == ModelFeatureType.SurfacePlanarData;

        /// <summary>
        /// Checks if this features specify type is Surface Radiate data
        /// </summary>
        public bool IsSurfaceRadiateData => FeatureType == ModelFeatureType.SurfaceRadiateData;

        /// <summary>
        /// Checks if this features specify type is Surface Revolve data
        /// </summary>
        public bool IsSurfaceRevolveData => FeatureType == ModelFeatureType.SurfaceRevolveData;

        /// <summary>
        /// Checks if this features specify type is Surface Ruled data
        /// </summary>
        public bool IsSurfaceRuledData => FeatureType == ModelFeatureType.SurfaceRuledData;

        /// <summary>
        /// Checks if this features specify type is Surface Trim data
        /// </summary>
        public bool IsSurfaceTrimData => FeatureType == ModelFeatureType.SurfaceTrimData;

        /// <summary>
        /// Checks if this features specify type is Sweep data
        /// </summary>
        public bool IsSweepData => FeatureType == ModelFeatureType.SweepData;

        /// <summary>
        /// Checks if this features specify type is Table Pattern data
        /// </summary>
        public bool IsTablePatternData => FeatureType == ModelFeatureType.TablePatternData;

        /// <summary>
        /// Checks if this features specify type is Thicken data
        /// </summary>
        public bool IsThickenData => FeatureType == ModelFeatureType.ThickenData;

        /// <summary>
        /// Checks if this features specify type is Tooling Split data
        /// </summary>
        public bool IsToolingSplitData => FeatureType == ModelFeatureType.ToolingSplitData;

        /// <summary>
        /// Checks if this features specify type is Variable Fillet data
        /// </summary>
        public bool IsVariableFilletData => FeatureType == ModelFeatureType.VariableFilletData;

        /// <summary>
        /// Checks if this features specify type is Weldment Bead data
        /// </summary>
        public bool IsWeldmentBeadData => FeatureType == ModelFeatureType.WeldmentBeadData;

        /// <summary>
        /// Checks if this features specify type is Weldment Cut List data
        /// </summary>
        public bool IsWeldmentCutListData => FeatureType == ModelFeatureType.WeldmentCutListData;

        /// <summary>
        /// Checks if this features specify type is Weldment Member data
        /// </summary>
        public bool IsWeldmentMemberData => FeatureType == ModelFeatureType.WeldmentMemberData;

        /// <summary>
        /// Checks if this features specify type is Weldment Trim Extend data
        /// </summary>
        public bool IsWeldmentTrimExtendData => FeatureType == ModelFeatureType.WeldmentTrimExtendData;

        /// <summary>
        /// Checks if this features specify type is Wrap Sketch data
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
            return mBaseObject.SetSuppression2((int)state, (int)configurationOption, configurationNames);
        }

        /// <summary>
        /// Gets whether the feature in the specified configurations is suppressed
        /// </summary>
        /// <param name="configurationOption">Configuration option as defined in <see cref="ModelConfigurationOptions"/></param>
        /// <param name="configurationNames">Array of configuration names</param>
        /// <returns>Array of Booleans indicating the suppression states for the feature in the specified configurations</returns>
        public bool[] IsSuppressed(ModelConfigurationOptions configurationOption, string[] configurationNames = null)
        {
            return (bool[])mBaseObject.IsSuppressed2((int)configurationOption, configurationNames);
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
