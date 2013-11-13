﻿using NCop.Aspects.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCop.Aspects.Engine
{
    public class ActionInterceptionArgsImpl<TInstance, TArg1> : ActionExecutionArgs<TInstance, TArg1>, IInterceptable
    {
        private readonly IActionBinding<TInstance, TArg1> actionBinding = null;

        public ActionInterceptionArgsImpl(TInstance instance, IActionBinding<TInstance, TArg1> actionBinding, TArg1 arg1) {
            Arg1 = arg1;
            Instance = instance;
            this.actionBinding = actionBinding;
        }

        public override void Proceed() {
            var instance = Instance;

            actionBinding.Invoke(ref instance, Arg1);
        }
    }
}
