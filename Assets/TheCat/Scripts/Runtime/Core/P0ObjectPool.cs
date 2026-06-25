using System;
using System.Collections.Generic;

namespace TheCat.Core
{
    public sealed class P0ObjectPool<T> where T : class
    {
        private readonly Stack<T> available = new Stack<T>();
        private readonly Func<T> create;
        private readonly Action<T> onRent;
        private readonly Action<T> onRelease;
        private readonly int maxRetained;

        public P0ObjectPool(Func<T> create, Action<T> onRent = null, Action<T> onRelease = null, int maxRetained = 64)
        {
            this.create = create ?? throw new ArgumentNullException(nameof(create));
            if (maxRetained < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRetained), maxRetained, "Max retained count must not be negative.");
            }

            this.onRent = onRent;
            this.onRelease = onRelease;
            this.maxRetained = maxRetained;
        }

        public int RetainedCount => available.Count;

        public int ActiveCount { get; private set; }

        public T Rent()
        {
            T instance = available.Count > 0 ? available.Pop() : create();
            if (instance == null)
            {
                throw new InvalidOperationException("P0 object pool create callback returned null.");
            }

            ActiveCount++;
            onRent?.Invoke(instance);
            return instance;
        }

        public bool Release(T instance)
        {
            if (instance == null)
            {
                return false;
            }

            if (ActiveCount <= 0)
            {
                return false;
            }

            ActiveCount--;
            onRelease?.Invoke(instance);
            if (available.Count >= maxRetained)
            {
                return false;
            }

            available.Push(instance);
            return true;
        }

        public void Clear(Action<T> dispose = null)
        {
            while (available.Count > 0)
            {
                T instance = available.Pop();
                dispose?.Invoke(instance);
            }
        }
    }
}
