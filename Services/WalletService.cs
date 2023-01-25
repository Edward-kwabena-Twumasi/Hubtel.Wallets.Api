
using System.Collections.Generic;
using System.Linq;
using Hubtel.Wallets.Api.Models;
using System.Threading.Tasks;

namespace Hubtel.Wallets.Api.Services
{
    public class WalletService : IWalletService
    {
        private readonly WalletDbContext _dbContext;

        public WalletService(WalletDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool IsValid(string accountNumber)
        {
            var wallet = new Wallet(accountNumber);

            if (wallet.Type == Wallet.AccountType.Invalid)
            {
                return false;
            }
            // return false;
            return true;
        }

        public async Task<IEnumerable<Wallet>> GetAll()
        {
            return await Task.Run(() =>
            {
                return _dbContext.Wallets.ToList();
            });

        }

        public async Task<Wallet> GetById(string id)
        {
            return await Task.Run(() =>
            {
                return _dbContext.Wallets.FirstOrDefault(w => w.Id == id);
            });
        }



        //Add new wallet
        public async Task<Wallet> AddNewWallet(string accountNumber, string name, string owner)
        {
            var wallet = new Wallet(accountNumber);
            wallet.Name = name;
            wallet.Owner = owner;
            return await Add(wallet);
        }

        public async Task<Wallet> Add(Wallet wallet)
        {
            var existingWallet = await Task.Run(() =>
            {
                return _dbContext.Wallets.FirstOrDefault(w => w.AccountNumber == wallet.AccountNumber);
            });

            if (existingWallet != null)
            {
                return null;
            }

            var userWallets = await Task.Run(() =>
            {
                return _dbContext.Wallets.Where(w => w.Owner == wallet.Owner).ToList();
            });

            if (userWallets.Count() >= 4)
            {
                return null;
            }

            await _dbContext.Wallets.AddAsync(wallet);
            await _dbContext.SaveChangesAsync();
            return wallet;
        }

        public async Task<Wallet> Delete(string id)
        {
            var wallet = await Task.Run(() =>
            {
                return _dbContext.Wallets.FirstOrDefault(w => w.Id == id);
            });

            if (wallet != null)
            {
                await Task.Run(() =>
                {
                    return _dbContext.Wallets.Remove(wallet);
                });

                await _dbContext.SaveChangesAsync();
            }
            return wallet;
        }
    }
}



