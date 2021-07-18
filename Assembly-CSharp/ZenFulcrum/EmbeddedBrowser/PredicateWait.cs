using System;

namespace ZenFulcrum.EmbeddedBrowser
{
	internal class PredicateWait
	{
		public Func<TimeData, bool> predicate;

		public float timeStarted;

		public IPendingPromise pendingPromise;

		public TimeData timeData;
	}
}
