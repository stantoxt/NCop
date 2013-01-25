﻿using NCop.Aspects.Advices;
using NCop.Aspects.Aspects.Interception;
using NCop.Aspects.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCop.Aspects.Aspects
{
    public interface IOnMethodBoundryContract : IAspect
    {
        [OnInvokeAdvice]
        void OnInvoke(IMethodExecution methodExecution);

        [OnFinallyAdvice]
        void Finally(IMethodExecution methodExecution);

        [OnSuccessAdvice]
        void OnSuccess(IMethodExecution methodExecution);

        [OnErrorAdvice]
        void OnError(IMethodExecution methodExecution);
    }
}
