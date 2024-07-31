using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using minimalAPIMongo.Domains;
using System.Text.Json.Serialization;

namespace minimalAPIMongo.ViewModels
{
    public class OrderViewModel
    {

        public string? Id { get; set; }

        public DateTime? Date { get; set; }

        public string? Status { get; set; }

        public List<string>? products { get; set; }

        public string? ClientId { get; set; }

        public Dictionary<string, string> AdditionalAttributes { get; set; }

        public OrderViewModel()
        {

            AdditionalAttributes = new Dictionary<string, string>();
        }

    }
}
