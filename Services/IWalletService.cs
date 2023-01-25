using System.Collections.Generic;
using System.Threading.Tasks;
using Hubtel.Wallets.Api.Models;

namespace Hubtel.Wallets.Api.Services
{
    public interface IWalletService
    {
        bool IsValid(string accountNumber);
        Task<IEnumerable<Wallet>> GetAll();
        Task<Wallet> GetById(string id);
        Task<Wallet> AddNewWallet(string accountNumber, string name, string owner);
        Task<Wallet> Add(Wallet wallet);
        Task<Wallet> Delete(string id);
    }
}
