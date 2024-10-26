﻿namespace GicConsole.Interface
{
    internal interface IInputValidator
    {
        bool IsValidDate(string date);
        bool IsValidTransactionType(string transactionType);
        bool IsValidTransactionAmount(string transactionAmount);
        bool IsValidTransactionInput(string[] inputTransaction);
        bool IsValidInterestRuleRequest(string[] inputTransaction);
        bool IsValidAccountStatementRequest(string[] inputRequest);
    }
}
