using System;
using UnityEngine;

namespace JBrothers.PreIntegratedSkinShader2
{
	public class PreIntegratedSkinProfile : ScriptableObject
	{
		public void NormalizeOriginalWeights()
		{
			this.RecalculateDerived();
			this.gauss6_1.x = this._PSSProfileHigh_weighths1_var1.x;
			this.gauss6_1.y = this._PSSProfileHigh_weighths1_var1.y;
			this.gauss6_1.z = this._PSSProfileHigh_weighths1_var1.z;
			this.gauss6_2.x = this._PSSProfileHigh_weighths2_var2.x;
			this.gauss6_2.y = this._PSSProfileHigh_weighths2_var2.y;
			this.gauss6_2.z = this._PSSProfileHigh_weighths2_var2.z;
			this.gauss6_3.x = this._PSSProfileHigh_weighths3_var3.x;
			this.gauss6_3.y = this._PSSProfileHigh_weighths3_var3.y;
			this.gauss6_3.z = this._PSSProfileHigh_weighths3_var3.z;
			this.gauss6_4.x = this._PSSProfileHigh_weighths4_var4.x;
			this.gauss6_4.y = this._PSSProfileHigh_weighths4_var4.y;
			this.gauss6_4.z = this._PSSProfileHigh_weighths4_var4.z;
			this.gauss6_5.x = this._PSSProfileHigh_weighths5_var5.x;
			this.gauss6_5.y = this._PSSProfileHigh_weighths5_var5.y;
			this.gauss6_5.z = this._PSSProfileHigh_weighths5_var5.z;
			this.gauss6_6.x = this._PSSProfileHigh_weighths6_var6.x;
			this.gauss6_6.y = this._PSSProfileHigh_weighths6_var6.y;
			this.gauss6_6.z = this._PSSProfileHigh_weighths6_var6.z;
			this.needsRenormalize = false;
		}

