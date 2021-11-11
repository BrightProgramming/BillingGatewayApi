using System;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Payment.Gateway.Api.Repository;

namespace Payment.Gateway.Api.StartupConfiguration
{
    /// <summary>
    /// Mongo db configuration
    /// </summary>
    public static class MongoDbConfiguration
    {
        /// <summary>
        /// Add mongo configuration to dependency injection
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            ConfigureMongoClient();

            var mongoRunner = MongoDbRunner.Start(singleNodeReplSet: true,
                additionalMongodArguments: "--quiet --setParameter maxTransactionLockRequestTimeoutMillis=5000");

            var mongoClient = new MongoClient(mongoRunner.ConnectionString);
            services.AddSingleton<IMongoClient>(mongoClient);
            var mongoDbContext = new MongoDbContext(mongoClient);
            services.AddSingleton<IMongoDbContext>(mongoDbContext);

            return services;
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
    }
}
