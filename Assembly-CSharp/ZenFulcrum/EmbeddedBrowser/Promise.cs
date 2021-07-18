using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ZenFulcrum.EmbeddedBrowser.Promises;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class Promise<PromisedT> : IPromise<PromisedT>, IPendingPromise<PromisedT>, IPromiseInfo, IRejectable
	{
		public Promise()
		{
			this.CurState = PromiseState.Pending;
			this.Id = ++Promise.nextPromiseId;
			if (Promise.EnablePromiseTracking)
			{
				Promise.pendingPromises.Add(this);
			}
		}

		public Promise(Action<Action<PromisedT>, Action<Exception>> resolver)
		{
			this.CurState = PromiseState.Pending;
			this.Id = ++Promise.nextPromiseId;
			if (Promise.EnablePromiseTracking)
			{
				Promise.pendingPromises.Add(this);
			}
			try
			{
				resolver(delegate(PromisedT value)
				{
					this.Resolve(value);
				}, delegate(Exception ex)
				{
					this.Reject(ex);
				});
			}
			catch (Exception ex)
			{
				Exception ex2;
				this.Reject(ex2);
			}
		}

		public int Id { get; private set; }

		public string Name { get; private set; }

		public PromiseState CurState { get; private set; }

		private void AddRejectHandler(Action<Exception> onRejected, IRejectable rejectable)
		{
			if (this.rejectHandlers == null)
			{
				this.rejectHandlers = new List<RejectHandler>();
			}
			this.rejectHandlers.Add(new RejectHandler
			{
				callback = onRejected,
				rejectable = rejectable
			});
		}

		private void AddResolveHandler(Action<PromisedT> onResolved, IRejectable rejectable)
		{
			if (this.resolveCallbacks == null)
			{
				this.resolveCallbacks = new List<Action<PromisedT>>();
			}
			if (this.resolveRejectables == null)
			{
				this.resolveRejectables = new List<IRejectable>();
			}
			this.resolveCallbacks.Add(onResolved);
			this.resolveRejectables.Add(rejectable);
		}

		private void InvokeHandler<T>(Action<T> callback, IRejectable rejectable, T value)
		{
			try
			{
				callback(value);
			}
			catch (Exception ex)
			{
				rejectable.Reject(ex);
			}
		}

		private void ClearHandlers()
		{
			this.rejectHandlers = null;
			this.resolveCallbacks = null;
			this.resolveRejectables = null;
		}

		private void InvokeRejectHandlers(Exception ex)
		{
			if (this.rejectHandlers != null)
			{
				this.rejectHandlers.Each(delegate(RejectHandler handler)
				{
					this.InvokeHandler<Exception>(handler.callback, handler.rejectable, ex);
				});
			}
			this.ClearHandlers();
		}

		private void InvokeResolveHandlers(PromisedT value)
		{
			if (this.resolveCallbacks != null)
			{
				int i = 0;
				int count = this.resolveCallbacks.Count;
				while (i < count)
				{
					this.InvokeHandler<PromisedT>(this.resolveCallbacks[i], this.resolveRejectables[i], value);
					i++;
				}
			}
			this.ClearHandlers();
		}

		public void Reject(Exception ex)
		{
			if (this.CurState != PromiseState.Pending)
			{
				throw new ApplicationException(string.Concat(new object[]
				{
					"Attempt to reject a promise that is already in state: ",
					this.CurState,
					", a promise can only be rejected when it is still in state: ",
					PromiseState.Pending
				}));
			}
			this.rejectionException = ex;
			this.CurState = PromiseState.Rejected;
			if (Promise.EnablePromiseTracking)
			{
				Promise.pendingPromises.Remove(this);
			}
			this.InvokeRejectHandlers(ex);
		}

		public void Resolve(PromisedT value)
		{
			if (this.CurState != PromiseState.Pending)
			{
				throw new ApplicationException(string.Concat(new object[]
				{
					"Attempt to resolve a promise that is already in state: ",
					this.CurState,
					", a promise can only be resolved when it is still in state: ",
					PromiseState.Pending
				}));
			}
			this.resolveValue = value;
			this.CurState = PromiseState.Resolved;
			if (Promise.EnablePromiseTracking)
			{
				Promise.pendingPromises.Remove(this);
			}
			this.InvokeResolveHandlers(value);
		}

		public void Done(Action<PromisedT> onResolved, Action<Exception> onRejected)
		{
			this.Then(onResolved, onRejected).Catch(delegate(Exception ex)
			{
				Promise.PropagateUnhandledException(this, ex);
			});
		}

		public void Done(Action<PromisedT> onResolved)
		{
			this.Then(onResolved).Catch(delegate(Exception ex)
			{
				Promise.PropagateUnhandledException(this, ex);
			});
		}

		public void Done()
		{
			this.Catch(delegate(Exception ex)
			{
				Promise.PropagateUnhandledException(this, ex);
			});
		}

		public IPromise<PromisedT> WithName(string name)
		{
			this.Name = name;
			return this;
		}

		public IPromise<PromisedT> Catch(Action<Exception> onRejected)
		{
			Promise<PromisedT> resultPromise = new Promise<PromisedT>();
			resultPromise.WithName(this.Name);
			Action<PromisedT> resolveHandler = delegate(PromisedT v)
			{
				resultPromise.Resolve(v);
			};
			Action<Exception> rejectHandler = delegate(Exception ex)
			{
				onRejected(ex);
				resultPromise.Reject(ex);
			};
			this.ActionHandlers(resultPromise, resolveHandler, rejectHandler);
			return resultPromise;
		}

		public IPromise<ConvertedT> Then<ConvertedT>(Func<PromisedT, IPromise<ConvertedT>> onResolved)
		{
			return this.Then<ConvertedT>(onResolved, null);
		}

		public IPromise Then(Func<PromisedT, IPromise> onResolved)
		{
			return this.Then(onResolved, null);
		}

		public IPromise<PromisedT> Then(Action<PromisedT> onResolved)
		{
			return this.Then(onResolved, null);
		}

		public IPromise<ConvertedT> Then<ConvertedT>(Func<PromisedT, IPromise<ConvertedT>> onResolved, Action<Exception> onRejected)
		{
			Promise<ConvertedT> resultPromise = new Promise<ConvertedT>();
			resultPromise.WithName(this.Name);
			Action<PromisedT> resolveHandler = delegate(PromisedT v)
			{
				onResolved(v).Then(delegate(ConvertedT chainedValue)
				{
					resultPromise.Resolve(chainedValue);
				}, delegate(Exception ex)
				{
					resultPromise.Reject(ex);
				});
			};
			Action<Exception> rejectHandler = delegate(Exception ex)
			{
				if (onRejected != null)
				{
					onRejected(ex);
				}
				resultPromise.Reject(ex);
			};
			this.ActionHandlers(resultPromise, resolveHandler, rejectHandler);
			return resultPromise;
		}

		public IPromise Then(Func<PromisedT, IPromise> onResolved, Action<Exception> onRejected)
		{
			Promise resultPromise = new Promise();
			resultPromise.WithName(this.Name);
			Action<PromisedT> resolveHandler = delegate(PromisedT v)
			{
				if (onResolved != null)
				{
					onResolved(v).Then(delegate()
					{
						resultPromise.Resolve();
					}, delegate(Exception ex)
					{
						resultPromise.Reject(ex);
					});
				}
				else
				{
					resultPromise.Resolve();
				}
			};
			Action<Exception> rejectHandler = delegate(Exception ex)
			{
				if (onRejected != null)
				{
					onRejected(ex);
				}
				resultPromise.Reject(ex);
			};
			this.ActionHandlers(resultPromise, resolveHandler, rejectHandler);
			return resultPromise;
		}

		public IPromise<PromisedT> Then(Action<PromisedT> onResolved, Action<Exception> onRejected)
		{
			Promise<PromisedT> resultPromise = new Promise<PromisedT>();
			resultPromise.WithName(this.Name);
			Action<PromisedT> resolveHandler = delegate(PromisedT v)
			{
				if (onResolved != null)
				{
					onResolved(v);
				}
				resultPromise.Resolve(v);
			};
			Action<Exception> rejectHandler = delegate(Exception ex)
			{
				if (onRejected != null)
				{
					onRejected(ex);
				}
				resultPromise.Reject(ex);
			};
			this.ActionHandlers(resultPromise, resolveHandler, rejectHandler);
			return resultPromise;
		}

		public IPromise<ConvertedT> Then<ConvertedT>(Func<PromisedT, ConvertedT> transform)
		{
			return this.Then<ConvertedT>((PromisedT value) => Promise<ConvertedT>.Resolved(transform(value)));
		}

		[Obsolete("Use Then instead")]
		public IPromise<ConvertedT> Transform<ConvertedT>(Func<PromisedT, ConvertedT> transform)
		{
			return this.Then<ConvertedT>((PromisedT value) => Promise<ConvertedT>.Resolved(transform(value)));
		}

		private void ActionHandlers(IRejectable resultPromise, Action<PromisedT> resolveHandler, Action<Exception> rejectHandler)
		{
			if (this.CurState == PromiseState.Resolved)
			{
				this.InvokeHandler<PromisedT>(resolveHandler, resultPromise, this.resolveValue);
			}
			else if (this.CurState == PromiseState.Rejected)
			{
				this.InvokeHandler<Exception>(rejectHandler, resultPromise, this.rejectionException);
			}
			else
			{
				this.AddResolveHandler(resolveHandler, resultPromise);
				this.AddRejectHandler(rejectHandler, resultPromise);
			}
		}

		public IPromise<IEnumerable<ConvertedT>> ThenAll<ConvertedT>(Func<PromisedT, IEnumerable<IPromise<ConvertedT>>> chain)
		{
			return this.Then<IEnumerable<ConvertedT>>((PromisedT value) => Promise<ConvertedT>.All(chain(value)));
		}

		public IPromise ThenAll(Func<PromisedT, IEnumerable<IPromise>> chain)
		{
			return this.Then((PromisedT value) => Promise.All(chain(value)));
		}

		public static IPromise<IEnumerable<PromisedT>> All(params IPromise<PromisedT>[] promises)
		{
			return Promise<PromisedT>.All(promises);
		}

		public static IPromise<IEnumerable<PromisedT>> All(IEnumerable<IPromise<PromisedT>> promises)
		{
			IPromise<PromisedT>[] array = promises.ToArray<IPromise<PromisedT>>();
			if (array.Length == 0)
			{
				return Promise<IEnumerable<PromisedT>>.Resolved(EnumerableExt.Empty<PromisedT>());
			}
			int remainingCount = array.Length;
			PromisedT[] results = new PromisedT[remainingCount];
			Promise<IEnumerable<PromisedT>> resultPromise = new Promise<IEnumerable<PromisedT>>();
			resultPromise.WithName("All");
			array.Each(delegate(IPromise<PromisedT> promise, int index)
			{
				promise.Catch(delegate(Exception ex)
				{
					if (resultPromise.CurState == PromiseState.Pending)
					{
						resultPromise.Reject(ex);
					}
				}).Then(delegate(PromisedT result)
				{
					results[index] = result;
					remainingCount--;
					if (remainingCount <= 0)
					{
						resultPromise.Resolve(results);
					}
				}).Done();
			});
			return resultPromise;
		}

		public IPromise<ConvertedT> ThenRace<ConvertedT>(Func<PromisedT, IEnumerable<IPromise<ConvertedT>>> chain)
		{
			return this.Then<ConvertedT>((PromisedT value) => Promise<ConvertedT>.Race(chain(value)));
		}

		public IPromise ThenRace(Func<PromisedT, IEnumerable<IPromise>> chain)
		{
			return this.Then((PromisedT value) => Promise.Race(chain(value)));
		}

		public PromisedT Value
		{
			get
			{
				if (this.CurState == PromiseState.Pending)
				{
					throw new InvalidOperationException("Promise not settled");
				}
				if (this.CurState == PromiseState.Rejected)
				{
					throw this.rejectionException;
				}
				return this.resolveValue;
			}
		}

		public IEnumerator ToWaitFor(bool abortOnFail)
		{
			Promise<PromisedT>.Enumerated<PromisedT> result = new Promise<PromisedT>.Enumerated<PromisedT>(this, abortOnFail);
			this.Done(delegate(PromisedT x)
			{
			}, delegate(Exception ex)
			{
			});
			return result;
		}

		public static IPromise<PromisedT> Race(params IPromise<PromisedT>[] promises)
		{
			return Promise<PromisedT>.Race(promises);
		}

		public static IPromise<PromisedT> Race(IEnumerable<IPromise<PromisedT>> promises)
		{
			IPromise<PromisedT>[] array = promises.ToArray<IPromise<PromisedT>>();
			if (array.Length == 0)
			{
				throw new ApplicationException("At least 1 input promise must be provided for Race");
			}
			Promise<PromisedT> resultPromise = new Promise<PromisedT>();
			resultPromise.WithName("Race");
			array.Each(delegate(IPromise<PromisedT> promise, int index)
			{
				promise.Catch(delegate(Exception ex)
				{
					if (resultPromise.CurState == PromiseState.Pending)
					{
						resultPromise.Reject(ex);
					}
				}).Then(delegate(PromisedT result)
				{
					if (resultPromise.CurState == PromiseState.Pending)
					{
						resultPromise.Resolve(result);
					}
				}).Done();
			});
			return resultPromise;
		}

		public static IPromise<PromisedT> Resolved(PromisedT promisedValue)
		{
			Promise<PromisedT> promise = new Promise<PromisedT>();
			promise.Resolve(promisedValue);
			return promise;
		}

		public static IPromise<PromisedT> Rejected(Exception ex)
		{
			Promise<PromisedT> promise = new Promise<PromisedT>();
			promise.Reject(ex);
			return promise;
		}

		private Exception rejectionException;

		private PromisedT resolveValue;

		private List<RejectHandler> rejectHandlers;

		private List<Action<PromisedT>> resolveCallbacks;

		private List<IRejectable> resolveRejectables;

		private class Enumerated<T> : IEnumerator
		{
			public Enumerated(Promise<T> promise, bool abortOnFail)
			{
				this.promise = promise;
				this.abortOnFail = abortOnFail;
			}

			public bool MoveNext()
			{
				if (this.abortOnFail && this.promise.CurState == PromiseState.Rejected)
				{
					throw this.promise.rejectionException;
				}
				return this.promise.CurState == PromiseState.Pending;
			}

			public void Reset()
			{
			}

			public object Current
			{
				get
				{
					return null;
				}
			}

			private Promise<T> promise;

			private bool abortOnFail;
		}
	}
}
