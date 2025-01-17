using Domain.Filter;

namespace Infrastructure.Response;

    public class PaginationResponse<T> : BaseFilter
    {
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public T? Data { get; set; }

        public PaginationResponse(int pageSize, int pageNumber, int totalRecords, T data) : base(pageNumber, pageSize)
        {
            Data = data;
            TotalRecords = totalRecords;
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        }
}