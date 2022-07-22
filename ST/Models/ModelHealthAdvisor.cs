using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST.Models
{
    public partial class ModelHealthAdvisor : DbContext
    {
        public ModelHealthAdvisor()
            : base("name=ModelHealthAdvisor")

        {
            this.Database.CommandTimeout = 300;//300 antes
            this.Configuration.LazyLoadingEnabled = false;
            //SelectPdf.GlobalProperties.LicenseKey = "WnFremhva3pja2p6aGp0anppa3RraHRjY2Nj";
        }

        public virtual DbSet<UserSystem> UserSystem { get; set; }
        public virtual DbSet<UserExternal> UserExternal { get; set; }
        public virtual DbSet<SystemParameter> SystemParameter { get; set; }
        public virtual DbSet<Profiles> Profiles { get; set; }

        public virtual DbSet<Pregunta> Pregunta { get; set; }
        public virtual DbSet<PreguntaDetalle> PreguntaDetalle { get; set; }

        public virtual DbSet<Meta> Meta { get; set; }
        public virtual DbSet<MetaDetalle> MetaDetalle { get; set; }

        public virtual DbSet<Categorias> Categorias { get; set; }
        public virtual DbSet<LocalesNacionales> LocalesNacionales { get; set; }

        public virtual DbSet<Encuestas> Encuestas { get; set; }
        public virtual DbSet<EncuestasDetalle> EncuestasDetalle { get; set; }        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<Analysis>().HasKey(p => p.anId);

            modelBuilder.Entity<Analysis>()
                .Property(c => c.anId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Analysis>()
                .Property(e => e.anName)
                .IsFixedLength();

            modelBuilder.Entity<Analysis>()
                .Property(e => e.anDescription)
                .IsFixedLength();


            modelBuilder.Entity<Analysis>()
                .Property(e => e.anTooltip)
                .IsUnicode(false);

            modelBuilder.Entity<Analysis>()
                .Property(e => e.anCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Analysis>()
                .Property(e => e.anLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Analysis>()
                .Property(e => e.anDeleterUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Analysis>()
                .HasMany(e => e.AnalysisFeature)
                .WithOptional(e => e.Analysis)
                .HasForeignKey(e => e.anfAnalysisId);

            modelBuilder.Entity<Analysis>()
                .HasMany(e => e.LaboratoryDataInput)
                .WithOptional(e => e.Analysis)
                .HasForeignKey(e => e.ldiAnalysisId);

            modelBuilder.Entity<Analysis>()
            .HasMany(e => e.ReferenceMethodsDetailByAnalysis)
            .WithRequired(e => e.Analysis)
            .HasForeignKey(e => e.refDetAnalysisId)
            .WillCascadeOnDelete(false);




            modelBuilder.Entity<Analysis>()
              .HasMany(e => e.Parameter)
              .WithRequired(e => e.Analysis)
              .HasForeignKey(e => e.parAnalysisId)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Analysis>()
               .HasMany(e => e.SamplingDetail)
               .WithRequired(e => e.Analysis)
               .HasForeignKey(e => e.samdAnalysisId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<AnalysisFeature>()
                .Property(e => e.anfName)
                .IsUnicode(false);

            modelBuilder.Entity<AnalysisFeature>()
                .Property(e => e.anfCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<AnalysisFeature>()
                .Property(e => e.anfLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<AnalysisFeature>()
                .Property(e => e.anfDeleterUserId)
                .IsUnicode(false);

            modelBuilder.Entity<AnalysisFeature>()
                          .HasMany(e => e.SamplingAnalysisFeaturesList)
                          .WithRequired(e => e.AnalysisFeature)
                          .HasForeignKey(e => e.cmmAnlFeatId)
                          .WillCascadeOnDelete(false);


            modelBuilder.Entity<AnalysisGroup>().HasKey(p => p.agrId);

            modelBuilder.Entity<AnalysisGroup>()
             .Property(c => c.agrId)
             .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<AnalysisGroup>()
                .Property(e => e.agrDescription)
                .IsUnicode(false);

            modelBuilder.Entity<AnalysisGroup>()
                .Property(e => e.agrCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<AnalysisGroup>()
                .Property(e => e.agrLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<AnalysisGroup>()
                .Property(e => e.agrDeleterUserId)
                .IsUnicode(false);

            modelBuilder.Entity<AnalysisGroup>()
                .HasMany(e => e.Analysis)
                .WithOptional(e => e.AnalysisGroup)
                .HasForeignKey(e => e.anGroupId);

            modelBuilder.Entity<AnalysisGroup>()
                .HasMany(e => e.AnalysisGroupFeature)
                .WithOptional(e => e.AnalysisGroup)
                .HasForeignKey(e => e.agfGroupId);

            modelBuilder.Entity<AnalysisGroup>()
             .HasMany(e => e.ReferenceMethodsDetail)
             .WithRequired(e => e.AnalysisGroup)
             .HasForeignKey(e => e.refDetAnalysisGroupId)
             .WillCascadeOnDelete(false);            

            modelBuilder.Entity<AnalysisGroup>()
                .HasMany(e => e.Sampling)
                .WithRequired(e => e.AnalysisGroup)
                .HasForeignKey(e => e.sampAnalysisGroupId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AnalysisGroupFeature>()
                .Property(e => e.agfName)
                .IsUnicode(false);

            modelBuilder.Entity<AnalysisGroupFeature>()
                .Property(e => e.agfCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<AnalysisGroupFeature>()
                .Property(e => e.agfLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<AnalysisGroupFeature>()
                .Property(e => e.agfDeleterUserId)
                .IsUnicode(false);


            modelBuilder.Entity<AnalysisGroupFeature>()
                .HasMany(e => e.SamplingAnalysisGroupFeaturesList)
                .WithOptional(e => e.AnalysisGroupFeature)
                .HasForeignKey(e => e.cmmAnlGrpFeatId);

            modelBuilder.Entity<ClientMaster>()
              .Property(e => e.ClientID)
              .IsUnicode(false);

            modelBuilder.Entity<ClientMaster>()
                .Property(e => e.ClientSecret)
                .IsUnicode(false);

            modelBuilder.Entity<ClientMaster>()
                .Property(e => e.ClientName)
                .IsUnicode(false);

            modelBuilder.Entity<ClientMaster>()
                .Property(e => e.AllowedOrigin)
                .IsUnicode(false);

            modelBuilder.Entity<Company>().HasKey(p => p.CompAquasymId);

            modelBuilder.Entity<Company>()
             .Property(c => c.cmpID)
             .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Company>()
                .Property(e => e.cmpName)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
             .Property(e => e.cmpM3_ID)
             .IsUnicode(false);

            modelBuilder.Entity<Company>()
                .Property(e => e.cmpSendEmailsReports)
                .IsUnicode(false);

            modelBuilder.Entity<Company>()
            .Property(e => e.cmpArea)
            .IsUnicode(false);

            modelBuilder.Entity<Company>()
               .HasMany(e => e.Sampling)
               .WithRequired(e => e.Company)
               .HasForeignKey(e => e.sampCompanyAquasimId)
               .WillCascadeOnDelete(false);



            modelBuilder.Entity<Company>()
              .HasMany(e => e.Site)
              .WithRequired(e => e.Company)
              .HasForeignKey(e => e.SitAquasimCompanyId)
              .WillCascadeOnDelete(false);



            modelBuilder.Entity<Country>().HasKey(p => p.CounAquasimID);

            modelBuilder.Entity<Country>()
             .Property(c => c.couID)
             .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Country>()
              .Property(e => e.couIso)
              .IsFixedLength()
              .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .Property(e => e.couName)
                .IsUnicode(false);


            modelBuilder.Entity<Country>()
                .Property(e => e.couIso3)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
              .HasMany(e => e.Company)
              .WithRequired(e => e.Country)
              .HasForeignKey(e => e.CounAquasimCountryId)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Images>()
                .Property(e => e.imCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Images>()
                .Property(e => e.imLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Images>()
                .Property(e => e.imDeleterUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Laboratory>().HasKey(p => p.labId);

            modelBuilder.Entity<Laboratory>()
                .Property(c => c.labId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Laboratory>()
                .Property(e => e.labCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Laboratory>()
                .Property(e => e.labLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Laboratory>()
                .Property(e => e.labDeleterUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Laboratory>()
                .HasMany(e => e.LaboratoryDetail)
                .WithOptional(e => e.Laboratory)
                .HasForeignKey(e => e.labdLaboratoryId);

            modelBuilder.Entity<Laboratory>()
           .HasMany(e => e.Site)
           .WithOptional(e => e.Laboratory)
           .HasForeignKey(e => e.sitLaboratoryId);

            modelBuilder.Entity<LaboratoryDataInput>().HasKey(p => p.ldiId);

            modelBuilder.Entity<LaboratoryDataInput>()
             .Property(c => c.ldiId)
             .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<LaboratoryDataInput>()
               .Property(e => e.ldiObservation)
               .IsUnicode(false);


            modelBuilder.Entity<LaboratoryDataInput>()
                .Property(e => e.ldiCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<LaboratoryDataInput>()
                .Property(e => e.ldiLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<LaboratoryDataInput>()
                .Property(e => e.ldiDeleterUserId)
                .IsUnicode(false);

            modelBuilder.Entity<LaboratoryDataInput>()
                .HasMany(e => e.LaboratoryDataInputDetail)
                .WithRequired(e => e.LaboratoryDataInput)
                .HasForeignKey(e => e.ldidLaboratoryDataInputId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LaboratoryDataInputDetail>().HasKey(p => p.ldidId);

            modelBuilder.Entity<LaboratoryDataInputDetail>()
             .Property(c => c.ldidId)
             .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<LaboratoryDataInputDetail>()
                .Property(e => e.ldidValue)
                .IsUnicode(false);

            modelBuilder.Entity<LaboratoryDetail>().HasKey(p => p.labdId);

            modelBuilder.Entity<LaboratoryDetail>()
                .Property(c => c.labdId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<LaboratoryDataInputLog>().HasKey(p => p.Id);

            modelBuilder.Entity<LaboratoryDataInputLog>()
             .Property(c => c.Id)
             .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);



            modelBuilder.Entity<LaboratoryDataInputLog>()
                .HasMany(e => e.LaboratoryDataInputDetailLog)
                .WithRequired(e => e.LaboratoryDataInputLog)
                .HasForeignKey(e => e.IdLaboratoryDataInputId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LaboratoryDataInputDetailLog>().HasKey(p => p.Id);

            modelBuilder.Entity<LaboratoryDataInputDetailLog>()
             .Property(c => c.Id)
             .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Parameter>().HasKey(p => p.parId);

            modelBuilder.Entity<Parameter>()
                .Property(c => c.parId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Parameter>()
                .Property(e => e.parName)
                .IsUnicode(false);

            modelBuilder.Entity<Parameter>()
                .Property(e => e.parCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Parameter>()
                .Property(e => e.parLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Parameter>()
                .Property(e => e.parDeleterUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Parameter>()
             .HasMany(e => e.FormulaParameter)
             .WithRequired(e => e.Parameter)
             .HasForeignKey(e => e.formpParamId)
             .WillCascadeOnDelete(false);

            modelBuilder.Entity<Parameter>()
                .HasMany(e => e.LaboratoryDataInputDetail)
                .WithRequired(e => e.Parameter)
                .HasForeignKey(e => e.ldidParameterId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Parameter>()
                .HasMany(e => e.ParameterValue)
                .WithOptional(e => e.Parameter)
                .HasForeignKey(e => e.pvalueParameterId);

            modelBuilder.Entity<ParameterValue>().HasKey(p => p.pvalueId);

            modelBuilder.Entity<ParameterValue>()
                .Property(c => c.pvalueId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ParameterValue>()
                .Property(e => e.pvalueInfo)
                .IsUnicode(false);

            modelBuilder.Entity<ParameterValue>()
                .HasMany(e => e.FormulaParameter)
                .WithRequired(e => e.ParameterValue)
                .HasForeignKey(e => e.formpParameterValueId)
                .WillCascadeOnDelete(false);



            modelBuilder.Entity<Images>().HasKey(p => p.imId);

            modelBuilder.Entity<Images>()
                .Property(c => c.imId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            modelBuilder.Entity<Images>()
               .Property(e => e.imCreatorUserId)
               .IsUnicode(false);

            modelBuilder.Entity<Images>()
                .Property(e => e.imLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Images>()
                .Property(e => e.imDeleterUserId)
                .IsUnicode(false);

            modelBuilder.Entity<PathologicalDataInput>().HasKey(p => p.pdiId);

            modelBuilder.Entity<PathologicalDataInput>()
                .Property(c => c.pdiId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<PathologicalDataInput>()
                .Property(e => e.pdiRequestedBy)
                .IsUnicode(false);

            modelBuilder.Entity<PathologicalDataInput>()
                .Property(e => e.pdiRecommendation)
                .IsUnicode(false);

            modelBuilder.Entity<PathologicalDataInput>()
                .Property(e => e.pdiCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<PathologicalDataInput>()
                .Property(e => e.pdiLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<PathologicalDataInput>()
                .Property(e => e.pdiDeleterUserId)
                .IsUnicode(false);



            modelBuilder.Entity<PathologicalDataInput>()
                .HasMany(e => e.PathologicalDataInputDetail)
                .WithRequired(e => e.PathologicalDataInput)
                .HasForeignKey(e => e.pdidPathologicalDataInputId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PathologicalDataInputDetail>().HasKey(p => p.pdidId);

            modelBuilder.Entity<PathologicalDataInputDetail>()
                .Property(c => c.pdidId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            modelBuilder.Entity<PathologicalDataInputDetail>()
                .Property(e => e.pdidTextureName)
                .IsUnicode(false);

            modelBuilder.Entity<PathologicalDataInputDetail>()
                .Property(e => e.pdidAntennaName)
                .IsUnicode(false);

            modelBuilder.Entity<PathologicalDataInputDetail>()
                .Property(e => e.pdidUropodsName)
                .IsUnicode(false);

            modelBuilder.Entity<PathologicalDataInputDetail>()
                .Property(e => e.pdidGillName)
                .IsUnicode(false);

            modelBuilder.Entity<PathologicalDataInputDetail>()
                .Property(e => e.pdidNecrosisName)
                .IsUnicode(false);

            modelBuilder.Entity<PathologicalDataInputDetail>()
                .Property(e => e.pdidEctoparasiteName)
                .IsUnicode(false);

            modelBuilder.Entity<PathologicalDataInputDetail>()
                .Property(e => e.pdidLipidsName)
                .IsUnicode(false);

            modelBuilder.Entity<PathologicalDataInputDetail>()
                .Property(e => e.pdidTubularDeformityName)
                .IsUnicode(false);

            modelBuilder.Entity<PathologicalDataInputDetail>()
                .Property(e => e.pdidGregarineName)
                .IsUnicode(false);

            modelBuilder.Entity<PathologicalDataInputDetail>()
                .Property(e => e.pdidNematodesName)
                .IsUnicode(false);

            modelBuilder.Entity<PathologicalDataInputDetail>()
                .Property(e => e.pdidIntestinalName)
                .IsUnicode(false);

            modelBuilder.Entity<PathologicalDataInputDetail>()
                .Property(e => e.pdidGametocyteName)
                .IsUnicode(false);

            modelBuilder.Entity<PathologicalDataInputDetail>()
              .HasMany(e => e.Images)
              .WithRequired(e => e.PathologicalDataInputDetail)
              .HasForeignKey(e => e.imPathologicalDataInputDetailId)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<PathologicalKpi>().HasKey(p => p.KpiId);

            modelBuilder.Entity<PathologicalKpi>()
                .Property(c => c.KpiId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ProductionGroup>().HasKey(p => p.fgID);

            modelBuilder.Entity<ProductionGroup>()
                .Property(c => c.fgID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ProductionGroup>()
                .Property(e => e.fgInitialWeight)
                .HasPrecision(18, 6);

            modelBuilder.Entity<ProductionGroup>()
                .Property(e => e.fgStopWeight)
                .HasPrecision(18, 6);

            modelBuilder.Entity<ProductionGroup>()
                .Property(e => e.CreatorUserID)
                .IsUnicode(false);

            modelBuilder.Entity<ProductionGroup>()
                .Property(e => e.LastModifierID)
                .IsUnicode(false);

            modelBuilder.Entity<ProductionGroup>()
                .Property(e => e.DeleterUserID)
                .IsUnicode(false);

            modelBuilder.Entity<ProductionGroup>()
                .HasMany(e => e.PathologicalDataInputDetail)
                .WithRequired(e => e.ProductionGroup)
                .HasForeignKey(e => e.pdidProductionGroupAquasimId)
                .WillCascadeOnDelete(false);



            modelBuilder.Entity<ReferenceMethodsDetail>().HasKey(p => p.refDetId);

            modelBuilder.Entity<ReferenceMethodsDetail>()
                .Property(c => c.refDetId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ReferenceMethodsDetail>()
              .HasMany(e => e.SamplingReferenceMethods)
              .WithRequired(e => e.ReferenceMethodsDetail)
              .HasForeignKey(e => e.refSampReferenceMethodId)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceMethodsDetail>()
                .Property(e => e.refDetReference)
                .IsUnicode(false);

            modelBuilder.Entity<ReferenceMethodsDetail>()
                .Property(e => e.refDetDescription)
                .IsUnicode(false);

            modelBuilder.Entity<ReferenceMethodsDetail>()
                .Property(e => e.refDetCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<ReferenceMethodsDetail>()
                .Property(e => e.refDetLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<ReferenceMethodsDetail>()
                .Property(e => e.refDetDeleterUserId)
                .IsUnicode(false);


            modelBuilder.Entity<ReferenceMethodsDetailByAnalysis>().HasKey(p => p.refDetId);

            modelBuilder.Entity<ReferenceMethodsDetailByAnalysis>()
                .Property(c => c.refDetId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ReferenceMethodsDetailByAnalysis>()
              .HasMany(e => e.SamplingReferenceMethodsByAnalysis)
              .WithRequired(e => e.ReferenceMethodsDetailByAnalysis)
              .HasForeignKey(e => e.refSampReferenceMethodId)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReferenceMethodsDetailByAnalysis>()
                .Property(e => e.refDetReference)
                .IsUnicode(false);

            modelBuilder.Entity<ReferenceMethodsDetailByAnalysis>()
                .Property(e => e.refDetDescription)
                .IsUnicode(false);

            modelBuilder.Entity<ReferenceMethodsDetailByAnalysis>()
                .Property(e => e.refDetCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<ReferenceMethodsDetailByAnalysis>()
                .Property(e => e.refDetLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<ReferenceMethodsDetailByAnalysis>()
                .Property(e => e.refDetDeleterUserId)
                .IsUnicode(false);



            modelBuilder.Entity<Report>().HasKey(p => p.Id);

            modelBuilder.Entity<Report>()
                .Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Report>()
              .Property(e => e.anCreatorUserId)
              .IsUnicode(false);

            modelBuilder.Entity<Report>()
                .Property(e => e.anLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Report>()
                .Property(e => e.anDeleterUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Report>()
                .HasMany(e => e.ReportDetail)
                .WithRequired(e => e.Report)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Report>()
            .HasMany(e => e.TechnicalAdvise)
            .WithRequired(e => e.Report)
            .HasForeignKey(e => e.advReportId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReportDetail>().HasKey(p => p.Id);

            modelBuilder.Entity<ReportDetail>()
                .Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            modelBuilder.Entity<Sample>().HasKey(p => p.samId);

            modelBuilder.Entity<Sample>()
                .Property(c => c.samId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            modelBuilder.Entity<Sample>()
                .Property(e => e.samDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Sample>()
                .Property(e => e.samCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Sample>()
                .Property(e => e.samLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Sample>()
                .Property(e => e.samDeleterUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Sample>()
                .HasMany(e => e.SampleType)
                .WithOptional(e => e.Sample)
                .HasForeignKey(e => e.stypeSampleId);

            modelBuilder.Entity<SampleLocation>().HasKey(p => p.slocId);

            modelBuilder.Entity<SampleLocation>()
                .Property(c => c.slocId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<SampleLocation>()
              .Property(e => e.slocDescription)
              .IsUnicode(false);

            modelBuilder.Entity<SampleLocation>()
                .Property(e => e.slocCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<SampleLocation>()
                .Property(e => e.slocLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<SampleLocation>()
                .Property(e => e.slocDeleterUserId)
                .IsUnicode(false);

            modelBuilder.Entity<SampleLocation>()
                .HasMany(e => e.SampleLocationInstance)
                .WithRequired(e => e.SampleLocation)
                .HasForeignKey(e => e.slinSampleLocationId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SampleLocationInstance>().HasKey(p => p.slinId);

            modelBuilder.Entity<SampleLocationInstance>()
                .Property(c => c.slinId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<SampleLocationInstance>()
                .Property(e => e.slinDescription)
                .IsUnicode(false);

            modelBuilder.Entity<SampleLocationInstance>()
                .Property(e => e.slinCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<SampleLocationInstance>()
                .Property(e => e.slinLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<SampleLocationInstance>()
                .Property(e => e.slinDeleterUserId)
                .IsUnicode(false);

            modelBuilder.Entity<SampleLocationInstance>()
             .HasMany(e => e.Sampling)
             .WithRequired(e => e.SampleLocationInstance)
             .HasForeignKey(e => e.sampSampleLocationInstanceId)
             .WillCascadeOnDelete(false);



            modelBuilder.Entity<SampleType>().HasKey(p => p.stypeID);

            modelBuilder.Entity<SampleType>()
                .Property(c => c.stypeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<SampleType>()
                .Property(e => e.stypeDescription)
                .IsUnicode(false);

            modelBuilder.Entity<SampleType>()
                .Property(e => e.stypeCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<SampleType>()
                .Property(e => e.stypeLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<SampleType>()
                .Property(e => e.stypeDeleterUserId)
                .IsUnicode(false);

            modelBuilder.Entity<SampleType>()
               .HasMany(e => e.Sampling)
               .WithRequired(e => e.SampleType)
               .HasForeignKey(e => e.sampSampleTypeId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sampling>().HasKey(p => p.sampId);

            modelBuilder.Entity<Sampling>()
                .Property(c => c.sampId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            modelBuilder.Entity<Sampling>()
                .Property(e => e.sampObservation)
                .IsUnicode(false);

            modelBuilder.Entity<Sampling>()
                .Property(e => e.sampStatus)
                .IsUnicode(false);

            modelBuilder.Entity<Sampling>()
                .Property(e => e.sampCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Sampling>()
                .Property(e => e.sampLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Sampling>()
                .Property(e => e.sampDeleterUserId)
                .IsUnicode(false);


            modelBuilder.Entity<Sampling>()
                .HasMany(e => e.LaboratoryDataInput)
                .WithOptional(e => e.Sampling)
                .HasForeignKey(e => e.ldiSamplingId);




            modelBuilder.Entity<Sampling>()
              .HasMany(e => e.ReportDetail)
              .WithRequired(e => e.Sampling)
              .HasForeignKey(e => e.SamplingId)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sampling>()
                 .HasMany(e => e.SamplingDetail)
                 .WithRequired(e => e.Sampling)
                 .HasForeignKey(e => e.samdSamplingId)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sampling>()
            .HasMany(e => e.SamplingAnalysisGroupFeaturesList)
            .WithRequired(e => e.Sampling)
            .HasForeignKey(e => e.cmmSamplingId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sampling>()
               .HasMany(e => e.SamplingReferenceMethods)
               .WithRequired(e => e.Sampling)
               .HasForeignKey(e => e.refSamplingId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sampling>()
             .HasMany(e => e.SamplingReferenceMethodsByAnalysis)
             .WithRequired(e => e.Sampling)
             .HasForeignKey(e => e.refSamplingId)
             .WillCascadeOnDelete(false);

            modelBuilder.Entity<SamplingDetail>().HasKey(p => p.samdId);

            modelBuilder.Entity<SamplingDetail>()
                .Property(c => c.samdId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<SamplingDetail>()
                .Property(e => e.samdCommentary)
                .IsUnicode(false);

            modelBuilder.Entity<SamplingDetail>()
                .HasMany(e => e.SamplingAnalysisFeaturesList)
                .WithRequired(e => e.SamplingDetail)
                .HasForeignKey(e => e.cmmSamplingDetailId)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<SamplingAnalysisFeaturesList>().HasKey(p => p.cmmId);

            modelBuilder.Entity<SamplingAnalysisFeaturesList>()
                .Property(c => c.cmmId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<SamplingAnalysisGroupFeaturesList>().HasKey(p => p.cmmId);

            modelBuilder.Entity<SamplingAnalysisGroupFeaturesList>()
                .Property(c => c.cmmId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            modelBuilder.Entity<SamplingReferenceMethods>().HasKey(p => p.refId);

            modelBuilder.Entity<SamplingReferenceMethods>()
                .Property(c => c.refId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<SamplingReferenceMethodsByAnalysis>().HasKey(p => p.refId);

            modelBuilder.Entity<SamplingReferenceMethodsByAnalysis>()
                .Property(c => c.refId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);



            modelBuilder.Entity<ShrimpSector>().HasKey(p => p.shrId);

            modelBuilder.Entity<ShrimpSector>()
                .Property(c => c.shrId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ShrimpSector>()
                .Property(e => e.shrKey)
                .IsFixedLength();

            modelBuilder.Entity<ShrimpSector>()
                .Property(e => e.shrDescription)
                .IsUnicode(false);

            modelBuilder.Entity<ShrimpSector>()
                .Property(e => e.shrCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<ShrimpSector>()
                .Property(e => e.shrLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<ShrimpSector>()
                .Property(e => e.shrDeleterUserId)
                .IsUnicode(false);

            modelBuilder.Entity<ShrimpSector>()
             .HasMany(e => e.Site)
             .WithRequired(e => e.ShrimpSector)
             .HasForeignKey(e => e.sitShrimpSectorId)
             .WillCascadeOnDelete(false);

            modelBuilder.Entity<Site>().HasKey(p => p.SitAquasymId);

            modelBuilder.Entity<Site>()
                .Property(c => c.sitId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Site>()
                .Property(e => e.sitName)
                .IsUnicode(false);

            modelBuilder.Entity<Site>()
                .Property(e => e.sitRequestedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Site>()
                 .HasMany(e => e.PathologicalDataInput)
                 .WithRequired(e => e.Site)
                 .HasForeignKey(e => e.pdiSiteAquasimId)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<Site>()
              .HasMany(e => e.SampleLocationInstance)
              .WithRequired(e => e.Site)
              .HasForeignKey(e => e.slinSiteId)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Site>()
              .HasMany(e => e.SiteAddress)
              .WithRequired(e => e.Site)
              .HasForeignKey(e => e.sitaSiteAquasimId)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Site>()
                .HasMany(e => e.SiteGroupDetail)
                .WithRequired(e => e.Site)
                .HasForeignKey(e => e.sgdSiteAquasimId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Site>()
            .HasMany(e => e.Unit)
            .WithRequired(e => e.Site)
            .HasForeignKey(e => e.UniAquasimSiteId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<Site>()
            .HasMany(e => e.Sampling)
            .WithRequired(e => e.Site)
            .HasForeignKey(e => e.sampSiteAquasimId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<Site>()
             .Property(e => e.sitSendEmailsReports)
             .IsUnicode(false);



            modelBuilder.Entity<SiteAddress>().HasKey(p => p.sitaId);

            modelBuilder.Entity<SiteAddress>()
                .Property(c => c.sitaId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<SiteAddress>()
               .Property(e => e.sitaAddressNo)
               .IsUnicode(false);

            modelBuilder.Entity<SiteAddress>()
                .Property(e => e.CreatorUserID)
                .IsUnicode(false);

            modelBuilder.Entity<SiteAddress>()
                .Property(e => e.LastModifierID)
                .IsUnicode(false);

            modelBuilder.Entity<SiteAddress>()
                .Property(e => e.DeleterUserID)
                .IsUnicode(false);

            modelBuilder.Entity<SiteGroup>().HasKey(p => p.sgId);

            modelBuilder.Entity<SiteGroup>()
                .Property(c => c.sgId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<SiteGroup>()
               .Property(e => e.sgCode)
               .IsUnicode(false);

            modelBuilder.Entity<SiteGroup>()
                .Property(e => e.sgName)
                .IsUnicode(false);

            modelBuilder.Entity<SiteGroup>()
                .Property(e => e.CreatorUserID)
                .IsUnicode(false);

            modelBuilder.Entity<SiteGroup>()
                .Property(e => e.LastModifierID)
                .IsUnicode(false);

            modelBuilder.Entity<SiteGroup>()
                .Property(e => e.DeleterUserID)
                .IsUnicode(false);

            modelBuilder.Entity<SiteGroup>()
                .HasMany(e => e.SiteGroupDetail)
                .WithRequired(e => e.SiteGroup)
                .HasForeignKey(e => e.sgdSiteGroupId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SiteGroupDetail>().HasKey(p => p.sgdId);

            modelBuilder.Entity<SiteGroupDetail>()
                .Property(c => c.sgdId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<SiteGroupDetail>()
                .Property(e => e.CreatorUserID)
                .IsUnicode(false);

            modelBuilder.Entity<SiteGroupDetail>()
                .Property(e => e.LastModifierID)
                .IsUnicode(false);

            modelBuilder.Entity<SiteGroupDetail>()
                .Property(e => e.DeleterUserID)
                .IsUnicode(false);

            modelBuilder.Entity<SkrettingSector>().HasKey(p => p.skseId);

            modelBuilder.Entity<SkrettingSector>()
                .Property(c => c.skseId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //para consulta en get/id campos de precrias
            modelBuilder.Entity<SampleLocationInstance>().HasKey(p => p.slinId);

            modelBuilder.Entity<SampleLocationInstance>()
                .Property(c => c.slinId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            modelBuilder.Entity<SkrettingSector>()
                .Property(e => e.skseKey)
                .IsFixedLength();

            modelBuilder.Entity<SkrettingSector>()
                .Property(e => e.skseCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<SkrettingSector>()
                .Property(e => e.skseLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<SkrettingSector>()
                .Property(e => e.skseDeleterUserId)
                .IsUnicode(false);

            modelBuilder.Entity<SkrettingSector>()
                 .HasMany(e => e.Site)
                 .WithRequired(e => e.SkrettingSector)
                 .HasForeignKey(e => e.sitSkrettingSectorId)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<SkrettingSite>().HasKey(p => p.sksId);

            modelBuilder.Entity<SkrettingSite>()
                .Property(c => c.sksId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<SkrettingSite>()
                .Property(e => e.sksCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<SkrettingSite>()
                .Property(e => e.sksLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<SkrettingSite>()
                .Property(e => e.sksDeleterUserId)
                .IsUnicode(false);

            modelBuilder.Entity<SkrettingSite>()
                .HasMany(e => e.SkrettingSector)
                .WithOptional(e => e.SkrettingSite)
                .HasForeignKey(e => e.skseSkrettingSiteId);*/

            modelBuilder.Entity<SystemParameter>().HasKey(p => p.ID);

            modelBuilder.Entity<SystemParameter>()
                .Property(c => c.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            /*modelBuilder.Entity<SystemParameter>()
             .HasMany(e => e.LaboratoryDataInput)
             .WithRequired(e => e.SystemParameter)
             .HasForeignKey(e => e.ldiStatus)
             .WillCascadeOnDelete(false);

            modelBuilder.Entity<SystemParameter>()
               .HasMany(e => e.Parameter)
               .WithOptional(e => e.SystemParameter)
               .HasForeignKey(e => e.parGroupZoo);


            modelBuilder.Entity<SystemParameter>()
              .HasMany(e => e.ProductionGroup)
              .WithOptional(e => e.SystemParameter)
              .HasForeignKey(e => e.fgMaturation);

            modelBuilder.Entity<SystemParameter>()
                .HasMany(e => e.ProductionGroup1)
                .WithOptional(e => e.SystemParameter1)
                .HasForeignKey(e => e.fgLarvaLaboratory);

            modelBuilder.Entity<SystemParameter>()
                .HasMany(e => e.ProductionGroup2)
                .WithOptional(e => e.SystemParameter2)
                .HasForeignKey(e => e.fgManejo);

            modelBuilder.Entity<SystemParameter>()
                .HasMany(e => e.ProductionGroup3)
                .WithOptional(e => e.SystemParameter3)
                .HasForeignKey(e => e.fgAlimentador);

            modelBuilder.Entity<SystemParameter>()
                .HasMany(e => e.ProductionGroup4)
                .WithOptional(e => e.SystemParameter4)
                .HasForeignKey(e => e.fgSeedingType);

            modelBuilder.Entity<SystemParameter>()
                .HasMany(e => e.ProductionGroup5)
                .WithOptional(e => e.SystemParameter5)
                .HasForeignKey(e => e.fgFeedingType);

            modelBuilder.Entity<SystemParameter>()
                .HasMany(e => e.ProductionGroup6)
                .WithOptional(e => e.SystemParameter6)
                .HasForeignKey(e => e.fgSeedingType);

            modelBuilder.Entity<SystemParameter>()
                .HasMany(e => e.ProductionGroup7)
                .WithOptional(e => e.SystemParameter7)
                .HasForeignKey(e => e.fgFeedBrandLth3g);

            modelBuilder.Entity<SystemParameter>()
                .HasMany(e => e.ProductionGroup8)
                .WithOptional(e => e.SystemParameter8)
                .HasForeignKey(e => e.fgStartedFeedLineLth3g);

            modelBuilder.Entity<SystemParameter>()
                .HasMany(e => e.ProductionGroup9)
                .WithOptional(e => e.SystemParameter9)
                .HasForeignKey(e => e.fgFeedBrandLgh3g);

            modelBuilder.Entity<SystemParameter>()
                .HasMany(e => e.ProductionGroup10)
                .WithOptional(e => e.SystemParameter10)
                .HasForeignKey(e => e.fgStartedFeedLinegth3g);

            modelBuilder.Entity<SystemParameter>()
             .HasMany(e => e.PathologicalKpi)
             .WithRequired(e => e.SystemParameter)
             .HasForeignKey(e => e.KpiOptionId)
             .WillCascadeOnDelete(false);

            modelBuilder.Entity<SystemParameter>()
                .HasMany(e => e.ProductionGroup11)
                .WithOptional(e => e.SystemParameter11)
                .HasForeignKey(e => e.fgProtocolo);

            modelBuilder.Entity<SystemParameter>()
                .HasMany(e => e.Site)
                .WithOptional(e => e.SystemParameter)
                .HasForeignKey(e => e.sitProvince);



            modelBuilder.Entity<SystemParameter>()
                 .HasMany(e => e.Site1)
                 .WithOptional(e => e.SystemParameter1)
                 .HasForeignKey(e => e.sitWaterMgtSys);

            modelBuilder.Entity<SystemParameter>()
            .HasMany(e => e.Unit)
            .WithOptional(e => e.SystemParameter)
            .HasForeignKey(e => e.unidAdjustungFeedMethod);

            modelBuilder.Entity<SystemParameter>()
             .HasMany(e => e.Sampling)
             .WithRequired(e => e.SystemParameter)
             .HasForeignKey(e => e.sampStatusId)
             .WillCascadeOnDelete(false);*/


            /*modelBuilder.Entity<TechnicalAdvise>()
                .Property(e => e.advText)
                .IsUnicode(false);

            modelBuilder.Entity<TechnicalAdvise>()
                .Property(e => e.advCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<TechnicalAdvise>()
                .Property(e => e.advLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<TechnicalAdvise>()
                .Property(e => e.advDeleterUserId)
                .IsUnicode(false);

            modelBuilder.Entity<TechnicalAdvise>()
                .HasMany(e => e.TechnicalAdviseLog)
                .WithRequired(e => e.TechnicalAdvise)
                .HasForeignKey(e => e.logTechnicalAdviseId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TechnicalAdviseLog>()
                .Property(e => e.logText)
                .IsUnicode(false);

            modelBuilder.Entity<TechnicalAdviseLog>()
                .Property(e => e.logCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<TechnicalAdviseLog>()
                .Property(e => e.logLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<TechnicalAdviseLog>()
                .Property(e => e.logDeleterUserId)
                .IsUnicode(false);


            modelBuilder.Entity<Unit>().HasKey(p => p.UniAquasimId);

            modelBuilder.Entity<Unit>()
                .Property(c => c.uniID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Unit>()
               .Property(e => e.uniCode)
               .IsFixedLength();

            modelBuilder.Entity<Unit>()
                .Property(e => e.CreatorUserID)
                .IsUnicode(false);

            modelBuilder.Entity<Unit>()
                .Property(e => e.LastModifierID)
                .IsUnicode(false);

            modelBuilder.Entity<Unit>()
                .Property(e => e.DeleterUserID)
                .IsUnicode(false);



            modelBuilder.Entity<Unit>()
                .HasMany(e => e.PathologicalDataInputDetail)
                .WithRequired(e => e.Unit)
                .HasForeignKey(e => e.pdidUnitAquasimId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SampleLocationInstance>()
                .HasMany(e => e.PathologicalDataInputDetail)
                .WithRequired(e => e.SampleLocationInstance)
                .HasForeignKey(e => e.pdiSlinId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Unit>()
                .HasMany(e => e.ProductionGroup)
                .WithRequired(e => e.Unit)
                .HasForeignKey(e => e.PGUnitAquasymId)
                .WillCascadeOnDelete(false);



            modelBuilder.Entity<Unit>()
                          .HasMany(e => e.Sampling)
                          .WithOptional(e => e.Unit)
                          .HasForeignKey(e => e.sampUnitAquasimId);*/



            modelBuilder.Entity<UserSystem>().HasKey(p => p.usrId);

            modelBuilder.Entity<UserSystem>()
                .Property(c => c.usrId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<UserSystem>()
              .Property(e => e.usrLogon)
              .IsUnicode(false);



            /*modelBuilder.Entity<UserSystem>()
              .HasMany(e => e.LaboratoryDetail)
              .WithRequired(e => e.UserSystem)
              .HasForeignKey(e => e.labdUserId)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserSystem>()
              .HasMany(e => e.PathologicalDataInput)
              .WithOptional(e => e.UserSystem)
              .HasForeignKey(e => e.pdiTechnicalUserId);

            modelBuilder.Entity<UserSystem>()
                .HasMany(e => e.Sampling)
                .WithRequired(e => e.UserSystem)
                .HasForeignKey(e => e.sampUserTechnicalId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserSystem>()
                .HasMany(e => e.Sampling1)
                .WithOptional(e => e.UserSystem1)
                .HasForeignKey(e => e.sampUserTechnicalBehalfId);


            modelBuilder.Entity<UserSystem>()
                .HasMany(e => e.Visit)
                .WithRequired(e => e.UserSystem)
                .HasForeignKey(e => e.visUserId)
                .WillCascadeOnDelete(false);*/

            modelBuilder.Entity<SystemParameter>()
             .HasMany(e => e.UserSystem)
             .WithOptional(e => e.SystemParameter)
             .HasForeignKey(e => e.usrDegree);

            modelBuilder.Entity<SystemParameter>()
                .HasMany(e => e.UserSystem1)
                .WithOptional(e => e.SystemParameter1)
                .HasForeignKey(e => e.usrPosition);

            modelBuilder.Entity<Encuestas>().HasKey(p => p.EncuestaId);
            modelBuilder.Entity<Encuestas>()
                .Property(c => c.EncuestaId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Encuestas>()
                .Property(e => e.EncargadoId)
                .IsUnicode(false);

            modelBuilder.Entity<Encuestas>()
                .Property(e => e.Observacion)
                .IsUnicode(false);

            modelBuilder.Entity<Encuestas>()
                .Property(e => e.UserCreatorId)
                .IsUnicode(false);

            modelBuilder.Entity<Encuestas>()
                .Property(e => e.UserModifierId)
                .IsUnicode(false);
            modelBuilder.Entity<EncuestasDetalle>().HasKey(p => p.EncuestaDetalleId);

            modelBuilder.Entity<LocalesNacionales>().HasKey(p => p.CodigoLocal);
            modelBuilder.Entity<LocalesNacionales>()
                    .HasMany(e => e.Encuestas)
                    .WithOptional(e => e.LocalesNacionales)
                    .HasForeignKey(e => e.CodigoLocal);


            modelBuilder.Entity<Pregunta>().HasKey(p => p.PreguntaId);
            modelBuilder.Entity<Pregunta>()
                .Property(c => c.PreguntaId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Pregunta>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Pregunta>()
                .Property(e => e.UserCreatorId)
                .IsUnicode(false);

            modelBuilder.Entity<Pregunta>()
                .Property(e => e.UserModifierId)
                .IsUnicode(false);

            modelBuilder.Entity<Pregunta>()
                    .HasMany(e => e.EncuestasDetalle)
                    .WithRequired(e => e.Pregunta)
                    .HasForeignKey(e => e.PreguntaId);

            modelBuilder.Entity<Categorias>().HasKey(p => p.CategoriaId);
            modelBuilder.Entity<Categorias>()
                .Property(c => c.CategoriaId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Categorias>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Categorias>()
                    .HasMany(e => e.Pregunta)
                    .WithOptional(e => e.Categorias)
                    .HasForeignKey(e => e.CategoriaId);
            /*modelBuilder.Entity<Pregunta>()
            .Property(p => p.CategoriaId).IsOptional();*/
            /*modelBuilder.Entity<UserSystem>()
            .HasMany(e => e.SamplingDetail)
            .WithOptional(e => e.UserSystem)
            .HasForeignKey(e => e.samdUserApproved);*/


            //modelBuilder.Entity<UserSystem>()
            //    .HasMany(e => e.Sampling1)
            //    .WithOptional(e => e.UserSystem1)
            //    .HasForeignKey(e => e.sampAssignedUser);



            /*modelBuilder.Entity<UserSystem>()
                .HasMany(e => e.Sampling2)
                .WithOptional(e => e.UserSystem2)
                .HasForeignKey(e => e.sampAssignedUser);*/


            /*modelBuilder.Entity<Visit>().HasKey(p => p.visId);

            modelBuilder.Entity<Visit>()
                .Property(c => c.visId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);



            modelBuilder.Entity<Visit>()
                .Property(e => e.visCreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Visit>()
                .Property(e => e.visLastModificationUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Visit>()
                .Property(e => e.visDeleterUserId)
                .IsUnicode(false);*/




            modelBuilder.Entity<SystemParameter>().HasKey(p => p.ID);

            modelBuilder.Entity<SystemParameter>()
                .Property(c => c.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<SystemParameter>()
                .Property(e => e.field)
                .IsUnicode(false);

            modelBuilder.Entity<SystemParameter>()
                .Property(e => e.value)
                .IsUnicode(false);

            modelBuilder.Entity<SystemParameter>()
                .Property(e => e.Language)
                .IsFixedLength();





            /*modelBuilder.Entity<Logs>().HasKey(p => p.id);

            modelBuilder.Entity<Logs>()
                .Property(c => c.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Logs>()
                .Property(e => e.events)
                .IsUnicode(false);

            //Para nueva SampleDetail

            modelBuilder.Entity<SampleDetail>().HasKey(p => p.SamDId);

            modelBuilder.Entity<SampleDetail>()
                         .Property(c => c.SamDId)
                         .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Sample>()
                        .HasMany(e => e.SampleDetail)
                        .WithRequired(e => e.Sample)
                        .HasForeignKey(e => e.SamDSampleId)
                    .WillCascadeOnDelete(false);



            modelBuilder.Entity<AnalysisGroup>()
            .HasMany(e => e.SampleDetail)
            .WithOptional(e => e.AnalysisGroup)
            .HasForeignKey(e => e.SamDGroupId);*/
        }
    }
}