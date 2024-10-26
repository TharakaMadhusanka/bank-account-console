using System.ComponentModel.DataAnnotations;

namespace Domain.Enum
{
    public enum TransactionTypes
    {
        [Display(Name = "W")]
        Withdraw = 1,
        [Display(Name = "D")]
        Deposit,
    }
}
