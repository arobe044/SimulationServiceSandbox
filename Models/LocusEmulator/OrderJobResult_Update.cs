using System;

namespace Sandbox.Models;

//TODO: add optional values for other result types, abstract out to base class 
public class OrderJobResult_Update : OrderJobResult
{
		public string? RequestId { get; set; }

		public string? EventInfo { get; set; }
}
