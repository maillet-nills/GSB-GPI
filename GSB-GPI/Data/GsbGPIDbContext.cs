using System.ComponentModel.DataAnnotations;
using System.Linq;
using GSB_GPI.Models;
using Microsoft.EntityFrameworkCore;
using Tmds.DBus.Protocol;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace GSB_GPI.Data;

public class GsbGPIDbContext : DbContext
{

	public DbSet<Equipment> Materiels { get; set; }
	public DbSet<Server> Servers { get; set; }
	public DbSet<NetEquipment>  NetEquipments { get; set; }
	public DbSet<Workstation> workstations { get; set; }

	public DbSet<User> Users { get; set; }
	public DbSet<Technician> Technicians  { get; set; }

	public DbSet<Ticket> Tickets { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		string connectionstring = "Host=db.jtojtxlpznkgmwgbdxhm.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=mQNxiRgKhdttXsgPddGP";
		optionsBuilder.UseNpgsql(connectionstring);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Technician>()
			.Property(t => t.InterventionLevel)
			.HasConversion<string>();
	}

	//Methods

	public bool Register(string lastName, string firstName, string email, string password, string confirmPassword, string role)
	{
		bool exists = Users.Any(u => u.Email == email);

		if (exists)
		{
			//Ajout d'une alternative au MessageBox en cas de fail ? (Utilisateur déjà existant !)
			return false;
		}
		else if (password != confirmPassword)
		{
			//Ajout d'une alternative au MessageBox en cas de fail ? (Utilisateur déjà existant !)
			return false;
		}

		string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

		Level? interventionLevel = null; // Valeur par défaut

		switch (role)
		{
			case "Technicien N1":
				interventionLevel = Level.N1;
				break;
			case "Technicien N1.5":
				interventionLevel = Level.N1_5;
				break;
			case "Technicien N2":
				interventionLevel = Level.N2;
				break;
			case "Technicien N3":
				interventionLevel = Level.N3;
				break;
		}

		if (role.Contains("Technicien"))
		{
			var newTechnician = new Technician()
			{
				Email = email,
				HashedPassword = hashedPassword,
				Role = role,
				InterventionLevel = interventionLevel.Value,
				LastName = lastName,
				FirstName = firstName

			};
			Users.Add(newTechnician);
		}

		else
		{
			var newUser = new User
			{
				Email = email,
				HashedPassword = hashedPassword,
				Role = role,
				LastName = "Inconnu",
				FirstName = "Inconnu"

			};
			Users.Add(newUser);
		}

		SaveChanges();
		return true;

	}

	public bool Login(string email, string password)
	{
		var user = Users.FirstOrDefault(u => u.Email == email);

		if (user == null)
		{
			//Ajout d'une alternative au MessageBox en cas de fail ? (Utilisateur innexistant !)
			return false;
		}

		bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.HashedPassword);

		return isPasswordValid;
	}






}
