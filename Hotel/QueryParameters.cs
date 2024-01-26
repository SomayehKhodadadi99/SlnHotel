namespace Hotel
{
    public class QueryParameters
    {

        private int _pageSize = 15;
        public int StartIndex { get; set; }
        public int PageNumber { get; set; }
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }


        //const int maxPageSize = 50;
        //public int PageNumber { get; set; } = 1;
        //private int _pageSize = 15;

        //public int PageSize
        //{
        //    get
        //    {
        //        return _pageSize;
        //    }
        //    set
        //    {
        //        _pageSize =( value> maxPageSize )? maxPageSize:value;
        //    }
        //}
    }
}
