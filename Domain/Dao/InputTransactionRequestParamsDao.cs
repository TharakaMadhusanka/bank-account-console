using Domain.Enum;

namespace Domain.Dao
{
    public class InputTransactionRequestParamsDao
    {
        public required string AccountNo { get; set; }
        public required string Amount { get; set; }
        public required string TransactionDate { get; set; }
        public required TransactionTypes TransactionType { get; set; }
    }
}
