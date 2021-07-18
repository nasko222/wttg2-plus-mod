using System;

namespace ZenFulcrum.EmbeddedBrowser
{
	public interface IPendingPromise<PromisedT> : IRejectable
	{
		void Resolve(PromisedT value);
	}
}
