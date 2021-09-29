using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DBExerciseClassLibrary
{
    public class AccountAPIConsumercs
    {
        public void AddAccount(Account account)
        {
            string json = JsonSerializer.Serialize(account);

            var request = new RestRequest(Method.POST).AddJsonBody(json);

            var client = new RestClient("https://localhost:44321/api/v1/accounts");

            var response = client.Post<Account>(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Error when adding an account; Error was `{response.ErrorMessage}`\nDescription:{response.StatusDescription}");
            }
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            var client = new RestClient("https://localhost:44321/api/v1/accounts");

            var response = client.Execute<List<Account>>(new RestRequest());
            
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error when retrieving all accounts; Error was `{response.ErrorMessage}`\nDescription:{response.StatusDescription}");
            }
            return response.Data;
        }

        public void DeleteById(int id)
        {
            var client = new RestClient($"https://localhost:44321/api/v1/accounts/{id}");

            var request = new RestRequest(Method.DELETE);
            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Error when deleting account with id {id}; Error was `{response.ErrorMessage}`\nDescription:{response.StatusDescription}");
            }
        }
    }
}
