using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSB_GPI.Models;

[Table("Ticket")]
public class Ticket
{
	[Key]
    public int IdTicket { get; set; }
    public string Subject { get; set; }
    public Urgency NvUrgency { get; set; }
    public State State { get; set; }
    public DateTime Timestamp { get; set; }
    public Equipment Equipment { get; set; }
    public User User { get; set; }
    public Technician Technician { get; set; }

    public Ticket(){}
    public Ticket(int idTicket, string subject, Urgency nvUrgency, State state, DateTime timestamp, Equipment equipment, User user, Technician technician)
    {
        IdTicket = idTicket;
        Subject = subject;
        NvUrgency = nvUrgency;
        State = state;
        Timestamp = timestamp;
        Equipment = equipment;
        User = user;
        Technician = technician;
    }



}
