using System;
using System.Collections.Generic;

namespace TheCat.Core
{
    public sealed class EventBus
    {
        private readonly Dictionary<Type, List<Delegate>> handlersByType = new Dictionary<Type, List<Delegate>>();

        public IDisposable Subscribe<TEvent>(Action<TEvent> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            Type eventType = typeof(TEvent);
            if (!handlersByType.TryGetValue(eventType, out List<Delegate> handlers))
            {
                handlers = new List<Delegate>();
                handlersByType.Add(eventType, handlers);
            }

            handlers.Add(handler);
            return new Subscription(() => Unsubscribe(handler));
        }

        public void Publish<TEvent>(TEvent eventData)
        {
            Type eventType = typeof(TEvent);
            if (!handlersByType.TryGetValue(eventType, out List<Delegate> handlers))
            {
                return;
            }

            Delegate[] snapshot = handlers.ToArray();
            for (int i = 0; i < snapshot.Length; i++)
            {
                ((Action<TEvent>)snapshot[i]).Invoke(eventData);
            }
        }

        public int CountSubscribers<TEvent>()
        {
            return handlersByType.TryGetValue(typeof(TEvent), out List<Delegate> handlers) ? handlers.Count : 0;
        }

        private void Unsubscribe<TEvent>(Action<TEvent> handler)
        {
            Type eventType = typeof(TEvent);
            if (!handlersByType.TryGetValue(eventType, out List<Delegate> handlers))
            {
                return;
            }

            handlers.Remove(handler);
            if (handlers.Count == 0)
            {
                handlersByType.Remove(eventType);
            }
        }

        private sealed class Subscription : IDisposable
        {
            private Action disposeAction;

            public Subscription(Action disposeAction)
            {
                this.disposeAction = disposeAction;
            }

            public void Dispose()
            {
                Action action = disposeAction;
                disposeAction = null;
                action?.Invoke();
            }
        }
    }
}
