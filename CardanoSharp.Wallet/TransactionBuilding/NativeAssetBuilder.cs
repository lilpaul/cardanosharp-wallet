﻿using CardanoSharp.Wallet.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardanoSharp.Wallet.TransactionBuilding
{
    public interface INativeAssetBuilder: IABuilder<NativeAsset>
    {
        INativeAssetBuilder WithToken(Dictionary<byte[], ulong> token);
    }

    public class NativeAssetBuilder: ABuilder<NativeAsset>, INativeAssetBuilder
    {
        public NativeAssetBuilder()
        {
            _model = new NativeAsset();
        }

        private NativeAssetBuilder(NativeAsset model)
        {
            _model = model;
        }

        public static INativeAssetBuilder GetBuilder(NativeAsset model)
        {
            return new NativeAssetBuilder(model);
        }

        public INativeAssetBuilder WithToken(Dictionary<byte[], ulong> token)
        {
            _model.Token = token;
            return this;
        }
    }
}
