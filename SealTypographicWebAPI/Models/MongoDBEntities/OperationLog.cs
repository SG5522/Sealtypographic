using CommonLib.Enums;
using CommonLib.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SealTypographicWebAPI.Models.LogReport.OperationLog;

namespace SealTypographicWebAPI.Models.MongoDBModel
{
    /// <summary>
    /// 
    /// </summary>
    public class OperationLog : LogModel<OperationLogSave>
    {
        /// <summary>
        /// 
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
    }
}
