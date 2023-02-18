using Microsoft.EntityFrameworkCore;
using NEWSHORE_UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_UI.DataAccess
{
  public class JourneyysContext : DbContext
  {
    public JourneyysContext(DbContextOptions<JourneyysContext> options) : base(options)
    {
    }
    public DbSet<Journeys> Journeyys { get; set; }
    public DbSet<Flight> Flights { get; set; }
    public DbSet<Transport> Transports { get; set; }
    public DbSet<JourneyyFlight> JourneyyFilghts { get; set; }
    public DbSet<Route> Routes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Journeys>().ToTable("Journeyy");
      modelBuilder.Entity<Flight>().ToTable("Flight");
      modelBuilder.Entity<Transport>().ToTable("Transport");
      modelBuilder.Entity<JourneyyFlight>().ToTable("JourneyFlight");
      modelBuilder.Entity<Route>().ToTable("Route");


      modelBuilder.Entity<Flight>()
        .HasOne(t => t.Transport)
        .WithOne(f => f.Flight)
        .HasForeignKey<Transport>(t => t.flightID);


      modelBuilder.Entity<JourneyyFlight>()
        .HasKey(f => new {f.JourneyID ,f.FlightID});

      modelBuilder.Entity<JourneyyFlight>()
        .HasOne(jf => jf.Journeys)
        .WithMany(j => j.JourneyyFlights)
        .HasForeignKey(j => j.JourneyID);

      modelBuilder.Entity<JourneyyFlight>()
          .HasOne(jf => jf.Flight)
          .WithMany(f => f.JourneyyFlights)
          .HasForeignKey(f => f.FlightID);

    }

  }
}