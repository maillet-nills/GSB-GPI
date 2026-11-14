using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using GSB_GPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GSB_GPI.Data;

public static class DbToolBox
{
	private static string? _publicIp;

	//Methods

	public static void AddLog(string type, string message, string? username = null)
	{
		using (var context = new GsbGPIDbContext())
		{
			if (_publicIp == null)
			{
				try
				{
					using (var http = new HttpClient())
					{
						http.Timeout = TimeSpan.FromSeconds(3);
						_publicIp = http.GetStringAsync("https://api.ipify.org").GetAwaiter().GetResult().Trim();
					}
				}
				catch (Exception)
				{
					_publicIp = null;
				}
			}

			if (username == null && UserSession.CurrentUser != null)
			{
				username = $"{UserSession.CurrentUser.FirstName} {UserSession.CurrentUser.LastName}";
			}

			var newLog = new Log(type, message, _publicIp, username);
			context.Logs.Add(newLog);
			context.SaveChanges();

		}
	}

	//Register & Login methods
	public static bool Register(string lastName, string firstName, string email, string password, string confirmPassword, string role)
	{
		using (var context = new GsbGPIDbContext())
		{
			bool exists = context.Users.Any(u => u.Email == email);

			if (exists)
			{
				AddLog("WARNING", $"Tentative de Register, utilisateur déjà existant : {email}");
				//Ajout d'une alternative au MessageBox en cas de fail ? (Utilisateur déjà existant !)
				return false;
			}
			else if (password != confirmPassword)
			{
				AddLog("WARNING", $"Tentative de Register échouée, mots de passe différents : {email}", $"{firstName} {lastName}");
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
				context.Users.Add(newTechnician);
			}

			else
			{
				var newUser = new User
				{
					Email = email,
					HashedPassword = hashedPassword,
					Role = role,
					LastName = lastName,
					FirstName = firstName,

				};
				context.Users.Add(newUser);
			}

			context.SaveChanges();

			if (UserSession.CurrentUser != null)
			{
				AddLog("INFO", $"Nouvel utilisateur créé : {email}, rôle {role}, par {UserSession.CurrentUser.Email}");
			}
			else
			{
				AddLog("INFO", $"Nouvel utilisateur créé par inscription : {email}, rôle {role}", $"{firstName} {lastName}");
			}

			return true;

		}
	}

