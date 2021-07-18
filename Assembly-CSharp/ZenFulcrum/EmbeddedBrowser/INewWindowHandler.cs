using System;

namespace ZenFulcrum.EmbeddedBrowser
{
	public interface INewWindowHandler
	{
		Browser CreateBrowser(Browser parent);
	}
}
