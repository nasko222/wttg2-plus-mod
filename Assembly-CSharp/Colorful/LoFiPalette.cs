using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/artistic-effects/lofi-palette.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Artistic Effects/LoFi Palette")]
	public class LoFiPalette : LookupFilter3D
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.Palette != this.m_CurrentPreset)
			{
				this.m_CurrentPreset = this.Palette;
				if (this.Palette == LoFiPalette.Preset.None)
				{
					this.LookupTexture = null;
				}
				else
				{
					this.LookupTexture = Resources.Load<Texture2D>("LoFiPalettes/" + this.Palette.ToString());
				}
			}
			if (this.LookupTexture == null || this.Amount <= 0f)
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.m_Use2DLut || this.ForceCompatibility)
			{
				this.RenderLut2D(source, destination);
			}
			else
			{
				this.RenderLut3D(source, destination);
			}
		}

		protected override void RenderLut2D(RenderTexture source, RenderTexture destination)
		{
			float num = Mathf.Sqrt((float)this.LookupTexture.width);
			base.Material.SetTexture("_LookupTex", this.LookupTexture);
			base.Material.SetVector("_Params1", new Vector3(1f / (float)this.LookupTexture.width, 1f / (float)this.LookupTexture.height, num - 1f));
			base.Material.SetVector("_Params2", new Vector2(this.Amount, this.PixelSize));
			int pass = ((!this.Pixelize) ? 4 : 6) + ((!CLib.IsLinearColorSpace()) ? 0 : 1);
			Graphics.Blit(source, destination, base.Material, pass);
		}

		protected override void RenderLut3D(RenderTexture source, RenderTexture destination)
		{
			if (this.LookupTexture.name != this.m_BaseTextureName)
			{
				base.ConvertBaseTexture();
			}
			if (this.m_Lut3D == null)
			{
				base.SetIdentityLut();
			}
			this.m_Lut3D.filterMode = FilterMode.Point;
			base.Material.SetTexture("_LookupTex", this.m_Lut3D);
			float num = (float)this.m_Lut3D.width;
			base.Material.SetVector("_Params", new Vector4((num - 1f) / (1f * num), 1f / (2f * num), this.Amount, this.PixelSize));
			int pass = ((!this.Pixelize) ? 0 : 2) + ((!CLib.IsLinearColorSpace()) ? 0 : 1);
			Graphics.Blit(source, destination, base.Material, pass);
		}

		public LoFiPalette.Preset Palette;

		[Tooltip("Pixelize the display.")]
		public bool Pixelize = true;

		[Tooltip("The display height in pixels.")]
		public float PixelSize = 128f;

		protected LoFiPalette.Preset m_CurrentPreset;

		public enum Preset
		{
			None,
			AmstradCPC = 2,
			CGA,
			Commodore64,
			CommodorePlus,
			EGA,
			GameBoy,
			MacOS16,
			MacOS256,
			MasterSystem,
			RiscOS16,
			Teletex,
			Windows16,
			Windows256,
			ZXSpectrum,
			Andrae = 17,
			Anodomani,
			Crayolo,
			DB16,
			DB32,
			DJinn,
			DrazileA,
			DrazileB,
			DrazileC,
			Eggy,
			FinlalA,
			FinlalB,
			Hapiel,
			PavanzA,
			PavanzB,
			Peyton,
			SpeedyCube
		}
	}
}
