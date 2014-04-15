﻿using NCop.Aspects.Aspects;
using NCop.Core.Extensions;
using NCop.Weaving;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using NCop.Weaving.Extensions;

namespace NCop.Aspects.Weaving
{
    internal class TopBindingOnMethodBoundaryAspectWeaver : AbstractOnMethodBoundaryAspectWeaver
    {
        protected IArgumentsWeaver argumentsWeaver = null;

        internal TopBindingOnMethodBoundaryAspectWeaver(IAspectWeaver nestedWeaver, IAspectDefinition aspectDefinition, IAspectWeavingSettings settings)
            : base(nestedWeaver, aspectDefinition, settings) {
            var @params = weavingSettings.MethodInfoImpl.GetParameters();

            argumentsWeavingSetings.Parameters = @params.ToArray(@param => @param.ParameterType);
            argumentsWeaver = new TopBindingOnMethodExecutionArgumentsWeaver(argumentsWeavingSetings, settings);
        }

        protected override void AddEntryScopeWeavers(List<IMethodScopeWeaver> entryWeavers) {
            if (byRefArgumentsStoreWeaver.ContainsByRefParams) {
                Action<ILGenerator> storeArgsAction = byRefArgumentsStoreWeaver.StoreArgsIfNeeded;
                var storeArgsArgsWeaver = storeArgsAction.ToMethodScopeWeaver();

                entryWeavers.Add(storeArgsArgsWeaver);
            }
        }

        protected override void AddFinallyScopeWeavers(List<IMethodScopeWeaver> finallyWeavers) {
            finallyWeavers.Add(new TopAspectArgsMappingWeaverImpl(aspectWeavingSettings, argumentsWeavingSetings));
        }

        public override ILGenerator Weave(ILGenerator ilGenerator) {
            argumentsWeaver.Weave(ilGenerator);

            return weaver.Weave(ilGenerator);
        }
    }
}