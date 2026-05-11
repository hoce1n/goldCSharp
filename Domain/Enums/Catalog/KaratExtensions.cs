using Domain.Enums.Karat;

public static class KaratExtensions
{
    public static decimal Purity(this KaratType karat)
    {
        return karat switch
        {
            KaratType.K18 => 0.750m,
            KaratType.K21 => 0.875m,
            KaratType.K22 => 0.916m,
            KaratType.K24 => 0.999m,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
