using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSB_GPI.Models;

[Table("Log")]
public class Log
{
	[Key]
	[Column("id")]
	public long Id { get; set; }

	[Column("created_at")]
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	[Column("type")]
	public string Type { get; set; } = "";

	[Column("message")]
	public string Message { get; set; } = "";

	public Log() { }

	public Log(string type, string message)
	{
		Type = type;
		Message = message;
	}
}
