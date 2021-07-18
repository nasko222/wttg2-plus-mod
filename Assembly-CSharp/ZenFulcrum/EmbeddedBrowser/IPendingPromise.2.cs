using System;

namespace ZenFulcrum.EmbeddedBrowser
{
	public interface IPendingPromise : IRejectable
	{
		void Resolve();
	}
}
