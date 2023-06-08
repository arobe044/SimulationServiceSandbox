using System;

namespace Sandbox.Models;

public class JobTasks
{
	public List<OrderJobResultTask>? OrderJobResultTask { get; set; }
}

public class OrderJobResultTask
{
	public string? JobTaskId { get; set; }
	public string OrderId { get; set; }
	public string? OrderLineId { get; set; }
	public string? OrderTaskId { get; set; }
	public string CustOwner { get; set; }
	public string? SiteId { get; set; }
	public string TaskStatus { get; set; }
	public string TaskType { get; set; }
	public string TaskLocation { get; set; }
	public int? TaskQty { get; set; }
	public int? ExecQty { get; set; }
	public string ExecUser { get; set; }
	public DateTime ExecDate { get; set; }
	public string ExecRobot { get; set; }
	public string? ItemNo { get; set; }
	public string? ExceptionCode { get; set; }
	public string? ExceptionReason { get; set; }
	public string? Custom1 { get; set; }
	public string? Custom2 { get; set; }
	public string? Custom3 { get; set; }
	public string? Custom4 { get; set; }
	public string? Custom5 { get; set; }
	public string? Custom6 { get; set; }
	public string? Custom7 { get; set; }
	public string? Custom8 { get; set; }
	public string? Custom9 { get; set; }
	public string? Custom10 { get; set; }
	public string? LotNo { get; set; }
	public string? SerialNo { get; set; }
	public string? ExecBarcode { get; set; }
}