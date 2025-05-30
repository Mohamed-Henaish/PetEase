using PetEase.Core.DTOs;
using PetEase.Core.Models;
using PetEase.Core.Models.Enum;
using PetEase.Core.Repositories;
using PetEase.Services.Interface;
using Microsoft.AspNetCore.Identity.UI.Services;
using PetEase.Services.Service;
using Microsoft.Extensions.Configuration;
public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly string _adminEmail;
    public OrderService(IUnitOfWork unitOfWork, IEmailSender emailSender, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
        _adminEmail = configuration["EmailSettings:Email"] ?? "defaultadmin@example.com";
    }

    public async Task<int> CreateOrderAsync(CreateOrderDto orderDto)
    {
        if (orderDto.Products == null || !orderDto.Products.Any())
            throw new ArgumentException("Order must contain at least one product.");

        // Calculate subtotal & total
        var subtotal = orderDto.Products.Sum(p => p.UnitPrice * p.Quantity);
        var totalPrice = subtotal + orderDto.ShippingCost;

        // Create OrderHeader
        var orderHeader = new OrderHeader
        {
            Name = orderDto.Name,
            Email = orderDto.Email,
            Address = orderDto.Address,
            Government = orderDto.Government,
            City = orderDto.City,
            PostalCode = orderDto.PostalCode,
            PhoneNumber = orderDto.PhoneNumber,
            AdditionalPhoneNumber = orderDto.AdditionalPhoneNumber,
            Notes = orderDto.Notes,
            CarrierName = orderDto.CarrierName,
            ShippingCost = orderDto.ShippingCost,
            SubTotal = subtotal,
            TotalPrice = totalPrice,
            Status = OrderStatus.Pending,
            OrderDate = DateTime.UtcNow
        };

        await _unitOfWork.OrderHeaderRepository.AddAsync(orderHeader);
        await _unitOfWork.CompleteAsync();

        var shopIds = new HashSet<int>();
        var orderDetailsList = new List<OrderDetails>();

        // Add OrderDetails for each product
        foreach (var productDto in orderDto.Products)
        {
            var product = await _unitOfWork.ProductRepository.GetFirstorDefaultAsync(p => p.Id == productDto.ProductId);
            if (product == null) throw new Exception($"Product with ID {productDto.ProductId} not found.");

            shopIds.Add(product.ShopId);

            var orderDetails = new OrderDetails
            {
                OrderHeaderId = orderHeader.Id,
                ProductId = productDto.ProductId,
                Quantity = productDto.Quantity,
                UnitPrice = productDto.UnitPrice,
                ShopId = product.ShopId
            };

            orderDetailsList.Add(orderDetails);
            await _unitOfWork.OrderDetailsRepository.AddAsync(orderDetails);
        }

        await _unitOfWork.CompleteAsync();
  
        var fullOrderDetails = await _unitOfWork.OrderDetailsRepository.GetAllAsync(d => d.OrderHeaderId == orderHeader.Id, IncludeWord: "Product");

        
        string customerEmailBody = OrderEmailService.BuildCustomerOrderEmail(orderHeader, fullOrderDetails.ToList());
        await _emailSender.SendEmailAsync(orderHeader.Email,
            $"Order Confirmation #{orderHeader.Id}",
            customerEmailBody);

        // Notify vendors (one email per shop)
        foreach (var shopId in shopIds)
        {
            var shop = await _unitOfWork.ShopRepository.GetFirstorDefaultAsync(s => s.Id == shopId);
            if (shop == null || string.IsNullOrEmpty(shop.ContactEmail))
                continue;

           
            string vendorEmailBody = OrderEmailService.BuildAdminOrderNotificationEmail(orderHeader, fullOrderDetails.Where(d => d.ShopId == shopId).ToList());

            await _emailSender.SendEmailAsync(shop.ContactEmail,
                $"New Order Received #{orderHeader.Id}",
                vendorEmailBody);
        }

   
        string adminEmailBody = OrderEmailService.BuildAdminOrderNotificationEmail(orderHeader, fullOrderDetails.ToList());
        await _emailSender.SendEmailAsync(_adminEmail,
            $"New Order Notification #{orderHeader.Id}",
            adminEmailBody);

        return orderHeader.Id;
    }
}
