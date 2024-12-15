namespace Gramium.Examples.BudgetManager;

public static class MenuButtons
{
    public static (string, string) HomeMenu => ("Вернуться назад", "menu-home");
    public static (string, string) TransactionsMenu => ("Транзакции", "menu-transactions");
    public static (string, string) AccountsMenu => ("Счета", "menu-accounts");
    public static (string, string) StatisticsMenu => ("Статистика", "menu-statistics");
}

public static class TransactionButtons
{
    public static (string, string) Add => ("Добавить транзакцию", "transactions-add");
    public static (string, string) Remove => ("Удалить транзакцию", "transactions-remove");
    public static (string, string) Transfer => ("Перевод между счетами", "transactions-transfer");
}

public static class AccountButtons
{
    public static (string, string) Remove => ("Удалить счёт", "accounts-remove");
    public static (string, string) Add => ("Добавить счёт", "accounts-add");
}

public static class StatisticButtons
{
    public static class Get
    {
        public static (string, string) Incomes => ("Доходы", "statistics-get-incomes");
        public static (string, string) Expenses => ("Расходы", "statistics-get-expenses");
    }
}