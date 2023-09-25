using Microsoft.AspNetCore.Mvc;
using ClientManagement.BLL;
using PresentationLayer.Models; 
using System.Linq;
using DAL;
using System;
using ImportExportLayer;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;

public class ClientsController : Controller
{
    private readonly ClientService _clientService;
    private readonly ClientValidator _clientValidator;
    private readonly AddressTypeRepository _addressTypeRepository;
    private readonly XmlImportService _xmlImportService;
    public ClientsController(ClientService clientService, ClientValidator clientValidator,AddressTypeRepository addressTypeRepository, XmlImportService xmlImportService)
    {
        _clientService = clientService;
        _clientValidator = clientValidator;
        _addressTypeRepository = addressTypeRepository;
        _xmlImportService = xmlImportService;

    }

    
  
    public IActionResult Index(string clientIdFilter, string nameFilter, DateTime? birthDateFilter, string addressFilter)
    {
        var clients = _clientService.GetAllClients();

        if (!string.IsNullOrEmpty(clientIdFilter))
        {
            clients = clients.Where(c => c.ClientId.ToString() == clientIdFilter);
        }
        if (!string.IsNullOrEmpty(nameFilter))
        {
            clients = clients.Where(c => c.Name.Contains(nameFilter, StringComparison.OrdinalIgnoreCase));
        }
        if (birthDateFilter.HasValue)
        {
            clients = clients.Where(c => c.BirthDate.Date == birthDateFilter.Value.Date);
        }
        if (!string.IsNullOrEmpty(addressFilter))
        {
            clients = clients.Where(c => c.Addresses.Any(a => a.AddressDetail.Contains(addressFilter, StringComparison.OrdinalIgnoreCase)));
        }

        var clientViewModels = clients.Select(c => new ClientViewModel
        {
            ClientId = c.ClientId,
            Name = c.Name,
            BirthDate = c.BirthDate,
            Addresses = c.Addresses.ToList(),
        }).ToList();

        ViewData["ClientIdFilter"] = clientIdFilter;
        ViewData["NameFilter"] = nameFilter;
        ViewData["BirthDateFilter"] = birthDateFilter?.ToString("yyyy-MM-dd");
        ViewData["AddressFilter"] = addressFilter;

        return View(clientViewModels);
    }



   
    public IActionResult Create()
    {
        var addressTypes = _addressTypeRepository.GetAllAddressTypes();
        var clientViewModel = new ClientViewModel
        {
            AddressTypes = addressTypes.Select(at => new AddressTypeViewModel
            {
                TypeId = at.TypeId,
                TypeName = at.TypeName
            }).ToList()
        };
        return View(clientViewModel);
        
    }

   
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ClientViewModel clientViewModel)
    {
        if (ModelState.IsValid)
        {
            var client = new DAL.Client
            {
                ClientId = clientViewModel.ClientId,
                Name = clientViewModel.Name,
                BirthDate = clientViewModel.BirthDate,
                Addresses = clientViewModel.Addresses
            };

            try
            {
                _clientService.AddClient(client);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                
                ModelState.AddModelError(string.Empty, ex.Message);
            }

        }
        var addressTypes = _addressTypeRepository.GetAllAddressTypes();
         clientViewModel = new ClientViewModel
        {
            AddressTypes = addressTypes.Select(at => new AddressTypeViewModel
            {
                TypeId = at.TypeId,
                TypeName = at.TypeName
            }).ToList()
        };
        return View(clientViewModel);
    }

    public IActionResult Delete(int id)
    {
        var client = _clientService.GetClientById(id);
        if (client == null)
        {
            return NotFound();
        }
        return View(client);
    }

   
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        _clientService.DeleteClient(id);
        return RedirectToAction(nameof(Index));
    }
 
    public IActionResult ImportXml()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ImportXml(IFormFile xmlFile)
    {
        if (xmlFile == null || xmlFile.Length == 0)
        {
            ModelState.AddModelError("", "Invalid XML file.");
            return View(); 
        }

        try
        {
           
            var tempPath = Path.GetTempFileName();
            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                xmlFile.CopyTo(stream);
            }
            var clients = _xmlImportService.ImportClientsFromXml(tempPath);

            foreach (var client in clients)
            {
                var client_pom = new DAL.Client
                {
                    ClientId = client.ClientId,
                    Name = client.Name,
                    BirthDate = client.BirthDate,
                    Addresses = client.Addresses.Select(a => new DAL.Address
                    {
                        AddressDetail = a.AddressDetail,
                        Type = a.Type
                    }).ToList()
                };

                try
                {
                    _clientService.AddClient(client_pom);
                    
                }
                catch (ArgumentException ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            System.IO.File.Delete(tempPath);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Error importing XML: " + ex.Message);
            return View(); 
        }
    }
}
