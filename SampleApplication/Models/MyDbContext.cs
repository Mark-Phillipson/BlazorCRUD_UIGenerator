using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SampleApplication.Models;

public partial class MyDbContext : DbContext
{
    private readonly IConfiguration? _configuration;

    
    public MyDbContext(DbContextOptions<MyDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<AdditionalCommand> AdditionalCommands { get; set; }

    public virtual DbSet<ApplicationDetail> ApplicationDetails { get; set; }

    public virtual DbSet<ApplicationsToKill> ApplicationsToKills { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Computer> Computers { get; set; }

    public virtual DbSet<CssProperty> CssProperties { get; set; }

    public virtual DbSet<CurrentWindow> CurrentWindows { get; set; }

    public virtual DbSet<CustomIntelliSense> CustomIntelliSenses { get; set; }

    public virtual DbSet<CustomWindowsSpeechCommand> CustomWindowsSpeechCommands { get; set; }

    public virtual DbSet<Example> Examples { get; set; }

    public virtual DbSet<GeneralLookup> GeneralLookups { get; set; }

    public virtual DbSet<GrammarItem> GrammarItems { get; set; }

    public virtual DbSet<GrammarName> GrammarNames { get; set; }

    public virtual DbSet<HtmlTag> HtmlTags { get; set; }

    public virtual DbSet<Idiosyncrasy> Idiosyncrasies { get; set; }

    public virtual DbSet<Keyword> Keywords { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Launcher> Launchers { get; set; }

    public virtual DbSet<LauncherMultipleLauncherBridge> LauncherMultipleLauncherBridges { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<MousePosition> MousePositions { get; set; }

    public virtual DbSet<MultipleLauncher> MultipleLaunchers { get; set; }

    public virtual DbSet<PhraseListGrammar> PhraseListGrammars { get; set; }

    public virtual DbSet<PropertyTabPosition> PropertyTabPositions { get; set; }

    public virtual DbSet<SavedMousePosition> SavedMousePositions { get; set; }

    public virtual DbSet<Sqlkeyword> Sqlkeywords { get; set; }

    public virtual DbSet<Todo> Todos { get; set; }

    public virtual DbSet<ValuesToInsert> ValuesToInserts { get; set; }


    public virtual DbSet<VisualStudioCommand> VisualStudioCommands { get; set; }

    public virtual DbSet<WindowsSpeechVoiceCommand> WindowsSpeechVoiceCommands { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            if (_configuration!= null )
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            }
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_CI_AS");

        modelBuilder.Entity<AdditionalCommand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AdditionalCommands");

            entity.Property(e => e.WaitBefore).HasDefaultValueSql("((0.1))");

            entity.HasOne(d => d.CustomIntelliSense).WithMany(p => p.AdditionalCommands)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.AdditionalCommands_dbo.CustomIntelliSense_CustomIntelliSenseId");
        });

        modelBuilder.Entity<ApplicationsToKill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ID_ApplicationsToKill_PK");

            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.Property(e => e.Label).HasDefaultValueSql("((1))");
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Categories");
        });

        modelBuilder.Entity<Computer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Computers");
        });

        modelBuilder.Entity<CurrentWindow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.CurrentWindow");
        });

        modelBuilder.Entity<CustomIntelliSense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.CustomIntelliSense");

            entity.HasOne(d => d.Category).WithMany(p => p.CustomIntelliSenses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.CustomIntelliSense_dbo.Categories_CategoryID");

            entity.HasOne(d => d.Computer).WithMany(p => p.CustomIntelliSenses).HasConstraintName("FK_dbo.CustomIntelliSense_dbo.Computers_ComputerID");

            entity.HasOne(d => d.Language).WithMany(p => p.CustomIntelliSenses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dbo.CustomIntelliSense_dbo.Languages_LanguageID");
        });

        modelBuilder.Entity<CustomWindowsSpeechCommand>(entity =>
        {
            entity.Property(e => e.AlternateKey).HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.ControlKey).HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.HowToFormatDictation).HasDefaultValueSql("('Do Nothing')");
            entity.Property(e => e.ShiftKey).HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.WindowsKey).HasDefaultValueSql("(CONVERT([bit],(0)))");
        });

        modelBuilder.Entity<Example>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Examples");
        });

        modelBuilder.Entity<GeneralLookup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.GeneralLookups");
        });

        modelBuilder.Entity<HtmlTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.HtmlTags");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Languages");
        });

        modelBuilder.Entity<Launcher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Launcher");

            entity.HasOne(d => d.Category).WithMany(p => p.Launchers).HasConstraintName("FK_dbo.Launcher_dbo.Categories_CategoryID");

            entity.HasOne(d => d.Computer).WithMany(p => p.Launchers).HasConstraintName("FK_dbo.Launcher_dbo.Computers_ComputerID");
        });

        modelBuilder.Entity<LauncherMultipleLauncherBridge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.LauncherMultipleLauncherBridge");

            entity.HasOne(d => d.Launcher).WithMany(p => p.LauncherMultipleLauncherBridges)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LauncherMultipleLauncherBridge_Launcher");

            entity.HasOne(d => d.MultipleLauncher).WithMany(p => p.LauncherMultipleLauncherBridges).HasConstraintName("FK_LauncherMultipleLauncherBridge_MultipleLauncher");
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Logins");
        });

        modelBuilder.Entity<MousePosition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.MousePositions");
        });

        modelBuilder.Entity<MultipleLauncher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.MultipleLauncher");
        });

        modelBuilder.Entity<PropertyTabPosition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.PropertyTabPositions");
        });

        modelBuilder.Entity<SavedMousePosition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.SavedMousePosition");
        });

        modelBuilder.Entity<Todo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Todos");

            entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<ValuesToInsert>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ValuesToInsert");
        });


        modelBuilder.Entity<VisualStudioCommand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.VisualStudioCommands");
        });

        modelBuilder.Entity<WindowsSpeechVoiceCommand>(entity =>
        {
            entity.Property(e => e.AutoCreated).HasDefaultValueSql("(CONVERT([bit],(0)))");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
