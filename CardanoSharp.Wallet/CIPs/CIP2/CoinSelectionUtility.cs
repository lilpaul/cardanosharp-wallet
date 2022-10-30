﻿using System.Collections.Generic;
using System.Linq;
using CardanoSharp.Wallet.CIPs.CIP2.ChangeCreationStrategies;
using CardanoSharp.Wallet.CIPs.CIP2.Models;
using CardanoSharp.Wallet.Models;
using CardanoSharp.Wallet.Models.Transactions;
using CardanoSharp.Wallet.TransactionBuilding;

namespace CardanoSharp.Wallet.CIPs.CIP2
{
    public static class CoinSelectionUtility
    {
        public static CoinSelection UseLargestFirst(TransactionBodyBuilder tbb, List<Utxo> utxos, string changeAddress, int limit = 20, ulong fee = 0)
        {
            var cs = new CoinSelectionService(new LargestFirstStrategy(), new MultiTokenBundleStrategy());
            var tb = tbb.Build();
            return cs.GetCoinSelection(tb.TransactionOutputs.ToList(), utxos, changeAddress, limit, fee);
        }

        public static CoinSelection UseRandomImprove(TransactionBodyBuilder tbb, List<Utxo> utxos, string changeAddress, int limit = 20, ulong fee = 0)
        {
            var cs = new CoinSelectionService(new RandomImproveStrategy(), new MultiTokenBundleStrategy());
            var tb = tbb.Build();
            return cs.GetCoinSelection(tb.TransactionOutputs.ToList(), utxos, changeAddress, limit, fee);
        }
    }
}