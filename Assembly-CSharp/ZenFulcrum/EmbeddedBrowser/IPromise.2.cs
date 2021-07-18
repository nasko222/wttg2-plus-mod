using System;
using System.Collections;
using System.Collections.Generic;

namespace ZenFulcrum.EmbeddedBrowser
{
	public interface IPromise
	{
		IPromise WithName(string name);

		void Done(Action onResolved, Action<Exception> onRejected);

		void Done(Action onResolved);

		void Done();

		IPromise Catch(Action<Exception> onRejected);

		IPromise<ConvertedT> Then<ConvertedT>(Func<IPromise<ConvertedT>> onResolved);

		IPromise Then(Func<IPromise> onResolved);

		IPromise Then(Action onResolved);

		IPromise<ConvertedT> Then<ConvertedT>(Func<IPromise<ConvertedT>> onResolved, Action<Exception> onRejected);

		IPromise Then(Func<IPromise> onResolved, Action<Exception> onRejected);

		IPromise Then(Action onResolved, Action<Exception> onRejected);

		IPromise ThenAll(Func<IEnumerable<IPromise>> chain);

		IPromise<IEnumerable<ConvertedT>> ThenAll<ConvertedT>(Func<IEnumerable<IPromise<ConvertedT>>> chain);

		IPromise ThenSequence(Func<IEnumerable<Func<IPromise>>> chain);

		IPromise ThenRace(Func<IEnumerable<IPromise>> chain);

		IPromise<ConvertedT> ThenRace<ConvertedT>(Func<IEnumerable<IPromise<ConvertedT>>> chain);

		IEnumerator ToWaitFor(bool abortOnFail = false);
	}
}
