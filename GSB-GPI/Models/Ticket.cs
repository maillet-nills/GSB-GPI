using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSB_GPI.Models;

[Table("Ticket")]
public class Ticket
{
	[Key]
	[Column("id_ticket")]
	public int IdTicket { get; set; }

	[Column("object")]
	public string Subject { get; set; }

	[Column("emergency_level")]
	public Urgency NvUrgency { get; set; }

	[Column("state")]
	public State State { get; set; }

	[Column("timestamp")]
	public DateTime Timestamp { get; set; }

	[Column("intervention_level")]
	public Level InterventionLevel { get; set; }

	public User User { get; set; }
	public Equipment? Equipment { get; set; }
	public Technician Technician { get; set; }

    public Ticket(){}
    public Ticket(int idTicket, string subject, Urgency nvUrgency, State state, Equipment equipment, User user, Technician technician, Level interventionLevel)
    {
        IdTicket = idTicket;
        Subject = subject;
        NvUrgency = nvUrgency;
        State = state;
        Timestamp = DateTime.UtcNow;
        Equipment = equipment;
        User = user;
        Technician = technician;
        InterventionLevel = interventionLevel;
    }



}
