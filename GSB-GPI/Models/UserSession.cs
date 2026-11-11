namespace GSB_GPI.Models;

public class UserSession
{
	public static User? CurrentUser { get; set; }
	public static Technician? CurrentTechnician { get; set; }
}
