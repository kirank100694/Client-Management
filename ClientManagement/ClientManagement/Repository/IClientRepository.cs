using ClientManagement.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace ClientManagement.Repository
{
    public interface IClientRepository
    {
        Task<List<ClientModel>> GetClients();

        Task<ClientModel> GetClientsById(int clientId);

        Task<int> AddClients(Client clientModel);

        Task UpdateClients(ClientModel existingClient, ClientModel clientModel);

        Task UpdateClients(ClientModel existingClient, JsonPatchDocument clientModel);

        Task DeleteClientById(int clientId);

        Task<bool> IsClientsExists(int id);

        Task<bool> IsClientsExists(string clientName);
    }
}