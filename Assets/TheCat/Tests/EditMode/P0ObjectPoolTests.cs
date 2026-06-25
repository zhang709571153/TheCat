using NUnit.Framework;
using TheCat.Core;

namespace TheCat.Tests
{
    public sealed class P0ObjectPoolTests
    {
        [Test]
        public void Rent_CreatesInstanceAndTracksActiveCount()
        {
            int created = 0;
            P0ObjectPool<PooledToken> pool = new P0ObjectPool<PooledToken>(() => new PooledToken(++created));

            PooledToken token = pool.Rent();

            Assert.AreEqual(1, token.Id);
            Assert.AreEqual(1, created);
            Assert.AreEqual(1, pool.ActiveCount);
            Assert.AreEqual(0, pool.RetainedCount);
        }

        [Test]
        public void Release_RetainsAndReusesInstance()
        {
            int created = 0;
            int rentCallbacks = 0;
            int releaseCallbacks = 0;
            P0ObjectPool<PooledToken> pool = new P0ObjectPool<PooledToken>(
                () => new PooledToken(++created),
                onRent: token => token.RentCount = ++rentCallbacks,
                onRelease: token => token.ReleaseCount = ++releaseCallbacks);

            PooledToken first = pool.Rent();
            bool retained = pool.Release(first);
            PooledToken second = pool.Rent();

            Assert.IsTrue(retained);
            Assert.AreSame(first, second);
            Assert.AreEqual(1, created);
            Assert.AreEqual(1, pool.ActiveCount);
            Assert.AreEqual(0, pool.RetainedCount);
            Assert.AreEqual(2, second.RentCount);
            Assert.AreEqual(1, second.ReleaseCount);
        }

        [Test]
        public void Release_RespectsMaxRetained()
        {
            P0ObjectPool<PooledToken> pool = new P0ObjectPool<PooledToken>(
                () => new PooledToken(1),
                maxRetained: 0);
            PooledToken token = pool.Rent();

            bool retained = pool.Release(token);

            Assert.IsFalse(retained);
            Assert.AreEqual(0, pool.ActiveCount);
            Assert.AreEqual(0, pool.RetainedCount);
        }

        [Test]
        public void Clear_DisposesRetainedInstances()
        {
            int disposed = 0;
            P0ObjectPool<PooledToken> pool = new P0ObjectPool<PooledToken>(() => new PooledToken(1));
            pool.Release(pool.Rent());

            pool.Clear(token => disposed++);

            Assert.AreEqual(1, disposed);
            Assert.AreEqual(0, pool.ActiveCount);
            Assert.AreEqual(0, pool.RetainedCount);
        }

        [Test]
        public void Clear_DoesNotResetActiveCountForRentedInstances()
        {
            P0ObjectPool<PooledToken> pool = new P0ObjectPool<PooledToken>(() => new PooledToken(1));
            pool.Rent();

            pool.Clear();

            Assert.AreEqual(1, pool.ActiveCount);
            Assert.AreEqual(0, pool.RetainedCount);
        }

        [Test]
        public void Release_WhenNoActiveInstancesDoesNotRetain()
        {
            P0ObjectPool<PooledToken> pool = new P0ObjectPool<PooledToken>(() => new PooledToken(1));

            bool retained = pool.Release(new PooledToken(99));

            Assert.IsFalse(retained);
            Assert.AreEqual(0, pool.ActiveCount);
            Assert.AreEqual(0, pool.RetainedCount);
        }

        [Test]
        public void Constructor_RejectsInvalidCallbacksAndLimits()
        {
            Assert.Throws<System.ArgumentNullException>(() => new P0ObjectPool<PooledToken>(null));
            Assert.Throws<System.ArgumentOutOfRangeException>(() => new P0ObjectPool<PooledToken>(() => new PooledToken(1), maxRetained: -1));
            Assert.Throws<System.InvalidOperationException>(() => new P0ObjectPool<PooledToken>(() => null).Rent());
        }

        private sealed class PooledToken
        {
            public PooledToken(int id)
            {
                Id = id;
            }

            public int Id { get; }

            public int RentCount { get; set; }

            public int ReleaseCount { get; set; }
        }
    }
}
