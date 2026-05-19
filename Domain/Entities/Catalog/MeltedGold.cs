using Domain.Common;

namespace Domain.Entities.Catalog
{
    public class MeltedGold : BaseEntity
    {
        public decimal BuyFeePerGram { get; private set; }
        public decimal SellFeePerGram { get; private set; }

        public decimal MinTradeAmount { get; private set; }

        public bool IsActive { get; private set; }

        private MeltedGold() { }

        public MeltedGold(
            decimal buyFeePerGram,
            decimal sellFeePerGram,
            decimal minTradeAmount)
        {
            BuyFeePerGram = buyFeePerGram;
            SellFeePerGram = sellFeePerGram;
            MinTradeAmount = minTradeAmount;
            IsActive = true;
        }

        public void UpdateBuyFee(decimal buyFeePerGram)
        {
            BuyFeePerGram = buyFeePerGram;
            SetUpdated();
        }

        public void UpdateSellFee(decimal sellFeePerGram)
        {
            SellFeePerGram = sellFeePerGram;
            SetUpdated();
        }

        public void UpdateMinTradeAmount(decimal minTradeAmount)
        {
            MinTradeAmount = minTradeAmount;
            SetUpdated();
        }

        public void Activate()
        {
            IsActive = true;
            SetUpdated();
        }

        public void Deactivate()
        {
            IsActive = false;
            SetUpdated();
        }
    }
}
