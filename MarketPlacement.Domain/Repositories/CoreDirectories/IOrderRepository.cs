namespace MarketPlacement.Domain.Repositories.CoreDirectories;

using Application.Result;
using Application.Result.Generic;
using DTOs;
using Entities.CoreEntities;

public interface IOrderRepository
{
    public Task<Result<Pager<Order>>> GetPageOfOrders(int count, int page);

    public Task<Result<Order>> GetOrderById(int orderId);

    public Task<Result<Order>> CreateOrder(Order order);

    public Task<Result<Order>> EditOrder(Order order);

    public Task<Result> DeleteOrder(int orderId);
}