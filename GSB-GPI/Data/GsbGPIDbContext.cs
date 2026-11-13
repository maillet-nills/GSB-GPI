using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GSB_GPI.Models;
using Microsoft.EntityFrameworkCore;
using Tmds.DBus.Protocol;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace GSB_GPI.Data;

public class GsbGPIDbContext : DbContext
{

	public DbSet<Equipment> Equipments { get; set; }
	public DbSet<Server> Servers { get; set; }
	public DbSet<NetEquipment>  NetEquipments { get; set; }
	public DbSet<Workstation> workstations { get; set; }

	public DbSet<User> Users { get; set; }
	public DbSet<Technician> Technicians  { get; set; }

	public DbSet<Ticket> Tickets { get; set; }
	public DbSet<Log> Logs { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		string connectionstring = "Host=aws-0-eu-west-1.pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.jtojtxlpznkgmwgbdxhm;Password=mQNxiRgKhdttXsgPddGP;No Reset On Close=true;";
		optionsBuilder.UseNpgsql(connectionstring);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		//Conversion to avoid type conflict with the DB

		//Technician table
		modelBuilder.Entity<Technician>()
			.Property(t => t.InterventionLevel)
			.HasConversion<string>();

		//Ticket table
		modelBuilder.Entity<Ticket>()
			.Property(t => t.InterventionLevel)
			.HasConversion<string>();

		modelBuilder.Entity<Ticket>()
			.Property(t=>t.State)
			.HasConversion<string>();

		modelBuilder.Entity<Ticket>()
			.Property((t=> t.NvUrgency))
			.HasConversion<string>();

		//Foregin keys

		modelBuilder.Entity<Ticket>()
			.HasOne(t => t.User)
			.WithMany()
			.HasForeignKey("idUser");

		modelBuilder.Entity<Ticket>()
			.HasOne(t => t.Equipment)
			.WithMany()
			.HasForeignKey("idEquipment");

		modelBuilder.Entity<Ticket>()
			.HasOne(t => t.Technician)
			.WithMany()
			.HasForeignKey("idTechnician");

		modelBuilder.Entity<Equipment>()
			.HasOne(t=>t.User)
			.WithMany()
			.HasForeignKey("idUser");
	}
	}









