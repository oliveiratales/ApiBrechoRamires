namespace ApiBrechoRamires.ViewModels.Errors
{
    public class ResultError
    {
        public string? Details { get; set; }

        public short Status { get; set; }

        public string Error { get; set; }

        public DateTimeOffset TimestampUtc { get; set; } = DateTimeOffset.Now;

        public ResultError(string error, short status = 0, DateTimeOffset timestampUtc = default)
        {
            Error = error;

            Status = status;

            TimestampUtc = timestampUtc;
        }
    }
}