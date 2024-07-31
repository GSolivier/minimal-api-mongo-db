using MongoDB.Bson.Serialization.Attributes;

namespace minimalAPIMongo.Domains
{
    public class User
    {
        //define que esta prop é o Id do objeto
        [BsonId]
        //define o nome do campo no mongoDb como _id e o tipo como ObjectId
        [BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id {  get; set; }

        [BsonElement("name")]
        public string? UserName { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }  

        [BsonElement("password")]
        public string? UserPassword { get; set; }

        //adiciona um dicionário para atributos adicionais
        public Dictionary<string, string>? AdditionalAttributes { get; set; }

        /// <summary>
        /// Ao ser instanciado um obj da classe User, o atributo AdditionalAttributes já virá com um novo dicionário, 
        /// portanto, habilitado para adicionar + atributos
        /// </summary>
        public User()
        {
            //Criando a instância do novo obj
            AdditionalAttributes = new Dictionary<string, string>();
        }
    }
}
