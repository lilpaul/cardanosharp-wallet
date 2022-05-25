using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardanoSharp.Wallet.Models.WebWallet
{
    public class Paginate
    {
        public uint Page { get; set; }
        public uint Limit { get; set; }
    }
}
