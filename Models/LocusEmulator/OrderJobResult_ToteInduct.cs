using System;

namespace Sandbox.Models;

//TODO: add optional values for other result types, abstract out to base class 
public class OrderJobResult_ToteInduct : OrderJobResult
{
		public string? EventInfo { get; set; }

		public string? ToteId { get; set; }

		public string? JobRobot { get; set; }
}
