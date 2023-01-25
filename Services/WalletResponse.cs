
using Hubtel.Wallets.Api.Models;

namespace Hubtel.Wallets.Api.Services
{
    public class WalletResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public Wallet wallet { get; set; }
    }
}