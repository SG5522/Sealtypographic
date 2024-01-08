using CommonLib.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SealTypographicWebAPI.Models.MongoDBEntities;

namespace SealTypographicWebAPI.Models.MongoDBModel
{
    /// <summary>
    /// 
    /// </summary>
    public class OperationLog : LogModel<OperationLogForm>
    {
        /// <summary>
        /// 
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <summary>
        /// OperationLog AutoMap
        /// </summary>
        /// <param name="logModel"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static OperationLog MapFrom(LogModel<OperationLogForm> logModel)
        {
            if (logModel == null) throw new ArgumentNullException(nameof(logModel));

            return new OperationLog
            {
                Data = logModel.Data,
                DateTime = logModel.DateTime,
                OperateType = logModel.OperateType,
                FunctionType = logModel.FunctionType,
                LogLevel = logModel.LogLevel,
                SystemType = logModel.SystemType,
                UserId = logModel.UserId,
                UserName = logModel.UserName
            };
        }
    }
}
