﻿namespace GicConsole.Constants
{
    public static class GicConstants
    {
        public static readonly string WelcomeMessage = "Welcome to AwesomeGIC Bank! What would you like to do? \n";
        public static readonly string NextActionConsentMessage = "Is there anything else you'd like to do? \n";
        public static readonly string InputTransactions = "PLease enter transaction details in <Date> <Account> <Type> <Amount> format\n" +
            "(or enter blank to go to main menu)";
        public static readonly string Withdrawal = "W";
        public static readonly string Deposit = "D";
        public static readonly string InputTransactionStatement = "Account: <AccountNo> \n" +
            "|Date\t|Txn Id\t|Type\t|Amount|\n";
        public static readonly string ActionsMenuAndTitleMessage = "<TitleMessage>" +
            "[T] Input transactions \n" +
            "[I] Define interest rules \n" +
            "[P] Print statement \n" +
            "[Q] Quit";

    }
}