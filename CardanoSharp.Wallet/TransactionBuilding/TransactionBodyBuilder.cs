﻿using System.Linq;
using CardanoSharp.Wallet.Enums;
using CardanoSharp.Wallet.Extensions;
using CardanoSharp.Wallet.Extensions.Models.Transactions;
using CardanoSharp.Wallet.Models.Addresses;
using CardanoSharp.Wallet.Models.Transactions;
using CardanoSharp.Wallet.Utilities;


namespace CardanoSharp.Wallet.TransactionBuilding
{
    public interface ITransactionBodyBuilder: IABuilder<TransactionBody>
    {
        ITransactionBodyBuilder AddInput(byte[] transactionId, uint transactionIndex);
        ITransactionBodyBuilder AddInput(string transactionId, uint transactionIndex);
        ITransactionBodyBuilder AddOutput(byte[] address, ulong coin, ITokenBundleBuilder tokenBundleBuilder = null, OutputPurpose outputPurpose = OutputPurpose.Spend);
        ITransactionBodyBuilder AddOutput(Address address, ulong coin, ITokenBundleBuilder tokenBundleBuilder = null, OutputPurpose outputPurpose = OutputPurpose.Spend);
        ITransactionBodyBuilder SetCertificate(ICertificateBuilder certificateBuilder);
        ITransactionBodyBuilder SetFee(ulong fee);
        ITransactionBodyBuilder SetTtl(uint ttl);
        ITransactionBodyBuilder SetMetadataHash(IAuxiliaryDataBuilder auxiliaryDataBuilder);
        ITransactionBodyBuilder SetMint(ITokenBundleBuilder token);
        ITransactionBodyBuilder RemoveFeeFromChange(ulong? fee = null);
    }

    public class TransactionBodyBuilder : ABuilder<TransactionBody>, ITransactionBodyBuilder
    {
        private TransactionBodyBuilder()
        {
            _model = new TransactionBody();
        }

        private TransactionBodyBuilder(TransactionBody model)
        {
            _model = model;
        }

        public static ITransactionBodyBuilder GetBuilder(TransactionBody model)
        {
            if (model == null)
            {
                return new TransactionBodyBuilder();
            }
            return new TransactionBodyBuilder(model);
        }

        public static ITransactionBodyBuilder Create
        {
            get => new TransactionBodyBuilder();
        }

        public ITransactionBodyBuilder SetCertificate(ICertificateBuilder certificateBuilder)
        {
            _model.Certificate = certificateBuilder.Build();
            return this;
        }

        public ITransactionBodyBuilder AddInput(TransactionInput transactionInput)
        {

            _model.TransactionInputs.Add(transactionInput);
            return this;
        }

        public ITransactionBodyBuilder AddInput(byte[] transactionId, uint transactionIndex)
        {

            _model.TransactionInputs.Add(new TransactionInput()
            {
                TransactionId = transactionId,
                TransactionIndex = transactionIndex
            });
            return this;
        }

        public ITransactionBodyBuilder AddInput(string transactionIdStr, uint transactionIndex)
        {
            byte[] transactionId = transactionIdStr.HexToByteArray();
            _model.TransactionInputs.Add(new TransactionInput()
            {
                TransactionId = transactionId,
                TransactionIndex = transactionIndex
            });
            return this;
        }

        public ITransactionBodyBuilder AddOutput(TransactionOutput transactionOutput)
        {
            _model.TransactionOutputs.Add(transactionOutput);
            return this;
        }

        public ITransactionBodyBuilder AddOutput(Address address, ulong coin, ITokenBundleBuilder tokenBundleBuilder = null, OutputPurpose outputPurpose = OutputPurpose.Spend)
        {
            return AddOutput(address.GetBytes(), coin, tokenBundleBuilder, outputPurpose);
        }

        public ITransactionBodyBuilder AddOutput(byte[] address, ulong coin, ITokenBundleBuilder tokenBundleBuilder = null,  OutputPurpose outputPurpose = OutputPurpose.Spend)
        {
            var outputValue = new TransactionOutputValue()
            {
                Coin = coin
            };

            if (tokenBundleBuilder != null)
            {
                outputValue.MultiAsset = tokenBundleBuilder.Build();
            }

            _model.TransactionOutputs.Add(new TransactionOutput()
            {
                Address = address,
                Value = outputValue,
                OutputPurpose = outputPurpose
            });
            return this;
        }

        public ITransactionBodyBuilder SetFee(ulong fee)
        {
            _model.Fee = fee;
            return this;
        }

        public ITransactionBodyBuilder SetTtl(uint ttl)
        {
            _model.Ttl = ttl;
            return this;
        }

        public ITransactionBodyBuilder SetMetadataHash(IAuxiliaryDataBuilder auxiliaryDataBuilder) {
            _model.MetadataHash = HashUtility.Blake2b256(auxiliaryDataBuilder.Build().GetCBOR().EncodeToBytes()).ToStringHex();
            return this;
        }

        public ITransactionBodyBuilder SetMint(ITokenBundleBuilder tokenBuilder)
        {
            _model.Mint = tokenBuilder.Build();
            return this;
        }

        public ITransactionBodyBuilder RemoveFeeFromChange(ulong? fee = null)
        {
            if (fee is null)
                fee = _model.Fee;
            
            //get count of change outputs to deduct fee from evenly
            //note we are selecting only ones that dont have assets
            //  this is to respect minimum ada required for token bundles
            var countOfChangeOutputs = _model.TransactionOutputs
                .Count(x => x.OutputPurpose == OutputPurpose.Change
                    && (x.Value.MultiAsset is null 
                        || (x.Value.MultiAsset is not null 
                            && !x.Value.MultiAsset.Any())));
            
            //if we dont find any, we include token bundles
            if (countOfChangeOutputs == 0)
                countOfChangeOutputs = _model.TransactionOutputs
                    .Count(x => x.OutputPurpose == OutputPurpose.Change);
            
            ulong feePerChangeOutput = fee.Value / (ulong)countOfChangeOutputs;
            ulong feeRemaining = fee.Value % (ulong)countOfChangeOutputs;
            bool needToApplyRemaining = true;
            foreach (var o in _model.TransactionOutputs.Where(x =>
                         x.OutputPurpose == OutputPurpose.Change))
            {
                if (needToApplyRemaining)
                {
                    o.Value.Coin = o.Value.Coin - feePerChangeOutput - feeRemaining;
                    needToApplyRemaining = false;
                }else 
                    o.Value.Coin = o.Value.Coin - feePerChangeOutput;
            }

            return this;
        }
    }
}
