using Domain.DTO_s;
using Domain.Entities;
using Domain.Filter;
using Infrastructure.Response;

namespace Infrastructure.Service.CarService;


public interface ICarService
{
    Task<PaginationResponse<List<GetCarDto>>> GetAll(CarFilter filter);
    Task<ApiResponse<GetCarDto>> GetCarById(int id);
    Task<ApiResponse<string>> AddCar(CreateCarDto car);
    Task<ApiResponse<string>> UpdateCar(UpdateCarDto car);
    Task<ApiResponse<string>> DeleteCar(int id);
}