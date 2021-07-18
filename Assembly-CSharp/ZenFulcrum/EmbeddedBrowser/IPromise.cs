using System;
using System.Collections;
using System.Collections.Generic;

namespace ZenFulcrum.EmbeddedBrowser
{
	public interface IPromise<PromisedT>
	{
		IPromise<PromisedT> WithName(string name);

		void Done(Action<PromisedT> onResolved, Action<Exception> onRejected);

		void Done(Action<PromisedT> onResolved);

		void Done();

		IPromise<PromisedT> Catch(Action<Exception> onRejected);

		IPromise<ConvertedT> Then<ConvertedT>(Func<PromisedT, IPromise<ConvertedT>> onResolved);

		IPromise Then(Func<PromisedT, IPromise> onResolved);

		IPromise<PromisedT> Then(Action<PromisedT> onResolved);

		IPromise<ConvertedT> Then<ConvertedT>(Func<PromisedT, IPromise<ConvertedT>> onResolved, Action<Exception> onRejected);

		IPromise Then(Func<PromisedT, IPromise> onResolved, Action<Exception> onRejected);

		IPromise<PromisedT> Then(Action<PromisedT> onResolved, Action<Exception> onRejected);

		IPromise<ConvertedT> Then<ConvertedT>(Func<PromisedT, ConvertedT> transform);

		[Obsolete("Use Then instead")]
		IPromise<ConvertedT> Transform<ConvertedT>(Func<PromisedT, ConvertedT> transform);

		IPromise<IEnumerable<ConvertedT>> ThenAll<ConvertedT>(Func<PromisedT, IEnumerable<IPromise<ConvertedT>>> chain);

		IPromise ThenAll(Func<PromisedT, IEnumerable<IPromise>> chain);

		IPromise<ConvertedT> ThenRace<ConvertedT>(Func<PromisedT, IEnumerable<IPromise<ConvertedT>>> chain);

		IPromise ThenRace(Func<PromisedT, IEnumerable<IPromise>> chain);

		PromisedT Value { get; }

		IEnumerator ToWaitFor(bool abortOnFail = false);
	}
}
