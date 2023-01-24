using System;
using System.Collections.Generic;
using System.Linq;
using Hubtel.Wallets.Api.Models;

namespace Hubtel.Wallets.Api.Services
{
    public class WalletService
    {
       

        private static List<Wallet> _wallets = new List<Wallet>();

        public IEnumerable<Wallet> GetAll()
        {
            return _wallets;
        }

        public Wallet GetById(string id)
        {
            return _wallets.FirstOrDefault(w => w.Id == id);
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

        //Add new wallet
        public Wallet AddNewWallet(string accountNumber, string name, string owner)
        {
            var wallet = new Wallet(accountNumber);
            wallet.Name = name;
            wallet.Owner = owner;
            wallet.Id = _wallets.Count().ToString();
            return Add(wallet);
        }

        public Wallet Add(Wallet wallet)
        {
            var existingWallet = _wallets.Where(w => w.AccountNumber == wallet.AccountNumber);
            if (existingWallet.Count() > 0)
            {
                return null;
            }

            var userWallets = _wallets.Where(w => w.Owner == wallet.Owner);
            if (userWallets.Count() >= 4)
            {
                return null;
            }

            _wallets.Add(wallet);
            return wallet;
        }

        public Wallet Delete(string id)
        {
            var wallet = _wallets.FirstOrDefault(w => w.Id == id);
            if (wallet != null)
            {
                _wallets.Remove(wallet);
            }
            return wallet;
        }
    }
}



