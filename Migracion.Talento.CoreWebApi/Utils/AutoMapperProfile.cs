using Migracion.Talento.Models;
using AutoMapper;
using Migracion.Talento.Entities.Models;
using CommonTools.DTOs.Query;
using CommonTools.DTOs.Register;

namespace Migracion.Talento.CoreWebApi.Utils
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            //dtos a entidades, para inserts updates 
            CreateMap<ActivityRegisterDto, Activities>();
            CreateMap<AirLineRegisterDto, AirLines>();
            CreateMap<AirPortRegisterDto, AirPorts>();
            CreateMap<CompanyRegisterDto, Companies>();
            CreateMap<CountryRegisterDto, Countries>();
            CreateMap<EstateRegisterDto, Estates>();
            CreateMap<EventRegisterDto, Events>();
            CreateMap<GenderRegisterDto, Genders>();
            CreateMap<NationalityRegisterDto, Nationalities>();
            CreateMap<ProcessRegisterDto, Process>();
            CreateMap<RegEvenStateDateDto, RegEvenStateDate>();
            CreateMap<RegEventDto, RegEvents>();
            CreateMap<RegInviteDto, RegInvite>();
            CreateMap<RoleByCompanyDto, RoleByCompany>();
            CreateMap<StatusRegisterDto, Status>();
            CreateMap<UserRegisterDto, User>();
            CreateMap<EventTypesRegisterDto, EventTypes>();
            CreateMap<DocumentRegisterDto, Documents>();



            //entitades a dtos para querys
            CreateMap<Estates, EstatesDto>();
            CreateMap<EventRegisterDto, EventsDto>();
            CreateMap<Genders, GendersDto>();
            CreateMap<Countries, CountriesDto>();
            CreateMap<Nationalities, NationalitiesDto>();
            CreateMap<Activities, Activitiesdto>();
            CreateMap<Process, ProcessDto>();
            CreateMap<AirPorts, AirPortsDto>();
            CreateMap<Companies, CompaniesDto>();
            
            CreateMap<AirLines, AirLinesDto>();
            CreateMap<Status, StatusDto>();

            CreateMap<AirPortsDto, AirPorts>();
            CreateMap<CountriesDto, Countries>();
            CreateMap<NationalitiesDto, Nationalities>();
            CreateMap<RegEvents, QryRegEventDto>();
            CreateMap<RegInvite, RegInviteDto>();
            CreateMap<RegInvite, InviteDto>();
            CreateMap<RegEvenStateDate, RegEvenStateDateDto>();
            CreateMap<ProcessRegisterDto, Process>();
            CreateMap<EventTypesDto, EventTypes>();
            CreateMap<Documents,DocumentDto>();

        }
    }
}
