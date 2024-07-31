using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using minimalAPIMongo.Domains;
using minimalAPIMongo.Services;
using MongoDB.Driver;

namespace minimalAPIMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        //vamos armazenar os dados da collection
        private readonly IMongoCollection<User> _user;

        /// <summary>
        /// Construtor que receber como dependência o obj da classe mongoDbService
        /// </summary>
        /// <param name="mongoDbService">obj da classe mongoDbService</param>
        public UserController(MongoDbService mongoDbService)
        {
            //obtem a collection "user"
            _user = mongoDbService.GetDatabase.GetCollection<User>("user");
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            try
            {
                var userList = await _user.Find(FilterDefinition<User>.Empty).ToListAsync();

                return userList == null ? BadRequest() : Ok(userList);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            try
            {
                await _user.InsertOneAsync(user);

                return StatusCode(201, user);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpGet("id")]
        public async Task<ActionResult<User>> GetById(string id)
        {
            try
            {
                var userById = _user.Find((u) => u.Id == id).FirstOrDefault();

                return userById is not null ? Ok(userById) : NotFound();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<User>> Put(string id, User user)
        {
            try
            {
                await _user.ReplaceOneAsync((u) => u.Id == id, user);

                return Ok("Objeto atualizado com sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("id")]
        public async Task<ActionResult<User>> Delete(string id)
        {
            try
            {
                var userDelete = await _user.FindOneAndDeleteAsync((u) => u.Id == id);

                return Ok("usuário deletado com sucesso!");
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
    }
}
