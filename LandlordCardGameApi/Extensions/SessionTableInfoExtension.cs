using Azure.Core;
using Azure.Data.Tables;
using LandlordCardGameApi.Models;
using Newtonsoft.Json;

namespace LandlordCardGameApi.Extensions
{
    internal static class SessionInfoExtension
    {
        public static TableEntity ToTableEntity(this SessionInfo sessionInfo)
        {
            ArgumentNullException.ThrowIfNull(sessionInfo);
            ArgumentException.ThrowIfNullOrWhiteSpace(sessionInfo.PartitionKey);
            ArgumentException.ThrowIfNullOrWhiteSpace(sessionInfo.RoomId);
            
            return new(sessionInfo.PartitionKey, sessionInfo.RoomId)
            {
                [nameof(SessionInfo.AcsConnectionId)] = sessionInfo.AcsConnectionId,
                [nameof(SessionInfo.AcsUsers)] = JsonConvert.SerializeObject(sessionInfo.AcsUsers),
                [nameof(SessionInfo.RoomStatus)] = sessionInfo.RoomStatus.ToString(),
                [nameof(SessionInfo.StartTime)] = sessionInfo.StartTime,
                ETag = sessionInfo.ETag
            };
        }

        public static SessionInfo ToSessionInfo(this TableEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentException.ThrowIfNullOrWhiteSpace(entity.PartitionKey);
            ArgumentException.ThrowIfNullOrWhiteSpace(entity.RowKey);

            SessionInfo sessionInfo = new()
            {
                PartitionKey = entity.PartitionKey,
                RoomId = entity.RowKey,

                AcsConnectionId = entity.GetString(nameof(SessionInfo.AcsConnectionId)),
                RoomStatus = Enum.Parse<RoomStatus>(entity.GetString(nameof(SessionInfo.RoomStatus))),
                StartTime = entity.GetDateTimeOffset(nameof(SessionInfo.StartTime)).GetValueOrDefault().UtcDateTime,
                ETag = entity.ETag
            };

            string value = entity.GetString(nameof(SessionInfo.AcsUsers));
            if (!string.IsNullOrWhiteSpace(value))
            {
                sessionInfo.AcsUsers = JsonConvert.DeserializeObject<IList<AcsUser>>(value);
            }

            return sessionInfo;
        }
    }
}
