using MainModule.DataAccess;
using MainModule.Services;

namespace MainModule.Tests;

public class SymbolDataAccessTests
{
    private static readonly ConfigurationService dataAccessConfig = new();
    private static readonly SymbolAcess dataAccess = new(dataAccessConfig);

    [Fact]
    public void InsertSymbol_ShouldInsert()
    {
        int expected = 1;
        int actual = dataAccess.InsertSymbol(new()
        {
            Pair = "USD/EUR",
            AssetType = "Crypto"
        });

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void DeleteSymbol_ShouldDelete()
    {
        int expected = 1;
        int actual = dataAccess.DeleteSymbol(1);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void LoadSymbols_ShouldLoad()
    {
        int expected = 1;
        int actual = 0;

        if (dataAccess.QuerySymbolsAsync().IsCompletedSuccessfully) actual = expected;

        Assert.Equal(expected, actual);
    }
}
