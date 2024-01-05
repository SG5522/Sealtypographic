using CommonLib.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SealTypographicWebAPI.Models.LogReport;
using Serilog.Events;

namespace SealTypographicWebAPI.Models.MongoDBModel
{
    /// <summary>
    /// 
    /// </summary>
    public class OperationLog<T> : LogModel
    {
        /// <summary>
        /// 
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="logModel"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static OperationLog<T> MapFrom(LogModel<T> logModel)
        {
            if (logModel == null) throw new ArgumentNullException(nameof(logModel));

            return new OperationLog<T>
            {
                Data = logModel.Data,
                DateTime = logModel.DateTime,
                OperateType = logModel.OperateType,
                FunctionType = logModel.FunctionType,
                SystemType = logModel.SystemType,
                UserId = logModel.UserId,
                UserName = logModel.UserName                
            };
        }
    }
}
