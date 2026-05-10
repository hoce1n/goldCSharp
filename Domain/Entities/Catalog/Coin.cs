using Domain.Common;
using Domain.Enums.Karat;
using Domain.Events.Coin;
using Domain.Exceptions.Coin;
using Domain.Excetions.Coin;

namespace Domain.Entities.Catalog
{
    public class Coin : BaseEntity
    {
        private const decimal SootToGramRatio = 0.001m;

        public string Name { get; private set; }
        public int WeightInSoot {  get; private set; }
        public decimal WeightInGrams => WeightInSoot * SootToGramRatio;
        public KaratType Karat {  get; private set; }
        public decimal Purity => Karat.Purity();
        public decimal MintingFee { get; private set; }
        public int Stock { get; private set; }
        public string? ImageUrl { get; private set; }
        public string? Description { get; private set; }
        public bool IsActive { get; private set; }

        private Coin() { }

        public static Coin Create(
            string name,
            int weightInSoot,
            KaratType karat,
            decimal mintingFee,
            int initialStock,
            string? imageUrl = null,
            string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidCoinNameException();

            if (weightInSoot <= 0)
                throw new InvalidWeightException();

            if (mintingFee < 0)
                throw new InvalidMintingFeeException();

            if (initialStock < 0)
                throw new InvalidStockException();

            var coin = new Coin
            {
                Name = name,
                WeightInSoot = weightInSoot,
                Karat = karat,
                MintingFee = mintingFee,
                Stock = initialStock,
                ImageUrl = imageUrl,
                Description = description,
                IsActive = true
            };

            coin.AddDomainEvent(new CoinCreatedEvent(coin.Id, coin.Name));

            return coin;
        }
        public void UpdateDetails(
            string? name = null,
            int? weightInSoot = null,
            KaratType? karat = null,
            string? imageUrl = null,
            string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidCoinNameException();

            Name = name.Trim();

            if (weightInSoot.HasValue)
            {
                if (weightInSoot.Value <= 0)
                    throw new InvalidWeightException();
                WeightInSoot = weightInSoot.Value;
            }

            if (karat.HasValue)
            {
                Karat = karat.Value;
            }

            if (imageUrl != null)
                ImageUrl = imageUrl;

            if (description != null)
                Description = description;

            AddDomainEvent(new CoinUpdatedEvent(Id));
            SetUpdated();
        }

        public void UpdateStock(int stock)
        {
            if (stock < 0)
                throw new InvalidStockException();

            Stock = stock;
        }

        public void IncreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new InvalidStockException();

            var oldStock = Stock;
            Stock += quantity;
            SetUpdated();

            AddDomainEvent(new CoinStockChangedEvent(Id, oldStock, Stock));
        }

        public void DecreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new InvalidStockException();

            if (Stock < quantity)
                throw new InsufficientStockException(Stock, quantity);

            var oldStock = Stock;
            Stock -= quantity;
            SetUpdated();

            AddDomainEvent(new CoinStockChangedEvent(Id, oldStock, Stock));
        }

        public void SetMintingFee(decimal newFee)
        {
            if (newFee < 0)
                throw new InvalidMintingFeeException();

            var oldFee = MintingFee;
            MintingFee = newFee;
            SetUpdated();

            AddDomainEvent(new MintingFeeUpdatedEvent(Id, oldFee, newFee));
        }

        public void Activate()
        {
            if (IsActive)
                return;

            IsActive = true;
            SetUpdated();

            AddDomainEvent(new CoinActivatedEvent(Id));
        }

        public void Deactivate()
        {
            if (!IsActive)
                return;

            IsActive = false;
            SetUpdated();

            AddDomainEvent(new CoinDeactivatedEvent(Id));
        }

        public bool HasSufficientStock(int requestedQuantity) => Stock >= requestedQuantity;

        public decimal PureGoldWeight => WeightInGrams * Purity;

    }
}
