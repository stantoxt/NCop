﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace NCop.Core.Weaving
{
    public interface ITypeDefinitionWeaver
    {
        ITypeDefinition Weave();
    }
}