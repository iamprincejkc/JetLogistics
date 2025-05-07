namespace JetLogistics.Common.Models
{
    public class DbReturnModel
    {
        public string Message { get; set; }
        public bool Success { get; set; }

        public string Token { get; set; }
        public int BookingId { get; set; }
        public int Id { get; set; }

        public object[] Data { get; set; }
        public object[] ExportData { get; set; }

        public bool IsExist { get; set; }

        public int Count { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int CurrentFirstSize { get; set; }
        public int CurrentLastSize { get; set; }

        public string InvoiceTemplate { get; set; }

        public DbReturnModel(string message, bool success, object[] data)
        {
            Message = message;
            Success = success;
            Data = data ?? [];
        }

        public DbReturnModel(string message, bool success)
        {
            Message = message;
            Success = success;
        }

        public DbReturnModel(string message, int bookingId, bool success)
        {
            Message = message;
            BookingId = bookingId;
            Success = success;
        }

        public DbReturnModel(string message, int bookingId, bool success, object[] data, object[] exportData)
        {
            Message = message;
            BookingId = bookingId;
            Success = success;
            Data = data;
            ExportData = exportData;
        }

        public DbReturnModel(bool success)
        {
            Success = success;
        }

        public DbReturnModel(string message, bool success, bool isExist)
        {
            Message = message;
            Success = success;
            IsExist = isExist;
        }

        public DbReturnModel(string message, bool success, object[] data, int count, int currentPage, int pageSize, int currentFirstSize, int currentLastSize)
        {
            Message = message;
            Success = success;
            Data = data;
            Count = count;
            CurrentPage = currentPage;
            PageSize = pageSize;
            CurrentFirstSize = currentFirstSize;
            CurrentLastSize = currentLastSize;
        }

        public DbReturnModel(string message, bool success, object[] data, int count, int currentPage, int pageSize, int currentFirstSize, int currentLastSize, int id)
        {
            Message = message;
            Success = success;
            Data = data;
            Count = count;
            CurrentPage = currentPage;
            PageSize = pageSize;
            CurrentFirstSize = currentFirstSize;
            CurrentLastSize = currentLastSize;
            Id = id;
        }
    }
}
