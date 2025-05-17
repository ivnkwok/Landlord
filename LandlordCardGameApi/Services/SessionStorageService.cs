using Azure;
using Azure.Data.Tables;
using LandlordCardGameApi.Extensions;
using LandlordCardGameApi.Models;
using LandlordCardGameApi.Settings;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace LandlordCardGameApi.Services
{
    public class SessionStorageService : ISessionStorageService
    {
        private readonly SessionStorageSettings _settings;

        public SessionStorageService(IOptions<SessionStorageSettings> options)
        {
            _settings = options.Value;
        }

        public async Task<SessionInfo> Create(SessionInfo sessionInfo)
        {
            DateTime utcNow = DateTime.UtcNow;
            var tableClient = new TableClient(_settings.ConnectionString, GetTableName(utcNow));
            sessionInfo.StartTime = utcNow;
            sessionInfo.PartitionKey = GetPartitionKey(utcNow);
            sessionInfo.RoomId = GetRoomId();

            // Create the table if it doesn't exist
            tableClient.CreateIfNotExists();

            await tableClient.UpsertEntityAsync(sessionInfo.ToTableEntity());

            return sessionInfo;
        }

        public async Task<SessionInfo> Retrieve(string rowKey)
        {
            string queryFilter = TableClient.CreateQueryFilter<TableEntity>(
                 e => e.RowKey == rowKey);

            DateTime utcNow = DateTime.UtcNow;
            var tableNames = new List<string>() { GetTableName(utcNow), GetTableName(utcNow.AddDays(-1)) };
            foreach (var tableName in tableNames)
            {
                try
                {
                    var tableClient = new TableClient(_settings.ConnectionString, tableName);

                    AsyncPageable<TableEntity> items = tableClient.QueryAsync<TableEntity>(queryFilter);

                    var entities = await items.ToListAsync();
                    if (entities.Count > 0)
                    {
                        return entities.Select(e => e.ToSessionInfo())
                            .OrderByDescending(r => r.StartTime)
                            .First();
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }

            throw new RequestFailedException("Entity not found.");
        }

        public async Task<SessionInfo> Retrieve(string partitionKey, string rowKey)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(partitionKey);
            ArgumentException.ThrowIfNullOrWhiteSpace(rowKey);

            DateTime utcNow = DateTime.UtcNow;
            var tableNames = new List<string>() { GetTableName(utcNow), GetTableName(utcNow.AddDays(-1)) };
            TableEntity entity = null;

            foreach (var tableName in tableNames)
            {
                try
                {
                    var tableClient = new TableClient(_settings.ConnectionString, tableName);
                    entity = await tableClient.GetEntityAsync<TableEntity>(partitionKey, rowKey);
                    if (entity != null)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }
            }

            if (entity == null)
            {
                throw new RequestFailedException("Entity not found.");
            }

            return entity.ToSessionInfo();
        }

        public async Task Update(SessionInfo sessionInfo)
        {
            ArgumentNullException.ThrowIfNull(sessionInfo);

            DateTime utcNow = DateTime.UtcNow;
            var tableNames = new List<string>() { GetTableName(utcNow), GetTableName(utcNow.AddDays(-1)) };
            Response response = null;

            foreach (var tableName in tableNames)
            {
                try
                {
                    var tableClient = new TableClient(_settings.ConnectionString, tableName);
                    var entity = sessionInfo.ToTableEntity();
                    response = await tableClient.UpdateEntityAsync(sessionInfo.ToTableEntity(), entity.ETag, TableUpdateMode.Replace);
                    if (!response.IsError)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }
            }

            if (response == null || response.IsError)
            {
                throw new RequestFailedException("Entity not found.");
            }
        }

        private string GetPartitionKey(DateTime dateTime)
        {
            return (dateTime.Ticks / (this._settings.MaxRoomIdAge + this._settings.MaxClockSkew).Ticks)
                .ToString("D18", CultureInfo.InvariantCulture);
        }

        private string GetTableName(DateTime daytime)
        {
            return this._settings.TableName + daytime.ToString("yyyyMMdd");
        }

        private string GetRoomId()
        {
            int length = this._settings.RoomIdLength;
            string alphabet = this._settings.RoomIdAlphabet;
            var res = new StringBuilder(length);

            using (var rng = RandomNumberGenerator.Create())
            {
                int count = (int)Math.Ceiling(Math.Log(alphabet.Length, 2) / 8.0);

                int offset = BitConverter.IsLittleEndian ? 0 : sizeof(uint) - count;
                int max = (int)(Math.Pow(2, count * 8) / alphabet.Length) * alphabet.Length;
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (res.Length < length)
                {
                    rng.GetBytes(uintBuffer, offset, count);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    if (num < max)
                    {
                        res.Append(alphabet[(int)(num % alphabet.Length)]);
                    }
                }
            }

            return res.ToString();
        }
    }
}