		public void RecalculateDerived()
		{
			Vector3 a = Vector3.zero;
			Vector3 b = this.gauss6_1;
			Vector3 b2 = this.gauss6_2;
			Vector3 b3 = this.gauss6_3;
			Vector3 b4 = this.gauss6_4;
			Vector3 b5 = this.gauss6_5;
			Vector3 b6 = this.gauss6_6;
			a += b;
			a += b2;
			a += b3;
			a += b4;
			a += b5;
			a += b6;
			b.x /= a.x;
			b.y /= a.y;
			b.z /= a.z;
			b2.x /= a.x;
			b2.y /= a.y;
			b2.z /= a.z;
			b3.x /= a.x;
			b3.y /= a.y;
			b3.z /= a.z;
			b4.x /= a.x;
			b4.y /= a.y;
			b4.z /= a.z;
			b5.x /= a.x;
			b5.y /= a.y;
			b5.z /= a.z;
			b6.x /= a.x;
			b6.y /= a.y;
			b6.z /= a.z;
			Color color = new Color(b.x, b.y, b.z);
			float grayscale = color.grayscale;
			Color color2 = new Color(b2.x, b2.y, b2.z);
			float grayscale2 = color2.grayscale;
			Color color3 = new Color(b3.x, b3.y, b3.z);
			float grayscale3 = color3.grayscale;
			Color color4 = new Color(b4.x, b4.y, b4.z);
			float grayscale4 = color4.grayscale;
			Color color5 = new Color(b5.x, b5.y, b5.z);
			float grayscale5 = color5.grayscale;
			Color color6 = new Color(b6.x, b6.y, b6.z);
			float grayscale6 = color6.grayscale;
			this._PSSProfileHigh_weighths1_var1 = new Vector4(b.x, b.y, b.z, this.gauss6_1.w);
			this._PSSProfileHigh_weighths2_var2 = new Vector4(b2.x, b2.y, b2.z, this.gauss6_2.w);
			this._PSSProfileHigh_weighths3_var3 = new Vector4(b3.x, b3.y, b3.z, this.gauss6_3.w);
			this._PSSProfileHigh_weighths4_var4 = new Vector4(b4.x, b4.y, b4.z, this.gauss6_4.w);
			this._PSSProfileHigh_weighths5_var5 = new Vector4(b5.x, b5.y, b5.z, this.gauss6_5.w);
			this._PSSProfileHigh_weighths6_var6 = new Vector4(b6.x, b6.y, b6.z, this.gauss6_6.w);
			this._PSSProfileMedium_weighths1_var1 = new Vector4(b.x + b2.x, b.y + b2.y, b.z + b2.z, (this.gauss6_1.w * grayscale + this.gauss6_2.w * grayscale2) / (grayscale + grayscale2));
			this._PSSProfileMedium_weighths2_var2 = new Vector4(b3.x + b4.x, b3.y + b4.y, b3.z + b4.z, (this.gauss6_3.w * grayscale3 + this.gauss6_4.w * grayscale4) / (grayscale3 + grayscale4));
			this._PSSProfileMedium_weighths3_var3 = new Vector4(b5.x + b6.x, b5.y + b6.y, b5.z + b6.z, (this.gauss6_5.w * grayscale5 + this.gauss6_6.w * grayscale6) / (grayscale5 + grayscale6));
			this._PSSProfileLow_weighths1_var1 = new Vector4(b.x + b2.x + b3.x, b.y + b2.y + b3.y, b.z + b2.z + b3.z, (this.gauss6_1.w * grayscale + this.gauss6_2.w * grayscale2 + this.gauss6_3.w * grayscale3) / (grayscale + grayscale2 + grayscale3));
			this._PSSProfileLow_weighths2_var2 = new Vector4(b4.x + b5.x + b6.x, b4.y + b5.y + b6.y, b4.z + b5.z + b6.z, (this.gauss6_4.w * grayscale4 + this.gauss6_5.w * grayscale5 + this.gauss6_6.w * grayscale6) / (grayscale4 + grayscale5 + grayscale6));
			this._PSSProfileHigh_sqrtvar1234.x = Mathf.Sqrt(this._PSSProfileHigh_weighths1_var1.w);
			this._PSSProfileHigh_sqrtvar1234.y = Mathf.Sqrt(this._PSSProfileHigh_weighths2_var2.w);
			this._PSSProfileHigh_sqrtvar1234.z = Mathf.Sqrt(this._PSSProfileHigh_weighths3_var3.w);
			this._PSSProfileHigh_sqrtvar1234.w = Mathf.Sqrt(this._PSSProfileHigh_weighths4_var4.w);
			this._PSSProfileMedium_sqrtvar123.x = Mathf.Sqrt(this._PSSProfileMedium_weighths1_var1.w);
			this._PSSProfileMedium_sqrtvar123.y = Mathf.Sqrt(this._PSSProfileMedium_weighths2_var2.w);
			this._PSSProfileMedium_sqrtvar123.z = Mathf.Sqrt(this._PSSProfileMedium_weighths3_var3.w);
			this._PSSProfileLow_sqrtvar12.x = Mathf.Sqrt(this._PSSProfileLow_weighths1_var1.w);
			this._PSSProfileLow_sqrtvar12.y = Mathf.Sqrt(this._PSSProfileLow_weighths2_var2.w);
			this._PSSProfileHigh_transl123_sqrtvar5.w = Mathf.Sqrt(this._PSSProfileHigh_weighths5_var5.w);
			this._PSSProfileHigh_transl456_sqrtvar6.w = Mathf.Sqrt(this._PSSProfileHigh_weighths6_var6.w);
			float num = -1.442695f;
			this._PSSProfileHigh_transl123_sqrtvar5.x = num / this.gauss6_1.w;
			this._PSSProfileHigh_transl123_sqrtvar5.y = num / this.gauss6_2.w;
			this._PSSProfileHigh_transl123_sqrtvar5.z = num / this.gauss6_3.w;
			this._PSSProfileHigh_transl456_sqrtvar6.x = num / this.gauss6_4.w;
			this._PSSProfileHigh_transl456_sqrtvar6.y = num / this.gauss6_5.w;
			this._PSSProfileHigh_transl456_sqrtvar6.z = num / this.gauss6_6.w;
			this._PSSProfileMedium_transl123.x = num / this._PSSProfileMedium_weighths1_var1.w;
			this._PSSProfileMedium_transl123.y = num / this._PSSProfileMedium_weighths2_var2.w;
			this._PSSProfileMedium_transl123.z = num / this._PSSProfileMedium_weighths3_var3.w;
			Vector3 vector;
			vector.x = this.gauss6_1.w * b.x + this.gauss6_2.w * b2.x + this.gauss6_3.w * b3.x + this.gauss6_4.w * b4.x + this.gauss6_5.w * b5.x + this.gauss6_6.w * b6.x;
			vector.y = this.gauss6_1.w * b.y + this.gauss6_2.w * b2.y + this.gauss6_3.w * b3.y + this.gauss6_4.w * b4.y + this.gauss6_5.w * b5.y + this.gauss6_6.w * b6.y;
			vector.z = this.gauss6_1.w * b.z + this.gauss6_2.w * b2.z + this.gauss6_3.w * b3.z + this.gauss6_4.w * b4.z + this.gauss6_5.w * b5.z + this.gauss6_6.w * b6.z;
			this._PSSProfileLow_transl.x = num / vector.x;
			this._PSSProfileLow_transl.y = num / vector.y;
			this._PSSProfileLow_transl.z = num / vector.z;
			this.needsRecalcDerived = false;
		}

