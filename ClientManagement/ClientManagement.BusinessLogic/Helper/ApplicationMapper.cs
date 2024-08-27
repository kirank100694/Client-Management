using AutoMapper;
using ClientManagement.Models;

namespace ClientManagement.Helper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() 
        {
            CreateMap<Client , ClientModel>().ReverseMap();
        }
    }
}
