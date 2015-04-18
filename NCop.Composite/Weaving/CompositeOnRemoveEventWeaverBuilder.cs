﻿using NCop.Aspects.Aspects;
using NCop.Aspects.Weaving;
using NCop.Composite.Engine;
using NCop.Weaving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NCop.Composite.Weaving
{
    internal class CompositeOnRemoveEventWeaverBuilder : AbstractCompositeEventWeaverBuilder
    {
        internal CompositeOnRemoveEventWeaverBuilder(ICompositeEventMap compositeEventMap, ITypeDefinition typeDefinition, IAspectWeavingServices aspectWeavingServices)
            : base(compositeEventMap, typeDefinition, aspectWeavingServices) {
        }

        public override IMethodWeaver Build() {
            var weavingSettings = new WeavingSettingsImpl(contractType, typeDefinition);
            var removeMethod = compositeEventMap.ContractMember.RemoveMethod;

            if (compositeEventMap.HasAspectDefinitions) {
                var aspectWeavingSettings = new AspectWeavingSettingsImpl {
                    WeavingSettings = weavingSettings,
                    AspectRepository = aspectWeavingServices.AspectRepository,
                    AspectArgsMapper = aspectWeavingServices.AspectArgsMapper
                };

                return new CompositeOnRemoveEventWeaver(removeMethod, typeDefinition, compositeEventMap.AspectDefinitions, aspectWeavingSettings);
            }

            return null;// new GetPropertyDecoratorWeaver(compositePropertyMap.ContractMember.GetGetMethod(), weavingSettings);
        }
    }
}
