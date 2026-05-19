using Domain.Common;
using Domain.Exceptions.Wallet;

namespace Domain.Entities.Wallet
{
    public class Wallet : BaseEntity
    {
        public Guid UserId { get; private set; }
        public decimal Balance { get; private set; }
        public byte[] RowVersion { get; private set; } = null!;
        public ICollection<WalletLedger> Ledgers { get; private set; } = new List<WalletLedger>();

        private Wallet() { }

        private Wallet(Guid userId)
        {
            UserId = userId;
            Balance = 0;
            RowVersion = Array.Empty<byte>();
        }

        public static Wallet Create(Guid userId) 
        {
            return new Wallet(userId);
        }

        public WalletLedger Deposit(decimal amount, string description)
        {
            if (amount <= 0)
                throw new InvalidWalletAmountException();

            var ledger = WalletLedger.Create(
                this,
                amount,
                Enums.Wallet.WalletTransactionType.Deposit,
                description);

            Balance += amount;
            Ledgers.Add(ledger);
            return ledger;
        }

        public WalletLedger Debit(decimal amount, string description)
        {
            if (amount <= 0)
                throw new InvalidWalletAmountException();

            if (Balance < amount)
                throw new InsufficientBalanceException();


            var ledger = WalletLedger.Create(
                this,
                -amount,
                Enums.Wallet.WalletTransactionType.OrderPayment,
                description);

            Balance -= amount;

            Ledgers.Add(ledger);

            return ledger;
        }
    }
}
