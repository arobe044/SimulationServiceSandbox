using System;

namespace Sandbox.Models;

public class OrderJobResult_Pick_PickComplete : OrderJobResult
{
		public string? EventInfo { get; set; }

		public string? JobStation { get; set; }

		public string? ToteId { get; set; }

		public string? JobRobot { get; set; }

		public string? JobMethod { get; set; }

		//public JobTasks? JobTasks { get; set; }
}