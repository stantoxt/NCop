﻿using NCop.Aspects.Weaving;
using NCop.Aspects.Weaving.Expressions;
using System.Reflection;

namespace NCop.Aspects.Advices
{
	internal class OnMethodInvokeAdviceDefinition : AbstractAdviceDefinition
	{
		private readonly OnMethodInvokeAdviceAttribute advice = null;

		internal OnMethodInvokeAdviceDefinition(OnMethodInvokeAdviceAttribute advice, MethodInfo adviceMethod)
			: base(advice, adviceMethod) {
			this.advice = advice;
		}

		public override IAdviceExpression Accept(AdviceVisitor visitor) {
			return visitor.Visit(advice).Invoke(this);
		}

		public override void Accept(AdviceDiscoveryVisitor visitor) {
			visitor.Visit(advice);
		}
	}
}