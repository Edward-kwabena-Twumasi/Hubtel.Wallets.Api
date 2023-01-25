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
        private readonly WalletService _service;

        public WalletController(WalletService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Wallet>> Get()
        {
            return await _service.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Wallet>> Get(string id)
        {
            var wallet = await _service.GetById(id);
            if (wallet == null)
            {
                return NotFound();
            }
            return wallet;
        }

        [HttpPost]
        public async Task<ActionResult<Wallet>> Post(Wallet wallet)
        {
            
            if (!_service.IsValid(wallet.AccountNumber))
            {
                return BadRequest("Invalid wallet account number. ");
            }


            var addedWallet = await _service.AddNewWallet(wallet.AccountNumber, wallet.Name, wallet.Owner);
            if (addedWallet == null)
            {
                return Conflict("Account exists or you have exceeded maximum 4 accounts");

            }
            return CreatedAtAction(nameof(Get), new { id = addedWallet.Id }, addedWallet);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Wallet>> Delete(string id)
        {
            var wallet = await _service.Delete(id);
            if (wallet == null)
            {
                return NotFound();
            }
            return wallet;
        }
    }
}

