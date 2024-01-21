using Hotel.Data;

namespace Hotel.IRepository
{
    public interface IUnitOfWork:IDisposable
    {

        //IGenericRepository<Hotel.Data.Hotel> Hotels;   

         IGenericRepository<Hotel.Data.Hotel> Hotels { get; }

         IGenericRepository<Country> Countries { get; }

        Task Save();


    }
}
