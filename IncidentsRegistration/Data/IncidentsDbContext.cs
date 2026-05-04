using IncidentsRegistration.Models;
using Microsoft.EntityFrameworkCore;

namespace IncidentsRegistration.Data;

public partial class IncidentsDbContext : DbContext
{
    public IncidentsDbContext()
    {
    }

    public IncidentsDbContext(DbContextOptions<IncidentsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CriminalCase> CriminalCases { get; set; }

    public virtual DbSet<CriminalCasesView> CriminalCasesViews { get; set; }

    public virtual DbSet<Decision> Decisions { get; set; }

    public virtual DbSet<Incident> Incidents { get; set; }

    public virtual DbSet<IncidentLocation> IncidentLocations { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<ResponseTeam> ResponseTeams { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<SubjectRole> SubjectRoles { get; set; }

    public virtual DbSet<SystemUser> SystemUsers { get; set; }

    public virtual DbSet<SystemUserResponseTeam> SystemUserResponseTeams { get; set; }

    public virtual DbSet<TerritorialTransfer> TerritorialTransfers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Incidents_registration;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CriminalCase>(entity =>
        {
            entity.HasKey(e => e.СaseNumber).HasName("PK__Criminal__C2ED82CB52E28AF3");

            entity.HasOne(d => d.IdDecisionNavigation).WithOne(p => p.CriminalCase)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Criminal_case_Decision");
        });

        modelBuilder.Entity<CriminalCasesView>(entity =>
        {
            entity.ToView("CriminalCasesView");
        });

        modelBuilder.Entity<Decision>(entity =>
        {
            entity.HasKey(e => e.IdDecision).HasName("PK__Decision__ED0139231315A60D");

            entity.ToTable("Decision", tb => tb.HasTrigger("TR_Delete_Decision"));

            entity.HasOne(d => d.IdIncidentNavigation).WithOne(p => p.Decision)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Decision_Incident");

            entity.HasOne(d => d.IdResponseTeamNavigation).WithMany(p => p.Decisions).HasConstraintName("FK_Decision_Response_team");
        });

        modelBuilder.Entity<Incident>(entity =>
        {
            entity.HasKey(e => e.IdIncident).HasName("PK__Incident__0464F664EF322C4A");

            entity.ToTable("Incident", tb => tb.HasTrigger("TR_Delete_Incident"));

            entity.HasOne(d => d.IdResponseTeamNavigation).WithMany(p => p.Incidents).HasConstraintName("FK_Incident_Response_team");
        });

        modelBuilder.Entity<IncidentLocation>(entity =>
        {
            entity.HasKey(e => e.IdIncidentLocation).HasName("PK__Incident__FF7D2AA64050D228");

            entity.HasOne(d => d.IdIncidentNavigation).WithMany(p => p.IncidentLocations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Incident_Location_Incident");

            entity.HasOne(d => d.IdLocationNavigation).WithMany(p => p.IncidentLocations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Incident_Location_Location");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.IdLocation).HasName("PK__Location__2F2C70A7EAC7AD7E");
        });

        modelBuilder.Entity<ResponseTeam>(entity =>
        {
            entity.HasKey(e => e.IdResponseTeam).HasName("PK__Response__895AE83A7231D7A4");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.IdSubject).HasName("PK__Subject__20028FF4840A4E1F");

            entity.ToTable("Subject", tb => tb.HasTrigger("TR_Subject_Delete"));
        });

        modelBuilder.Entity<SubjectRole>(entity =>
        {
            entity.HasKey(e => e.IdSubjectRole).HasName("PK__Subject___5B8D0E53A3530190");

            entity.HasOne(d => d.IdIncidentNavigation).WithMany(p => p.SubjectRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subject_role_Incident");

            entity.HasOne(d => d.IdSubjectNavigation).WithMany(p => p.SubjectRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subject_role_Subject");
        });

        modelBuilder.Entity<SystemUser>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__System_u__D7B4671E4C6E890C");
        });

        modelBuilder.Entity<SystemUserResponseTeam>(entity =>
        {
            entity.HasKey(e => e.IdSystemUserResponseTeam).HasName("PK__System_u__1A7C4B04F15B8C75");

            entity.HasOne(d => d.IdResponseTeamNavigation).WithMany(p => p.SystemUserResponseTeams)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_System_user_Response_team_Response_team");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.SystemUserResponseTeams)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_System_user_Response_team_System_user");
        });

        modelBuilder.Entity<TerritorialTransfer>(entity =>
        {
            entity.HasKey(e => e.IdTransfer).HasName("PK__Territor__D9117FC149787D72");

            entity.HasOne(d => d.IdDecisionNavigation).WithOne(p => p.TerritorialTransfer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Territorial_transfer_Decision");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
