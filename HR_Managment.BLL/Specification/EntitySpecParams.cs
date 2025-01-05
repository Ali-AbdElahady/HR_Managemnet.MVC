using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Management.BLL.Specification
{
    public class EntitySpecParams
    {
        public string? Sort { get; set; }
        private int pageSize = 5;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value == 30 ? 30 : value > 10 ? 10 : value; }
        }
        public int pageNumber { get; set; } = 1;
        private string? search;
        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }
    }
}
