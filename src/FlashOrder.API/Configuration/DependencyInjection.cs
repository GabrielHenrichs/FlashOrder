using FlashOrder.Application.Abstractions;
using FlashOrder.Application.UseCases.Portfolio;
using FlashOrder.Application.UseCases.Stocks;
using FlashOrder.Application.UseCases.Trades;
using FlashOrder.Infra.Data.Repositories;

namespace FlashOrder.API.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationUseCases(this IServiceCollection services)
    {
        services.AddScoped<ICreateStockUseCase, CreateStockUseCase>();
        services.AddScoped<IGetStocksUseCase, GetStocksUseCase>();
        services.AddScoped<IBuyStockUseCase, BuyStockUseCase>();
        services.AddScoped<ISellStockUseCase, SellStockUseCase>();
        services.AddScoped<IGetPositionsUseCase, GetPositionsUseCase>();
        services.AddScoped<IGetTransactionsUseCase, GetTransactionsUseCase>();

        return services;
    }

    public static IServiceCollection AddDataRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IStockRepository, StockRepository>();
        services.AddSingleton<IPositionRepository, PositionRepository>();
        services.AddSingleton<ITransactionRepository, TransactionRepository>();

        return services;
    }
}
