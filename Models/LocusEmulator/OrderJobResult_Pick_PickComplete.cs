using System;

namespace Sandbox.Models;

public class OrderJobResult_Pick_PickComplete //: OrderJobResult  //TODO: Abstracting out changes order of serialization - does this order matter
{
		/// BASE OF ALL RESPONSES
		public string EventType { get; set; }

		public string JobId { get; set; }

		public string? JobStatus { get; set; } //docs say its required but locus docs show toteinduct without it... 

		public string JobDate { get; set; }

		/// ADDITIONAL

		public string? EventInfo { get; set; }
		
		public string? JobStation { get; set; }

		public string? ToteId { get; set; }

		public string? JobRobot { get; set; }

		public string? JobMethod { get; set; }

		public JobTasks? JobTasks { get; set; }
}