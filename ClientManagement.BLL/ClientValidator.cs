namespace ClientManagement.BLL
{
    using DAL; 
    using System;
    using System.Linq;

    public class ClientValidator
    {
        private readonly ClientRepository _clientRepository;
        private readonly AddressRepository _addressRepository;

        public ClientValidator(ClientRepository clientRepository, AddressRepository addressRepository)
        {
            _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
            _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        }
        public bool Validate(Client client, out string errorMessage)
        {
            if (client == null)
            {
                errorMessage = "Client cannot be null.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(client.Name))
            {
                errorMessage = "Client name is required.";
                return false;
            }

            if (client.BirthDate > DateTime.Now)
            {
                errorMessage = "Invalid birth date.";
                return false;
            }
            if (client.ClientId < 0)
            {
                errorMessage = "ID must be positive";
                return false;

            }
            
            if (client.Addresses == null || !client.Addresses.Any())
            {
                errorMessage = "Client must have at least one address.";
                return false;
            }

            foreach (var address in client.Addresses)
            {
                if (!ValidateAddress(address, out errorMessage))
                {
                    
                    return false;
                }
            }
            var existingClient = _clientRepository.GetAllClients()
         .FirstOrDefault(c => c.ClientId == client.ClientId);

            if (existingClient != null)
            {
    
                    errorMessage = "A client already exist";
                    return false;
                
            }
            errorMessage = null;
            return true;
        }

        private bool ValidateAddress(Address address, out string errorMessage)
        {
            if (address == null)
            {
                errorMessage = "Address cannot be null.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(address.AddressDetail))
            {
                if (address.Type == 1)
                {
                    errorMessage = "Address detail is required.";
                    return false;
                }
            }

            if (address.Type <= 0)
            {
                errorMessage = "Invalid address type.";
                return false;
            }
            errorMessage = null;
            return true;
        }
    }

}
