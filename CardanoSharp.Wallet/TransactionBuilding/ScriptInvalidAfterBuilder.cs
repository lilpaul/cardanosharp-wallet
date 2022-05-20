﻿using CardanoSharp.Wallet.Models.Transactions.TransactionWitness.Scripts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardanoSharp.Wallet.TransactionBuilding
{
    public interface IScriptInvalidAfterBuilder: IABuilder<ScriptInvalidAfter>
    {
        IScriptInvalidAfterBuilder WithAfter(uint after);
    }

    public class ScriptInvalidAfterBuilder: ABuilder<ScriptInvalidAfter>, IScriptInvalidAfterBuilder
    {
        public ScriptInvalidAfterBuilder()
        {
            _model = new ScriptInvalidAfter();
        }

        private ScriptInvalidAfterBuilder(ScriptInvalidAfter model)
        {
            _model = model;
        }

        public static IScriptInvalidAfterBuilder GetBuilder(ScriptInvalidAfter model)
        {
            return new ScriptInvalidAfterBuilder(model);
        }

        public IScriptInvalidAfterBuilder WithAfter(uint after)
        {
            _model.After = after;
            return this;
        }
    }
}
