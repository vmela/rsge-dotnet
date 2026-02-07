namespace RsGe.NET.WayBill.Soap.Models;

public class WayBillType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class WayBillUnit
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class TransportType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public enum WayBillStatus
{
    Draft = 0,
    Active = 1,
    Completed = 2,
    Cancelled = -1,
    Rejected = -2
}

public class BarCode
{
    public string Code { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string UnitName { get; set; } = string.Empty;
    public int UnitId { get; set; }
}

public class ExciseStamp
{
    public string Series { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public class WoodType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class AkcizCode
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}

public class WayBillTemplate
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public WayBillDocument? WayBill { get; set; }
}

public class CarNumber
{
    public string Number { get; set; } = string.Empty;
}
