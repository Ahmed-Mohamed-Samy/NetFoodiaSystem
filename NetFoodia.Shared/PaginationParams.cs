using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Shared
{
    public abstract class PaginationParams
    {
        private const int DefaultPageSize = 10;
        private const int MaxPageSize = 20;

        private int _pageIndex = 1;
        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = value <= 0 ? 1 : value;
        }

        private int _pageSize = DefaultPageSize;
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value <= 0) _pageSize = DefaultPageSize;
                else if (value > MaxPageSize) _pageSize = MaxPageSize;
                else _pageSize = value;
            }
        }
    }
}
