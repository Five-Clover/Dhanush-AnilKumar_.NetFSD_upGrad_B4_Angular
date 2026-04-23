using EMS.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.DAL.Data
{
    public class EMSDbContext : DbContext
    {
        public EMSDbContext(DbContextOptions<EMSDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserInfo> Users { get; set; }
        public DbSet<EventDetails> Events { get; set; }
        public DbSet<SpeakersDetails> Speakers { get; set; }
        public DbSet<SessionInfo> Sessions { get; set; }
        public DbSet<ParticipantEventDetails> ParticipantEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Event → Sessions
            modelBuilder.Entity<SessionInfo>()
                .HasOne(s => s.Event)
                .WithMany()
                .HasForeignKey(s => s.EventId)
                .OnDelete(DeleteBehavior.NoAction);

            // Speaker → Sessions
            modelBuilder.Entity<SessionInfo>()
                .HasOne(s => s.Speaker)
                .WithMany()
                .HasForeignKey(s => s.SpeakerId)
                .OnDelete(DeleteBehavior.NoAction);

            // Prevent duplicate EventName
            modelBuilder.Entity<EventDetails>()
                .HasIndex(e => e.EventName)
                .IsUnique();

            // Prevent duplicate SpeakerName
            modelBuilder.Entity<SpeakersDetails>()
                .HasIndex(s => s.SpeakerName)
                .IsUnique();

            // Prevent duplicate SessionTitle
            modelBuilder.Entity<SessionInfo>()
                .HasIndex(s => s.SessionTitle)
                .IsUnique();

            // Prevent duplicate Email
            modelBuilder.Entity<UserInfo>()
                .HasIndex(u => u.EmailId)
                .IsUnique();
        }
    }
}
