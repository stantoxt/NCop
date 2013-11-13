﻿using System;
using NCop.Aspects.Advices;
using NCop.Aspects.Engine;
using System.Diagnostics;
using NCop.Aspects.Aspects;
using NCop.Aspects.Framework;

namespace NCop.Aspects.Framework
{
    public abstract class OnFunctionBoundaryAspect<TInstance, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>
    {	
		[OnMethodEntryAdvice]
		public virtual void OnEntry(FunctionExecutionArgs<TInstance, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult> args) { }

		[FinallyAdvice]
		public virtual void OnExit(FunctionExecutionArgs<TInstance, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult> args) { }

		[OnMethodSuccessAdvice]
		public virtual void OnSuccess(FunctionExecutionArgs<TInstance, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult> args) { }

		[OnMethodExceptionAdvice]
		public virtual void OnException(FunctionExecutionArgs<TInstance, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult> args) { }
	}
}
