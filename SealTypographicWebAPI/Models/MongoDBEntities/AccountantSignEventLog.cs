using CommonLib.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SealTypographicWebAPI.Models.LogReport.AccountantSignLog;
using System;

namespace SealTypographicWebAPI.Models.MongoDBModel
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountantSignEventLog : LogModel<AccountantSignEventLogSave>
    {
        private DateTime dateTime;

        /// <summary>
        /// ID
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
    }
}
