using System;
using Sandbox.Entities;

namespace Sandbox.Models;

public class TaskData
{
	public OrderJobTask Task { get; set; } = new OrderJobTask();
	public string RequestData { get; set; } = "";
}