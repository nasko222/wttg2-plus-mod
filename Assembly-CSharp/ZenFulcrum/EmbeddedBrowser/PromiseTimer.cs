using System;
using System.Collections.Generic;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class PromiseTimer : IPromiseTimer
	{
		public IPromise WaitFor(float seconds)
		{
			return this.WaitUntil((TimeData t) => t.elapsedTime >= seconds);
		}

		public IPromise WaitWhile(Func<TimeData, bool> predicate)
		{
			return this.WaitUntil((TimeData t) => !predicate(t));
		}

		public IPromise WaitUntil(Func<TimeData, bool> predicate)
		{
			Promise promise = new Promise();
			PredicateWait item = new PredicateWait
			{
				timeStarted = this.curTime,
				pendingPromise = promise,
				timeData = default(TimeData),
				predicate = predicate
			};
			this.waiting.Add(item);
			return promise;
		}

		public void Update(float deltaTime)
		{
			this.curTime += deltaTime;
			int i = 0;
			while (i < this.waiting.Count)
			{
				PredicateWait predicateWait = this.waiting[i];
				float num = this.curTime - predicateWait.timeStarted;
				predicateWait.timeData.deltaTime = num - predicateWait.timeData.elapsedTime;
				predicateWait.timeData.elapsedTime = num;
				bool flag;
				try
				{
					flag = predicateWait.predicate(predicateWait.timeData);
				}
				catch (Exception ex)
				{
					predicateWait.pendingPromise.Reject(ex);
					this.waiting.RemoveAt(i);
					continue;
				}
				if (flag)
				{
					predicateWait.pendingPromise.Resolve();
					this.waiting.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}

		private float curTime;

		private List<PredicateWait> waiting = new List<PredicateWait>();
	}
}
