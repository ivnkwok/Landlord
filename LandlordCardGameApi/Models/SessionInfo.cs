namespace LandlordCardGameApi.Models
{
    public enum RoomStatus
    {
        WaitPlayers,
        Started,
        Closed
    }

    public class SessionInfo
    {
        public string PartitionKey { get; set; }

        public string RoomId { get; set; }

        public RoomStatus RoomStatus { get; set; }

        public IList<AcsUser> AcsUsers { get; set; } = new List<AcsUser>();

        public string AcsConnectionId { get; set; }

        public DateTime StartTime { get; set; }

        public Azure.ETag ETag { get; set; }
    }
}
