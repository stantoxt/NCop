﻿using NCop.Aspects.Extensions;
using NCop.Core.Extensions;
using NCop.Weaving.Extensions;
using System.Reflection;
using System.Reflection.Emit;

namespace NCop.Aspects.Weaving
{
    internal class MethodDecoratorArgumentsWeaver : IArgumentsWeaver
    {
        private readonly MethodInfo method = null;
        private readonly IByRefArgumentsStoreWeaver byRefArgumentsStoreWeaver = null;

        internal MethodDecoratorArgumentsWeaver(MethodInfo method, IByRefArgumentsStoreWeaver byRefArgumentsStoreWeaver) {
            this.method = method;
            this.byRefArgumentsStoreWeaver = byRefArgumentsStoreWeaver;
        }

        public void Weave(ILGenerator ilGenerator) {
            var methodImplParameters = method.GetParameters();
            var aspectArgsType = method.ToAspectArgumentContract();

            ilGenerator.EmitLoadArg(1);
            ilGenerator.Emit(OpCodes.Ldind_Ref);

            methodImplParameters.ForEach(param => {
                var argPosition = param.Position + 1;

                if (byRefArgumentsStoreWeaver.Contains(argPosition)) {
                    byRefArgumentsStoreWeaver.EmitLoadLocalAddress(ilGenerator, argPosition);
                }
                else {
                    var property = aspectArgsType.GetProperty("Arg{0}".Fmt(argPosition));

                    ilGenerator.EmitLoadArg(2);
                    ilGenerator.Emit(OpCodes.Callvirt, property.GetGetMethod());
                }
            });
        }
    }
}
