﻿using NCop.Aspects.Advices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCop.Aspects.Framework
{
    public abstract class ActionInterceptionAspect<TInstance, TArg1>
    {
        [OnMethodInvokeAdvice]
        public virtual void OnInvoke(ActionInterceptionArgs<TInstance, TArg1> args) { }
    }
}
