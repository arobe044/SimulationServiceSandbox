using System;

namespace Sandbox.Models;

//TODO: add optional values for other result types, abstract out to base class 
public class OrderJobResult
{
		public string EventType { get; set; }

		public string JobId { get; set; }

		public string? JobStatus { get; set; } //docs say its required but locus docs show toteinduct without it... 

		public string JobDate { get; set; }

}