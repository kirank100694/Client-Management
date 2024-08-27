using ClientManagement.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace ClientManagement.BusinessLogic.Contracts
{
    public interface IClientRepository
    {
        Task<List<ClientModel>> GetClients(string name = "", string sortBy = "", bool sortByDescending = false,
            int page = 0, int pageSize = 0);

        Task<ClientModel> GetClientsById(int clientId);

        Task<int> AddClients(Client clientModel);

        Task UpdateClient(ClientModel existingClient, ClientModel clientModel);

        Task UpdateClients(ClientModel existingClient, JsonPatchDocument clientModel);

        Task DeleteClientById(int clientId);

        Task<bool> IsClientsExists(int id);

        Task<bool> IsClientsExists(string clientName);
    }
}