using System;

namespace Sandbox.Models;

public class OrderJobResult
{
		public string EventType { get; set; }

		public string JobId { get; set; }

		public string? JobStatus { get; set; } //docs say its required but locus docs show toteinduct without it... 

		public string JobDate { get; set; }

}