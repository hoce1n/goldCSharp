namespace Domain.Services.Catalog
{
    public class PricingEngine
    {
        public static long CalculateFinalPrice(
            int weightInSoot,
            decimal mintingFee,
            long current18GramPrice)
        {
            decimal pricePerSootGold18 = current18GramPrice / 1000m;

            decimal goldValue = weightInSoot * pricePerSootGold18;

            return (long)Math.Round(goldValue + mintingFee);
        }

    }
}
