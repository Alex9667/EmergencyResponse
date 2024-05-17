using EmergencyResponse.Model;

namespace EmergencyResponse.Services.DataExport
{
    public interface IDataExportService
    {
        Task<string> ExportAddressesToJson(List<Address> addressesn, OutputLanguage outputLanguage);
    }
}
