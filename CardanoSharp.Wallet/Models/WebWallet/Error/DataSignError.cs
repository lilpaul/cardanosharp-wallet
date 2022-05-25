using CardanoSharp.Wallet.Enums.WebWallet;

namespace CardanoSharp.Wallet.Models.WebWallet.Error
{
    public class DataSignError
    {
        public DataSignErrorCode Code { get; set; }
        public string Info { get; set; }
    }
}
