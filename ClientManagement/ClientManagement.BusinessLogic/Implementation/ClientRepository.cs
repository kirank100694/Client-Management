using AutoMapper;
using ClientManagement.BusinessLogic.Contracts;
using ClientManagement.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.BusinessLogic.Implementation
{
    public class ClientRepository : IClientRepository
    {
        private readonly ClientManagmentContext _clientContext;
        private readonly IMapper _mapper;

        public ClientRepository(ClientManagmentContext clientContext, IMapper mapper)
        {
            _clientContext = clientContext;
            _mapper = mapper;
        }

        public async Task<List<ClientModel>> GetClients(string name = "", string sortBy = "", bool sortByDescending = false,
            int page = 0, int pageSize = 0)
        {
            var clients = _clientContext.Clients.AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(name))
            {
                clients = clients.Where(m => m.ClientName.ToLower().Contains(name) || m.LicenceKey.ToString().Contains(name));
            }

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy == "name")
                {
                    clients = sortByDescending ? clients.OrderByDescending(m => m.ClientName) : clients.OrderBy(m => m.ClientName);
                }
                else if (sortBy == "email")
                {
                    clients = sortByDescending ? clients.OrderByDescending(m => m.LicenceKey) : clients.OrderBy(m => m.LicenceKey);
                }
            }

            // Pagination
            var totalItems = await clients.CountAsync();

            var pagedEmployees = await clients.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var employeeModels = _mapper.Map<List<ClientModel>>(pagedEmployees);

            return employeeModels;
        }

        public async Task<ClientModel> GetClientsById(int clientId)
        {
            var clients = await _clientContext.Clients.FindAsync(clientId);
            return _mapper.Map<ClientModel>(clients);
        }

        public async Task<int> AddClients(Client clientModel)
        {
            var clients = _mapper.Map<Client>(clientModel);
            clients.CreatedDate = DateTime.Now;

            _clientContext.Clients.Add(clients);
            await _clientContext.SaveChangesAsync();

            return clients.ClientId;
        }

        public async Task UpdateClient(ClientModel existingClient, ClientModel clientModel)
        {
            _mapper.Map(clientModel, existingClient);

            existingClient.UpdatedDate = DateTime.Now;

            await _clientContext.SaveChangesAsync();
        }

        public async Task UpdateClients(ClientModel existingClient, JsonPatchDocument clientModel)
        {
            clientModel.ApplyTo(existingClient);

            existingClient.UpdatedDate = DateTime.Now;

            await _clientContext.SaveChangesAsync();
        }

        public async Task DeleteClientById(int clientId)
        {
            var clients = new Client() { ClientId = clientId };

            _clientContext.Clients.Remove(clients);

            await _clientContext.SaveChangesAsync();
        }

        public async Task<bool> IsClientsExists(int id)
        {
            return await _clientContext.Clients.AnyAsync(e => e.ClientId == id);
        }

        public async Task<bool> IsClientsExists(string clientName)
        {
            return await _clientContext.Clients.AnyAsync(c => c.ClientName == clientName);
        }
    }
}
