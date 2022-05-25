using CardanoSharp.Wallet.Enums.WebWallet;

namespace CardanoSharp.Wallet.Models.WebWallet.Error
{
    public class TxSignError
    {
        public TxSignErrorCode Code { get; set; }
        public string Info { get; set; }
    }
}
