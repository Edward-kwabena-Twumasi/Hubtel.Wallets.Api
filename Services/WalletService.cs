
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
            Wallet wallet = new Wallet(accountNumber);

            if (wallet.Type == Wallet.AccountType.Invalid)
            {
                return false;
            }

            return true;
        }

        // Get all wallets
        public async Task<IEnumerable<Wallet>> GetAll()
        {
            return await Task.Run(() =>
            {
                return _dbContext.Wallets.ToList();
            });

        }

        // Get wallet by specified id
        public async Task<Wallet> GetById(string id)
        {
            return await Task.Run(() =>
            {
                return _dbContext.Wallets.FirstOrDefault(w => w.Id == id);
            });
        }


        //Add new wallet
        public async Task<WalletApiResponse> AddNewWallet(string accountNumber, string name, string owner)
        {
            Wallet wallet = new Wallet(accountNumber);
            wallet.Name = name;
            wallet.Owner = owner;
            return await Add(wallet);
        }

        public async Task<WalletApiResponse> Add(Wallet wallet)
        {
            Wallet existingWallet = await Task.Run(() =>
            {
                return wallet.Type == Wallet.AccountType.Card ?
                _dbContext.Wallets.FirstOrDefault(w => w.AccountNumber == wallet.AccountNumber.Substring(0, 6)) :
                _dbContext.Wallets.FirstOrDefault(w => w.AccountNumber == wallet.AccountNumber);
            });

            List<Wallet> userWallets = await Task.Run(() =>
            {
                return _dbContext.Wallets.Where(w => w.Owner == wallet.Owner).ToList();
            });

            if (existingWallet != null)
            {
                return new WalletApiResponse
                {
                    status = "400",
                    message = "Account number already exists"
                };
            }

            if (userWallets.Count() >= 4)
            {
                return new WalletApiResponse
                {
                    status = "400",
                    message = "Too many wallets.Cant add 5th card"
                };
            }

            if (wallet.Scheme == Wallet.AccountScheme.Unsupported)
            {
                return new WalletApiResponse
                {
                    status = "400",
                    message = "Unsupported scheme. Should be one of [Mtn,Vodafone,Airteltigo,Visa,Mastercard]"
                };
            }

            // Get first 6 numbers of account number when creating a Card wallet 
            if (wallet.Type == Wallet.AccountType.Card)
            {
                wallet.AccountNumber = wallet.AccountNumber.Substring(0, 6);
            }

            await _dbContext.Wallets.AddAsync(wallet);
            await _dbContext.SaveChangesAsync();

            return new WalletApiResponse
            {
                status = "201",
                message = "Wallet created successfully",
                wallet = wallet
            };
        }

        //Delete wallet
        public async Task<Wallet> Delete(string id)
        {
            Wallet wallet = await Task.Run(() =>
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



