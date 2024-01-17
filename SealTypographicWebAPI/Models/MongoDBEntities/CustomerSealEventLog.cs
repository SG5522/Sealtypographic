using CommonLib.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SealTypographicWebAPI.Models.LogReport.CustomerSealEventLog;
using System;

namespace SealTypographicWebAPI.Models.MongoDBModel
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerSealEventLog : LogModel<CustomerSealEventLogSave>
    {
        private DateTime dateTime;

        /// <summary>
        /// 
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
    }
}
