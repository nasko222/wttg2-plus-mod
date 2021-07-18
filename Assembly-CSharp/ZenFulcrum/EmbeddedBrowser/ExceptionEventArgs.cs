using System;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class ExceptionEventArgs : EventArgs
	{
		internal ExceptionEventArgs(Exception exception)
		{
			this.Exception = exception;
		}

		public Exception Exception { get; private set; }
	}
}
