using System;

namespace GSB_GPI.Models;

public class Ticket
{
    public int IdTicket { get; set; }
    public string Subject { get; set; }
    public Urgency NvUrgency { get; set; }
    public State State { get; set; }
    public DateTime Timestamp { get; set; }
    public Equipment Equipment { get; set; }
    public User User { get; set; }
    public Technician Technician { get; set; }

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
