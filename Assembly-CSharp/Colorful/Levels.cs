using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/levels.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Levels")]
	public class Levels : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.Mode == Levels.ColorMode.Monochrome)
			{
				base.Material.SetVector("_InputMin", new Vector4(this.InputL.x / 255f, this.InputL.x / 255f, this.InputL.x / 255f, 1f));
				base.Material.SetVector("_InputMax", new Vector4(this.InputL.y / 255f, this.InputL.y / 255f, this.InputL.y / 255f, 1f));
				base.Material.SetVector("_InputGamma", new Vector4(this.InputL.z, this.InputL.z, this.InputL.z, 1f));
				base.Material.SetVector("_OutputMin", new Vector4(this.OutputL.x / 255f, this.OutputL.x / 255f, this.OutputL.x / 255f, 1f));
				base.Material.SetVector("_OutputMax", new Vector4(this.OutputL.y / 255f, this.OutputL.y / 255f, this.OutputL.y / 255f, 1f));
			}
			else
			{
				base.Material.SetVector("_InputMin", new Vector4(this.InputR.x / 255f, this.InputG.x / 255f, this.InputB.x / 255f, 1f));
				base.Material.SetVector("_InputMax", new Vector4(this.InputR.y / 255f, this.InputG.y / 255f, this.InputB.y / 255f, 1f));
				base.Material.SetVector("_InputGamma", new Vector4(this.InputR.z, this.InputG.z, this.InputB.z, 1f));
				base.Material.SetVector("_OutputMin", new Vector4(this.OutputR.x / 255f, this.OutputG.x / 255f, this.OutputB.x / 255f, 1f));
				base.Material.SetVector("_OutputMax", new Vector4(this.OutputR.y / 255f, this.OutputG.y / 255f, this.OutputB.y / 255f, 1f));
			}
			Graphics.Blit(source, destination, base.Material);
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Levels";
		}

		public Levels.ColorMode Mode;

		public Vector3 InputL = new Vector3(0f, 255f, 1f);

		public Vector3 InputR = new Vector3(0f, 255f, 1f);

		public Vector3 InputG = new Vector3(0f, 255f, 1f);

		public Vector3 InputB = new Vector3(0f, 255f, 1f);

		public Vector2 OutputL = new Vector2(0f, 255f);

		public Vector2 OutputR = new Vector2(0f, 255f);

		public Vector2 OutputG = new Vector2(0f, 255f);

		public Vector2 OutputB = new Vector2(0f, 255f);

		public enum ColorMode
		{
			Monochrome,
			RGB
		}
	}
}
