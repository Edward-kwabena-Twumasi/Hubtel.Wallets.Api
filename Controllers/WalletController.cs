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
        public async Task<ActionResult<Wallet>> Post([FromBody] Wallet wallet)
        {
            //Validate Wallet fields supplied in request body
            if (ModelState.IsValid)
            {
                if (!_walletService.IsValid(wallet.AccountNumber))
                {
                    return BadRequest("Invalid wallet account number. ");
                }

                WalletApiResponse addNewWallet = await _walletService.AddNewWallet(wallet.AccountNumber, wallet.Name, wallet.Owner);

                if (addNewWallet.status == "400")
                {
                    return BadRequest(addNewWallet.message);
                }

                return CreatedAtAction(nameof(Get), new { id = addNewWallet.wallet.Id }, addNewWallet.wallet);

            }
            else
            {
                // model is not valid, return a list of validation errors
                return BadRequest(ModelState);
            }


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

