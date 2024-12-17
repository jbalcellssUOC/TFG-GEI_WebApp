using Entities.Models;
using AutoMapper;
using Entities.DTOs;

namespace BusinessLogicLayer.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // Barcode Mappings
            CreateMap<AppCBStatic, ApiBCStaticDTO>()                                                                    // From POCO to DTO
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.IsoDateC));
            CreateMap<PaginatedStaticBarcodeList<AppCBStatic>, PaginatedStaticBarcodeList<ApiBCStaticDTO>>()            // From POCO to DTO
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<AppCBDynamic, ApiBCDynamicDTO>()                                                                  // From POCO to DTO
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.IsoDateC));                            
            CreateMap<PaginatedDynamicBarcodeList<AppCBDynamic>, PaginatedDynamicBarcodeList<ApiBCDynamicDTO>>()        // From POCO to DTO
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            // Product Mappings
            CreateMap<PaginatedProductList<AppProduct>, PaginatedProductList<ApiProductDTO>>()                          // From POCO to DTO    
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<AppProduct, ApiProductDTO>()                                                                      // From POCO to DTO    
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.IsoDateC));

            CreateMap<BodyProductDTO, AppProduct>();                                                                    // From DTO to POCO

            CreateMap<AppProduct, AppProduct>();                                                                        // From POCO to POCO

            // Other Mappings
            CreateMap<AppUser, UserDetailsDTO>()                                                                        // From POCO to DTO
                .ForMember(dest => dest.AppUsersStats, opt => opt.MapFrom(src => src.AppUsersStats));
        }
    }
}
