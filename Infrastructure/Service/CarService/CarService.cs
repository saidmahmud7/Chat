
﻿using System.Net;
using AutoMapper;
using Domain;
using Domain.DTO_s;
using Domain.Entities;
using Domain.Filter;
using Infrastructure.Data;
using Infrastructure.Response;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.CarService;

public class CarService(DataContext context, IMapper mapper) : ICarService
{
    public async Task<PaginationResponse<List<GetCarDto>>> GetAll(CarFilter filter)
    {
        IQueryable<Car> cars = context.Cars;
        if (!string.IsNullOrEmpty(filter.Model))
            cars = cars.Where(c => c.Model.ToLower().Contains(filter.Model.ToLower()));
        if (filter.Year.HasValue)
            cars = cars.Where(c => c.Year == filter.Year);
        if (!string.IsNullOrEmpty(filter.Color))
            cars = cars.Where(c => c.Color.ToLower().Contains(filter.Color.ToLower()));
        int total = await cars.CountAsync();
        var result = await cars
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(x => mapper.Map<GetCarDto>(x))
            .ToListAsync();
        return new PaginationResponse<List<GetCarDto>>(filter.PageSize, filter.PageNumber, total, result);
    }

    public async Task<ApiResponse<GetCarDto>> GetCarById(int id)
    {
        var cars = await context.Cars.FirstOrDefaultAsync(x => x.Id == id);
        var carDto = mapper.Map<GetCarDto>(cars);
        return new ApiResponse<GetCarDto>(carDto);
    }

    public async Task<ApiResponse<string>> AddCar(CreateCarDto car)
    {
        var cars = mapper.Map<Car>(car);
        await context.Cars.AddAsync(cars);
        var res = await context.SaveChangesAsync();
        return res == 0
            ? new ApiResponse<string>(HttpStatusCode.InternalServerError, "Internal Server Error")
            : new ApiResponse<string>(HttpStatusCode.OK, "Car created");
    }

    public async Task<ApiResponse<string>> UpdateCar(UpdateCarDto car)
    {
        var existingCar = await context.Cars.FirstOrDefaultAsync(x => x.Id == car.Id);
        if (existingCar == null)
            return new ApiResponse<string>(HttpStatusCode.NotFound,"Car Not Found");
        
        existingCar.Id = car.Id;
        existingCar.Model = car.Model;
        existingCar.Year = car.Year;
        existingCar.Price = car.Price;
        existingCar.Mileage = car.Mileage;
        existingCar.Color = car.Color;
        existingCar.FuelType = car.FuelType;
        existingCar.Transmission = car.Transmission;
        existingCar.IsAvailable = car.IsAvailable;
        existingCar.Description = car.Description;
        existingCar.ImageUrl = car.ImageUrl;
  
        var res = await context.SaveChangesAsync();
        return res == 0
            ? new ApiResponse<string>(HttpStatusCode.InternalServerError, "Internal Server Error")
            : new ApiResponse<string>(HttpStatusCode.OK, "Car updated");
    }

    public async Task<ApiResponse<string>> DeleteCar(int id)
    {
        var existingCar = await context.Cars.FirstOrDefaultAsync(x => x.Id == id);
        if (existingCar == null)
            return new ApiResponse<string>(HttpStatusCode.NotFound,"Car Not Found");
        context.Cars.Remove(existingCar);
        var res = await context.SaveChangesAsync();
        return res == 0
            ? new ApiResponse<string>(HttpStatusCode.InternalServerError, "Internal Server Error")
            : new ApiResponse<string>(HttpStatusCode.OK, "Car deleted");
    }
}
