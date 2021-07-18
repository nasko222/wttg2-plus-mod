using System;
using MirzaBeig.Scripting.Effects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ParticleForceFieldsDemo : MonoBehaviour
{
	private void Start()
	{
		if (this.postProcessing)
		{
			this.postProcessingToggle.isOn = this.postProcessing.enabled;
		}
		this.particleSystemMainModule = this.particleSystem.main;
		this.particleSystemEmissionModule = this.particleSystem.emission;
		this.maxParticlesSlider.value = (float)this.particleSystemMainModule.maxParticles;
		this.particlesPerSecondSlider.value = this.particleSystemEmissionModule.rateOverTime.constant;
		this.maxParticlesText.text = "Max Particles: " + this.maxParticlesSlider.value;
		this.particlesPerSecondText.text = "Particles Per Second: " + this.particlesPerSecondSlider.value;
		this.attractionParticleForceFieldRadiusSlider.value = this.attractionParticleForceField.radius;
		this.attractionParticleForceFieldMaxForceSlider.value = this.attractionParticleForceField.force;
		this.attractionParticleForceFieldArrivalRadiusSlider.value = this.attractionParticleForceField.arrivalRadius;
		this.attractionParticleForceFieldArrivedRadiusSlider.value = this.attractionParticleForceField.arrivedRadius;
		Vector3 position = this.attractionParticleForceField.transform.position;
		this.attractionParticleForceFieldPositionSliderX.value = position.x;
		this.attractionParticleForceFieldPositionSliderY.value = position.y;
		this.attractionParticleForceFieldPositionSliderZ.value = position.z;
		this.attractionParticleForceFieldRadiusText.text = "Radius: " + this.attractionParticleForceFieldRadiusSlider.value;
		this.attractionParticleForceFieldMaxForceText.text = "Max Force: " + this.attractionParticleForceFieldMaxForceSlider.value;
		this.attractionParticleForceFieldArrivalRadiusText.text = "Arrival Radius: " + this.attractionParticleForceFieldArrivalRadiusSlider.value;
		this.attractionParticleForceFieldArrivedRadiusText.text = "Arrived Radius: " + this.attractionParticleForceFieldArrivedRadiusSlider.value;
		this.attractionParticleForceFieldPositionTextX.text = "Position X: " + this.attractionParticleForceFieldPositionSliderX.value;
		this.attractionParticleForceFieldPositionTextY.text = "Position Y: " + this.attractionParticleForceFieldPositionSliderY.value;
		this.attractionParticleForceFieldPositionTextZ.text = "Position Z: " + this.attractionParticleForceFieldPositionSliderZ.value;
		this.vortexParticleForceFieldRadiusSlider.value = this.vortexParticleForceField.radius;
		this.vortexParticleForceFieldMaxForceSlider.value = this.vortexParticleForceField.force;
		Vector3 eulerAngles = this.vortexParticleForceField.transform.eulerAngles;
		this.vortexParticleForceFieldRotationSliderX.value = eulerAngles.x;
		this.vortexParticleForceFieldRotationSliderY.value = eulerAngles.y;
		this.vortexParticleForceFieldRotationSliderZ.value = eulerAngles.z;
		Vector3 position2 = this.vortexParticleForceField.transform.position;
		this.vortexParticleForceFieldPositionSliderX.value = position2.x;
		this.vortexParticleForceFieldPositionSliderY.value = position2.y;
		this.vortexParticleForceFieldPositionSliderZ.value = position2.z;
		this.vortexParticleForceFieldRadiusText.text = "Radius: " + this.vortexParticleForceFieldRadiusSlider.value;
		this.vortexParticleForceFieldMaxForceText.text = "Max Force: " + this.vortexParticleForceFieldMaxForceSlider.value;
		this.vortexParticleForceFieldRotationTextX.text = "Rotation X: " + this.vortexParticleForceFieldRotationSliderX.value;
		this.vortexParticleForceFieldRotationTextY.text = "Rotation Y: " + this.vortexParticleForceFieldRotationSliderY.value;
		this.vortexParticleForceFieldRotationTextZ.text = "Rotation Z: " + this.vortexParticleForceFieldRotationSliderZ.value;
		this.vortexParticleForceFieldPositionTextX.text = "Position X: " + this.vortexParticleForceFieldPositionSliderX.value;
		this.vortexParticleForceFieldPositionTextY.text = "Position Y: " + this.vortexParticleForceFieldPositionSliderY.value;
		this.vortexParticleForceFieldPositionTextZ.text = "Position Z: " + this.vortexParticleForceFieldPositionSliderZ.value;
	}

	private void Update()
	{
		this.FPSText.text = "FPS: " + 1f / Time.deltaTime;
		this.particleCountText.text = "Particle Count: " + this.particleSystem.particleCount;
	}

	public void ReloadScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void SetMaxParticles(float value)
	{
		this.particleSystemMainModule.maxParticles = (int)value;
		this.maxParticlesText.text = "Max Particles: " + value;
	}

	public void SetParticleEmissionPerSecond(float value)
	{
		this.particleSystemEmissionModule.rateOverTime = value;
		this.particlesPerSecondText.text = "Particles Per Second: " + value;
	}

	public void SetAttractionParticleForceFieldRadius(float value)
	{
		this.attractionParticleForceField.radius = value;
		this.attractionParticleForceFieldRadiusText.text = "Radius: " + value;
	}

	public void SetAttractionParticleForceFieldMaxForce(float value)
	{
		this.attractionParticleForceField.force = value;
		this.attractionParticleForceFieldMaxForceText.text = "Max Force: " + value;
	}

	public void SetAttractionParticleForceFieldArrivalRadius(float value)
	{
		this.attractionParticleForceField.arrivalRadius = value;
		this.attractionParticleForceFieldArrivalRadiusText.text = "Arrival Radius: " + value;
	}

	public void SetAttractionParticleForceFieldArrivedRadius(float value)
	{
		this.attractionParticleForceField.arrivedRadius = value;
		this.attractionParticleForceFieldArrivedRadiusText.text = "Arrived Radius: " + value;
	}

	public void SetAttractionParticleForceFieldPositionX(float value)
	{
		Vector3 position = this.attractionParticleForceField.transform.position;
		position.x = value;
		this.attractionParticleForceField.transform.position = position;
		this.attractionParticleForceFieldPositionTextX.text = "Position X: " + value;
	}

	public void SetAttractionParticleForceFieldPositionY(float value)
	{
		Vector3 position = this.attractionParticleForceField.transform.position;
		position.y = value;
		this.attractionParticleForceField.transform.position = position;
		this.attractionParticleForceFieldPositionTextY.text = "Position Y: " + value;
	}

	public void SetAttractionParticleForceFieldPositionZ(float value)
	{
		Vector3 position = this.attractionParticleForceField.transform.position;
		position.z = value;
		this.attractionParticleForceField.transform.position = position;
		this.attractionParticleForceFieldPositionTextZ.text = "Position Z: " + value;
	}

	public void SetVortexParticleForceFieldRadius(float value)
	{
		this.vortexParticleForceField.radius = value;
		this.vortexParticleForceFieldRadiusText.text = "Radius: " + value;
	}

	public void SetVortexParticleForceFieldMaxForce(float value)
	{
		this.vortexParticleForceField.force = value;
		this.vortexParticleForceFieldMaxForceText.text = "Max Force: " + value;
	}

	public void SetVortexParticleForceFieldRotationX(float value)
	{
		Vector3 eulerAngles = this.vortexParticleForceField.transform.eulerAngles;
		eulerAngles.x = value;
		this.vortexParticleForceField.transform.eulerAngles = eulerAngles;
		this.vortexParticleForceFieldRotationTextX.text = "Rotation X: " + value;
	}

	public void SetVortexParticleForceFieldRotationY(float value)
	{
		Vector3 eulerAngles = this.vortexParticleForceField.transform.eulerAngles;
		eulerAngles.y = value;
		this.vortexParticleForceField.transform.eulerAngles = eulerAngles;
		this.vortexParticleForceFieldRotationTextY.text = "Rotation Y: " + value;
	}

	public void SetVortexParticleForceFieldRotationZ(float value)
	{
		Vector3 eulerAngles = this.vortexParticleForceField.transform.eulerAngles;
		eulerAngles.z = value;
		this.vortexParticleForceField.transform.eulerAngles = eulerAngles;
		this.vortexParticleForceFieldRotationTextZ.text = "Rotation Z: " + value;
	}

	public void SetVortexParticleForceFieldPositionX(float value)
	{
		Vector3 position = this.vortexParticleForceField.transform.position;
		position.x = value;
		this.vortexParticleForceField.transform.position = position;
		this.vortexParticleForceFieldPositionTextX.text = "Position X: " + value;
	}

	public void SetVortexParticleForceFieldPositionY(float value)
	{
		Vector3 position = this.vortexParticleForceField.transform.position;
		position.y = value;
		this.vortexParticleForceField.transform.position = position;
		this.vortexParticleForceFieldPositionTextY.text = "Position Y: " + value;
	}

	public void SetVortexParticleForceFieldPositionZ(float value)
	{
		Vector3 position = this.vortexParticleForceField.transform.position;
		position.z = value;
		this.vortexParticleForceField.transform.position = position;
		this.vortexParticleForceFieldPositionTextZ.text = "Position Z: " + value;
	}

	[Header("Overview")]
	public Text FPSText;

	public Text particleCountText;

	public Toggle postProcessingToggle;

	public MonoBehaviour postProcessing;

	[Header("Particle System Settings")]
	public ParticleSystem particleSystem;

	private ParticleSystem.MainModule particleSystemMainModule;

	private ParticleSystem.EmissionModule particleSystemEmissionModule;

	public Text maxParticlesText;

	public Text particlesPerSecondText;

	public Slider maxParticlesSlider;

	public Slider particlesPerSecondSlider;

	[Header("Attraction Particle Force Field Settings")]
	public AttractionParticleForceField attractionParticleForceField;

	public Text attractionParticleForceFieldRadiusText;

	public Text attractionParticleForceFieldMaxForceText;

	public Text attractionParticleForceFieldArrivalRadiusText;

	public Text attractionParticleForceFieldArrivedRadiusText;

	public Text attractionParticleForceFieldPositionTextX;

	public Text attractionParticleForceFieldPositionTextY;

	public Text attractionParticleForceFieldPositionTextZ;

	public Slider attractionParticleForceFieldRadiusSlider;

	public Slider attractionParticleForceFieldMaxForceSlider;

	public Slider attractionParticleForceFieldArrivalRadiusSlider;

	public Slider attractionParticleForceFieldArrivedRadiusSlider;

	public Slider attractionParticleForceFieldPositionSliderX;

	public Slider attractionParticleForceFieldPositionSliderY;

	public Slider attractionParticleForceFieldPositionSliderZ;

	[Header("Vortex Particle Force Field Settings")]
	public VortexParticleForceField vortexParticleForceField;

	public Text vortexParticleForceFieldRadiusText;

	public Text vortexParticleForceFieldMaxForceText;

	public Text vortexParticleForceFieldRotationTextX;

	public Text vortexParticleForceFieldRotationTextY;

	public Text vortexParticleForceFieldRotationTextZ;

	public Text vortexParticleForceFieldPositionTextX;

	public Text vortexParticleForceFieldPositionTextY;

	public Text vortexParticleForceFieldPositionTextZ;

	public Slider vortexParticleForceFieldRadiusSlider;

	public Slider vortexParticleForceFieldMaxForceSlider;

	public Slider vortexParticleForceFieldRotationSliderX;

	public Slider vortexParticleForceFieldRotationSliderY;

	public Slider vortexParticleForceFieldRotationSliderZ;

	public Slider vortexParticleForceFieldPositionSliderX;

	public Slider vortexParticleForceFieldPositionSliderY;

	public Slider vortexParticleForceFieldPositionSliderZ;
}
