using EmergencyResponse.SharedClasses.Mapping.interfaces;

namespace EmergencyResponse.Services.DataExport.Mapping
{
    public interface ILanguageStrategyFactory
    {
        ILanguageStrategy Create(OutputLanguage language);
    }
}
