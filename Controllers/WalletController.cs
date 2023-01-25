using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Hubtel.Wallets.Api.Models;
using Hubtel.Wallets.Api.Services;
using System.Threading.Tasks;


namespace Hubtel.Wallets.Api.Controllers
{
    [ApiController]
    [Route("api/wallets")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet]
        public async Task<IEnumerable<Wallet>> Get()
        {
            return await _walletService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Wallet>> Get(string id)
        {
            var wallet = await _walletService.GetById(id);
            if (wallet == null)
            {
                return NotFound();
            }
            return wallet;
        }

        [HttpPost]
        public async Task<ActionResult<Wallet>> Post(Wallet wallet)
        {

            if (!_walletService.IsValid(wallet.AccountNumber))
            {
                return BadRequest("Invalid wallet account number. ");
            }


            var addedWallet = await _walletService.AddNewWallet(wallet.AccountNumber, wallet.Name, wallet.Owner);
            if (addedWallet == null)
            {
                return Conflict("Account exists or you have exceeded maximum 4 accounts");

            }
            return CreatedAtAction(nameof(Get), new { id = addedWallet.Id }, addedWallet);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Wallet>> Delete(string id)
        {
            var wallet = await _walletService.Delete(id);
            if (wallet == null)
            {
                return NotFound();
            }
            return wallet;
        }
    }
}

