using System.Text.Json.Serialization;

namespace Sandbox.Entities;

public class OrderJobTask {
    
    [JsonPropertyName("JobTaskId")]
    public string? JobTaskId { get; set; }
    
    [JsonPropertyName("OrderId")]
    public string? OrderId { get; set; }
    
    [JsonPropertyName("OrderLineId")]
    public string? OrderLineId { get; set; }
    
    [JsonPropertyName("OrderTaskId")]
    public string? OrderTaskId { get; set; }
    
    [JsonPropertyName("CustOwner")]
    public string? CustOwner { get; set; }
    
    [JsonPropertyName("SiteId")]
    public string? SiteId { get; set; }
    
    [JsonPropertyName("TaskType")]
    public string? TaskType { get; set; }
    
    [JsonPropertyName("TaskSequence")]
    public string? TaskSequence { get; set; }

    [JsonPropertyName("TaskSubSequence")]
    public string? TaskSubSequence { get; set; }
    
    [JsonPropertyName("TaskTravelPriority")]
    public string? TaskTravelPriority { get; set; }
    
    [JsonPropertyName("TaskLocation")]
    public string? TaskLocation { get; set; }
    
    [JsonPropertyName("TaskZone")]
    public string? TaskZone { get; set; }
    
    [JsonPropertyName("TaskWorkArea")]
    public string? TaskWorkArea { get; set; }
    
    [JsonPropertyName("TaskQty")]
    public string? TaskQty { get; set; }
    
    [JsonPropertyName("ItemNo")]
    public string? ItemNo { get; set; }
    
    [JsonPropertyName("ItemUPC")]
    public string? ItemUPC { get; set; }
    
    [JsonPropertyName("ItemDesc")]
    public string? ItemDesc { get; set; }
    
    [JsonPropertyName("ItemStyle")]
    public string? ItemStyle { get; set; }
    
    [JsonPropertyName("ItemColor")]
    public string? ItemColor { get; set; }
    
    [JsonPropertyName("ItemSize")]
    public string? ItemSize { get; set; }
    
    [JsonPropertyName("ItemLength")]
    public string? ItemLength { get; set; }
    
    [JsonPropertyName("ItemWidth")]
    public string? ItemWidth { get; set; }
    
    [JsonPropertyName("ItemHeight")]
    public string? ItemHeight { get; set; }
    
    [JsonPropertyName("ItemWeight")]
    public string? ItemWeight { get; set; }
    
    [JsonPropertyName("ItemImageUrl")]
    public string? ItemImageUrl { get; set; }
    
    [JsonPropertyName("Custom1")]
    public string? Custom1 { get; set; }
    
    [JsonPropertyName("Custom2")]
    public string? Custom2 { get; set; }
    
    [JsonPropertyName("Custom3")]
    public string? Custom3 { get; set; }
    
    [JsonPropertyName("Custom4")]
    public string? Custom4 { get; set; }
    
    [JsonPropertyName("Custom5")]
    public string? Custom5 { get; set; }
    
    [JsonPropertyName("Custom6")]
    public string? Custom6 { get; set; }
    
    [JsonPropertyName("Custom7")]
    public string? Custom7 { get; set; }
    
    [JsonPropertyName("Custom8")]
    public string? Custom8 { get; set; }
    
    [JsonPropertyName("Custom9")]
    public string? Custom9 { get; set; }
    
    [JsonPropertyName("Custom10")]
    public string? Custom10 { get; set; }
    
    [JsonPropertyName("LotNo")]
    public string? LotNo { get; set; }
    
    [JsonPropertyName("SerialNo")]
    public string? SerialNo { get; set; }
}

public class JobTasks {
    
    [JsonPropertyName("OrderJobTask")]
    public List<OrderJobTask> OrderJobTask { get; set; }
}

public class OrderJobRequest {
    
    [JsonPropertyName("EventType")]
    public string? EventType { get; set; }
    
    [JsonPropertyName("JobId")]
    public string? JobId { get; set; }
    
    [JsonPropertyName("JobDate")]
    public string? JobDate { get; set; }
    
    [JsonPropertyName("JobPriority")]
    public string? JobPriority { get; set; }
    
    [JsonPropertyName("JobPriorityGroup")]
    public string? JobPriorityGroup { get; set; }
    
    [JsonPropertyName("RequestId")]
    public string? RequestId { get; set; }
    
    [JsonPropertyName("SingleUnit")]
    public string? SingleUnit { get; set; }
    
    [JsonPropertyName("JobTasks")]
    public JobTasks JobTasks { get; set; }
}
