using AutoMapper;
using ClientManagement.Models;
using ClientManagement.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ClientManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;

        public ClientController(IClientRepository clientRepository) 
        {
            _clientRepository = clientRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _clientRepository.GetClients();

            if (clients != null && clients.Count == 0)
            {
                return BadRequest("No clients found.");
            }

            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetClientsById([FromRoute] int id)
        {
            var clients = await _clientRepository.GetClientsById(id);

            if (clients == null)
            {
                return BadRequest($"Client with ID {id} not found.");
            }

            return Ok(clients);
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
        public async Task<ActionResult> UpdateClients([FromBody] ClientModel clientModel, [FromRoute] int id)
        {
            var existingClient = await _clientRepository.GetClientsById(id);

            if (existingClient == null)
            {
                return BadRequest($"Client Id {id} is not found.");
            }

            await _clientRepository.UpdateClients(existingClient, clientModel);

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

