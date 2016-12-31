namespace AngelSix.SolidDna
{
    /// <summary>
    /// Represents the specific feature type of a <see cref="ModelFeature"/>
    /// 
    /// NOTE: Known types are here http://help.solidworks.com/2016/english/api/sldworksapi/SolidWorks.Interop.sldworks~SolidWorks.Interop.sldworks.IFeature~GetTypeName2.html
    /// 
    /// </summary>
    public enum ModelFeatureType
    {
        /// <summary>
        /// For a feature that cannot have any more information extracted about it
        /// </summary>
        None,

        /// <summary>
        /// The feature is <see cref="FeatureSketch"/>
        /// </summary>
        Sketch,

        /// <summary>
        /// The feature is <see cref="ModelFeature"/> itself (such as an assembly as an InContextFeatureHolder)
        /// </summary>
        Feature,

        /// <summary>
        /// The feature is <see cref="FeatureMate"/>
        /// </summary>
        Mate,

        /// <summary>
        /// The feature is <see cref="FeatureMateReference"/>
        /// </summary>
        MateReference,

        /// <summary>
        /// The feature is <see cref="FeatureSmartComponentData"/>
        /// </summary>
        SmartComponentFeatureData,

        /// <summary>
        /// The feature is <see cref="FeatureFillPatternData"/>
        /// </summary>
        FillPatternData,

        /// <summary>
        /// The feature is <see cref="FeatureExtrudeData"/>
        /// </summary>
        ExtrudeData,

        /// <summary>
        /// The feature is <see cref="FeatureLoftData"/>
        /// </summary>
        LoftData,

        /// <summary>
        /// The feature is <see cref="FeatureChamferData"/>
        /// </summary>
        ChamferData,

        /// <summary>
        /// The feature is <see cref="FeatureCircularPatternData"/>
        /// </summary>
        CircularPatternData,

        /// <summary>
        /// The feature is <see cref="FeatureCombineBodiesData"/>
        /// </summary>
        CombineBodiesData,

        /// <summary>
        /// The feature is <see cref="FeatureCosmeticThreadData"/>
        /// </summary>
        CosmeticThreadData,

        /// <summary>
        /// The feature is <see cref="FeatureSaveBodyData"/>
        /// </summary>
        SaveBodyData,

        /// <summary>
        /// The feature is <see cref="FeatureCurveDrivenPatternData"/>
        /// </summary>
        CurveDrivenPatternData,

        /// <summary>
        /// The feature is <see cref="FeatureDeleteBodyData"/>
        /// </summary>
        DeleteBodyData,

        /// <summary>
        /// The feature is <see cref="FeatureDeleteFaceData"/>
        /// </summary>
        DeleteFaceData,

        /// <summary>
        /// The feature is <see cref="FeatureDerivedPatternData"/>
        /// </summary>
        DerivedPatternData,

        /// <summary>
        /// The feature is <see cref="FeatureDimPatternData"/>
        /// </summary>
        DimPatternData,

        /// <summary>
        /// The feature is <see cref="FeatureDomeData"/>
        /// </summary>
        DomeData,

        /// <summary>
        /// The feature is <see cref="FeatureDraftData"/>
        /// </summary>
        DraftData,

        /// <summary>
        /// The feature is <see cref="FeatureHealEdgesData"/>
        /// </summary>
        HealEdgesData,

        /// <summary>
        /// The feature is <see cref="FeatureWrapSketchData"/>
        /// </summary>
        WrapSketchData,

        /// <summary>
        /// The feature is <see cref="FeatureSimpleFilletData"/>
        /// </summary>
        SimpleFilletData,

        /// <summary>
        /// The feature is <see cref="FeatureHelixData"/>
        /// </summary>
        HelixData,

        /// <summary>
        /// The feature is <see cref="FeatureHoleSeriesData"/>
        /// </summary>
        HoleSeriesData,

        /// <summary>
        /// The feature is <see cref="FeatureHoleWizardData"/>
        /// </summary>
        HoleWizardData,

        /// <summary>
        /// The feature is <see cref="FeatureChainPatternData"/>
        /// </summary>
        ChainPatternData,

        /// <summary>
        /// The feature is <see cref="FeatureLocalCircularPatternData"/>
        /// </summary>
        LocalCircularPatternData,

        /// <summary>
        /// The feature is <see cref="FeatureLocalCurvePatternData"/>
        /// </summary>
        LocalCurvePatternData,

        /// <summary>
        /// The feature is <see cref="FeatureLocalLinearPatternData"/>
        /// </summary>
        LocalLinearPatternData,

        /// <summary>
        /// The feature is <see cref="FeatureLinearPatternData"/>
        /// </summary>
        LinearPatternData,

        /// <summary>
        /// The feature is <see cref="FeatureMacroData"/>
        /// </summary>
        MacroData,

        /// <summary>
        /// The feature is <see cref="FeatureMirrorPatternData"/>
        /// </summary>
        MirrorPatternData,

        /// <summary>
        /// This feature is <see cref="FeatureMirrorSolidData"/>
        /// </summary>
        MirrorSolidData,

        /// <summary>
        /// This feature is <see cref="FeatureMirrorPartData"/>
        /// </summary>
        MirrorPartData,

        /// <summary>
        /// This feature is <see cref="FeatureMoveCopyBodyData"/>
        /// </summary>
        MoveCopyBodyData,

        /// <summary>
        /// This feature is <see cref="FeatureBoundaryBossData"/>
        /// </summary>
        BoundaryBossData,

        /// <summary>
        /// This feature is <see cref="FeatureIndentData"/>
        /// </summary>
        IndentData,

        /// <summary>
        /// This feature is <see cref="FeatureReplaceFaceData"/>
        /// </summary>
        ReplaceFaceData,

        /// <summary>
        /// This feature is <see cref="FeatureRevolveData"/>
        /// </summary>
        RevolveData,

        /// <summary>
        /// This feature is <see cref="FeatureRibData"/>
        /// </summary>
        RibData,

        /// <summary>
        /// This feature is <see cref="FeatureRipData"/>
        /// </summary>
        RipData,

        /// <summary>
        /// This feature is <see cref="FeatureIntersectData"/>
        /// </summary>
        IntersectData,

        /// <summary>
        /// This feature is <see cref="FeatureShellData"/>
        /// </summary>
        ShellData,

        /// <summary>
        /// This feature is <see cref="FeatureSplitBodyData"/>
        /// </summary>
        SplitBodyData,

        /// <summary>
        /// This feature is <see cref="FeatureDerivedPartData"/>
        /// </summary>
        DerivedPartData,

        /// <summary>
        /// This feature is <see cref="FeatureSweepData"/>
        /// </summary>
        SweepData,

        /// <summary>
        /// This feature is <see cref="FeatureTablePatternData"/>
        /// </summary>
        TablePatternData,

        /// <summary>
        /// This feature is <see cref="FeatureThickenData"/>
        /// </summary>
        ThickenData,

        /// <summary>
        /// This feature is <see cref="FeatureVariableFilletData"/>
        /// </summary>
        VariableFilletData,

        /// <summary>
        /// This feature is <see cref="FeatureTableAnchor"/>
        /// </summary>
        TableAnchor,

        /// <summary>
        /// This feature is <see cref="FeatureBom"/>
        /// </summary>
        Bom,

        /// <summary>
        /// This feature is <see cref="FeatureDetailCircle"/>
        /// </summary>
        DetailCircle,

        /// <summary>
        /// This feature is <see cref="FeatureDrSection"/>
        /// </summary>
        DrSection,

        /// <summary>
        /// This feature is <see cref="FeatureBrokenOutSectionData"/>
        /// </summary>
        BrokenOutSectionData,

        /// <summary>
        /// This feature is <see cref="FeatureRefPlane"/>
        /// </summary>
        ReferencePlane,

        /// <summary>
        /// This feature is <see cref="FeatureCommentFolder"/>
        /// </summary>
        CommentFolder,

        /// <summary>
        /// This feature is <see cref="FeatureCosmeticWeldBeadFolder"/>
        /// </summary>
        CosmeticWeldBeadFolder,

        /// <summary>
        /// This feature is <see cref="FeatureBodyFolder"/>
        /// </summary>
        BodyFolder,

        /// <summary>
        /// This feature is <see cref="FeatureFeatureFolder"/>
        /// </summary>
        FeatureFolder,

        /// <summary>
        /// This feature is <see cref="FeatureAttribute"/>
        /// </summary>
        Attribute,

        /// <summary>
        /// This feature is <see cref="FeatureFreePointCurveData"/>
        /// </summary>
        FreePointCurveData,

        /// <summary>
        /// This feature is <see cref="FeatureLibraryFeatureData"/>
        /// </summary>
        LibraryFeatureData,

        /// <summary>
        /// This feature is <see cref="FeatureScaleData"/>
        /// </summary>
        ScaleData,

        /// <summary>
        /// This feature is <see cref="FeatureSensor"/>
        /// </summary>
        Sensor,

        /// <summary>
        /// This feature is <see cref="FeatureCavityData"/>
        /// </summary>
        CavityData,

        /// <summary>
        /// This feature is <see cref="FeatureToolingSplitData"/>
        /// </summary>
        ToolingSplitData,

        /// <summary>
        /// This feature is <see cref="FeaturePartingSurfaceData"/>
        /// </summary>
        PartingSurfaceData,

        /// <summary>
        /// This feature is <see cref="FeaturePartingLineData"/>
        /// </summary>
        PartingLineData,

        /// <summary>
        /// This feature is <see cref="FeatureShutOffSurfaceData"/>
        /// </summary>
        ShutOffSurfaceData,

        /// <summary>
        /// This feature is <see cref="FeatureCoreData"/>
        /// </summary>
        CoreData,

        /// <summary>
        /// This feature is <see cref="FeatureSimulation3DContactData"/>
        /// </summary>
        Simulation3DContactData,

        /// <summary>
        /// This feature is <see cref="FeatureSimulationGravityData"/>
        /// </summary>
        SimulationGravityData,

        /// <summary>
        /// This feature is <see cref="FeatureSimulationDamperData"/>
        /// </summary>
        SimulationDamperData,

        /// <summary>
        /// This feature is <see cref="FeatureSimulationMotorData"/>
        /// </summary>
        SimulationMotorData,

        /// <summary>
        /// This feature is <see cref="FeatureSimulationLinearSpringData"/>
        /// </summary>
        SimulationLinearSpringData,

        /// <summary>
        /// This feature is <see cref="FeatureSimulationForceData"/>
        /// </summary>
        SimulationForceData,

        /// <summary>
        /// This feature is <see cref="FeatureMotionPlotData"/>
        /// </summary>
        MotionPlotData,

        /// <summary>
        /// This feature is <see cref="FeatureMotionPlotAxisData"/>
        /// </summary>
        MotionPlotAxisData,

        /// <summary>
        /// This feature is <see cref="FeatureMotionStudyResults"/>
        /// </summary>
        MotionStudyResults,

        /// <summary>
        /// This feature is <see cref="FeatureCoordinateSystemData"/>
        /// </summary>
        CoordinateSystemData,

        /// <summary>
        /// This feature is <see cref="FeatureRefAxis"/>
        /// </summary>
        ReferenceAxis,

        /// <summary>
        /// This feature is <see cref="FeatureReferenceAxisData"/>
        /// </summary>
        ReferenceAxisData,

        /// <summary>
        /// This feature is <see cref="FeatureReferencePlaneData"/>
        /// </summary>
        ReferencePlaneData,

        /// <summary>
        /// This feature is <see cref="FeatureLight"/>
        /// </summary>
        Light,

        /// <summary>
        /// This feature is <see cref="FeatureCamera"/>
        /// </summary>
        Camera,

        /// <summary>
        /// This feature is <see cref="FeatureBaseFlangeData"/>
        /// </summary>
        BaseFlangeData,

        /// <summary>
        /// This feature is <see cref="FeatureBreakCornerData"/>
        /// </summary>
        BreakCornerData,

        /// <summary>
        /// This feature is <see cref="FeatureCrossBreakData"/>
        /// </summary>
        CrossBreakData,

        /// <summary>
        /// This feature is <see cref="FeatureEdgeFlangeData"/>
        /// </summary>
        EdgeFlangeData,

        /// <summary>
        /// This feature is <see cref="FeatureFlatPatternData"/>
        /// </summary>
        FlatPatternData,

        /// <summary>
        /// This feature is <see cref="FeatureBendsData"/>
        /// </summary>
        BendsData,

        /// <summary>
        /// This feature is <see cref="FeatureFoldsData"/>
        /// </summary>
        FoldsData,

        /// <summary>
        /// This feature is <see cref="FeatureHemData"/>
        /// </summary>
        HemData,

        /// <summary>
        /// This feature is <see cref="FeatureJogData"/>
        /// </summary>
        JogData,

        /// <summary>
        /// This feature is <see cref="FeatureLoftedBendsData"/>
        /// </summary>
        LoftedBendsData,

        /// <summary>
        /// This feature is <see cref="FeatureOneBendData"/>
        /// </summary>
        OneBendData,

        /// <summary>
        /// This feature is <see cref="FeatureSheetMetalData"/>
        /// </summary>
        SheetMetalData,

        /// <summary>
        /// This feature is <see cref="FeatureSketchedBendData"/>
        /// </summary>
        SketchedBendData,

        /// <summary>
        /// This feature is <see cref="FeatureSheetMetalGussetData"/>
        /// </summary>
        SheetMetalGussetData,

        /// <summary>
        /// This feature is <see cref="FeatureMiterFlangeData"/>
        /// </summary>
        MiterFlangeData,

        /// <summary>
        /// This feature is <see cref="FeatureReferenceCurve"/>
        /// </summary>
        ReferenceCurve,

        /// <summary>
        /// This feature is <see cref="FeatureReferencePointCurveData"/>
        /// </summary>
        ReferencePointCurveData,

        /// <summary>
        /// This feature is <see cref="FeatureCompositeCurveData"/>
        /// </summary>
        CompositeCurveData,

        /// <summary>
        /// This feature is <see cref="FeatureImportedCurveData"/>
        /// </summary>
        ImportedCurveData,

        /// <summary>
        /// This feature is <see cref="FeatureSplitLineData"/>
        /// </summary>
        SplitLineData,

        /// <summary>
        /// This feature is <see cref="FeatureProjectionCurveData"/>
        /// </summary>
        ProjectionCurveData,

        /// <summary>
        /// This feature is <see cref="FeatureRefPoint"/>
        /// </summary>
        ReferencePoint,

        /// <summary>
        /// This feature is <see cref="FeatureReferencePointData"/>
        /// </summary>
        ReferencePointData,

        /// <summary>
        /// This feature is <see cref="FeatureSketchBlockDefinition"/>
        /// </summary>
        SketchBlockDefinition,

        /// <summary>
        /// This feature is <see cref="FeatureSketchBlockInstance"/>
        /// </summary>
        SketchBlockInstance,

        /// <summary>
        /// This feature is <see cref="FeatureSimpleHoleData"/>
        /// </summary>
        SimpleHoleData,

        /// <summary>
        /// This feature is <see cref="FeatureSketchPatternData"/>
        /// </summary>
        SketchPatternData,

        /// <summary>
        /// This feature is <see cref="FeatureSketchPicture"/>
        /// </summary>
        SketchPicture,

        /// <summary>
        /// This feature is <see cref="FeatureSurfaceExtendData"/>
        /// </summary>
        SurfaceExtendData,

        /// <summary>
        /// This feature is <see cref="FeatureSurfaceExtrudeData"/>
        /// </summary>
        SurfaceExtrudeData,

        /// <summary>
        /// This feature is <see cref="FeatureSurfaceFillData"/>
        /// </summary>
        SurfaceFillData,

        /// <summary>
        /// This feature is <see cref="FeatureSurfaceFlattenData"/>
        /// </summary>
        SurfaceFlattenData,

        /// <summary>
        /// This feature is <see cref="FeatureSurfaceMid"/>
        /// </summary>
        SurfaceMid,

        /// <summary>
        /// This feature is <see cref="FeatureSurfaceOffsetData"/>
        /// </summary>
        SurfaceOffsetData,

        /// <summary>
        /// This feature is <see cref="FeatureSurfacePlanarData"/>
        /// </summary>
        SurfacePlanarData,

        /// <summary>
        /// This feature is <see cref="FeatureSurfaceRadiateData"/>
        /// </summary>
        SurfaceRadiateData,

        /// <summary>
        /// This feature is <see cref="FeatureSurfaceRevolveData"/>
        /// </summary>
        SurfaceRevolveData,

        /// <summary>
        /// This feature is <see cref="FeatureSurfaceRuledData"/>
        /// </summary>
        SurfaceRuledData,

        /// <summary>
        /// This feature is <see cref="FeatureSurfaceKnitData"/>
        /// </summary>
        SurfaceKnitData,

        /// <summary>
        /// This feature is <see cref="FeatureSurfaceCutData"/>
        /// </summary>
        SurfaceCutData,

        /// <summary>
        /// This feature is <see cref="FeatureSurfaceTrimData"/>
        /// </summary>
        SurfaceTrimData,

        /// <summary>
        /// This feature is <see cref="FeatureCosmeticWeldBeadData"/>
        /// </summary>
        CosmeticWeldBeadData,

        /// <summary>
        /// This feature is <see cref="FeatureEndCapData"/>
        /// </summary>
        EndCapData,

        /// <summary>
        /// This feature is <see cref="FeatureGussetData"/>
        /// </summary>
        GussetData,

        /// <summary>
        /// This feature is <see cref="FeatureWeldmentBeadData"/>
        /// </summary>
        WeldmentBeadData,

        /// <summary>
        /// This feature is <see cref="FeatureWeldmentTrimExtendData"/>
        /// </summary>
        WeldmentTrimExtendData,

        /// <summary>
        /// This feature is <see cref="FeatureWeldmentMemberData"/>
        /// </summary>
        WeldmentMemberData,

        /// <summary>
        /// This feature is <see cref="FeatureWeldmentCutListData"/>
        /// </summary>
        WeldmentCutListData,

        /// <summary>
        /// This feature does not have an interface
        /// </summary>
        Flex,

        /// <summary>
        /// This feature does not have an interface
        /// </summary>
        Deform,

        /// <summary>
        /// This feature does not have an interface
        /// </summary>
        Imported,

        /// <summary>
        /// This feature does not have an interface
        /// </summary>
        Grid,

        /// <summary>
        /// This feature does not have an interface
        /// </summary>
        TorsionalSpring,

        /// <summary>
        /// This feature does not have an interface
        /// </summary>
        FormTool,

        /// <summary>
        /// This feature does not have an interface
        /// </summary>
        SurfaceLoft,

        /// <summary>
        /// This feature does not have an interface
        /// </summary>
        SurfaceImported,

        /// <summary>
        /// This feature does not have an interface
        /// </summary>
        SurfaceSweep,

        /// <summary>
        /// This feature does not have an interface
        /// </summary>
        SurfaceUntrim,

        /// <summary>
        /// This feature does not have an interface
        /// </summary>
        SplitBody,

        /// <summary>
        /// This feature is obsolete and has no interface
        /// </summary>
        BlockFolder,

        /// <summary>
        /// This feature is obsolete and has no interface
        /// </summary>
        BlockDef,

        /// <summary>
        /// This feature is obsolete and has no interface
        /// </summary>
        Shape,

        /// <summary>
        /// This feature is obsolete and has no interface
        /// </summary>
        ViewBodyFeature,
    }
}
