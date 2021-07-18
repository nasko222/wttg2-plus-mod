using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/vintage-fast.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Vintage")]
	public class VintageFast : LookupFilter3D
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.Filter != this.m_CurrentFilter)
			{
				this.m_CurrentFilter = this.Filter;
				if (this.Filter == Vintage.InstragramFilter.None)
				{
					this.LookupTexture = null;
				}
				else
				{
					this.LookupTexture = Resources.Load<Texture2D>("InstagramFast/" + this.Filter.ToString());
				}
			}
			base.OnRenderImage(source, destination);
		}

		public Vintage.InstragramFilter Filter;

		protected Vintage.InstragramFilter m_CurrentFilter;
	}
}
