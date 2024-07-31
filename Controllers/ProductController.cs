using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Domains;
using minimalAPIMongo.Services;
using MongoDB.Driver;

namespace minimalAPIMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        /// <summary>
        /// Armazena os dados de acesso da collection
        /// </summary>
        private readonly IMongoCollection<Product> _product;

        /// <summary>
        /// Construtor que receber como dependência o obj da classe mongoDbService
        /// </summary>
        /// <param name="mongoDbService">objeto da classe mongoDbService</param>
        public ProductController(MongoDbService mongoDbService)
        {
            //obtem a collection "product"
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        [HttpGet]
        //task para trabalhar com a função assincrona e ActionResulta para retornar um status code
        public async Task<ActionResult<List<Product>>> Get()
        {
            try
            {
                var products = await _product.Find(FilterDefinition<Product>.Empty).ToListAsync();

                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post(Product product)
        {
            try
            {
                await _product.InsertOneAsync(product);

                return StatusCode(201, product);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("id")]
        public async Task<ActionResult<Product>> Delete(string id)
        {
            try
            {
                var productId = _product.FindOneAndDeleteAsync(p  => p.Id == id).Result;

                return StatusCode(201);
            }
            catch (Exception)
            {
                return BadRequest($"Could not delete {id}");
            }
        }

        [HttpGet("id")]
        public async Task<ActionResult<Product>> GetById(string id)
        {
            try
            {
                var productId = _product.Find(p => p.Id == id).FirstOrDefault();

                return productId is not null ? Ok(productId) : NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Product>> Put(string id, Product product)
        {
            try
            {
                //await _product.ReplaceOneAsync((e) => e.Id == id, product);
                //ou
                var filter = Builders<Product>.Filter.Eq(x => x.Id, product.Id);
                
                await _product.ReplaceOneAsync(filter, product);

                return Ok(product);
            }
            catch (Exception)
            {
                return BadRequest($"Could not update the following product: {product}");
            }
        }
    }
}

//criar a classe User na pasta Domains
//id, name, email, password e addittionalAttributes

//criar a classe Client na pasta Domains
//id, UserId, cpf, phone, address, addittionalAttributes

//Criar a classe Order na pasta Domains
//Id, date, status
//referência aos produtos do pedido
//referência ao cliente que fez o pedido

//criar os controllers