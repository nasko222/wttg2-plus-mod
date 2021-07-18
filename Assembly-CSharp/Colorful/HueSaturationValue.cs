using System;
using UnityEngine;

namespace Colorful
{
	[HelpURL("http://www.thomashourdel.com/colorful/doc/color-correction/hue-saturation-value.html")]
	[ExecuteInEditMode]
	[AddComponentMenu("Colorful FX/Color Correction/Hue, Saturation, Value")]
	public class HueSaturationValue : BaseEffect
	{
		protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.Material.SetVector("_Master", new Vector3(this.MasterHue / 360f, (this.MasterSaturation + 100f) * 0.01f, (this.MasterValue + 100f) * 0.01f));
			if (this.AdvancedMode)
			{
				base.Material.SetVector("_Reds", new Vector3(this.RedsHue / 360f, (this.RedsSaturation + 100f) * 0.01f, (this.RedsValue + 100f) * 0.01f));
				base.Material.SetVector("_Yellows", new Vector3(this.YellowsHue / 360f, (this.YellowsSaturation + 100f) * 0.01f, (this.YellowsValue + 100f) * 0.01f));
				base.Material.SetVector("_Greens", new Vector3(this.GreensHue / 360f, (this.GreensSaturation + 100f) * 0.01f, (this.GreensValue + 100f) * 0.01f));
				base.Material.SetVector("_Cyans", new Vector3(this.CyansHue / 360f, (this.CyansSaturation + 100f) * 0.01f, (this.CyansValue + 100f) * 0.01f));
				base.Material.SetVector("_Blues", new Vector3(this.BluesHue / 360f, (this.BluesSaturation + 100f) * 0.01f, (this.BluesValue + 100f) * 0.01f));
				base.Material.SetVector("_Magentas", new Vector3(this.MagentasHue / 360f, (this.MagentasSaturation + 100f) * 0.01f, (this.MagentasValue + 100f) * 0.01f));
				Graphics.Blit(source, destination, base.Material, 1);
			}
			else
			{
				Graphics.Blit(source, destination, base.Material, 0);
			}
		}

		protected override string GetShaderName()
		{
			return "Hidden/Colorful/Hue Saturation Value";
		}

		[Range(-180f, 180f)]
		public float MasterHue;

		[Range(-100f, 100f)]
		public float MasterSaturation;

		[Range(-100f, 100f)]
		public float MasterValue;

		[Range(-180f, 180f)]
		public float RedsHue;

		[Range(-100f, 100f)]
		public float RedsSaturation;

		[Range(-100f, 100f)]
		public float RedsValue;

		[Range(-180f, 180f)]
		public float YellowsHue;

		[Range(-100f, 100f)]
		public float YellowsSaturation;

		[Range(-100f, 100f)]
		public float YellowsValue;

		[Range(-180f, 180f)]
		public float GreensHue;

		[Range(-100f, 100f)]
		public float GreensSaturation;

		[Range(-100f, 100f)]
		public float GreensValue;

		[Range(-180f, 180f)]
		public float CyansHue;

		[Range(-100f, 100f)]
		public float CyansSaturation;

		[Range(-100f, 100f)]
		public float CyansValue;

		[Range(-180f, 180f)]
		public float BluesHue;

		[Range(-100f, 100f)]
		public float BluesSaturation;

		[Range(-100f, 100f)]
		public float BluesValue;

		[Range(-180f, 180f)]
		public float MagentasHue;

		[Range(-100f, 100f)]
		public float MagentasSaturation;

		[Range(-100f, 100f)]
		public float MagentasValue;

		public bool AdvancedMode;
	}
}
