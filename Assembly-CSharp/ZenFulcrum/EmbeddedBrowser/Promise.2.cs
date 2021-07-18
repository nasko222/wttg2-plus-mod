using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZenFulcrum.EmbeddedBrowser.Promises;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class Promise : IPromise, IPendingPromise, IPromiseInfo, IRejectable
	{
		static Promise()
		{
			Promise.UnhandledException += delegate(object sender, ExceptionEventArgs args)
			{
				Debug.LogWarning("Rejection: " + args.Exception.Message + "\n" + args.Exception.StackTrace);
			};
		}

		public Promise()
		{
			this.CurState = PromiseState.Pending;
			if (Promise.EnablePromiseTracking)
			{
				Promise.pendingPromises.Add(this);
			}
		}

		public Promise(Action<Action, Action<Exception>> resolver)
		{
			this.CurState = PromiseState.Pending;
			if (Promise.EnablePromiseTracking)
			{
				Promise.pendingPromises.Add(this);
			}
			try
			{
				resolver(delegate
				{
					this.Resolve();
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

		public static event EventHandler<ExceptionEventArgs> UnhandledException
		{
			add
			{
				Promise.unhandlerException = (EventHandler<ExceptionEventArgs>)Delegate.Combine(Promise.unhandlerException, value);
			}
			remove
			{
				Promise.unhandlerException = (EventHandler<ExceptionEventArgs>)Delegate.Remove(Promise.unhandlerException, value);
			}
		}

		public static IEnumerable<IPromiseInfo> GetPendingPromises()
		{
			return Promise.pendingPromises;
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

		private void AddResolveHandler(Action onResolved, IRejectable rejectable)
		{
			if (this.resolveHandlers == null)
			{
				this.resolveHandlers = new List<Promise.ResolveHandler>();
			}
			this.resolveHandlers.Add(new Promise.ResolveHandler
			{
				callback = onResolved,
				rejectable = rejectable
			});
		}

		private void InvokeRejectHandler(Action<Exception> callback, IRejectable rejectable, Exception value)
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

		private void InvokeResolveHandler(Action callback, IRejectable rejectable)
		{
			try
			{
				callback();
			}
			catch (Exception ex)
			{
				rejectable.Reject(ex);
			}
		}

		private void ClearHandlers()
		{
			this.rejectHandlers = null;
			this.resolveHandlers = null;
		}

		private void InvokeRejectHandlers(Exception ex)
		{
			if (this.rejectHandlers != null)
			{
				this.rejectHandlers.Each(delegate(RejectHandler handler)
				{
					this.InvokeRejectHandler(handler.callback, handler.rejectable, ex);
				});
			}
			this.ClearHandlers();
		}

		private void InvokeResolveHandlers()
		{
			if (this.resolveHandlers != null)
			{
				this.resolveHandlers.Each(delegate(Promise.ResolveHandler handler)
				{
					this.InvokeResolveHandler(handler.callback, handler.rejectable);
				});
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

		public void Resolve()
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
			this.CurState = PromiseState.Resolved;
			if (Promise.EnablePromiseTracking)
			{
				Promise.pendingPromises.Remove(this);
			}
			this.InvokeResolveHandlers();
		}

		public void Done(Action onResolved, Action<Exception> onRejected)
		{
			this.Then(onResolved, onRejected).Catch(delegate(Exception ex)
			{
				Promise.PropagateUnhandledException(this, ex);
			});
		}

		public void Done(Action onResolved)
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

		public IPromise WithName(string name)
		{
			this.Name = name;
			return this;
		}

		public IPromise Catch(Action<Exception> onRejected)
		{
			Promise resultPromise = new Promise();
			resultPromise.WithName(this.Name);
			Action resolveHandler = delegate()
			{
				resultPromise.Resolve();
			};
			Action<Exception> rejectHandler = delegate(Exception ex)
			{
				onRejected(ex);
				resultPromise.Reject(ex);
			};
			this.ActionHandlers(resultPromise, resolveHandler, rejectHandler);
			return resultPromise;
		}

		public IPromise<ConvertedT> Then<ConvertedT>(Func<IPromise<ConvertedT>> onResolved)
		{
			return this.Then<ConvertedT>(onResolved, null);
		}

		public IPromise Then(Func<IPromise> onResolved)
		{
			return this.Then(onResolved, null);
		}

		public IPromise Then(Action onResolved)
		{
			return this.Then(onResolved, null);
		}

		public IPromise<ConvertedT> Then<ConvertedT>(Func<IPromise<ConvertedT>> onResolved, Action<Exception> onRejected)
		{
			Promise<ConvertedT> resultPromise = new Promise<ConvertedT>();
			resultPromise.WithName(this.Name);
			Action resolveHandler = delegate()
			{
				onResolved().Then(delegate(ConvertedT chainedValue)
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

		public IPromise Then(Func<IPromise> onResolved, Action<Exception> onRejected)
		{
			Promise resultPromise = new Promise();
			resultPromise.WithName(this.Name);
			Action resolveHandler = delegate()
			{
				if (onResolved != null)
				{
					onResolved().Then(delegate()
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

		public IPromise Then(Action onResolved, Action<Exception> onRejected)
		{
			Promise resultPromise = new Promise();
			resultPromise.WithName(this.Name);
			Action resolveHandler = delegate()
			{
				if (onResolved != null)
				{
					onResolved();
				}
				resultPromise.Resolve();
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

		private void ActionHandlers(IRejectable resultPromise, Action resolveHandler, Action<Exception> rejectHandler)
		{
			if (this.CurState == PromiseState.Resolved)
			{
				this.InvokeResolveHandler(resolveHandler, resultPromise);
			}
			else if (this.CurState == PromiseState.Rejected)
			{
				this.InvokeRejectHandler(rejectHandler, resultPromise, this.rejectionException);
			}
			else
			{
				this.AddResolveHandler(resolveHandler, resultPromise);
				this.AddRejectHandler(rejectHandler, resultPromise);
			}
		}

		public IPromise ThenAll(Func<IEnumerable<IPromise>> chain)
		{
			return this.Then(() => Promise.All(chain()));
		}

		public IPromise<IEnumerable<ConvertedT>> ThenAll<ConvertedT>(Func<IEnumerable<IPromise<ConvertedT>>> chain)
		{
			return this.Then<IEnumerable<ConvertedT>>(() => Promise<ConvertedT>.All(chain()));
		}

		public static IPromise All(params IPromise[] promises)
		{
			return Promise.All(promises);
		}

		public static IPromise All(IEnumerable<IPromise> promises)
		{
			IPromise[] array = promises.ToArray<IPromise>();
			if (array.Length == 0)
			{
				return Promise.Resolved();
			}
			int remainingCount = array.Length;
			Promise resultPromise = new Promise();
			resultPromise.WithName("All");
			array.Each(delegate(IPromise promise, int index)
			{
				promise.Catch(delegate(Exception ex)
				{
					if (resultPromise.CurState == PromiseState.Pending)
					{
						resultPromise.Reject(ex);
					}
				}).Then(delegate()
				{
					remainingCount--;
					if (remainingCount <= 0)
					{
						resultPromise.Resolve();
					}
				}).Done();
			});
			return resultPromise;
		}

		public IPromise ThenSequence(Func<IEnumerable<Func<IPromise>>> chain)
		{
			return this.Then(() => Promise.Sequence(chain()));
		}

		public static IPromise Sequence(params Func<IPromise>[] fns)
		{
			return Promise.Sequence(fns);
		}

		public static IPromise Sequence(IEnumerable<Func<IPromise>> fns)
		{
			return fns.Aggregate(Promise.Resolved(), (IPromise prevPromise, Func<IPromise> fn) => prevPromise.Then(() => fn()));
		}

		public IPromise ThenRace(Func<IEnumerable<IPromise>> chain)
		{
			return this.Then(() => Promise.Race(chain()));
		}

		public IPromise<ConvertedT> ThenRace<ConvertedT>(Func<IEnumerable<IPromise<ConvertedT>>> chain)
		{
			return this.Then<ConvertedT>(() => Promise<ConvertedT>.Race(chain()));
		}

		public static IPromise Race(params IPromise[] promises)
		{
			return Promise.Race(promises);
		}

		public static IPromise Race(IEnumerable<IPromise> promises)
		{
			IPromise[] array = promises.ToArray<IPromise>();
			if (array.Length == 0)
			{
				throw new ApplicationException("At least 1 input promise must be provided for Race");
			}
			Promise resultPromise = new Promise();
			resultPromise.WithName("Race");
			array.Each(delegate(IPromise promise, int index)
			{
				promise.Catch(delegate(Exception ex)
				{
					if (resultPromise.CurState == PromiseState.Pending)
					{
						resultPromise.Reject(ex);
					}
				}).Then(delegate()
				{
					if (resultPromise.CurState == PromiseState.Pending)
					{
						resultPromise.Resolve();
					}
				}).Done();
			});
			return resultPromise;
		}

		public static IPromise Resolved()
		{
			Promise promise = new Promise();
			promise.Resolve();
			return promise;
		}

		public static IPromise Rejected(Exception ex)
		{
			Promise promise = new Promise();
			promise.Reject(ex);
			return promise;
		}

		internal static void PropagateUnhandledException(object sender, Exception ex)
		{
			if (Promise.unhandlerException != null)
			{
				Promise.unhandlerException(sender, new ExceptionEventArgs(ex));
			}
		}

		public IEnumerator ToWaitFor(bool abortOnFail = false)
		{
			Promise.Enumerated result = new Promise.Enumerated(this, abortOnFail);
			this.Done(delegate()
			{
			}, delegate(Exception ex)
			{
			});
			return result;
		}

		public static bool EnablePromiseTracking = false;

		private static EventHandler<ExceptionEventArgs> unhandlerException;

		internal static int nextPromiseId = 0;

		internal static HashSet<IPromiseInfo> pendingPromises = new HashSet<IPromiseInfo>();

		private Exception rejectionException;

		private List<RejectHandler> rejectHandlers;

		private List<Promise.ResolveHandler> resolveHandlers;

		public struct ResolveHandler
		{
			public Action callback;

			public IRejectable rejectable;
		}

		private class Enumerated : IEnumerator
		{
			public Enumerated(Promise promise, bool abortOnFail)
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

			private Promise promise;

			private bool abortOnFail;
		}
	}
}
