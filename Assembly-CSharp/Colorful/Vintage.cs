using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/vintage.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Vintage (Deprecated)")]
	public class Vintage : LookupFilter
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
					this.LookupTexture = Resources.Load<Texture2D>("Instagram/" + this.Filter.ToString());
				}
			}
			base.OnRenderImage(source, destination);
		}

		public Vintage.InstragramFilter Filter;

		protected Vintage.InstragramFilter m_CurrentFilter;

		public enum InstragramFilter
		{
			None,
			F1977,
			Aden,
			Amaro,
			Brannan,
			Crema,
			Earlybird,
			Hefe,
			Hudson,
			Inkwell,
			Juno,
			Kelvin,
			Lark,
			LoFi,
			Ludwig,
			Mayfair,
			Nashville,
			Perpetua,
			Reyes,
			Rise,
			Sierra,
			Slumber,
			Sutro,
			Toaster,
			Valencia,
			Walden,
			Willow,
			XProII
		}
	}
}
