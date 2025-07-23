using PetShop.Application.Dtos;

namespace PetShop.Application.AppResponses ;

    public class CreatePetResponse(bool success, string message) :BaseResponse(success, message);
    
    public class GetAllPetsResponse(bool success, string message, List<PetDto> pets ) : BaseResponse(success, message)
    {
        public List<PetDto> Data { get; set; } = pets;
    }