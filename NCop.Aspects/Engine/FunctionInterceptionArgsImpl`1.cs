﻿using NCop.Aspects.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCop.Aspects.Engine
{
    public class FunctionInterceptionArgsImpl<TInstance, TArg1, TResult> : FunctionExecutionArgs<TInstance, TArg1, TResult>, IInterceptable
	{
        private readonly IFunctionBinding<TArg1, TResult> funcBinding = null;

        public FunctionInterceptionArgsImpl(object instance, IFunctionBinding<TArg1, TResult> funcBinding, TArg1 arg1) {
            Arg1 = arg1;
            Instance = instance;
            this.funcBinding = funcBinding;
        }

        public void Proceed() {
            var instance = Instance;

            funcBinding.Invoke(ref instance, Arg1);
        }
    }
}