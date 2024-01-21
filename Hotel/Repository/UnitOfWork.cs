using Hotel.Data;
using Hotel.IRepository;

namespace Hotel.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly DataBaseContext _context;
        private IGenericRepository<Country> _countryRepository;
        private IGenericRepository<Hotel.Data.Hotel> _hotelRepository;


        public UnitOfWork(DataBaseContext context)
        {
                _context = context;
        }
        //? nulable

        //?? اگر null بود

        //=> READ ONLY
        public IGenericRepository<Data.Hotel> Hotels => _hotelRepository?? new GenericRepository<Hotel.Data.Hotel>(_context);

        public IGenericRepository<Country> Countries => _countryRepository ?? new GenericRepository<Country>(_context);

        //public IGenericRepository<Data.Hotel> Hotels 
        //{
        //    get
        //    {
        //        return _hotelRepository;
        //    }
        //}
        //public IGenericRepository<Country> Countries 
        //{
        //    get
        //    {
        //        return _countryRepository;
        //    }
        //}

        public void Dispose()
        {
          _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
