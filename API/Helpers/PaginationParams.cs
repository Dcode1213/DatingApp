﻿namespace API.Helpers
{
    public class PaginationParams
    {
        private const int MaxPageSize = 50;
        public int pageNumber { get; set; } = 1;
        public int pageSize = 10;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
