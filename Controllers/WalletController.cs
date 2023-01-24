using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Hubtel.Wallets.Api.Models;
using Hubtel.Wallets.Api.Services;

namespace Hubtel.Wallets.Api.Controllers
{
    [ApiController]
    [Route("api/wallets")]
    public class WalletController : ControllerBase
    {
        private readonly WalletService _service;

        public WalletController(WalletService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<Wallet> Get()
        {
            return _service.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Wallet> Get(string id)
        {
            var wallet = _service.GetById(id);
            if (wallet == null)
            {
                return NotFound();
            }
            return wallet;
        }

        [HttpPost]
        public ActionResult<Wallet> Post(Wallet wallet)
        {
            Console.WriteLine(wallet.AccountNumber);
            Console.WriteLine("......................");
            if (!_service.IsValid(wallet.AccountNumber))
            {
                return BadRequest("Invalid wallet account number. ");
            }


            var addedWallet = _service.AddNewWallet(wallet.AccountNumber, wallet.Name, wallet.Owner);
            if (addedWallet == null)
            {
                return Conflict("Account exists or you have exceeded maximum 4 accounts");

            }
            return CreatedAtAction(nameof(Get), new { id = addedWallet.Id }, addedWallet);
        }

        [HttpDelete("{id}")]
        public ActionResult<Wallet> Delete(string id)
        {
            var wallet = _service.Delete(id);
            if (wallet == null)
            {
                return NotFound();
            }
            return wallet;
        }
    }
}

