namespace RsGe.NET.Core.Models;

/// <summary>
/// Error codes returned by RS.GE service
/// </summary>
public static class RsGeErrorCodes
{
    public const int Success = 0;
    public const int InvalidCredentials = -1;
    public const int InvalidRequest = -2;
    public const int WaybillNotFound = -3;
    public const int InvalidStatus = -4;
    public const int InvalidBuyerTin = -5;
    public const int InvalidDriverTin = -6;
    public const int InvalidGoods = -7;
    public const int ServerError = -99;
}
