﻿using System;
using System.Collections.Concurrent;
using NUnit.Framework;

namespace DAX.Cson.Tests
{
    public abstract class FixtureBase
    {
        readonly ConcurrentStack<IDisposable> _disposables = new ConcurrentStack<IDisposable>();

        protected void CleanUpDisposables()
        {
            IDisposable disposable;
            while (_disposables.TryPop(out disposable))
            {
                disposable.Dispose();
            }
        }

        protected TDisposable Using<TDisposable>(TDisposable disposable) where TDisposable : IDisposable
        {
            _disposables.Push(disposable);
            return disposable;
        }

        [SetUp]
        public void InnerSetUp()
        {
            SetUp();
        }

        protected virtual void SetUp()
        {
        }

        [TearDown]
        public void InnerTearDown()
        {
            CleanUpDisposables();
        }
    }
}
