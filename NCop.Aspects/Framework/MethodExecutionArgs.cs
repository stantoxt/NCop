﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCop.Aspects.Framework
{
    public class MethodExecutionArgs<TInstance> : AdviceArgs<TInstance>
    {
        public FlowBehavior FlowBehavior { get; }
    }
}
