using EmergencyResponse.SharedClasses.Mapping;
using EmergencyResponse.SharedClasses.Mapping.interfaces;
using Microsoft.Extensions.Logging;

namespace EmergencyResponse.Services.DataExport.Mapping
{
    public class LanguageStrategyFactory : ILanguageStrategyFactory
    {
        private readonly ILogger _logger;

        public LanguageStrategyFactory(ILogger<LanguageStrategyFactory> logger)
        {
            _logger = logger;
        }

        public ILanguageStrategy Create(OutputLanguage language)
        {
            switch (language)
            {
                case OutputLanguage.Danish:
                    return new DanishLanguageStrategy();
                case OutputLanguage.English:
                    return new EnglishLanguageStrategy();
                default:
                    // Optionally, log this unexpected scenario
                    _logger.LogWarning("Received unexpected language setting: {Language}. Defaulting to English.", language);
                    return new EnglishLanguageStrategy();
            }
        }
    }
}
