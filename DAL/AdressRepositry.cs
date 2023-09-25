using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AddressRepository : IDisposable
    {
        private readonly ClientDbContext _context;

        public AddressRepository(ClientDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Address> GetAllAddresses()
        {
            return _context.Addresses.Include(a => a.Client).Include(a => a.AddressType).ToList();
        }
        public IEnumerable<Address> GetClientAddresses(int clientId)
        {
            return _context.Addresses
                .Include(a => a.Client)
                .Include(a => a.AddressType)
                .Where(a => a.ClientId == clientId)
                .ToList();
        }

        public Address GetAddressById(int id)
        {
            return _context.Addresses.Include(a => a.Client).Include(a => a.AddressType).FirstOrDefault(a => a.AddressId == id);
        }

        public void AddAddresses(ICollection<Address> addresses, Client client)
        {
            if (addresses == null || client == null)
                throw new ArgumentNullException(nameof(addresses));
            var addressList = addresses.ToList();
            foreach (var address in addressList)
            {
                address.ClientId = client.ClientId;

                
                var existingAddress = _context.Addresses
                    .FirstOrDefault(a => a.ClientId == address.ClientId
                                         && a.Type == address.Type
                                         && a.AddressDetail == address.AddressDetail);
                if (existingAddress == null)
                {
                    var newAddress = new Address
                    {
                        ClientId = address.ClientId,
                        Type = address.Type,
                        AddressDetail = address.AddressDetail,
                    };

                    _context.Addresses.Add(newAddress);
                }
            }

            _context.SaveChanges();
        }

  

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
