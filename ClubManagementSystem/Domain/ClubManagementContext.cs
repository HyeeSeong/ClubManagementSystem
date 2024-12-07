namespace ClubManagementSystem.Domain;
using Microsoft.EntityFrameworkCore;
using System;

public class ClubManagementContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Professor> Professors { get; set; } = null!;
        public DbSet<Staff> Staffs { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Club> Clubs { get; set; } = null!;
        public DbSet<ClubRoom> ClubRooms { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Report> Reports { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<Evaluation> Evaluations { get; set; } = null!;
        public DbSet<Participation> Participations { get; set; } = null!;
        public DbSet<Notifies> Notifies { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // MySQL 연동 예시
            optionsBuilder.UseMySql("Server=YOUR_SERVER;Database=club_management;User=YOUR_USER;Password=YOUR_PASSWORD;",
                new MySqlServerVersion(new Version(8, 0, 21)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TPT 전략 설정
            // Base Class: User
            modelBuilder.Entity<User>()
                .ToTable("Users")
                .HasKey(u => u.UserID);

            modelBuilder.Entity<Professor>()
                .ToTable("Professors")
                .HasBaseType<User>();

            modelBuilder.Entity<Staff>()
                .ToTable("Staffs")
                .HasBaseType<User>();

            modelBuilder.Entity<Student>()
                .ToTable("Students")
                .HasBaseType<User>();

            // Club
            modelBuilder.Entity<Club>()
                .HasKey(c => c.ClubID);
            modelBuilder.Entity<Club>()
                .HasOne(c => c.ClubRoom)
                .WithMany(r => r.Clubs)
                .HasForeignKey(c => c.ClubRoomID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Club>()
                .HasOne(c => c.Professor)
                .WithMany() // Professor가 Clubs 컬렉션을 가질 필요 없다면
                .HasForeignKey(c => c.ProfessorID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Club>()
                .HasOne(c => c.Staff)
                .WithMany() // Staff가 Clubs 컬렉션을 가질 필요 없다면
                .HasForeignKey(c => c.StaffID)
                .OnDelete(DeleteBehavior.Restrict);

            // ClubRoom
            modelBuilder.Entity<ClubRoom>()
                .HasKey(r => r.ClubRoomID);

            // Project
            modelBuilder.Entity<Project>()
                .HasKey(p => p.ProjectID);
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Club)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.ClubID)
                .OnDelete(DeleteBehavior.Cascade);

            // Report
            modelBuilder.Entity<Report>()
                .HasKey(r => r.ReportID);
            modelBuilder.Entity<Report>()
                .HasOne(r => r.Project)
                .WithMany(p => p.Reports)
                .HasForeignKey(r => r.ProjectID)
                .OnDelete(DeleteBehavior.Cascade);

            // Participation (Student - Project N:M)
            modelBuilder.Entity<Participation>()
                .HasKey(pa => new { pa.StudentID, pa.ProjectID });
            modelBuilder.Entity<Participation>()
                .HasOne(pa => pa.Student)
                .WithMany(s => s.Participations)
                .HasForeignKey(pa => pa.StudentID)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Participation>()
                .HasOne(pa => pa.Project)
                .WithMany(p => p.Participations)
                .HasForeignKey(pa => pa.ProjectID)
                .OnDelete(DeleteBehavior.Cascade);

            // Evaluation (Professor - Project N:M + 속성)
            modelBuilder.Entity<Evaluation>()
                .HasKey(e => e.EvaluationID);
            modelBuilder.Entity<Evaluation>()
                .HasOne(e => e.Professor)
                .WithMany()
                .HasForeignKey(e => e.ProfessorID)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Evaluation>()
                .HasOne(e => e.Project)
                .WithMany(p => p.Evaluations)
                .HasForeignKey(e => e.ProjectID)
                .OnDelete(DeleteBehavior.Cascade);

            // Notification
            modelBuilder.Entity<Notification>()
                .HasKey(n => n.AnnounceID);

            // Notifies (Notification - User N:M)
            modelBuilder.Entity<Notifies>()
                .HasKey(n => new { n.NotificationID, n.UserID });
            modelBuilder.Entity<Notifies>()
                .HasOne(n => n.Notification)
                .WithMany(notif => notif.Notifies)
                .HasForeignKey(n => n.NotificationID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notifies>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifies)
                .HasForeignKey(n => n.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }