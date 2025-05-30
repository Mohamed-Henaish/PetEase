using PetEase.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetEase.Services.Service
{
    public static class OrderEmailService
    {
        public static string BuildCustomerOrderEmail(OrderHeader order, List<OrderDetails> details)
        {
            var productsHtml = new StringBuilder();
            foreach (var item in details)
            {
                productsHtml.Append($@"
            <tr style='border-bottom:1px solid #ddd;'>
                <td style='padding:8px;'>{item.Product?.Name ?? "Product"}</td>
                <td style='padding:8px; text-align:center;'>{item.Quantity}</td>
                <td style='padding:8px; text-align:right;'>{item.UnitPrice:C}</td>
                <td style='padding:8px; text-align:right;'>{item.TotalUnitPrice:C}</td>
            </tr>");
            }

            return $@"
    <html>
    <body style='font-family: Arial, sans-serif; font-size:14px; color:#333;'>
        <h2>Thank you for your order #{order.Id}</h2>
        <p>Order Date: {order.OrderDate:yyyy-MM-dd HH:mm}</p>
        <h3>Shipping & Contact Information</h3>
        <p>
            Name: {order.Name}<br/>
            Email: {order.Email}<br/>
            Phone: {order.PhoneNumber}<br/>
            Additional Phone: {order.AdditionalPhoneNumber ?? "-"}<br/>
            Address: {order.Address}, {order.City}, {order.Government}, Postal Code: {order.PostalCode ?? 0}<br/>
            Carrier: {order.CarrierName}<br/>
            Notes: {order.Notes ?? "-"}
        </p>
        <h3>Order Details</h3>
        <table style='width:100%; border-collapse: collapse;'>
            <thead>
                <tr style='background:#f4f4f4;'>
                    <th style='padding:8px; text-align:left;'>Product</th>
                    <th style='padding:8px; text-align:center;'>Quantity</th>
                    <th style='padding:8px; text-align:right;'>Unit Price</th>
                    <th style='padding:8px; text-align:right;'>Total</th>
                </tr>
            </thead>
            <tbody>
                {productsHtml}
            </tbody>
        </table>
        <p style='text-align:right; font-weight:bold;'>
            Subtotal: {order.SubTotal:C}<br/>
            Shipping Cost: {order.ShippingCost:C}<br/>
            <span style='font-size:18px;'>Total Price: {order.TotalPrice:C}</span>
        </p>
        <p>We will notify you once your order is shipped. Thank you for shopping with us!</p>
    </body>
    </html>
    ";
        }

        public static string BuildAdminOrderNotificationEmail(OrderHeader order, List<OrderDetails> details)
        {
            var productsHtml = new StringBuilder();
            foreach (var item in details)
            {
                productsHtml.Append($@"
            <tr style='border-bottom:1px solid #ddd;'>
                <td style='padding:8px;'>{item.Product?.Name ?? "Product"}</td>
                <td style='padding:8px; text-align:center;'>{item.Quantity}</td>
                <td style='padding:8px; text-align:right;'>{item.UnitPrice:C}</td>
                <td style='padding:8px; text-align:right;'>{item.TotalUnitPrice:C}</td>
                <td style='padding:8px; text-align:center;'>{item.ShopId}</td>
            </tr>");
            }

            return $@"
    <html>
    <body style='font-family: Arial, sans-serif; font-size:14px; color:#333;'>
        <h2>New Order Received - #{order.Id}</h2>
        <p>Order Date: {order.OrderDate:yyyy-MM-dd HH:mm}</p>
        <h3>Customer Info</h3>
        <p>
            Name: {order.Name}<br/>
            Email: {order.Email}<br/>
            Phone: {order.PhoneNumber}<br/>
            Address: {order.Address}, {order.City}, {order.Government}, Postal Code: {order.PostalCode ?? 0}<br/>
            Carrier: {order.CarrierName}<br/>
            Notes: {order.Notes ?? "-"}
        </p>
        <h3>Order Details</h3>
        <table style='width:100%; border-collapse: collapse;'>
            <thead>
                <tr style='background:#f4f4f4;'>
                    <th style='padding:8px;'>Product</th>
                    <th style='padding:8px; text-align:center;'>Quantity</th>
                    <th style='padding:8px; text-align:right;'>Unit Price</th>
                    <th style='padding:8px; text-align:right;'>Total</th>
                    <th style='padding:8px; text-align:center;'>Shop Id</th>
                </tr>
            </thead>
            <tbody>
                {productsHtml}
            </tbody>
        </table>
        <p style='font-weight:bold;'>
            Subtotal: {order.SubTotal:C}<br/>
            Shipping Cost: {order.ShippingCost:C}<br/>
            Total Price: {order.TotalPrice:C}
        </p>
        <p>Please process this order as soon as possible.</p>
    </body>
    </html>
    ";
        }

    }
}
