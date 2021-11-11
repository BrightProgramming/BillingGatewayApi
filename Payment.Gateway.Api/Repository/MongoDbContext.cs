using MongoDB.Driver;
using Payment.Gateway.Api.Repository.Models;

namespace Payment.Gateway.Api.Repository
{
    /// <summary>
    /// The mongo db context
    /// </summary>
    public class MongoDbContext : IMongoDbContext
    {
        /// <summary>
        /// The collection for payment requests
        /// </summary>
        public const string PaymentRequestCollectionName = "PaymentRequest";
        private const string PaymentGatewayDatabase = "PaymentGatewayDatabase";

        private readonly IMongoDatabase _database;

        private IMongoCollection<PaymentRequest> _paymentRequests;

        /// <summary>
        /// Constructor
        /// </summary>
        public MongoDbContext(IMongoClient client)
        {
            _database = client.GetDatabase(PaymentGatewayDatabase);

            var indexBuilder = Builders<Models.PaymentRequest>.IndexKeys;
            var paymentIdIndexKeyDefinition = indexBuilder.Ascending(x => x.PaymentId);
            var merchantIdIndexKeyDefinition = indexBuilder.Ascending(x => x.MerchantId);

            var paymentIdAndMerchantIdIndexKeyDefinition  = indexBuilder.Combine(paymentIdIndexKeyDefinition, merchantIdIndexKeyDefinition);

            var paymentIdAndMerchantIdIndex= new CreateIndexModel<PaymentRequest>(paymentIdAndMerchantIdIndexKeyDefinition, new CreateIndexOptions
            {
                Name = "PaymentIdAndMerchantId",
                Unique = true,
            });

            PaymentRequestCollection.Indexes.CreateMany(new[]
            {
                paymentIdAndMerchantIdIndex,
            });
        }

        /// <summary>
        /// The collection of payment requests
        /// </summary>
        public IMongoCollection<PaymentRequest> PaymentRequestCollection => _paymentRequests ??= _database.GetCollection<Models.PaymentRequest>(PaymentRequestCollectionName);
    }
}
