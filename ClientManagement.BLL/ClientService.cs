using DAL;
using System;
using System.Collections.Generic;
using System.Linq;



namespace ClientManagement.BLL
{
    public class ClientService
    {
        private readonly ClientRepository _clientRepository;
        private readonly ClientValidator _clientValidator;
        private readonly AddressRepository _addressrepository;

        public ClientService(ClientRepository clientRepository, ClientValidator clientValidator, AddressRepository addressRepository)
        {
            _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
            _clientValidator = clientValidator ?? throw new ArgumentNullException(nameof(clientValidator));
            _addressrepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        }

        public IEnumerable<Client> GetAllClients()
        {
            return _clientRepository.GetAllClients();
        }

        public Client GetClientById(int id)
        {
            return _clientRepository.GetClientById(id);
        }

        public void AddClient(Client client)
        {
            if (_clientValidator.Validate(client, out string errorMessage))
            {

                var addressList = client.Addresses.ToList(); 
                addressList.RemoveAll(address => address.AddressDetail == null); 

           
                client.Addresses = addressList;


                _clientRepository.AddClient(client);
            }
            else
            {
                throw new ArgumentException(errorMessage);
            }
        }
        public void DeleteClient(int id)
        {
            _clientRepository.DeleteClient(id);
        }
    }
}