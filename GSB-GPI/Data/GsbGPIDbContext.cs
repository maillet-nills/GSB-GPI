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
	public DbSet<Log> Logs { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		string connectionstring = "Host=aws-0-eu-west-1.pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.jtojtxlpznkgmwgbdxhm;Password=mQNxiRgKhdttXsgPddGP;No Reset On Close=true;";
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

	public void AddLog(string type, string message)
	{
		var newLog = new Log(type, message);
		Logs.Add(newLog);
		SaveChanges();
	}

	public bool Register(string lastName, string firstName, string email, string password, string confirmPassword, string role)
	{
		bool exists = Users.Any(u => u.Email == email);

		if (exists)
		{
			AddLog("WARNING", $"Tentative de Register, utilisateur déjà existant : {email}");
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
			AddLog("INFO", $"Nouvel utilisateur Technicien créé : {email}");
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
			AddLog("INFO", $"Nouvel utilisateur créé : {email}");
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
			AddLog("WARNING", $"Tentative de Login échoué, utilisateur non existant : {email}");
			//Ajout d'une alternative au MessageBox en cas de fail ? (Utilisateur innexistant !)
			return false;
		}

		bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.HashedPassword);
		if (isPasswordValid)
		{
			AddLog("INFO", $"Login réussi, utilisateur : {email}");
		}
		else
		{
			AddLog("WARNING", $"Tentative de Login échoué, mauvais mot de passe : {email}");
		}

		return isPasswordValid;
	}






}
