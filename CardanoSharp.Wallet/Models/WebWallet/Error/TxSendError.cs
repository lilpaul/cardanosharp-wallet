using CardanoSharp.Wallet.Enums.WebWallet;

namespace CardanoSharp.Wallet.Models.WebWallet.Error
{
    public class TxSendError
    {
        public TxSendErrorCode Code { get; set; }
        public string Info { get; set; }
    }
}
