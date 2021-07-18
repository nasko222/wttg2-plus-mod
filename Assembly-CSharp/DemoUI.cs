using System;
using UnityEngine;

public class DemoUI : MonoBehaviour
{
	private void Start()
	{
		this.m_SSAOPro = base.GetComponent<SSAOPro>();
	}

	private void OnGUI()
	{
		GUI.Box(new Rect(10f, 10f, 130f, 194f), string.Empty);
		GUI.BeginGroup(new Rect(20f, 15f, 200f, 200f));
		this.m_SSAOPro.enabled = GUILayout.Toggle(this.m_SSAOPro.enabled, "Enable SSAO", new GUILayoutOption[0]);
		this.m_SSAOPro.DebugAO = GUILayout.Toggle(this.m_SSAOPro.DebugAO, "Show AO Only", new GUILayoutOption[0]);
		bool flag = this.m_SSAOPro.Blur == SSAOPro.BlurMode.HighQualityBilateral;
		flag = GUILayout.Toggle(flag, "HQ Bilateral Blur", new GUILayoutOption[0]);
		this.m_SSAOPro.Blur = ((!flag) ? SSAOPro.BlurMode.None : SSAOPro.BlurMode.HighQualityBilateral);
		GUILayout.Space(10f);
		bool flag2 = this.m_SSAOPro.Samples == SSAOPro.SampleCount.VeryLow;
		flag2 = GUILayout.Toggle(flag2, "4 samples", new GUILayoutOption[0]);
		this.m_SSAOPro.Samples = ((!flag2) ? this.m_SSAOPro.Samples : SSAOPro.SampleCount.VeryLow);
		flag2 = (this.m_SSAOPro.Samples == SSAOPro.SampleCount.Low);
		flag2 = GUILayout.Toggle(flag2, "8 samples", new GUILayoutOption[0]);
		this.m_SSAOPro.Samples = ((!flag2) ? this.m_SSAOPro.Samples : SSAOPro.SampleCount.Low);
		flag2 = (this.m_SSAOPro.Samples == SSAOPro.SampleCount.Medium);
		flag2 = GUILayout.Toggle(flag2, "12 samples", new GUILayoutOption[0]);
		this.m_SSAOPro.Samples = ((!flag2) ? this.m_SSAOPro.Samples : SSAOPro.SampleCount.Medium);
		flag2 = (this.m_SSAOPro.Samples == SSAOPro.SampleCount.High);
		flag2 = GUILayout.Toggle(flag2, "16 samples", new GUILayoutOption[0]);
		this.m_SSAOPro.Samples = ((!flag2) ? this.m_SSAOPro.Samples : SSAOPro.SampleCount.High);
		flag2 = (this.m_SSAOPro.Samples == SSAOPro.SampleCount.Ultra);
		flag2 = GUILayout.Toggle(flag2, "20 samples", new GUILayoutOption[0]);
		this.m_SSAOPro.Samples = ((!flag2) ? this.m_SSAOPro.Samples : SSAOPro.SampleCount.Ultra);
		GUI.EndGroup();
	}

	protected SSAOPro m_SSAOPro;
}
