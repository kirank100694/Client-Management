using ClientManagement.BusinessLogic.Contracts;
using ClientManagement.Caching;
using ClientManagement.Models;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ClientManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly ICacheProvider _cacheProvider;

        public ClientController(IClientRepository clientRepository, ICacheProvider cacheProvider) 
        {
            _clientRepository = clientRepository;
            _cacheProvider = cacheProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients(string name = "", string sortBy = "", bool sortByDescending = false,
            int page = 0, int pageSize = 0)
        {
            var clients = await _clientRepository.GetClients(name, sortBy, sortByDescending,
             page, pageSize);

            if (clients != null && clients.Count == 0)
            {
                return BadRequest("No clients found.");
            }

            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetClientsById([FromRoute] int id)
        {
            if (!_cacheProvider.TryGetValue(CacheKeys.Client, out List<ClientModel> patients))
            {
                patients = await _clientRepository.GetClients();

                var cacheEntryOption = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(30),
                    Size = 1024
                };

                _cacheProvider.Set(CacheKeys.Client, patients, cacheEntryOption);

                if (patients == null)
                {
                    return BadRequest($"patient with ID {id} not found.");
                }
            }

            return Ok(patients);
        }

        [HttpPost]
        public async Task<ActionResult> AddCLients([FromBody] Client clientModel)
        {
            if (await _clientRepository.IsClientsExists(clientModel.ClientName))
            {
                return BadRequest("Client is already exists.");
            }

            var id = await _clientRepository.AddClients(clientModel);

            return CreatedAtAction(nameof(GetClientsById), new { id = id, Controller = "client" }, id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateClient([FromBody] ClientModel clientModel, [FromRoute] int id)
        {
            var existingClient = await _clientRepository.GetClientsById(id);

            if (existingClient == null)
            {
                return BadRequest($"Client Id {id} is not found.");
            }

            await _clientRepository.UpdateClient(existingClient, clientModel);

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateClients([FromBody] JsonPatchDocument clientModel, [FromRoute] int id)
        {
            var existingClient = await _clientRepository.GetClientsById(id);

            if (existingClient == null)
            {
                return BadRequest($"Client Id {id} is not found.");
            }

            await _clientRepository.UpdateClients(existingClient, clientModel);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteClientById(int id)
        {

            if (!await _clientRepository.IsClientsExists(id))
            {
                return BadRequest($"CLient Id {id} is not found.");
            }

            await _clientRepository.DeleteClientById(id);

            return Ok();
        }
    }
}

