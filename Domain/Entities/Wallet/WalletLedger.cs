using Domain.Common;
using Domain.Enums.Wallet;

namespace Domain.Entities.Wallet
{
    public class WalletLedger : BaseEntity
    {
        public Guid WalletId { get; private set; }
        public Wallet Wallet { get; private set; } = null!;
        public decimal Amount { get; private set; }
        public WalletTransactionType Type { get; private set; }
        public string Description { get; private set; }
        
        private WalletLedger() { }

        private WalletLedger(
            Guid walletId, 
            Wallet wallet,
            decimal amount, 
            WalletTransactionType type, 
            string description)
        {
            WalletId = walletId;
            Amount = amount;
            Type = type;
            Description = description;
        }

        public static WalletLedger Create(
            Wallet wallet,
            decimal amount,
            WalletTransactionType type,
            string description)
        {
            return new WalletLedger
            (
                wallet.Id,
                wallet,
                amount,
                type,
                description
            );
        }

    }
}
