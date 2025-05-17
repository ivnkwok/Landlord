namespace LandlordCardGameApi.Settings
{
    public class SessionStorageSettings
    {
        public int RoomIdLength { get; set; }

        public string RoomIdAlphabet { get; set; }

        public string TableRetentionSpan { get; set; }

        public TimeSpan MaxRoomIdAge { get; set; }

        public TimeSpan MaxClockSkew { get; set; }

        public string TableName { get; set; }

        public string ConnectionString { get; set; }
    }
}
