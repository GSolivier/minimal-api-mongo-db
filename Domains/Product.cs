using MongoDB.Bson.Serialization.Attributes;

namespace minimalAPIMongo.Domains
{
    public class Product
    {
        //define que esta prop é o Id do objeto
        [BsonId]
        //define o nome do campo no mongoDb como _id e o tipo como ObjectId
        [BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("price")]
        public decimal? Price { get; set; }

        //adiciona um dicionário para atributos adicionais
        public Dictionary<string, string>? AdditionalAttributes{ get; set; }

        /// <summary>
        /// Ao ser instanciado um obj da classe Product, o atributo AdditionalAttributes já virá com um novo dicionário, 
        /// portanto, habilitado para adicionar + atributos
        /// </summary>
        public Product()
        {
            //criando a instância de um novo dicionário
            AdditionalAttributes = new Dictionary<string, string>();
        }
    }
}
