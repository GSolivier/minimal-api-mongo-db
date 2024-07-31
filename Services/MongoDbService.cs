using MongoDB.Driver;

namespace minimalAPIMongo.Services
{
    public class MongoDbService
    {
        //responsável por criar as conexões com o banco de dados
        /// <summary>
        /// Armazena a configuração da aplicação
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Armazena uma referência ao MongoDb
        /// </summary>
        private readonly IMongoDatabase _database;

        /// <summary>
        /// Recebe a config da aplicação como parâmetro
        /// </summary>
        /// <param name="configuration">Objeto configuration</param>
        public MongoDbService(IConfiguration configuration)
        {
            //atribui a config recebida em _configuration
            _configuration = configuration;

            //obter a string de conexão do _configuration
            var connectionString = _configuration.GetConnectionString("DbConnection");

            //Cria um obj MongoUrl que receber como parâmetro a string de conexão
            var mongoUrl = MongoUrl.Create(connectionString);

            //cria um client mongoClient para se conectar ao MongoDB
            var mongoClient = new MongoClient(mongoUrl);

            //obtem a referência ao banco com o nome especificado na string de conexão
            _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        //retorna para o banco de dados
        /// <summary>
        /// Propriedade para acessar o banco de dados
        /// Retorna a referência ao db
        /// </summary>
        public IMongoDatabase GetDatabase => _database;
    }
}
