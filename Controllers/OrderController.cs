using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using minimalAPIMongo.Domains;
using minimalAPIMongo.Services;
using minimalAPIMongo.ViewModels;
using MongoDB.Driver;

namespace minimalAPIMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMongoCollection<Order> _order;
        private readonly IMongoCollection<Client> _client;
        private readonly IMongoCollection<Product> _product;

        public OrderController(MongoDbService mongoDbService)
        {
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
            _order = mongoDbService.GetDatabase.GetCollection<Order>("order");
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> Get()
        {
            try
            {
                var orderList = await _order.Find(FilterDefinition<Order>.Empty).ToListAsync();

                return Ok(orderList);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(OrderViewModel orderViewModel)
        {
            try
            {
                var client = await _client.Find(e => e.Id == orderViewModel.ClientId).FirstOrDefaultAsync();

                if (client == null)
                {
                    return BadRequest("Cliente não encontrado");
                }

                List<Product> products = [];

                if (orderViewModel.products != null)
                {
                    foreach (var id in orderViewModel.products)
                    {
                        var product = await _product.Find(e => e.Id == id).FirstOrDefaultAsync();

                        if (product == null)
                        {
                            return BadRequest("Produto não encontrado");
                        }

                        products.Add(product);
                    }
                }

                var newOrder = new Order
                {
                    Client = client,
                    Products = products,
                    AdditionalAttributes = orderViewModel.AdditionalAttributes,
                    Date = orderViewModel.Date,
                    Status = orderViewModel.Status,
                };

                await _order.InsertOneAsync(newOrder);

                return StatusCode(201, newOrder);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<Order>> Put(OrderViewModel orderViewModel, string orderId)
        {
            try
            {
                var client = await _client.Find(e => e.Id == orderViewModel.ClientId).FirstOrDefaultAsync();

                if (client == null)
                {
                    return BadRequest("Cliente não encontrado");
                }

                List<Product> products = [];

                if (orderViewModel.products != null)
                {
                    foreach (var id in orderViewModel.products)
                    {
                        var product = await _product.Find(e => e.Id == id).FirstOrDefaultAsync();

                        if (product == null)
                        {
                            return BadRequest("Produto não encontrado");
                        }

                        products.Add(product);
                    }
                }

                var newOrder = new Order
                {
                    Client = client,
                    Products = products,
                    AdditionalAttributes = orderViewModel.AdditionalAttributes,
                    Date = orderViewModel.Date,
                    Status = orderViewModel.Status,
                };

                await _order.ReplaceOneAsync(e => e.Id == orderId, newOrder);

                return NoContent();
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpDelete("id")]
        public async Task<ActionResult<Order>> Delete(string id)
        {
            try
            {
                var orderId = await _order.FindOneAndDeleteAsync((o) => o.Id == id);

                return Ok("Pedido deletado com sucesso!");
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
    }
}
