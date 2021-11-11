using System;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Payment.Gateway.Api.Repository;

namespace Payment.Gateway.Api.IntegrationTests.Repositories
{
    public class MongoDbClassFixture : IDisposable
    {
        private MongoDbRunner _mongoDbRunner;

        static MongoDbClassFixture()
        {
            ConfigureMongoClient();
        }

        /// <summary>
        /// This will create an in memory representation of mongodb for use in Unit Tests
        /// </summary>
        public IMongoDbContext CreateMongoDbContext()
        {
            this._mongoDbRunner = MongoDbRunner.Start(singleNodeReplSet: true,
                additionalMongodArguments: "--quiet --setParameter maxTransactionLockRequestTimeoutMillis=5000");

            var mongoClient = new MongoClient(_mongoDbRunner.ConnectionString);
            var mongoDbContext = new MongoDbContext(mongoClient);

            return mongoDbContext;
        }

        /// <summary>
        /// Configure the mongo client
        /// </summary>
        public static void ConfigureMongoClient()
        {
            var conventionPack = new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String),
                new CamelCaseElementNameConvention(),
            };

            ConventionRegistry.Register("ConventionPack", conventionPack, type => true);

            BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
            BsonSerializer.RegisterSerializer(typeof(decimal?), new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
            BsonSerializer.RegisterSerializer(typeof(DateTimeOffset), new DateTimeOffsetSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(typeof(DateTimeOffset?), new NullableSerializer<DateTimeOffset>(new DateTimeOffsetSerializer(BsonType.String)));
            BsonSerializer.RegisterSerializer(typeof(Guid), new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(typeof(Guid?), new NullableSerializer<Guid>(new GuidSerializer(BsonType.String)));
        }

        public void Dispose()
        {
            if (this._mongoDbRunner != null)
            {
                this._mongoDbRunner.Dispose();
            }
        }
    }
}
