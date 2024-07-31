using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Domains;
using minimalAPIMongo.Services;
using minimalAPIMongo.ViewModels;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace minimalAPIMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ClientController : ControllerBase
    {
        private readonly IMongoCollection<Client> _client;
        private readonly IMongoCollection<User> _user;

        public ClientController(MongoDbService mongoDbService)
        {
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
            _user = mongoDbService.GetDatabase.GetCollection<User>("user");
        }

        [HttpGet]
        public async Task<ActionResult<List<Client>>> Get()
        {
            try
            {
                var clientList = await _client.Find(FilterDefinition<Client>.Empty).ToListAsync();

                return Ok(clientList);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Client>> Post(ClientViewModel client)
        {
            try
            {
                var newUser = new User { 
                   UserName = client.UserName,
                   UserPassword = client.UserPassword,
                   Email = client.Email,
                   AdditionalAttributes = client.AdditionalAttributes
                };

                await _user.InsertOneAsync(newUser);

                var newClient = new Client { 
                    Address = client.Address,
                    Cpf = client.Cpf,
                    Phone = client.Phone,
                    User = newUser,
                };

                await _client.InsertOneAsync(newClient);

                return StatusCode(201, newClient);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet("id")]
        public async Task<ActionResult<Client>> GetById(string id)
        {
            try
            {
                var client = await _client.Find(c => c.Id == id).FirstOrDefaultAsync();

                if (client == null)
                {
                    return NotFound();
                }

                User? user = await _user.Find(u => u.Id == client.User!.Id).FirstOrDefaultAsync();

                client.User = user;

                return Ok(client);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpDelete("id")]
        public async Task<ActionResult<Client>> Delete(string id)
        {
            try
            {

                var clientDelete = await _client.FindOneAndDeleteAsync((u) => u.Id == id);

                return Ok("usuário deletado com sucesso!");
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(string id, Client updatedClient)
        {
            try
            {
                var client = await _client.Find(c => c.Id == id).FirstOrDefaultAsync();

                if (client == null)
                {
                    return NotFound();
                }

                updatedClient.Id = id; // Assegura que o Id do cliente não será modificado

                await _client.ReplaceOneAsync(c => c.Id == id, updatedClient);

                User? user = await _user.Find(u => u.Id == updatedClient.User.Id).FirstOrDefaultAsync();

               client.User = user;

                return Ok(client);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
    }
}
