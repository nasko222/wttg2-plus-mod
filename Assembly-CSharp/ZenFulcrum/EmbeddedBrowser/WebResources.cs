using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	public abstract class WebResources
	{
		public virtual WebResources.Response this[string url]
		{
			get
			{
				if (string.IsNullOrEmpty(url) || url[0] != '/')
				{
					return this.GetError("Invalid path", 500);
				}
				if (url.IndexOf('?') >= 0)
				{
					url = url.Substring(0, url.IndexOf('?'));
				}
				if (url.IndexOf('#') >= 0)
				{
					url = url.Substring(0, url.IndexOf('#'));
				}
				string text = WWW.UnEscapeURL(url);
				text = this.matchDots.Replace(text, ".");
				WebResources.Response result;
				try
				{
					byte[] data = this.GetData(text);
					if (data == null)
					{
						Debug.LogWarning("WebResources: File not found fetching " + text);
						result = this.GetError("Not found", 404);
					}
					else
					{
						string text2 = Path.GetExtension(text);
						if (text2.Length > 0)
						{
							text2 = text2.Substring(1);
						}
						string mimeType;
						if (!WebResources.extensionMimeTypes.TryGetValue(text2, out mimeType))
						{
							mimeType = WebResources.extensionMimeTypes["*"];
						}
						result = new WebResources.Response
						{
							mimeType = mimeType,
							data = data,
							responseCode = 200
						};
					}
				}
				catch (Exception exception)
				{
					Debug.LogError("WebResources: Failed to fetch URL " + text);
					Debug.LogException(exception);
					result = this.GetError("Internal error", 500);
				}
				return result;
			}
		}

		public abstract byte[] GetData(string path);

		public WebResources.Response GetError(string text, int status = 500)
		{
			return new WebResources.Response
			{
				data = Encoding.UTF8.GetBytes(text),
				mimeType = "text/plain",
				responseCode = status
			};
		}

		public static readonly Dictionary<string, string> extensionMimeTypes = new Dictionary<string, string>
		{
			{
				"css",
				"text/css"
			},
			{
				"gif",
				"image/gif"
			},
			{
				"html",
				"text/html"
			},
			{
				"jpeg",
				"image/jpeg"
			},
			{
				"jpg",
				"image/jpeg"
			},
			{
				"js",
				"application/javascript"
			},
			{
				"mp3",
				"audio/mpeg"
			},
			{
				"mpeg",
				"video/mpeg"
			},
			{
				"ogg",
				"application/ogg"
			},
			{
				"ogv",
				"video/ogg"
			},
			{
				"webm",
				"video/webm"
			},
			{
				"png",
				"image/png"
			},
			{
				"svg",
				"image/svg+xml"
			},
			{
				"txt",
				"text/plain"
			},
			{
				"xml",
				"application/xml"
			},
			{
				"*",
				"application/octet-stream"
			}
		};

		private readonly Regex matchDots = new Regex("\\.[2,]");

		public struct Response
		{
			public int responseCode;

			public string mimeType;

			public byte[] data;
		}
	}
}
