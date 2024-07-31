using minimalAPIMongo.Domains;
using MongoDB.Bson.Serialization.Attributes;

namespace minimalAPIMongo.ViewModels
{
    public class ClientViewModel
    {
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? UserPassword { get; set; }

 
        public string? Cpf { get; set; }


        public string? Phone { get; set; }


        public string? Address { get; set; }

        public Dictionary<string, string>? AdditionalAttributes { get; set; }

        public ClientViewModel()
        {
            
            AdditionalAttributes = new Dictionary<string, string>();
        }
    }
}
