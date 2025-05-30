using PetEase.Core.DTOs;
using PetEase.Core.Models;

namespace PetEase.Services.Interface
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(CreateOrderDto orderDto);
    }
}
