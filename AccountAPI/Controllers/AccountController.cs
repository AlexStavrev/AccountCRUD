using DBExerciseClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountAPI.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class AccountController : ControllerBase
    {
        private IDataAccessLayer DataAccessLayer { get; set; }

        public AccountController(IDataAccessLayer dataAccessLater)
        {
            DataAccessLayer = dataAccessLater;
        }

        [HttpGet]
        [Route("accounts")]
        public IEnumerable<Account> Get()
        {
            return DataAccessLayer.GetAccounts();
        }

        [HttpDelete]
        [Route("accounts/{id}")]
        public ActionResult DeleteById(int id)
        {
            //if (DataAccessLayer.FindById(id) is null)
            //{
            //    return NotFound();
            //}
            DataAccessLayer.DeleteById(id);
            return Ok();
        }

        [HttpPost]
        [Route("accounts")]
        public ActionResult<Account> AddAccount(Account account)
        {
            DataAccessLayer.Insert(account);
            return Created($"api/v1/account/{account.Id}", account);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Account> GetById(int id)
        {
            Account account = DataAccessLayer.FindById(id);
            if (account is null)
            {
                return NotFound();
            }
            return Ok(account);
        }
    }
}
