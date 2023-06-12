using System;

namespace Sandbox.Models;

public class OrderJobResult_ToteInduct //: OrderJobResult  //TODO: Abstracting out changes order of serialization - does this order matter
{
		/// BASE OF ALL RESPONSES
		public string EventType { get; set; }

		public string JobId { get; set; }

		public string? JobStatus { get; set; } //docs say its required but locus docs show toteinduct without it... 

		public string JobDate { get; set; }

		/// ADDITIONAL

		public string? EventInfo { get; set; }

		public string? ToteId { get; set; }

		public string? JobRobot { get; set; }
}
