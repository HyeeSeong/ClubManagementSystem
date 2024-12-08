namespace ClubManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;
using System;

public class ClubManagementContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Professor> Professors { get; set; } = null!;
        public DbSet<Staff> Staffs { get; set; } = null!;
        public DbSet<Club> Clubs { get; set; } = null!;
        public DbSet<ClubRoom> ClubRooms { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<Report> Reports { get; set; } = null!;
        public DbSet<Participation> Participations { get; set; } = null!;
        public DbSet<Evaluation> Evaluations { get; set; } = null!;
        public DbSet<Notifies> Notifies { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // MySQL 연동
            optionsBuilder.UseMySql("Server=localhost;" +
                                    "Port=3306;" +
                                    "Database=club_management;" +
                                    "User=hyeseong;" +
                                    "Password=1234;",
                new MySqlServerVersion(new Version(8, 0, 21)));
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserID);
                entity.Property(e => e.UserID).ValueGeneratedOnAdd();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.PhoneNumber).HasDefaultValue("000-0000-0000").HasMaxLength(20);
                entity.Property(e => e.Birth).HasDefaultValue(DateTime.Now);
            });

            // Student
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.UserID);
                entity.Property(e => e.UserID).ValueGeneratedOnAdd();
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Year).HasDefaultValue(1);
                entity.HasOne(e => e.User)
                    .WithOne(u => u.Student)
                    .HasForeignKey<Student>(e => e.UserID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Club)
                    .WithMany(c => c.Students)
                    .HasForeignKey(e => e.ClubID)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Professor
            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasKey(e => e.UserID);
                entity.Property(e => e.UserID).ValueGeneratedOnAdd();
                entity.HasOne(e => e.User)
                    .WithOne(u => u.Professor)
                    .HasForeignKey<Professor>(e => e.UserID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Staff
            modelBuilder.Entity<Staff>(entity =>
            {
                entity.HasKey(e => e.UserID);
                entity.Property(e => e.UserID).ValueGeneratedOnAdd();
                entity.HasOne(e => e.User)
                    .WithOne(u => u.Staff)
                    .HasForeignKey<Staff>(e => e.UserID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Club
            modelBuilder.Entity<Club>(entity =>
            {
                entity.HasKey(e => e.ClubID);
                entity.Property(e => e.ClubID).ValueGeneratedOnAdd();
                entity.Property(e => e.ClubName).IsRequired().HasMaxLength(100);
                
                entity.HasOne(e => e.ClubRoom)
                    .WithMany(cr => cr.Clubs)
                    .HasForeignKey(e => e.ClubRoomID)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Professor)
                    .WithMany(p => p.Clubs)
                    .HasForeignKey(e => e.ProfessorID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Staff)
                    .WithMany(s => s.Clubs)
                    .HasForeignKey(e => e.StaffID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ClubRoom
            modelBuilder.Entity<ClubRoom>(entity =>
            {
                entity.HasKey(e => e.ClubRoomID);
                entity.Property(e => e.ClubRoomID).ValueGeneratedOnAdd();
                entity.Property(e => e.Location).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Size).IsRequired();
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
            });

            // Project
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.ProjectID);
                entity.Property(e => e.ProjectID).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

                entity.HasOne(e => e.Club)
                    .WithMany(c => c.Projects)
                    .HasForeignKey(e => e.ClubID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Notification
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.AnnounceID);
                entity.Property(e => e.AnnounceID).ValueGeneratedOnAdd();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Date).HasDefaultValueSql("CURRENT_DATE()");
            });

            // Report (Weak Entity)
            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(e => e.ReportID);
                entity.Property(e => e.ReportID).ValueGeneratedOnAdd();
                entity.Property(e => e.Date).IsRequired();
                
                entity.HasOne(e => e.Project)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(e => e.ProjectID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Participation (N:M)
            modelBuilder.Entity<Participation>(entity =>
            {
                entity.HasKey(e => new { e.StudentID, e.ProjectID });

                entity.HasOne(e => e.Student)
                    .WithMany(s => s.Participations)
                    .HasForeignKey(e => e.StudentID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Project)
                    .WithMany(p => p.Participations)
                    .HasForeignKey(e => e.ProjectID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Evaluation (Professor - Project N:M with additional attributes)
            modelBuilder.Entity<Evaluation>(entity =>
            {
                entity.HasKey(e => e.EvaluationID);
                entity.Property(e => e.EvaluationID).ValueGeneratedOnAdd();
                entity.Property(e => e.Score).IsRequired().HasColumnType("decimal(5,2)");
                entity.Property(e => e.Date).HasDefaultValueSql("CURRENT_DATE()");

                entity.HasOne(e => e.Professor)
                    .WithMany(p => p.Evaluations)
                    .HasForeignKey(e => e.ProfessorID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Project)
                    .WithMany(p => p.Evaluations)
                    .HasForeignKey(e => e.ProjectID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Notifies (Notification - User N:M)
            modelBuilder.Entity<Notifies>(entity =>
            {
                entity.HasKey(e => new { e.NotificationID, e.UserID });

                entity.HasOne(e => e.Notification)
                    .WithMany(n => n.Notifies)
                    .HasForeignKey(e => e.NotificationID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Notifies)
                    .HasForeignKey(e => e.UserID)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }