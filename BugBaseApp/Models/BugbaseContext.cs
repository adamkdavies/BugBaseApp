using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BugBaseApp.Models;

public partial class BugbaseContext : DbContext
{
    public BugbaseContext()
    {
    }

    public BugbaseContext(DbContextOptions<BugbaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketChangeHistory> TicketChangeHistories { get; set; }

    public virtual DbSet<TicketChangeType> TicketChangeTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=.\\\\\\\\Data\\\\\\\\bugbase.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Note>(entity =>
        {
            entity.ToTable("Note");

            entity.HasIndex(e => e.NoteId, "IX_Note_NoteId").IsUnique();

            entity.Property(e => e.NoteId).ValueGeneratedOnAdd();
            entity.Property(e => e.TicketId).HasColumnName("TicketID");

            entity.HasOne(d => d.NoteOwner).WithMany(p => p.Notes).HasForeignKey(d => d.NoteOwnerId);

            entity.HasOne(d => d.Ticket).WithMany(p => p.Notes).HasForeignKey(d => d.TicketId);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.HasIndex(e => e.RoleName, "IX_Role_RoleName").IsUnique();
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.ToTable("State");

            entity.Property(e => e.StateId)
                .ValueGeneratedOnAdd()
                .HasColumnName("StateID");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("Ticket");

            entity.Property(e => e.TicketId).ValueGeneratedOnAdd();
            entity.Property(e => e.QaownerId).HasColumnName("QAOwnerId");

            entity.HasOne(d => d.AssignedTo).WithMany(p => p.TicketAssignedTos).HasForeignKey(d => d.AssignedToId);

            entity.HasOne(d => d.DevOwner).WithMany(p => p.TicketDevOwners).HasForeignKey(d => d.DevOwnerId);

            entity.HasOne(d => d.Qaowner).WithMany(p => p.TicketQaowners).HasForeignKey(d => d.QaownerId);

            entity.HasOne(d => d.State).WithMany(p => p.Tickets).HasForeignKey(d => d.StateId);
        });

        modelBuilder.Entity<TicketChangeHistory>(entity =>
        {
            entity.ToTable("TicketChangeHistory");

            entity.HasIndex(e => e.TicketChangeHistoryId, "IX_TicketChangeHistory_TicketChangeHistoryId").IsUnique();

            entity.Property(e => e.QaownerId).HasColumnName("QAOwnerId");

            entity.HasOne(d => d.AssignedTo).WithMany(p => p.TicketChangeHistoryAssignedTos).HasForeignKey(d => d.AssignedToId);

            entity.HasOne(d => d.DevOwner).WithMany(p => p.TicketChangeHistoryDevOwners).HasForeignKey(d => d.DevOwnerId);

            entity.HasOne(d => d.Qaowner).WithMany(p => p.TicketChangeHistoryQaowners).HasForeignKey(d => d.QaownerId);

            entity.HasOne(d => d.State).WithMany(p => p.TicketChangeHistories).HasForeignKey(d => d.StateId);

            entity.HasOne(d => d.TicketChangeType).WithMany(p => p.TicketChangeHistories).HasForeignKey(d => d.TicketChangeTypeId);

            entity.HasOne(d => d.Ticket).WithMany(p => p.TicketChangeHistories).HasForeignKey(d => d.TicketId);
        });

        modelBuilder.Entity<TicketChangeType>(entity =>
        {
            entity.ToTable("TicketChangeType");

            entity.HasIndex(e => e.TicketChangeTypeId, "IX_TicketChangeType_TicketChangeTypeId").IsUnique();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasIndex(e => e.UserId, "IX_User_UserId").IsUnique();

            entity.HasIndex(e => e.UserName, "IX_User_UserName").IsUnique();

            entity.Property(e => e.UserId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Role).WithMany(p => p.Users).HasForeignKey(d => d.RoleId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
