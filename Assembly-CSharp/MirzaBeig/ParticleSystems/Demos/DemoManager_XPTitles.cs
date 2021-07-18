using System;
using UnityEngine;
using UnityEngine.UI;

namespace MirzaBeig.ParticleSystems.Demos
{
	[Serializable]
	public class DemoManager_XPTitles : MonoBehaviour
	{
		private void Awake()
		{
			(this.list = base.GetComponent<LoopingParticleSystemsManager>()).Init();
		}

		private void Start()
		{
			this.cameraRotator = Camera.main.GetComponentInParent<Rotator>();
			this.updateCurrentParticleSystemNameText();
		}

		public void ToggleRotation()
		{
			this.cameraRotator.enabled = !this.cameraRotator.enabled;
		}

		public void ResetRotation()
		{
			this.cameraRotator.transform.eulerAngles = Vector3.zero;
		}

		private void Update()
		{
			if (Input.GetAxis("Mouse ScrollWheel") < 0f)
			{
				this.Next();
			}
			else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
			{
				this.previous();
			}
		}

		private void LateUpdate()
		{
			if (this.particleCountText)
			{
				this.particleCountText.text = "PARTICLE COUNT: ";
				Text text = this.particleCountText;
				text.text += this.list.GetParticleCount().ToString();
			}
		}

		public void Next()
		{
			this.list.Next();
			this.updateCurrentParticleSystemNameText();
		}

		public void previous()
		{
			this.list.Previous();
			this.updateCurrentParticleSystemNameText();
		}

		private void updateCurrentParticleSystemNameText()
		{
			if (this.currentParticleSystemText)
			{
				this.currentParticleSystemText.text = this.list.GetCurrentPrefabName(true);
			}
		}

		private LoopingParticleSystemsManager list;

		public Text particleCountText;

		public Text currentParticleSystemText;

		private Rotator cameraRotator;
	}
}
