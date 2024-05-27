using MainModule.DataAccess;
using MainModule.Services;
using System.Configuration;

namespace MainModule.Tests;

public class AccountDataAccessTests
{
    private static Services.ConfigurationService dataAccessConfig = new ();
    private static AccountAccess dataAccess = new(dataAccessConfig);

    [Fact]
    public void InsertAccount_ShouldInsert()
    {
        int expected = 1;
        
        int actual = dataAccess.InsertAccount(new()
        {
            Name = "Testo",
            InitialBalance = 100,
            CurrentBalance = 100,
            IsSelected = 1,
        });

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void UpdateAccount_ShouldUpdate()
    {
        int expected = 1;

        int actual = dataAccess.UpdateAccountBalance(1, 200);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void DeleteAccount_ShouldDelete()
    {
        int expected = 1;

        int actual = dataAccess.DeleteAccount(1);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void QueryAccountsAsync_shouldLoadAccounts()
    {
        int expected = 1;

        int actual = 0;

        if (dataAccess.QueryAccountsAsync().IsCompletedSuccessfully) actual = 1;

        Assert.Equal(expected, actual);
    }
}
