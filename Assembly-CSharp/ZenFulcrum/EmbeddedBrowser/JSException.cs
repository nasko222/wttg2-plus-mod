using System;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class JSException : Exception
	{
		public JSException(string what) : base(what)
		{
		}
	}
}
