namespace ApiBrechoRamires.ViewModels.ResponseModels
{
    public class ListModel<T>
    {
      public uint PageNumber { get; set; }
      public uint PageSize { get; set; }
      public int TotalNumberOfPages { get; set; }
      public int TotalNumberOfRecords { get; set; }

      public IList<T> Results { get; set; } = new List<T>();
    }

}