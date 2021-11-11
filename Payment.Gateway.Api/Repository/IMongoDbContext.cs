using MongoDB.Driver;

namespace Payment.Gateway.Api.Repository
{
    /// <summary>
    /// Interface for MongoDb
    /// </summary>
    public interface IMongoDbContext
    {
        /// <summary>
        /// Collection of Payment Requests
        /// </summary>
        IMongoCollection<Models.PaymentRequest> PaymentRequestCollection { get; }
    }
}
