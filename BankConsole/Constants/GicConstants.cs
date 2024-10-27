namespace GicConsole.Constants
{
    public static class GicConstants
    {
        public static readonly string WelcomeMessage = "Welcome to AwesomeGIC Bank! What would you like to do? \n";
        public static readonly string NextActionConsentMessage = "Is there anything else you'd like to do? \n";
        public static readonly string InputTransactions = "Please enter transaction details in <Date> <Account> <Type> <Amount> format\n" +
            "(or enter blank to go to main menu)";
        public static readonly string Withdrawal = "W";
        public static readonly string Deposit = "D";
        public static readonly string InputTransactionStatement = "Account: <AccountNo> \n" +
            "|Date\t|Txn Id\t|Type\t|Amount|\n";
        public static readonly string AccountStatement = "Account: <AccountNo> \n" +
                    "|Date\t|Txn Id\t|Type\t|Amount\t|Balance|\n";
        public static readonly string ActionsMenuAndTitleMessage = "<TitleMessage>" +
            "[T] Input transactions \n" +
            "[I] Define interest rules \n" +
            "[P] Print statement \n" +
            "[Q] Quit";
        public static readonly string DefineInterestRule = "Please enter interest rules details in <Date> <RuleId> <Rate in %> format" +
            "(or enter blank to go to main menu)";
        public static readonly string InterestRulesStatement = "Interest Rules: \n" +
                            "|Date\t|RuleId\t|Rate (%)\t|\n";
        public static readonly string PrintAccountStatementMessage = "Please enter account and month to generate the statement <Account> <Year><Month>" +
            "(or enter blank to go to main menu)";
        public static readonly string QuitMessage = "Thank you for banking with AwesomeGIC Bank.\n" +
            "Have a nice day!";
        public static string SectionBreaker = "**************************************************\n";
    }
}
