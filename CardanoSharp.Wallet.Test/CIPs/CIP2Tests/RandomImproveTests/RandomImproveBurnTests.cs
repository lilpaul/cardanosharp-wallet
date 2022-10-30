using System.Collections.Generic;
using System.Linq;
using CardanoSharp.Wallet.CIPs.CIP2;
using CardanoSharp.Wallet.CIPs.CIP2.ChangeCreationStrategies;
using CardanoSharp.Wallet.Extensions;
using CardanoSharp.Wallet.Models;
using CardanoSharp.Wallet.Models.Transactions;
using Xunit;

namespace CardanoSharp.Wallet.Test.CIPs;

public partial class CIP2Tests
{
    [Fact]
    public void RandomImprove_SingleOutput_Burn_Test()
    {
        var coinSelection = new CoinSelectionService(new RandomImproveStrategy(), new SingleTokenBundleStrategy());
        var outputs = new List<TransactionOutput>() { output_10_ada_1_burned_assets };
        var utxos = new List<Utxo>()
        {
            utxo_10_ada_1_owned_mint_asset,
            utxo_40_ada_no_assets
        };

        //act
        var response = coinSelection.GetCoinSelection(outputs, utxos, address);

        //assert
        long totalSelected = 0;
        response.SelectedUtxos.ForEach(s => totalSelected = totalSelected + (long)s.Balance.Lovelaces);
        long totalOutput = 0;
        outputs.ForEach(o => totalOutput = totalOutput + (long)o.Value.Coin);
        long totalChange = 0;
        response.ChangeOutputs.ForEach(s => totalChange = totalChange + (long)s.Value.Coin);

        var selectedUTXOsSumAsset1 = response.SelectedUtxos.Where(x => x.Balance.Assets is not null).Sum(x => 
            x.Balance.Assets.Where(y => 
                y.PolicyId.Equals(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().PolicyId))
                    ?.Sum(z => (long)z.Quantity) ?? 0);

        var outputsSumAsset1 = outputs.Sum(x =>
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        var changeOutputSumAsset1 = response.ChangeOutputs.Sum(x => 
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        Assert.Equal(totalSelected, totalOutput + totalChange);
        Assert.Equal(selectedUTXOsSumAsset1 + outputsSumAsset1 + changeOutputSumAsset1, 0);
    }

    [Fact]
    public void RandomImprove_MultiOutput_Burn_Test()
    {
        var coinSelection = new CoinSelectionService(new RandomImproveStrategy(), new SingleTokenBundleStrategy());
        var outputs = new List<TransactionOutput>() { output_1_ada_no_assets, output_10_ada_1_burned_assets, output_1_ada_no_assets, output_1_ada_no_assets };
        var utxos = new List<Utxo>()
        {
            utxo_30_ada_no_assets,
            utxo_10_ada_1_owned_mint_asset,
            utxo_40_ada_no_assets
        };

        //act
        var response = coinSelection.GetCoinSelection(outputs, utxos, address);

        //assert
        long totalSelected = 0;
        response.SelectedUtxos.ForEach(s => totalSelected = totalSelected + (long)s.Balance.Lovelaces);
        long totalOutput = 0;
        outputs.ForEach(o => totalOutput = totalOutput + (long)o.Value.Coin);
        long totalChange = 0;
        response.ChangeOutputs.ForEach(s => totalChange = totalChange + (long)s.Value.Coin);

        var selectedUTXOsSumAsset1 = response.SelectedUtxos.Where(x => x.Balance.Assets is not null).Sum(x => 
            x.Balance.Assets.Where(y => 
                y.PolicyId.Equals(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().PolicyId))
                    ?.Sum(z => (long)z.Quantity) ?? 0);

        var outputsSumAsset1 = outputs.Sum(x =>
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        var changeOutputSumAsset1 = response.ChangeOutputs.Sum(x => 
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        Assert.Equal(totalSelected, totalOutput + totalChange);
        Assert.Equal(selectedUTXOsSumAsset1 + outputsSumAsset1 + changeOutputSumAsset1, 0);
    }

    [Fact]
    public void RandomImprove_SingleOutput_MultiBurn_Test()
    {
        var coinSelection = new CoinSelectionService(new RandomImproveStrategy(), new SingleTokenBundleStrategy());
        var outputs = new List<TransactionOutput>() { output_10_ada_2_burned_assets };
        var utxos = new List<Utxo>()
        {
            utxo_10_ada_1_owned_mint_asset,
            utxo_10_ada_1_owned_mint_asset_two
        };

        //act
        var response = coinSelection.GetCoinSelection(outputs, utxos, address);

        //assert
        long totalSelected = 0;
        response.SelectedUtxos.ForEach(s => totalSelected = totalSelected + (long)s.Balance.Lovelaces);
        long totalOutput = 0;
        outputs.ForEach(o => totalOutput = totalOutput + (long)o.Value.Coin);
        long totalChange = 0;
        response.ChangeOutputs.ForEach(s => totalChange = totalChange + (long)s.Value.Coin);

        var selectedUTXOsSumAsset1 = response.SelectedUtxos.Where(x => x.Balance.Assets is not null).Sum(x => 
            x.Balance.Assets.Where(y => 
                y.PolicyId.Equals(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().PolicyId))
                    ?.Sum(z => (long)z.Quantity) ?? 0);

        var outputsSumAsset1 = outputs.Sum(x =>
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        var changeOutputSumAsset1 = response.ChangeOutputs.Sum(x => 
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        var selectedUTXOsSumAsset2 = response.SelectedUtxos.Where(x => x.Balance.Assets is not null).Sum(x => 
            x.Balance.Assets.Where(y => 
                y.PolicyId.Equals(utxo_10_ada_50_tokens.Balance.Assets.FirstOrDefault().PolicyId))
                    ?.Sum(z => (long)z.Quantity) ?? 0);

        var outputsSumAsset2 = outputs.Sum(x =>
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_50_tokens.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_50_tokens.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        var changeOutputSumAsset2 = response.ChangeOutputs.Sum(x => 
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_50_tokens.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_50_tokens.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        Assert.Equal(totalSelected, totalOutput + totalChange);
        Assert.Equal(selectedUTXOsSumAsset1 + outputsSumAsset1 + changeOutputSumAsset1, 0);
        Assert.Equal(selectedUTXOsSumAsset2 + outputsSumAsset2 + changeOutputSumAsset2, 0);
    }

    [Fact]
    public void RandomImprove_MultiOutput_MultiBurn_Test()
    {
        var coinSelection = new CoinSelectionService(new RandomImproveStrategy(), new SingleTokenBundleStrategy());
        var outputs = new List<TransactionOutput>() { output_10_ada_2_burned_assets, output_10_ada_2_burned_assets, output_10_ada_1_burned_assets };
        var utxos = new List<Utxo>()
        {
            utxo_10_ada_1_owned_mint_asset,
            utxo_10_ada_1_owned_mint_asset_two,
            utxo_10_ada_1_owned_mint_asset,
            utxo_10_ada_1_owned_mint_asset_two,
            utxo_10_ada_1_owned_mint_asset,
        };

        //act
        var response = coinSelection.GetCoinSelection(outputs, utxos, address);

        //assert
        long totalSelected = 0;
        response.SelectedUtxos.ForEach(s => totalSelected = totalSelected + (long)s.Balance.Lovelaces);
        long totalOutput = 0;
        outputs.ForEach(o => totalOutput = totalOutput + (long)o.Value.Coin);
        long totalChange = 0;
        response.ChangeOutputs.ForEach(s => totalChange = totalChange + (long)s.Value.Coin);

        var selectedUTXOsSumAsset1 = response.SelectedUtxos.Where(x => x.Balance.Assets is not null).Sum(x => 
            x.Balance.Assets.Where(y => 
                y.PolicyId.Equals(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().PolicyId))
                    ?.Sum(z => (long)z.Quantity) ?? 0);

        var outputsSumAsset1 = outputs.Sum(x =>
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        var changeOutputSumAsset1 = response.ChangeOutputs.Sum(x => 
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        var selectedUTXOsSumAsset2 = response.SelectedUtxos.Where(x => x.Balance.Assets is not null).Sum(x => 
            x.Balance.Assets.Where(y => 
                y.PolicyId.Equals(utxo_10_ada_50_tokens.Balance.Assets.FirstOrDefault().PolicyId))
                    ?.Sum(z => (long)z.Quantity) ?? 0);

        var outputsSumAsset2 = outputs.Sum(x =>
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_50_tokens.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_50_tokens.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        var changeOutputSumAsset2 = response.ChangeOutputs.Sum(x => 
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_50_tokens.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_50_tokens.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        Assert.Equal(totalSelected, totalOutput + totalChange);
        Assert.Equal(selectedUTXOsSumAsset1 + outputsSumAsset1 + changeOutputSumAsset1, 0);
        Assert.Equal(selectedUTXOsSumAsset2 + outputsSumAsset2 + changeOutputSumAsset2, 0);
    }

    [Fact]
    public void RandomImprove_MultiUtxo_MultiOutput_MultiBurn_Test()
    {
        var coinSelection = new CoinSelectionService(new RandomImproveStrategy(), new SingleTokenBundleStrategy());
        var outputs = new List<TransactionOutput>() { output_10_ada_2_burned_assets, output_10_ada_50_tokens, output_100_ada_no_assets, output_10_ada_1_already_minted_assets, output_10_ada_2_burned_assets, output_10_ada_1_burned_assets };
        var utxos = new List<Utxo>()
        {
            utxo_50_ada_no_assets,
            utxo_10_ada_1_owned_mint_asset_two,
            utxo_10_ada_1_owned_mint_asset,
            utxo_70_ada_no_assets,
            utxo_10_ada_1_owned_mint_asset_two,
            utxo_10_ada_40_tokens,
            utxo_10_ada_20_tokens,
            utxo_10_ada_1_owned_mint_asset,
            utxo_10_ada_1_owned_mint_asset,
            utxo_60_ada_no_assets,
            utxo_10_ada_10_tokens,
            utxo_10_ada_1_owned_mint_asset,
        };

        //act
        var response = coinSelection.GetCoinSelection(outputs, utxos, address);

        //assert
        long totalSelected = 0;
        response.SelectedUtxos.ForEach(s => totalSelected = totalSelected + (long)s.Balance.Lovelaces);
        long totalOutput = 0;
        outputs.ForEach(o => totalOutput = totalOutput + (long)o.Value.Coin);
        long totalChange = 0;
        response.ChangeOutputs.ForEach(s => totalChange = totalChange + (long)s.Value.Coin);

        var selectedUTXOsSumAsset1 = response.SelectedUtxos.Where(x => x.Balance.Assets is not null).Sum(x => 
            x.Balance.Assets.Where(y => 
                y.PolicyId.Equals(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().PolicyId))
                    ?.Sum(z => (long)z.Quantity) ?? 0);

        var outputsSumAsset1 = outputs.Sum(x =>
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        var changeOutputSumAsset1 = response.ChangeOutputs.Sum(x => 
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        var selectedUTXOsSumAsset2 = response.SelectedUtxos.Where(x => x.Balance.Assets is not null).Sum(x => 
            x.Balance.Assets.Where(y => 
                y.PolicyId.Equals(utxo_10_ada_1_owned_mint_asset_two.Balance.Assets.FirstOrDefault().PolicyId))
                    ?.Sum(z => (long)z.Quantity) ?? 0);

        var outputsSumAsset2 = outputs.Sum(x =>
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset_two.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset_two.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        var changeOutputSumAsset2 = response.ChangeOutputs.Sum(x => 
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset_two.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset_two.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        var selectedUTXOsSumAsset3 = response.SelectedUtxos.Where(x => x.Balance.Assets is not null).Sum(x => 
            x.Balance.Assets.Where(y => 
                y.PolicyId.Equals(utxo_10_ada_40_tokens.Balance.Assets.FirstOrDefault().PolicyId))
                    ?.Sum(z => (long)z.Quantity) ?? 0);

        var outputsSumAsset3 = outputs.Sum(x =>
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_40_tokens.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_40_tokens.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        var changeOutputSumAsset3 = response.ChangeOutputs.Sum(x => 
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_40_tokens.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_40_tokens.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);


        Assert.Equal(totalSelected, totalOutput + totalChange);
        Assert.Equal(selectedUTXOsSumAsset1 + outputsSumAsset1 + changeOutputSumAsset1, 2);
        Assert.Equal(selectedUTXOsSumAsset2 + outputsSumAsset2 + changeOutputSumAsset2, 0);
        Assert.Equal(selectedUTXOsSumAsset3, outputsSumAsset3 + changeOutputSumAsset3);
    }

    [Fact]
    public void RandomImprove_MultiUtxo_MultiOutput_MultiMint_MultiBurn_Test()
    {
        var coinSelection = new CoinSelectionService(new RandomImproveStrategy(), new SingleTokenBundleStrategy());
        var outputs = new List<TransactionOutput>() { output_10_ada_2_burned_assets, output_10_ada_50_tokens, output_100_ada_no_assets, output_10_ada_1_already_minted_assets, output_10_ada_100_minted_assets, output_10_ada_1_minted_assets, output_10_ada_1_minted_assets, output_10_ada_2_burned_assets, output_10_ada_1_burned_assets };
        var utxos = new List<Utxo>()
        {
            utxo_50_ada_no_assets,
            utxo_10_ada_1_owned_mint_asset_two,
            utxo_10_ada_1_owned_mint_asset,
            utxo_70_ada_no_assets,
            utxo_10_ada_1_owned_mint_asset_two,
            utxo_10_ada_40_tokens,
            utxo_10_ada_20_tokens,
            utxo_10_ada_1_owned_mint_asset,
            utxo_10_ada_1_owned_mint_asset,
            utxo_60_ada_no_assets,
            utxo_10_ada_10_tokens,
            utxo_10_ada_1_owned_mint_asset,
            utxo_10_ada_30_tokens,
            utxo_10_ada_40_tokens,
            utxo_80_ada_no_assets,
        };

        //act
        var response = coinSelection.GetCoinSelection(outputs, utxos, address);

        //assert
        long totalSelected = 0;
        response.SelectedUtxos.ForEach(s => totalSelected = totalSelected + (long)s.Balance.Lovelaces);
        long totalOutput = 0;
        outputs.ForEach(o => totalOutput = totalOutput + (long)o.Value.Coin);
        long totalChange = 0;
        response.ChangeOutputs.ForEach(s => totalChange = totalChange + (long)s.Value.Coin);

        var selectedUTXOsSumAsset1 = response.SelectedUtxos.Where(x => x.Balance.Assets is not null).Sum(x => 
            x.Balance.Assets.Where(y => 
                y.PolicyId.Equals(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().PolicyId))
                    ?.Sum(z => (long)z.Quantity) ?? 0);

        var outputsSumAsset1 = outputs.Sum(x =>
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        var changeOutputSumAsset1 = response.ChangeOutputs.Sum(x => 
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        var selectedUTXOsSumAsset2 = response.SelectedUtxos.Where(x => x.Balance.Assets is not null).Sum(x => 
            x.Balance.Assets.Where(y => 
                y.PolicyId.Equals(utxo_10_ada_1_owned_mint_asset_two.Balance.Assets.FirstOrDefault().PolicyId))
                    ?.Sum(z => (long)z.Quantity) ?? 0);

        var outputsSumAsset2 = outputs.Sum(x =>
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset_two.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset_two.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        var changeOutputSumAsset2 = response.ChangeOutputs.Sum(x => 
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset_two.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_1_owned_mint_asset_two.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        var selectedUTXOsSumAsset3 = response.SelectedUtxos.Where(x => x.Balance.Assets is not null).Sum(x => 
            x.Balance.Assets.Where(y => 
                y.PolicyId.Equals(utxo_10_ada_40_tokens.Balance.Assets.FirstOrDefault().PolicyId))
                    ?.Sum(z => (long)z.Quantity) ?? 0);

        var outputsSumAsset3 = outputs.Sum(x =>
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_40_tokens.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_40_tokens.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);

        var changeOutputSumAsset3 = response.ChangeOutputs.Sum(x => 
            x.Value.MultiAsset?.Where(y => y.Key.ToStringHex().SequenceEqual(utxo_10_ada_40_tokens.Balance.Assets.FirstOrDefault().PolicyId)).Sum(y => 
                y.Value.Token.Where(z => z.Key.ToStringHex().SequenceEqual(utxo_10_ada_40_tokens.Balance.Assets.FirstOrDefault().Name)).Sum(z => (long)z.Value)) ?? 0);


        Assert.Equal(totalSelected, totalOutput + totalChange);
        Assert.Equal(selectedUTXOsSumAsset1 + outputsSumAsset1 + changeOutputSumAsset1, 4);
        Assert.Equal(selectedUTXOsSumAsset2 + outputsSumAsset2 + changeOutputSumAsset2, 0);
        Assert.Equal(selectedUTXOsSumAsset3, outputsSumAsset3 + changeOutputSumAsset3);
    }
}