using Domain.Constants;
using Domain.Dao;
using Domain.Dto;
using Domain.Enum;
using Domain.GicDomainInterface;
using Domain.Interfaces;
using System.Globalization;
using System.Linq.Expressions;

namespace Domain.GicDomainServices
{
    public class GicTransactionDomainService : IGicTransactionDomainService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountHolderRepository _accountHolderRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IInterestRuleRepository _interestRuleRepository;
        public GicTransactionDomainService(IAccountRepository accountRepository,
            IAccountHolderRepository accountHolderRepository,
            ITransactionRepository transactionRepository,
            IInterestRuleRepository interestRuleRepository)
        {
            _accountRepository = accountRepository;
            _accountHolderRepository = accountHolderRepository;
            _transactionRepository = transactionRepository;
            _interestRuleRepository = interestRuleRepository;
        }
        public async Task<InputTransactionResponseDto> InputTransactionHandleAsync(InputTransactionRequestParamsDao inputTransactionRequestParamsDao)
        {
            // 1. Check Account Exist or not
            // 2. Create new Account + new Account Holder
            // 3. Create transaction, Update Total Balance
            // 4. Use transaction scope
            // 5. Handle failure
            var existingAccount = await _accountRepository.GetAccountByAccountNoAsync(inputTransactionRequestParamsDao.AccountNo);
            if (existingAccount is null)
            {
                return await CreateNewAccountCheckAsync(inputTransactionRequestParamsDao);
            }
            return await UpdateTransactionDetailsAsync(inputTransactionRequestParamsDao, existingAccount);
        }

        #region Create Account
        private async Task<InputTransactionResponseDto> CreateNewAccountCheckAsync(InputTransactionRequestParamsDao inputTransactionRequestParamsDao)
        {

            if (inputTransactionRequestParamsDao.TransactionType == TransactionTypes.Withdraw)
            {
                return new InputTransactionResponseDto
                {
                    AccountNo = inputTransactionRequestParamsDao.AccountNo,
                    IsTransactionSuccess = false,
                    CommonError = new CommonErrorDto { ErrorMessage = "Invalid Transaction Request" }
                };
            }
            return await CreateNewAccountAsync(inputTransactionRequestParamsDao);
        }

        private async Task<InputTransactionResponseDto> CreateNewAccountAsync(InputTransactionRequestParamsDao inputTransactionRequestParamsDao)
        {
            DateTime.TryParseExact(inputTransactionRequestParamsDao.TransactionDate, CommonConstant.TransactionDateInputFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateToUse);
            // create new Account Holder
            var accountHolder = new AccountHolder
            {
                FirstName = "Dummy",
                LastName = "Name",
                Id = Guid.NewGuid(),
                CreatedDate = dateToUse,
                UpdatedDate = dateToUse,
                IdentificationNo = $"ID-{new Random().Next(1, 9999)}"
            };
            // create new Account
            var account = new Accounts
            {
                AccountHolderId = accountHolder.Id,
                AccountNo = inputTransactionRequestParamsDao.AccountNo,
                CreatedDate = dateToUse,
                LastTransactionDate = dateToUse,
                UpdatedDate = dateToUse,
                AccountType = ((int)AccountType.AdHoc).ToString(),
                AccountHolder = accountHolder,
                TotalBalance = Convert.ToDecimal(inputTransactionRequestParamsDao.Amount),
            };
            // create Transaction Record
            var transaction = new Transactions
            {
                TransactionNumber = $"{inputTransactionRequestParamsDao.TransactionDate}-01",
                Id = Guid.NewGuid(),
                Account = account,
                AccountId = account.Id,
                CreatedDate = dateToUse,
                Source = ((int)TransactionSource.ATM).ToString(),
                CodeTransactionType = ((int)TransactionTypes.Deposit).ToString(),
                Amount = Decimal.Parse(inputTransactionRequestParamsDao.Amount)
            };

            await _accountHolderRepository.AddAsync(accountHolder);
            await _accountRepository.AddAsync(account);
            await _transactionRepository.AddAsync(transaction);

            return await ConstructInputTransactionStatement(account.Id, account.AccountNo);
        }
        #endregion

        private async Task<InputTransactionResponseDto> UpdateTransactionDetailsAsync(InputTransactionRequestParamsDao inputTransactionRequestParamsDao,
            Accounts existingAccount)
        {
            existingAccount.TotalBalance +=
                inputTransactionRequestParamsDao.TransactionType == TransactionTypes.Deposit
                ? Decimal.Parse(inputTransactionRequestParamsDao.Amount)
                : -Decimal.Parse(inputTransactionRequestParamsDao.Amount);
            existingAccount.UpdatedDate = DateTime.UtcNow;

            // For final balance 0 or less after transaction happens
            if (existingAccount.TotalBalance <= 0)
            {
                return new InputTransactionResponseDto
                {
                    AccountNo = inputTransactionRequestParamsDao.AccountNo,
                    IsTransactionSuccess = false,
                    CommonError = new CommonErrorDto { ErrorMessage = "Don't empty your account !!" }
                };
            }

            var latestTransactionNo = await _transactionRepository.GetLatestRunNoSequenceAsync(inputTransactionRequestParamsDao.TransactionDate);
            // create Transaction Record
            var transaction = new Transactions
            {
                TransactionNumber = $"{inputTransactionRequestParamsDao.TransactionDate}-{++latestTransactionNo}",
                Id = Guid.NewGuid(),
                Account = existingAccount,
                AccountId = existingAccount.Id,
                CreatedDate = DateTime.UtcNow,
                Source = ((int)TransactionSource.ATM).ToString(),
                CodeTransactionType = ((int)inputTransactionRequestParamsDao.TransactionType).ToString(),
                Amount = Decimal.Parse(inputTransactionRequestParamsDao.Amount),
            };

            await _accountRepository.UpdateAsync(existingAccount);
            await _transactionRepository.AddAsync(transaction);
            return await ConstructInputTransactionStatement(existingAccount.Id, existingAccount.AccountNo);
        }

        private async Task<InputTransactionResponseDto> ConstructInputTransactionStatement(Guid accountId, string accountNo)
        {
            Expression<Func<Transactions, bool>> expression = x => x.AccountId == accountId;
            var transactionsList = await _transactionRepository.GetTransactionsPrintStatementByExpressionAsync(expression);
            return new InputTransactionResponseDto(transactionsList) { IsTransactionSuccess = true, AccountNo = accountNo };
        }

    }
}
