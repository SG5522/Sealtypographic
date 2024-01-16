using CommonLib.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SealTypographicWebAPI.Models.LogReport.CustomerSealGroupLog;

namespace SealTypographicWebAPI.Models.MongoDBModel
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerSealGroupLog : LogModel<CustomerSealGroupLogSave>
    {
        /// <summary>
        /// 
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
    }
}
