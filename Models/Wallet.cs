using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
            AirtelTigo,
            Unsupported
        }


        public string Id { get; set; }

        [Required(ErrorMessage = " Specify 'name' field in request body.")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "Value should range from 3-25 characters")]
        [JsonPropertyName("name")]
        public string Name { get; set; }
        public AccountType Type { get; set; }


        [JsonPropertyName("accountNumber")]
        [Required(ErrorMessage = " Specify 'accountNumber' field in request body")]
        public string AccountNumber { get; set; }
        public AccountScheme Scheme { get; set; }
        public DateTime CreatedAt { get; set; }


        [JsonPropertyName("ownerPhone")]
        [RegularExpression(@"^(\+233|0)[23459]\d{8}$", ErrorMessage = "The phone number is not valid.")]
        [Required(ErrorMessage = "Specify 'ownerPhone' field in request body.")]
        public string Owner { get; set; }

        //Algorithm to check valid ghana number
        public bool IsValidGhanaNumber(string number)
        {
            if (!Regex.IsMatch(number, @"^(\+233|0)[23459]\d{8}$"))
                return false;
            return true;
        }
        
        // Algorithm to check card number is valid(Lhun Algorithm)
        public bool IsValidCardNumber(string cardNumber)
        {
            int sum = 0;
            bool isSecondDigit = false;
            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int digit = int.Parse(cardNumber[i].ToString());
                if (isSecondDigit)
                {
                    digit *= 2;
                    if (digit > 9)
                    {
                        digit -= 9;
                    }
                }
                sum += digit;
                isSecondDigit = !isSecondDigit;
            }
            return sum % 10 == 0;
        }


        public Wallet(string accountNumber)
        {
            CreatedAt = DateTime.Now;
            Id = Guid.NewGuid().ToString();
            AccountNumber = accountNumber;

            bool isNumeric = long.TryParse(accountNumber, out _);

            if (IsValidGhanaNumber(accountNumber))
            {
                var tenDigitAccNumber = accountNumber.Replace("+233", "0");
                AccountNumber = tenDigitAccNumber;

                Type = AccountType.Momo;

                if (Array.Exists(new[] { "24", "54", "59", "55" }, s => tenDigitAccNumber.Substring(1, 2) == s))
                {
                    Scheme = AccountScheme.MTN;
                }

                else if (Array.Exists(new[] { "20", "50" }, s => tenDigitAccNumber.Substring(1, 2) == s))
                {
                    Scheme = AccountScheme.Vodafone;
                }

                else if (Array.Exists(new[] { "26", "27", "56", "57" }, s => tenDigitAccNumber.Substring(1, 2) == s))
                {
                    Scheme = AccountScheme.AirtelTigo;
                }
                else Scheme = AccountScheme.Unsupported;
            }

            else if (IsValidCardNumber(accountNumber))
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
                else Scheme = AccountScheme.Unsupported;

            }
            else Type = AccountType.Invalid;
        }

    }
}
