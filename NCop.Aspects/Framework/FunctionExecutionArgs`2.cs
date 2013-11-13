﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCop.Aspects.Framework
{
	public abstract class FunctionExecutionArgs<TInstance, TArg1, TArg2, TResult> : FunctionExecutionArgs<TInstance, TArg1, TResult>
	{
        public TArg2 Arg2 { get; protected set; }
	}
}
