﻿using NCop.Aspects.Advices;
using NCop.Aspects.Aspects;
using NCop.Aspects.Weaving.Expressions;
using NCop.Composite.Weaving;
using NCop.Weaving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using NCop.Aspects.Framework;
using NCop.Aspects.Engine;
using NCop.Core.Extensions;
using System.Threading;
using NCop.Weaving.Extensions;

namespace NCop.Aspects.Weaving
{
	internal class MethodDecoratorArgumentsWeaver : AbstractArgumentsWeaver
	{
		internal MethodDecoratorArgumentsWeaver(Type argumentType, Type[] parameters, IWeavingSettings weavingSettings)
			: base(argumentType, parameters, weavingSettings, null) {
		}

		public override void Weave(ILGenerator ilGenerator) {
			Type[] argumentTypes = ArgumentType.GetGenericArguments();
			IEnumerable<Type> @params = argumentTypes.Skip(1);

			if (IsFunction) {
				var last = argumentTypes[argumentTypes.Length - 1];

				@params = @params.TakeWhile(arg => arg != last);
			}

			ilGenerator.EmitLoadArg(1);
			ilGenerator.Emit(OpCodes.Ldind_Ref);
			ilGenerator.EmitLoadArg(2);

			@params.ForEach(1, (parameter, i) => {
				var property = ArgumentType.GetProperty("Arg{0}".Fmt(i));

				ilGenerator.Emit(OpCodes.Callvirt, property.GetGetMethod());
			});
		}
	}
}