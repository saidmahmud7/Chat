using Domain.DTO_s;
using Domain.Filter;
using Infrastructure.Response;
using Infrastructure.Service.CarService;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;
[ApiController]
[Route("api/[controller]")]
public class CarController(ICarService service)
{
    [HttpGet]
    public async Task<PaginationResponse<List<GetCarDto>>> GetAll([FromQuery] CarFilter filter) => await service.GetAll(filter);
    [HttpPost]
    public async Task<ApiResponse<string>> AddCar(CreateCarDto car ) => await service.AddCar(car);
}