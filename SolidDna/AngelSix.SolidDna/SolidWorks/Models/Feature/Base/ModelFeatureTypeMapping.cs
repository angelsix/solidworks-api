using SolidWorks.Interop.sldworks;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Maps SolidWorks model Features to their specific types
    /// </summary>
    public static class ModelFeatureTypeMapping
    {
        public static ModelFeatureType SpecificFeatureType(this ModelFeature feature)
        {
            // Get type name
            var type = feature?.FeatureTypeName;

            // Make sure we have one
            if (feature == null || string.IsNullOrEmpty(type))
                return ModelFeatureType.None;

            // Map to feature types based on this list
            // http://help.solidworks.com/2020/english/api/sldworksapi/SolidWorks.Interop.sldworks~SolidWorks.Interop.sldworks.IFeature~GetTypeName2.html

            switch (type)
            {
                #region Assembly

                // NOTE: No interface
                case "AsmExploder":
                    return ModelFeatureType.AssemblyExplodedView;

                // NOTE: No interface
                case "CompExplodeStep":
                    return ModelFeatureType.AssemblyExplodeStep;

                case "ExplodeLineProfileFeature":
                    return ModelFeatureType.Sketch;

                case "InContextFeatHolder":
                case "MagneticGroundPlane":
                    return ModelFeatureType.Feature;

                case "MateCamTangent":
                    return ModelFeatureType.CamFollowerMateData;

                case "MateCoincident":
                    return VersionYear < 2019 ? ModelFeatureType.Mate : ModelFeatureType.CoincidentMateData;

                case "MateConcentric":
                    return VersionYear < 2019 ? ModelFeatureType.Mate : ModelFeatureType.ConcentricMateData;
                
                case "MateDistanceDim":
                    return VersionYear < 2018 ? ModelFeatureType.Mate : ModelFeatureType.DistanceMateData;
                
                case "MateGearDim":
                    return ModelFeatureType.GearMateData;

                case "MateGroup":
                    return ModelFeatureType.MateGroup;

                case "MateHinge":
                    return ModelFeatureType.HingeMateData;
                
                case "MateInPlace":
                    return ModelFeatureType.Mate;

                case "MateLinearCoupler":
                    return ModelFeatureType.LinearCouplerMateData;

                case "MateLock":
                    return ModelFeatureType.LockMateData;

                case "MateParallel":
                    return VersionYear < 2019 ? ModelFeatureType.Mate : ModelFeatureType.ParallelMateData;

                case "MatePerpendicular":
                    return VersionYear < 2019 ? ModelFeatureType.Mate : ModelFeatureType.PerpendicularMateData;

                case "MatePlanarAngleDim":
                    return VersionYear < 2018 ? ModelFeatureType.Mate : ModelFeatureType.AngleMateData;

                case "MateProfileCenter":
                    return ModelFeatureType.ProfileCenterMateData;

                case "MateRackPinionDim":
                    return ModelFeatureType.RackPinionMateData;

                case "MateScrew":
                    return ModelFeatureType.ScrewMateData;

                case "MateSlot":
                    return ModelFeatureType.SlotMateData;

                case "MateSymmetric":
                    return VersionYear < 2018 ? ModelFeatureType.Mate : ModelFeatureType.SymmetricMateData;

                case "MateTangent":
                    return VersionYear < 2019 ? ModelFeatureType.Mate : ModelFeatureType.TangentMateData;
                
                case "MateUniversalJoint":
                    return ModelFeatureType.UniversalJointMateData;

                case "MateWidth":
                    return VersionYear < 2018 ? ModelFeatureType.Mate : ModelFeatureType.WidthMateData;

                case "Reference": // removed from the 2018 and later help page
                case "PosGroupFolder":
                    return ModelFeatureType.MateReference;

                case "SmartComponentFeature":
                    return ModelFeatureType.SmartComponentFeatureData;

                #endregion

                #region Body

                case "AdvHoleWzd":
                    return ModelFeatureType.AdvancedHoleWizardData;

                case "APattern":
                    return ModelFeatureType.FillPatternData;

                case "BaseBody":
                    return ModelFeatureType.ExtrudeData;

                // NOTE: No interface
                case "Bending":
                    return ModelFeatureType.Flex;

                case "Blend":
                case "BlendCut":
                    return ModelFeatureType.LoftData;

                // NOTE: No interface
                case "BodyExplodeStep":
                    return ModelFeatureType.MultiBodyPartExplodeStep;

                case "Boss":
                case "BossThin":
                    return ModelFeatureType.ExtrudeData;

                case "Chamfer":
                    return ModelFeatureType.ChamferData;

                case "CirPattern":
                    return ModelFeatureType.CircularPatternData;

                case "CombineBodies":
                    return ModelFeatureType.CombineBodiesData;

                case "CosmeticThread":
                    return ModelFeatureType.CosmeticThreadData;

                case "CreateAssemFeat":
                    return ModelFeatureType.SaveBodyData;

                case "CurvePattern":
                    return ModelFeatureType.CurveDrivenPatternData;

                case "Cut":
                case "CutThin":
                    return ModelFeatureType.ExtrudeData;

                // NOTE: No interface
                case "Deform":
                    return ModelFeatureType.Deform;

                case "DeleteBody":
                    return ModelFeatureType.DeleteBodyData;

                case "DelFace":
                    return ModelFeatureType.DeleteFaceData;

                case "DerivedCirPattern":
                case "DerivedLPattern":
                    return ModelFeatureType.DerivedPatternData;

                case "DimPattern":
                    return ModelFeatureType.DimPatternData;

                case "Dome":
                    return ModelFeatureType.DomeData;

                case "Draft":
                    return ModelFeatureType.DraftData;

                case "EdgeMerge":
                    return ModelFeatureType.HealEdgesData;

                case "Emboss":
                    return ModelFeatureType.WrapSketchData;

                case "Extrusion":
                    return ModelFeatureType.ExtrudeData;

                case "Fillet":
                    return ModelFeatureType.SimpleFilletData;

                case "Helix":
                    return ModelFeatureType.HelixData;

                case "HoleSeries":
                    return ModelFeatureType.HoleSeriesData;

                case "HoleWzd":
                    return ModelFeatureType.HoleWizardData;

                // NOTE: No interface
                case "Imported":
                    return ModelFeatureType.Imported;

                case "LocalChainPattern":
                    return ModelFeatureType.ChainPatternData;

                case "LocalCirPattern":
                    return ModelFeatureType.LocalCircularPatternData;

                case "LocalCurvePattern":
                    return ModelFeatureType.LocalCurvePatternData;

                case "LocalLPattern":
                    return ModelFeatureType.LocalLinearPatternData;

                case "LocalSketchPattern":
                    return ModelFeatureType.LocalSketchPatternData;

                case "LPattern":
                    return ModelFeatureType.LinearPatternData;

                case "MacroFeature":
                    return ModelFeatureType.MacroData;

                case "MirrorCompFeat":
                    return ModelFeatureType.MirrorComponentData;

                case "MirrorPattern":
                    return ModelFeatureType.MirrorPatternData;

                case "MirrorSolid":
                    return ModelFeatureType.MirrorSolidData;

                case "MirrorStock":
                    return ModelFeatureType.MirrorPartData;

                case "MoveCopyBody":
                    return ModelFeatureType.MoveCopyBodyData;

                case "NetBlend":
                    return ModelFeatureType.BoundaryBossData;

                // NOTE: No interface
                case "PrtExploder":
                    return ModelFeatureType.MultiBodyPartExplodedView;

                case "Punch":
                    return ModelFeatureType.IndentData;

                case "ReplaceFace":
                    return ModelFeatureType.ReplaceFaceData;

                case "RevCut":
                    return ModelFeatureType.RevolveData;

                case "Round fillet corner":
                    return ModelFeatureType.SimpleFilletData;

                case "Revolution":
                case "RevolutionThin":
                    return ModelFeatureType.RevolveData;

                case "Rib":
                    return ModelFeatureType.RibData;

                case "Rip":
                    return ModelFeatureType.RipData;

                case "Sculpt":
                    return ModelFeatureType.IntersectData;

                // NOTE: Obsolete and no interface
                case "Shape":
                    return ModelFeatureType.Shape;

                case "Shell":
                    return ModelFeatureType.ShellData;

                case "Split":
                    return ModelFeatureType.SplitBodyData;

                // NOTE: No interface; returned for a body created by splitting a part and saving the body to a part; you cannot access the data of a split body saved to a part
                case "SplitBody":
                    return ModelFeatureType.SplitBody;

                case "Stock":
                    return ModelFeatureType.DerivedPartData;

                case "Sweep":
                case "SweepCut":
                    return ModelFeatureType.SweepData;

                case "SweepThread":
                    return ModelFeatureType.ThreadData;

                case "TablePattern":
                    return ModelFeatureType.TablePatternData;

                case "Thicken":
                case "ThickenCut":
                    return ModelFeatureType.ThickenData;

                case "VarFillet":
                    return ModelFeatureType.VariableFilletData;

                #endregion

                #region Drawing

                case "BendTableAchor":
                    return ModelFeatureType.TableAnchor;

                case "BomFeat":
                    return ModelFeatureType.Bom;

                case "BomTemplate":
                    return ModelFeatureType.TableAnchor;

                case "DetailCircle":
                    return ModelFeatureType.DetailCircle;

                case "DrBreakoutSectionLine":

                    if (feature.SpecificFeature as IBrokenOutSectionFeatureData != null)
                        return ModelFeatureType.BrokenOutSectionData;
                    else if (feature.SpecificFeature as IDrSection != null)
                        return ModelFeatureType.DrSection;

                    return ModelFeatureType.None;

                case "DrSectionLine":
                    return ModelFeatureType.DrSection;

                case "GeneralTableAnchor":
                case "HoleTableAnchor":
                    return ModelFeatureType.TableAnchor;

                case "LiveSection":
                    return ModelFeatureType.ReferencePlane;

                case "PunchTableAnchor":
                case "RevisionTableAnchor":
                case "WeldmentTableAnchor":
                case "WeldTableAnchor":
                    return ModelFeatureType.TableAnchor;

                #endregion

                #region Folder

                // NOTE: Obsolete and no interface
                case "BlockFolder":
                    return ModelFeatureType.BlockFolder;

                case "CommentsFolder":
                    return ModelFeatureType.CommentFolder;

                case "CosmeticWeldSubFolder":
                    return ModelFeatureType.CosmeticWeldBeadFolder;

                case "CutListFolder":
                case "FeatSolidBodyFolder":
                case "FeatSurfaceBodyFolder":
                    return ModelFeatureType.BodyFolder;

                case "FtrFolder":
                case "InsertedFeatureFolder":
                case "MateReferenceGroupFolder":
                case "ProfileFtrFolder":
                case "RefAxisFtrFolder":
                case "RefPlaneFtrFolder":
                case "SketchSliceFolder":
                    return ModelFeatureType.FeatureFolder;

                case "SolidBodyFolder":
                case "SubAtomFolder":
                case "SubWeldFolder":
                case "SurfaceBodyFolder":

                    // As SubAtomFolder states "IBodyFolder if a body" we double check here
                    if (feature.SpecificFeature as IBodyFolder != null)
                        return ModelFeatureType.BodyFolder;
                    else
                        return ModelFeatureType.None;

                case "TemplateFlatPattern":
                    return ModelFeatureType.FlatPatternFolder;

                #endregion

                #region Imported file

                case "MBimport":
                    return ModelFeatureType.Import3DInterconnectData;
                
                #endregion

                #region Miscellaneous

                case "Attribute":
                    return ModelFeatureType.Attribute;

                // NOTE: Obsolete and no interface
                case "BlockDef":
                    return ModelFeatureType.BlockDef;

                case "CurveInFile":
                    return ModelFeatureType.FreePointCurveData;

                // NOTE: No interface
                case "GridFeature":
                    return ModelFeatureType.Grid;

                case "LibraryFeature":
                    return ModelFeatureType.LibraryFeatureData;

                case "Scale":
                    return ModelFeatureType.ScaleData;

                case "Sensor":
                    return ModelFeatureType.Sensor;

                // NOTE: Obsolete
                case "ViewBodyFeature":
                    return ModelFeatureType.ViewBodyFeature;

                #endregion

                #region Mold

                case "Cavity":
                    return ModelFeatureType.CavityData;

                case "MoldCoreCavitySolids":
                    return ModelFeatureType.ToolingSplitData;

                case "MoldPartingGeom":
                    return ModelFeatureType.PartingSurfaceData;

                case "MoldPartLine":
                    return ModelFeatureType.PartingLineData;

                case "MoldShutOffSrf":
                    return ModelFeatureType.ShutOffSurfaceData;

                case "SideCore":
                    return ModelFeatureType.CoreData;

                case "XformStock":
                    return ModelFeatureType.DerivedPartData;

                #endregion

                #region Motion and Simulation

                case "AEM3DContact":
                    return ModelFeatureType.Simulation3DContactData;

                case "AEMGravity":
                    return ModelFeatureType.SimulationGravityData;

                case "AEMLinearDamper":
                    return ModelFeatureType.SimulationDamperData;

                case "AEMLinearMotor":
                    return ModelFeatureType.SimulationMotorData;

                case "AEMLinearSpring":
                    return ModelFeatureType.SimulationLinearSpringData;

                case "AEMRotationalMotor":
                    return ModelFeatureType.SimulationMotorData;

                case "AEMTorque":
                    return ModelFeatureType.SimulationForceData;

                case "AEMTorsionalDamper":
                    return ModelFeatureType.SimulationDamperData;

                // NOTE: No interface
                case "AEMTorsionalSpring":
                    return ModelFeatureType.TorsionalSpring;

                case "SimPlotFeature":
                    return ModelFeatureType.MotionPlotData;

                case "SimPlotXAxisFeature":
                case "SimPlotYAxisFeature":
                    return ModelFeatureType.MotionPlotAxisData;

                case "SimResultFolder":
                    return ModelFeatureType.MotionStudyResults;

                #endregion

                #region Reference Geometry

                case "BoundingBox":
                    return ModelFeatureType.BoundingBoxData;

                case "CoordSys":
                    return ModelFeatureType.CoordinateSystemData;

                case "GroundPlane":
                    return ModelFeatureType.GroundPlaneData;

                case "RefAxis":

                    if (feature.SpecificFeature as IRefAxisFeatureData != null)
                        return ModelFeatureType.ReferenceAxisData;
                    else if (feature.SpecificFeature as IRefAxis != null)
                        return ModelFeatureType.ReferenceAxis;
                    else
                        return ModelFeatureType.None;

                case "RefPlane":
                    return ModelFeatureType.ReferencePlaneData;

                #endregion

                #region Scenes, Lights, Cameras

                case "AmbientLight":
                    return ModelFeatureType.Light;

                case "CameraFeature":
                    return ModelFeatureType.Camera;

                case "DirectionLight":
                case "PointLight":
                case "SpotLight":
                    return ModelFeatureType.Light;

                #endregion

                #region Sheet Metal

                case "SMBaseFlange":
                    return ModelFeatureType.BaseFlangeData;

                case "BreakCorner":
                case "CornerTrim":
                    return ModelFeatureType.BreakCornerData;

                case "CrossBreak":
                    return ModelFeatureType.CrossBreakData;

                case "EdgeFlange":
                    return ModelFeatureType.EdgeFlangeData;

                case "FlatPattern":
                    return ModelFeatureType.FlatPatternData;

                case "FlattenBends":
                    return ModelFeatureType.BendsData;

                case "Fold":
                    return ModelFeatureType.FoldsData;

                // NOTE: No interface
                case "FormToolInstance":
                    return ModelFeatureType.FormTool;

                case "Hem":
                    return ModelFeatureType.HemData;

                case "Jog":
                    return ModelFeatureType.JogData;

                case "LoftedBend":
                    return ModelFeatureType.LoftedBendsData;

                case "NormalCut":
                    return ModelFeatureType.NormalCutData;

                case "OneBend":
                    return ModelFeatureType.OneBendData;

                case "ProcessBends":
                    return ModelFeatureType.BendsData;

                case "SheetMetal":
                    return ModelFeatureType.SheetMetalData;

                case "SketchBend":
                    return ModelFeatureType.OneBendData;

                case "SM3dBend":
                    return ModelFeatureType.SketchedBendData;

                case "SMGusset":
                    return ModelFeatureType.SheetMetalGussetData;

                case "SMMiteredFlange":
                    return ModelFeatureType.MiterFlangeData;

                // According to the help, SolidWorks 2017 and newer return a SheetMetalFolder, even though that interface exists since 2014
                case "TemplateSheetMetal":
                    return VersionYear < 2017 ? ModelFeatureType.SheetMetalData : ModelFeatureType.SheetMetalFolder;

                case "ToroidalBend":
                    return ModelFeatureType.OneBendData;

                case "UnFold":
                    return ModelFeatureType.FoldsData;

                #endregion

                #region Sketch

                case "3DProfileFeature":
                    return ModelFeatureType.Sketch;

                case "3DSplineCurve":

                    if (feature.SpecificFeature as IReferencePointCurveFeatureData != null)
                        return ModelFeatureType.ReferencePointCurveData;
                    if (feature.SpecificFeature as IReferenceCurve != null)
                        return ModelFeatureType.ReferenceCurve;
                    else
                        return ModelFeatureType.None;

                case "CompositeCurve":

                    if (feature.SpecificFeature as ICompositeCurveFeatureData != null)
                        return ModelFeatureType.CompositeCurveData;
                    else if (feature.SpecificFeature as IReferenceCurve != null)
                        return ModelFeatureType.ReferenceCurve;
                    else
                        return ModelFeatureType.None;

                case "ImportedCurve":

                    if (feature.SpecificFeature as IImportedCurveFeatureData != null)
                        return ModelFeatureType.ImportedCurveData;
                    else if (feature.SpecificFeature as IReferenceCurve != null)
                        return ModelFeatureType.ReferenceCurve;
                    else
                        return ModelFeatureType.None;

                case "PLine":
                    return ModelFeatureType.SplitLineData;

                case "ProfileFeature":
                    return ModelFeatureType.Sketch;

                case "RefCurve":

                    if (feature.SpecificFeature as IProjectionCurveFeatureData != null)
                        return ModelFeatureType.ProjectionCurveData;
                    else if (feature.SpecificFeature as IReferenceCurve != null)
                        return ModelFeatureType.ReferenceCurve;
                    else
                        return ModelFeatureType.None;

                case "SketchBlockDef":
                    return ModelFeatureType.SketchBlockDefinition;

                case "SketchBlockInst":
                    return ModelFeatureType.SketchBlockInstance;

                case "SketchHole":
                    return ModelFeatureType.SimpleHoleData;

                case "SketchPattern":
                    return ModelFeatureType.SketchPatternData;

                case "SketchBitmap":
                    return ModelFeatureType.SketchPicture;

                #endregion

                #region Surface

                // NOTE: No interface
                case "BlendRefSurface":
                    return ModelFeatureType.SurfaceLoft;

                case "ExtendRefSurface":
                    return ModelFeatureType.SurfaceExtendData;

                case "ExtruRefSurface":
                    return ModelFeatureType.SurfaceExtrudeData;

                case "FillRefSurface":
                    return ModelFeatureType.SurfaceFillData;

                case "FlattenSurface":
                    return ModelFeatureType.SurfaceFlattenData;

                case "MidRefSurface":
                    return ModelFeatureType.SurfaceMid;

                case "OffsetRefSuface":
                    return ModelFeatureType.SurfaceOffsetData;

                case "PlanarSurface":
                    return ModelFeatureType.SurfacePlanarData;

                case "RadiateRefSurface":
                    return ModelFeatureType.SurfaceRadiateData;

                // NOTE: No interface
                case "RefSurface":
                    return ModelFeatureType.SurfaceImported;

                case "RevolvRefSurf":
                    return ModelFeatureType.SurfaceRevolveData;

                case "RuledSrfFromEdge":
                    return ModelFeatureType.SurfaceRuledData;

                case "SewRefSurface":
                    return ModelFeatureType.SurfaceKnitData;

                case "SurfCut":
                    return ModelFeatureType.SurfaceCutData;

                // NOTE: No interface until 2018
                case "SweepRefSurface":
                    return ModelFeatureType.SurfaceSweepData;

                case "TrimRefSurface":
                    return ModelFeatureType.SurfaceTrimData;

                // NOTE: No interface
                case "UnTrimRefSurf":
                    return ModelFeatureType.SurfaceUntrim;

                #endregion

                #region Weldment

                case "CosmeticWeldBead":
                    return ModelFeatureType.CosmeticWeldBeadData;

                case "EndCap":
                    return ModelFeatureType.EndCapData;

                case "Gusset":
                    return ModelFeatureType.GussetData;

                case "Weldment":
                    return ModelFeatureType.Weldment;

                case "WeldBeadFeat":
                    return ModelFeatureType.WeldmentBeadData;

                case "WeldCornerFeat":
                    return ModelFeatureType.WeldmentTrimExtendData;

                case "WeldMemberFeat":
                    return ModelFeatureType.WeldmentMemberData;

                case "WeldmentTableFeat":
                    return ModelFeatureType.WeldmentCutListData;

                #endregion

                default:
                    return ModelFeatureType.None;
            }
        }

        /// <summary>
        /// Helper property to get the SOLIDWORKS version year.
        /// If unknown will return -1.
        /// </summary>
        private static int VersionYear => SolidWorksEnvironment.Application.SolidWorksVersion.Version;
    }
}
