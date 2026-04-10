
using BookingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Data
{
    public class BookingSystemContext : DbContext
    {
        public BookingSystemContext(DbContextOptions<BookingSystemContext> options)
            : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Prescription> Prescription  { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<ConsultationRecord>  ConssultationRecords { get; set; }
    }
}