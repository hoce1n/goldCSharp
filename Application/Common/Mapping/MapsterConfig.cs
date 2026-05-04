using Application.Features.Catalog.Coins.Commands.CreateCoin;
using Application.Features.Catalog.Coins.Queries.GetAllCoins;
using Domain.Entities.Catalog;
using Domain.Entities.Identity;
using Mapster;

namespace Application.Common.Mapping
{
    public static class MapsterConfig
    {
        public static void Configure()
        {

            TypeAdapterConfig<Coin, CreateCoinResponse>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.WeightInSoot, src => src.WeightInSoot)
            .Map(dest => dest.Karat, src => src.Karat)
            .Map(dest => dest.MintingFee, src => src.MintingFee)
            .Map(dest => dest.Stock, src => src.Stock)
            .Map(dest => dest.IsActive, src => src.IsActive);

            TypeAdapterConfig<Coin, CoinDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.WeightInSoot, src => src.WeightInSoot)
                .Map(dest => dest.Karat, src => src.Karat)
                .Map(dest => dest.MintingFee, src => src.MintingFee)
                .Map(dest => dest.Stock, src => src.Stock)
                .Map(dest => dest.ImageUrl, src => src.ImageUrl)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsActive, src => src.IsActive)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt);
        }
    }
}
