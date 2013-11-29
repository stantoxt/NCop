﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCop.Aspects.Engine
{
    public interface IFunctionBinding<TInstance, TArg1, TResult>
    {
        TResult Invoke(ref TInstance instance, TArg1 arg1);
    }
}
