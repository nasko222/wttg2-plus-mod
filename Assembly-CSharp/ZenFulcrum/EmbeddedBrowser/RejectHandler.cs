using System;

namespace ZenFulcrum.EmbeddedBrowser
{
	public struct RejectHandler
	{
		public Action<Exception> callback;

		public IRejectable rejectable;
	}
}
