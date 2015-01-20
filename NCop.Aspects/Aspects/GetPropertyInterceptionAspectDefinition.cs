﻿using System;
using System.Reflection;
using NCop.Aspects.Advices;
using NCop.Aspects.Engine;
using NCop.Aspects.Framework;
using NCop.Aspects.Weaving.Expressions;
using NCop.Core.Extensions;

namespace NCop.Aspects.Aspects
{
    internal class GetPropertyInterceptionAspectDefinition : AbstractMethodAspectDefinition
    {
        private readonly GetPropertyInterceptionAspectAttribute aspect = null;

        public GetPropertyInterceptionAspectDefinition(GetPropertyInterceptionAspectAttribute aspect, Type aspectDeclaringType, MethodInfo method, PropertyInfo property)
            : base(aspect, aspectDeclaringType, method) {
            this.aspect = aspect;
        }

        public override AspectType AspectType {
            get {
                return AspectType.GetPropertyInterceptionAspect;
            }
        }

        public override IAspectDefinition BuildAdvices() {
            Aspect.AspectType
                 .GetOverridenMethods()
                 .ForEach(method => {
                     TryBulidAdvice<OnGetPropertyAdviceAttribute>(method, (advice, mi) => {
                         return new OnGetPropertyAdviceDefinition(advice, mi);
                     });
                 });

            return this;
        }

        public override IAspectExpressionBuilder Accept(IAspectDefinitionVisitor visitor) {
            return visitor.Visit(aspect).Invoke(this);
        }
    }
}
