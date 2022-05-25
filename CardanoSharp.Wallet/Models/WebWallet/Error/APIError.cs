using CardanoSharp.Wallet.Enums.WebWallet;

namespace CardanoSharp.Wallet.Models.WebWallet.Error
{
    public class APIError
    {
        public APIErrorCode Code { get; set; }
        public string Info { get; set; }
    }
}
