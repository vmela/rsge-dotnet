using System.Data;

namespace RsGe.NET.SpecInvoice.Soap.Models;

#region Auth Results

public class CheckInResult
{
    public bool Success { get; set; }
    public int Sui { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class CheckSpecUsersResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

#endregion

#region Invoice Results

public class SaveInvoiceResult
{
    public bool Success { get; set; }
    public int InvoiceId { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class InvoiceDataSet
{
    public DataSet? Data { get; set; }
}

public class ChangeStatusResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

#endregion

#region Description Results

public class SaveInvoiceDescResult
{
    public bool Success { get; set; }
    public int DescriptionId { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class DeleteInvoiceDescResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

#endregion

#region Search Results

public class InvoiceFilterCount
{
    public int Status0 { get; set; }
    public int Status1 { get; set; }
    public int Status2 { get; set; }
    public int Status3 { get; set; }
    public int Status4 { get; set; }
    public int Status5 { get; set; }
    public int Status6 { get; set; }
    public int Status7 { get; set; }
    public int Status8 { get; set; }
}

#endregion

#region SSAF/SSD Results

public class AddSpecInvoicesSsafResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class DelSpecInvoicesSsafResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class AddSpecInvoicesSsdResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class DelSpecInvoicesSsdResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

#endregion

#region Transport Results

public class StartTransportResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class CorrectDriverInfoResult
{
    public int Result { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class CorrectTransportMarkResult
{
    public int Result { get; set; }
    public string Message { get; set; } = string.Empty;
}

#endregion

#region Other Results

public class CancellationReasonResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

#endregion
