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
        Task<WalletApiResponse> AddNewWallet(string accountNumber, string name, string owner);
        Task<WalletApiResponse> Add(Wallet wallet);
        Task<Wallet> Delete(string id);
    }
}
