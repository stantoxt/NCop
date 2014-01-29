﻿using NCop.Aspects.Framework;
using NCop.Core.Extensions;
using NCop.Weaving;
using NCop.Weaving.Extensions;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace NCop.Aspects.Weaving
{
    internal class BeginCatchBlockMethodScopeWeaver : IMethodScopeWeaver
    {
        private readonly FieldInfo aspectMember = null;
        private readonly Type aspectArgumentType = null;
        private readonly ILocalBuilderRepository localBuilderRepository = null;

        internal BeginCatchBlockMethodScopeWeaver(TryCatchFinallySettings tryCatchFinallySettings) {
            aspectMember = tryCatchFinallySettings.AspectMember;
            aspectArgumentType = tryCatchFinallySettings.AspectArgumentType;
            localBuilderRepository = tryCatchFinallySettings.LocalBuilderRepository;
        }

        public ILGenerator Weave(ILGenerator ilGenerator) {
            var typeofException = typeof(Exception);
            LocalBuilder localExceptionBuilder = null;
            var argsImplLocalBuilder = localBuilderRepository.Get(aspectArgumentType);
            var onExceptionMethodInfo = aspectMember.FieldType.GetMethod("OnException");
            var setExceptionMethodInfo = aspectArgumentType.GetProperty("Exception").GetSetMethod();
            var flowBehaviorContinueLabel = ilGenerator.DefineLabel();

            localExceptionBuilder = localBuilderRepository.GetOrDeclare(typeofException, () => {
                return ilGenerator.DeclareLocal(typeofException);
            });

            ilGenerator.BeginCatchBlock(typeofException);
            ilGenerator.EmitStoreLocal(localExceptionBuilder);
            ilGenerator.EmitLoadLocal(argsImplLocalBuilder);
            ilGenerator.EmitLoadLocal(localExceptionBuilder);
            ilGenerator.Emit(OpCodes.Callvirt, setExceptionMethodInfo);
            ilGenerator.Emit(OpCodes.Ldsfld, aspectMember);
            ilGenerator.EmitLoadLocal(argsImplLocalBuilder);
            ilGenerator.Emit(OpCodes.Callvirt, onExceptionMethodInfo);
            //ilGenerator.EmitPushInteger((int)FlowBehavior.ThrowException);
            //ilGenerator.Emit(OpCodes.Bne_Un_S, flowBehaviorContinueLabel);
            //ilGenerator.EmitLoadLocal(localExceptionBuilder);
            //ilGenerator.Emit(OpCodes.Rethrow);
            //ilGenerator.MarkLabel(flowBehaviorContinueLabel);

            return ilGenerator;
        }
    }
}