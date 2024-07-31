using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace minimalAPIMongo.Domains
{
    public class Order
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("products")]
        public List<Product>? Products { get; set; }

        [BsonElement("client")]
        public Client? Client { get; set; }

        [BsonElement("date")]
        public DateTime? Date { get; set; }

        [BsonElement("status")]
        public string? Status { get; set; }

        public Dictionary<string, string> AdditionalAttributes { get; set; }

        public Order()
        {
            AdditionalAttributes = new Dictionary<string, string>();
        }
    }
}
