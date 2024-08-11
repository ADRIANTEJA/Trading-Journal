

using MainModule.Services;

namespace MainModule.DataAccess
{
    public class NoteAccess
    {
        private readonly IConfigurationService _dataAccessConfig;

        public NoteAccess(IConfigurationService dataAccessConfig)
        {
            _dataAccessConfig = dataAccessConfig;
        }

        public int InsertTradeNote()
        {
            var command = @"INSERT INTO AnalysisNote (tradeId, title, text)
                            VALUES (@TradeId, @Title, @Text)";
            return 1;
        }
    }
}