		public void ApplyProfile(Material material)
		{
			this.ApplyProfile(material, false);
		}

		public void ApplyProfile(Material material, bool noWarn)
		{
			if (this.needsRecalcDerived)
			{
				this.RecalculateDerived();
			}
			material.SetVector("_PSSProfileHigh_weighths1_var1", this._PSSProfileHigh_weighths1_var1);
			material.SetVector("_PSSProfileHigh_weighths2_var2", this._PSSProfileHigh_weighths2_var2);
			material.SetVector("_PSSProfileHigh_weighths3_var3", this._PSSProfileHigh_weighths3_var3);
			material.SetVector("_PSSProfileHigh_weighths4_var4", this._PSSProfileHigh_weighths4_var4);
			material.SetVector("_PSSProfileHigh_weighths5_var5", this._PSSProfileHigh_weighths5_var5);
			material.SetVector("_PSSProfileHigh_weighths6_var6", this._PSSProfileHigh_weighths6_var6);
			material.SetVector("_PSSProfileHigh_sqrtvar1234", this._PSSProfileHigh_sqrtvar1234);
			material.SetVector("_PSSProfileHigh_transl123_sqrtvar5", this._PSSProfileHigh_transl123_sqrtvar5);
			material.SetVector("_PSSProfileHigh_transl456_sqrtvar6", this._PSSProfileHigh_transl456_sqrtvar6);
			material.SetVector("_PSSProfileMedium_weighths1_var1", this._PSSProfileMedium_weighths1_var1);
			material.SetVector("_PSSProfileMedium_weighths2_var2", this._PSSProfileMedium_weighths2_var2);
			material.SetVector("_PSSProfileMedium_weighths3_var3", this._PSSProfileMedium_weighths3_var3);
			material.SetVector("_PSSProfileMedium_transl123", this._PSSProfileMedium_transl123);
			material.SetVector("_PSSProfileMedium_sqrtvar123", this._PSSProfileMedium_sqrtvar123);
			material.SetVector("_PSSProfileLow_weighths1_var1", this._PSSProfileLow_weighths1_var1);
			material.SetVector("_PSSProfileLow_weighths2_var2", this._PSSProfileLow_weighths2_var2);
			material.SetVector("_PSSProfileLow_sqrtvar12", this._PSSProfileLow_sqrtvar12);
			material.SetVector("_PSSProfileLow_transl", this._PSSProfileLow_transl);
		}

		public Vector4 gauss6_1;

		public Vector4 gauss6_2;

		public Vector4 gauss6_3;

		public Vector4 gauss6_4;

		public Vector4 gauss6_5;

		public Vector4 gauss6_6;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileHigh_weighths1_var1;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileHigh_weighths2_var2;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileHigh_weighths3_var3;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileHigh_weighths4_var4;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileHigh_weighths5_var5;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileHigh_weighths6_var6;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileHigh_sqrtvar1234;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileHigh_transl123_sqrtvar5;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileHigh_transl456_sqrtvar6;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileMedium_weighths1_var1;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileMedium_weighths2_var2;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileMedium_weighths3_var3;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileMedium_transl123;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileMedium_sqrtvar123;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileLow_weighths1_var1;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileLow_weighths2_var2;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileLow_sqrtvar12;

		[HideInInspector]
		[SerializeField]
		private Vector4 _PSSProfileLow_transl;

		[HideInInspector]
		public bool needsRenormalize = true;

		[HideInInspector]
		public bool needsRecalcDerived = true;

		[HideInInspector]
		public Texture2D referenceTexture;
	}
}
