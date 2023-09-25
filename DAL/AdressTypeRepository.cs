using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class AddressTypeRepository : IDisposable
    {
        private readonly ClientDbContext _context;

        public AddressTypeRepository(ClientDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<AddressType> GetAllAddressTypes()
        {
            return _context.AddressTypes.ToList();
        }

        public AddressType GetAddressTypeById(int id)
        {
            return _context.AddressTypes.FirstOrDefault(at => at.TypeId == id);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
