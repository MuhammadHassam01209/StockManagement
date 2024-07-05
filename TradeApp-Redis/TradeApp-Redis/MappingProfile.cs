using AutoMapper;
using DataAccess.Entities;
using TradeApp_Redis.DTO;
namespace TradeApp_Redis
{
   
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<StockDTO, Stock>();
            CreateMap<Stock, StockDTO>();
            //CreateMap<NewStockDTO,Stock>();
            CreateMap<NewStockDTO, Stock>();
            CreateMap<Stock, NewStockDTO>();
           // CreateMap<UpdateStockDTO, Stock>();
            CreateMap<Stock, UpdateStockDTO>();
            CreateMap<UpdateStockDTO, Stock>()
            .ForMember(dest => dest.Symbol, opt => opt.Ignore()); // Ignore Symbol if it's not supposed to be updated

        }
    }
}