	public static bool Login(string email, string password)
	{
		using (var context = new GsbGPIDbContext())
		{
			var user = context.Users.FirstOrDefault(u => u.Email == email);

			if (user == null)
			{
				AddLog("WARNING", $"Tentative de Login échoué, utilisateur non existant : {email}");
				//Ajout d'une alternative au MessageBox en cas de fail ? (Utilisateur innexistant !)
				return false;
			}

			bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.HashedPassword);
			if (isPasswordValid)
			{
				AddLog("INFO", $"Login réussi, utilisateur : {email}", $"{user.FirstName} {user.LastName}");
				UserSession.CurrentUser = user;
			}
			else
			{
				AddLog("WARNING", $"Tentative de Login échoué, mauvais mot de passe : {email}", $"{user.FirstName} {user.LastName}");
			}

			return isPasswordValid;

		}
	}

	//Users & Technicians & Equipments getters

	public static List<User> getAllUsers()
	{
		using (var context = new GsbGPIDbContext())
		{
			return context.Users.ToList();
		}
	}

	public static List<Technician> getAllTechnicians()
	{
		using (var context = new GsbGPIDbContext())
		{
			return context.Technicians.ToList();
		}
	}

	//Equipments methods

	//Create equipment
	public static void CreateEquipment(
		string idGsb, string brand, string model, DateTime purchaseDate,
		bool rental, bool guarantee, string provider, string type,
		string? processor, string version, int? ram, int diskVolumeInt, string? os, string selectedtype, User? userselected)
	{
		using (var db = new GsbGPIDbContext())
		{
			var existingUser = userselected == null ? null : db.Users.Find((userselected?.IdUser));

			Equipment equipment = selectedtype switch
			{
				"Équipement réseau" => new NetEquipment(),
				"Serveur" => new Server(),
				"Poste de travail" => new Workstation(),
				_ => new Equipment()
			};

			equipment.IdGsb = idGsb;
			equipment.Brand = brand;
			equipment.Model = model;
			equipment.PurchaseDate = DateTime.SpecifyKind(purchaseDate, DateTimeKind.Utc);
			equipment.Rental = rental;
			equipment.Guarantee = guarantee;
			equipment.Provider = provider;
			equipment.User = existingUser;

			switch (equipment)
			{
				case Server server:
					server.DiskVolume = diskVolumeInt;
					server.Version = version;
					server.Ram = ram;
					break;

				case NetEquipment netEquipment:
					netEquipment.Type = type;
					break;

				case Workstation workstation:
					workstation.Processor = processor;
					workstation.Os = os;
					workstation.Ram = ram;
					workstation.DiskVolume = diskVolumeInt;
					break;
			}

			db.Add(equipment);
			db.SaveChanges();
			AddLog("INFO", $"Équipement créé : {idGsb} ({brand} {model}), type {selectedtype}, par {UserSession.CurrentUser?.Email}");

			if (existingUser != null)
			{
				AddLog("INFO", $"Matériel {idGsb} attribué à {existingUser.Email} par {UserSession.CurrentUser?.Email}");
			}
		}
	}

	//Equipment select
	public static List<Equipment> LoadEquipments()
	{
		var EquipmentsLoaded = new List<Equipment>();

		using (var context = new GsbGPIDbContext())
		{
			if (UserSession.CurrentUser.Role.Contains("Technicien") || UserSession.CurrentUser.Role.Contains("Admin"))
			{
				 EquipmentsLoaded = context.Equipments
					 .Include(u => u.User)
					 .ToList();
			}
			else
			{
				EquipmentsLoaded = context.Equipments
					.Include(u => u.User)
					.Where(e=>e.User.IdUser==UserSession.CurrentUser.IdUser)
					.ToList();
			}

			return EquipmentsLoaded;
		}
	}

	//Ticket methods

	//Create ticket
	public static void CreateTicket(string subject, User user, Technician? technician, Urgency urgency, State state, Level level,Equipment? equipment)
	{
		using (var db = new GsbGPIDbContext())
		{

			var existingUser = db.Users.Find((user?.IdUser));
			var existingTechnician = technician == null ? null : db.Technicians.Find(technician.IdUser);
			var existingEquipment = equipment == null ? null : db.Equipments.Find(equipment.Id);

			if (existingUser == null )
			{
				//Ajout d'une MessageBox bientot (normalement ce if devrait jamais arrivé c'est quasi impossible mais vaut mieut prévenir que guérir.)

				AddLog("ERROR", "Création de ticket impossible, l'utilisateur sélectionné ne figure plus dans la BD");
				return;
			}

			var ticket = new Ticket
			{
				Subject = subject,
				User = existingUser,
				Technician = existingTechnician,
				NvUrgency = urgency,
				State = state,
				InterventionLevel = level,
				Timestamp = DateTime.UtcNow,
				Equipment = existingEquipment,

			};

			db.Tickets.Add(ticket);
			db.SaveChanges();
			AddLog("INFO", $"Ticket créé : #{ticket.IdTicket} {subject}, pour {existingUser.Email}, équipement {existingEquipment?.IdGsb ?? "aucun"}, urgence {urgency}, niveau {level}, état {state}, par {UserSession.CurrentUser?.Email}");

			if (existingTechnician != null)
			{
				AddLog("INFO", $"Ticket #{ticket.IdTicket} assigné au technicien {existingTechnician.Email} par {UserSession.CurrentUser?.Email}");
			}
		}
	}

	//Tickets select
	public static List<Ticket> LoadTickets()
	{
		var TicketsLoaded = new List<Ticket>();

		using (var context = new GsbGPIDbContext())
		{
			if (UserSession.CurrentUser.Role.Contains("Technicien") || UserSession.CurrentUser.Role.Contains("Admin"))
			{
				TicketsLoaded = context.Tickets
					.Include(t => t.User)
					.Include(t => t.Technician)
					.Include(t => t.Equipment)
					.ToList();
			}
			else
			{
				TicketsLoaded = context.Tickets
					.Include(t => t.User)
					.Include(t => t.Technician)
					.Include(t => t.Equipment)
					.Where(e => e.User.IdUser == UserSession.CurrentUser.IdUser)
					.ToList();
			}

			return TicketsLoaded;
		}
	}

	//Tickets & User & Equipment remover methods

	public static void DeleteTicket(Ticket SelectedTicket)
	{
		using (var db = new GsbGPIDbContext())
		{
			db.Tickets.Remove(SelectedTicket);
			db.SaveChanges();
			AddLog("WARNING", $"Ticket supprimé : #{SelectedTicket.IdTicket} {SelectedTicket.Subject}, état {SelectedTicket.State}, pour {SelectedTicket.User?.Email}, par {UserSession.CurrentUser?.Email}");
		}
	}

	public static void DeleteUser(User SelectedUser)
	{
		using (var db = new GsbGPIDbContext())
		{
			db.Users.Remove(SelectedUser);
			db.SaveChanges();
			AddLog("WARNING", $"Utilisateur supprimé : {SelectedUser.Email}, rôle {SelectedUser.Role}, par {UserSession.CurrentUser?.Email}");
		}
	}

	public static void DeleteEquipment(Equipment SelectedEquipment)
	{
		using (var db = new GsbGPIDbContext())
		{
			db.Equipments.Remove(SelectedEquipment);
			db.SaveChanges();
			AddLog("WARNING", $"Équipement supprimé : {SelectedEquipment.IdGsb} ({SelectedEquipment.Brand} {SelectedEquipment.Model}), attribué à {SelectedEquipment.User?.Email ?? "personne"}, par {UserSession.CurrentUser?.Email}");
		}
	}

	//Edit Methods------

	//Equipment---

	//Load--
	public static void LoadEditEquipmentForm(
		Equipment selectedEquipment,
		ref string idGsb,
		ref string brand,
		ref string model,
		ref bool rental,
		ref bool guarantee,
		ref string provider,
		ref User userSelected,

		//Child class atributes
		ref string? netType,
		ref string? version,
		ref string? processor,
		ref long? ram,
		ref int? diskVolumeInt,
		ref string? diskVolumeString,
		ref string? os)
	{
		if (selectedEquipment == null) return;

		idGsb = selectedEquipment.IdGsb;
		brand = selectedEquipment.Brand;
		model = selectedEquipment.Model;
		rental = selectedEquipment.Rental;
		guarantee = selectedEquipment.Guarantee;
		provider = selectedEquipment.Provider;
		userSelected = selectedEquipment.User;

		switch (selectedEquipment)
		{
			case Server server:
				version = server.Version;
				processor = server.Processor;
				ram = server.Ram;
				diskVolumeInt = server.DiskVolume;
				break;

			case Workstation workstation:
				processor = workstation.Processor;
				os = workstation.Os;
				ram = workstation.Ram;
				diskVolumeString = workstation.DiskVolume.ToString();
				break;

			case NetEquipment netEquipment:
				netType = netEquipment.Type;
				break;
		}


	}

	//Edit--
	public static void UpdateEquipment(
		Equipment selectedEquipment,
		string idGsb,
		string brand,
		string model,
		bool rental,
		bool guarantee,
		string provider,
		User? userSelected,
		string? netType,
		string? version,
		string? processor,
		int? ram,
		int diskVolumeInt,
		string? os)

	{
		using (var context = new GsbGPIDbContext())
		{
			var equipment = context.Equipments.Include(e => e.User).FirstOrDefault(e => e.Id == selectedEquipment.Id);
			if (equipment == null)
			{
				AddLog("ERROR", $"Modification impossible, équipement introuvable dans la BD : {selectedEquipment.IdGsb}");
				return;
			}

			var oldUser = equipment.User;
			var existingUser = userSelected != null ? context.Users.Find(userSelected.IdUser) : null;

			//save change si ancienne valeur (en base) est différente de la nouvelle
			var changes = new List<string>();
			
			if (equipment.IdGsb != idGsb) changes.Add($"IdGsb : {equipment.IdGsb} -> {idGsb}");
			if (equipment.Brand != brand) changes.Add($"Brand : {equipment.Brand} -> {brand}");
			if (equipment.Model != model) changes.Add($"Model : {equipment.Model} -> {model}");
			if (equipment.Rental != rental) changes.Add($"Rental : {equipment.Rental} -> {rental}");
			if (equipment.Guarantee != guarantee) changes.Add($"Guarantee : {equipment.Guarantee} -> {guarantee}");
			if (equipment.Provider != provider) changes.Add($"Provider : {equipment.Provider} -> {provider}");

			equipment.IdGsb = idGsb;
			equipment.Brand = brand;
			equipment.Model = model;
			equipment.Rental = rental;
			equipment.Guarantee = guarantee;
			equipment.Provider = provider;
			equipment.User = existingUser;

			switch (equipment)
			{
				case Server server:
					
					if (server.Version != version) changes.Add($"Version : {server.Version} -> {version}");
					if (server.Processor != processor) changes.Add($"Processor : {server.Processor} -> {processor}");
					if (server.Ram != ram) changes.Add($"Ram : {server.Ram} -> {ram}");
					if (server.DiskVolume != diskVolumeInt) changes.Add($"DiskVolume : {server.DiskVolume} -> {diskVolumeInt}");
					
					server.Version = version;
					server.Processor = processor;
					server.Ram = ram;
					server.DiskVolume = diskVolumeInt;
					break;

				case Workstation workstation:
					
					if (workstation.Processor != processor) changes.Add($"Processor : {workstation.Processor} -> {processor}");
					if (workstation.Os != os) changes.Add($"Os : {workstation.Os} -> {os}");
					if (workstation.Ram != ram) changes.Add($"Ram : {workstation.Ram} -> {ram}");
					if (workstation.DiskVolume != diskVolumeInt) changes.Add($"DiskVolume : {workstation.DiskVolume} -> {diskVolumeInt}");

					workstation.Processor = processor;
					workstation.Os = os;
					workstation.Ram = ram;
					workstation.DiskVolume = diskVolumeInt;
					break;

				case NetEquipment netEquipment:
					if (netEquipment.Type != netType) changes.Add($"Type : {netEquipment.Type} -> {netType}");
					netEquipment.Type = netType;
					break;
			}

			context.SaveChanges();

			//Pas de log si rien n'a changé
			if (changes.Count > 0)
			{
				AddLog("INFO", $"Équipement modifié : {idGsb} ({string.Join(", ", changes)}), par {UserSession.CurrentUser?.Email}");
			}

			//Changement d'attribution (séparé du reste)
			if (oldUser?.IdUser != existingUser?.IdUser)
			{
				if (oldUser == null)
				{
					AddLog("INFO", $"Matériel {idGsb} attribué à {existingUser.Email} par {UserSession.CurrentUser?.Email}");
				}
				else if (existingUser == null)
				{
					AddLog("INFO", $"Matériel {idGsb} retiré à {oldUser.Email} par {UserSession.CurrentUser?.Email}");
				}
				else
				{
					AddLog("INFO", $"Matériel {idGsb} réattribué de {oldUser.Email} à {existingUser.Email} par {UserSession.CurrentUser?.Email}");
				}
			}

		}}

	//Tickets---

	//Load--

	public static void LoadEditTicketForm(
		Ticket selectedTicket,
		ref string subject,
		ref Equipment? Selectedequipment,
		ref User? Selecteduser,
		ref Technician? SelectedTechnician,
		ref Urgency SelectedUrgency,
		ref State SelectedState,
		ref Level SelectedLevel)
	{
		if (selectedTicket == null) return;

		subject = selectedTicket.Subject;
		Selectedequipment = selectedTicket.Equipment;
		Selecteduser = selectedTicket.User;
		SelectedTechnician = selectedTicket.Technician;
		SelectedUrgency = selectedTicket.NvUrgency;
		SelectedState = selectedTicket.State;
		SelectedLevel = selectedTicket.InterventionLevel;
	}

	//Edit--

	public static void UpdateTicket(
		Ticket SelectedTicket,
		string subject,
		Equipment? Selectedequipment,
		User? Selecteduser,
		Technician? SelectedTechnician,
		Urgency SelectedUrgency,
		State SelectedState,
		Level SelectedLevel)
	{
		using (var context = new GsbGPIDbContext())
		{
			var ticket = context.Tickets
				.Include(t => t.User)
				.Include(t => t.Technician)
				.Include(t => t.Equipment)
				.FirstOrDefault(t => t.IdTicket == SelectedTicket.IdTicket);
			if (ticket == null)
			{
				AddLog("ERROR", $"Modification impossible, ticket introuvable dans la BD : #{SelectedTicket.IdTicket}");
				return;
			}

			var oldUser = ticket.User;
			var oldTechnician = ticket.Technician;
			var oldEquipment = ticket.Equipment;

			var existingEquipment = Selectedequipment != null ? context.Equipments.Find(Selectedequipment.Id) : null;
			var existingUser = Selecteduser != null ? context.Users.Find(Selecteduser.IdUser) : null;
			var existingTechnician = SelectedTechnician != null ? context.Technicians.Find(SelectedTechnician.IdUser) : null;

			//save change si ancienne valeur différente de la nouvelle
			var changes = new List<string>();
			
			if (ticket.Subject != subject) changes.Add($"Subject : {ticket.Subject} -> {subject}");
			if (ticket.NvUrgency != SelectedUrgency) changes.Add($"NvUrgency : {ticket.NvUrgency} -> {SelectedUrgency}");
			if (ticket.State != SelectedState) changes.Add($"State : {ticket.State} -> {SelectedState}");
			if (ticket.InterventionLevel != SelectedLevel) changes.Add($"InterventionLevel : {ticket.InterventionLevel} -> {SelectedLevel}");
			if (oldUser?.IdUser != existingUser?.IdUser) changes.Add($"utilisateur : {oldUser?.Email ?? "aucun"} -> {existingUser?.Email ?? "aucun"}");
			if (oldEquipment?.Id != existingEquipment?.Id) changes.Add($"équipement : {oldEquipment?.IdGsb ?? "aucun"} -> {existingEquipment?.IdGsb ?? "aucun"}");

			ticket.Subject = subject;
			ticket.Equipment = existingEquipment;
			ticket.User = existingUser;
			ticket.Technician = existingTechnician;
			ticket.NvUrgency = SelectedUrgency;
			ticket.State = SelectedState;
			ticket.InterventionLevel = SelectedLevel;

			context.SaveChanges();

			if (changes.Count > 0)
			{
				AddLog("INFO", $"Ticket modifié : #{ticket.IdTicket} ({string.Join(", ", changes)}), par {UserSession.CurrentUser?.Email}");
			}

			//Assignation du technicien (séparé du reste)
			if (oldTechnician?.IdUser != existingTechnician?.IdUser)
			{
				if (oldTechnician == null)
				{
					AddLog("INFO", $"Ticket #{ticket.IdTicket} assigné au technicien {existingTechnician.Email} par {UserSession.CurrentUser?.Email}");
				}
				else if (existingTechnician == null)
				{
					AddLog("INFO", $"Ticket #{ticket.IdTicket} retiré au technicien {oldTechnician.Email} par {UserSession.CurrentUser?.Email}");
				}
				else
				{
					AddLog("INFO", $"Ticket #{ticket.IdTicket} réassigné de {oldTechnician.Email} à {existingTechnician.Email} par {UserSession.CurrentUser?.Email}");
				}
			}
		}
	}

	//Verification methods to grant access

	public static bool IsTechnician()
	{
		if (UserSession.CurrentUser.Role.Contains("Technicien") || UserSession.CurrentUser.Role.Contains("Admin"))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public static bool IsAdmin()
	{
		if (UserSession.CurrentUser.Role.Contains("Admin"))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	}



