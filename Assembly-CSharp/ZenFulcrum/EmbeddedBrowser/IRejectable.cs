using System;

namespace ZenFulcrum.EmbeddedBrowser
{
	public interface IRejectable
	{
		void Reject(Exception ex);
	}
}
