﻿using Domain;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class AccountHolderRepository(GicBankDbContext context) : GenericRepository<AccountHolder>(context), IAccountHolderRepository
    {
    }
}