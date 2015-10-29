﻿using NCop.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace NCop.Aspects.Engine
{
    public abstract class AbstractActionEventBroker<TInstance, TArg1, TArg2, TArg3, TArg4> : IEventBroker<Action<TArg1, TArg2, TArg3, TArg4>>
    {
        protected readonly EventInfo @event = null;
        protected TInstance instance = default(TInstance);
        private readonly LinkedList<Action<TArg1, TArg2, TArg3, TArg4>> linkedHandlers = null;
        private readonly Action<IEventActionArgs<TArg1, TArg2, TArg3, TArg4>> argsHandler = null;

        protected AbstractActionEventBroker(TInstance instance, EventInfo @event, Action<IEventActionArgs<TArg1, TArg2, TArg3, TArg4>> argsHandler) {
            this.@event = @event;
            this.instance = instance;
            this.argsHandler = argsHandler;
            linkedHandlers = new LinkedList<Action<TArg1, TArg2, TArg3, TArg4>>();
        }

        public void AddHandler(Action<TArg1, TArg2, TArg3, TArg4> handler) {
            var isFirst = linkedHandlers.First.IsNull();

            if (isFirst) {
                SubscribeImpl();
            }

            linkedHandlers.AddLast(handler);
        }

        protected void OnEventFired(TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4) {
            var args = new EventActionInterceptionArgsImpl<TInstance, TArg1, TArg2, TArg3, TArg4>();

            args.Arg1 = arg1;
            args.Arg2 = arg2;
            args.Arg3 = arg3;
            args.Arg4 = arg4;
            args.Event = @event;
            args.EventBroker = this;

            for (var i = linkedHandlers.First; i != null; i = i.Next) {
                args.Handler = i.Value;
                argsHandler(args);
            }
        }

        public void RemoveHandler(Action<TArg1, TArg2, TArg3, TArg4> handler) {
            linkedHandlers.Remove(handler);

            if (linkedHandlers.First.IsNull()) {
                UnsubscribeImpl();
            }
        }

        protected abstract void SubscribeImpl();

        protected abstract void UnsubscribeImpl();
    }
}
