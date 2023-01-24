using System;

namespace Hubtel.Wallets.Api.Models
{
    public class Wallet
    {
        public enum AccountType
        {
            Momo,
            Card,
            Invalid
        }
        public enum AccountScheme
        {
            Visa,
            Mastercard,
            MTN,
            Vodafone,
            AirtelTigo
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public AccountType Type { get; set; }
        public string AccountNumber { get; set; }
        public AccountScheme Scheme { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Owner { get; set; }

        public Wallet(string accountNumber)
        {
             CreatedAt = DateTime.Now;
            AccountNumber = accountNumber;
           
            if (accountNumber.Length == 10 && IsDigit(accountNumber))
            {
                Type = AccountType.Momo;
                if (accountNumber.StartsWith("024") || accountNumber.StartsWith("054") || accountNumber.StartsWith("059"))
                {
                    Scheme = AccountScheme.MTN;
                }
                else if (accountNumber.StartsWith("050") || accountNumber.StartsWith("020"))
                {
                    Scheme = AccountScheme.Vodafone;
                }
                else if (accountNumber.StartsWith("026") || accountNumber.StartsWith("056"))
                {
                    Scheme = AccountScheme.AirtelTigo;
                }
            }
            else if (accountNumber.Length == 16)
            {
                Type = AccountType.Card;
                if (accountNumber.StartsWith("4"))
                {
                    Scheme = AccountScheme.Visa;
                }
                else if (accountNumber.StartsWith("5"))
                {
                    Scheme = AccountScheme.Mastercard;
                }

            }
            else Type = AccountType.Invalid;
        }
    }
}
