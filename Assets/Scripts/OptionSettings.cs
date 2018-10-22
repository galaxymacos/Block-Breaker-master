using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionSettings : MonoBehaviour
{

	[SerializeField] private TextMeshPro graphicsText;
	[SerializeField] private Text qualityText;
	[SerializeField] private Slider graphicsSlider;
	
	public void ChangeGraphics()
	{
		QualitySettings.SetQualityLevel((int) graphicsSlider.value);
		switch ((int)graphicsSlider.value)
		{
				case 0:
					graphicsText.text = "Low";
					qualityText.text = "Low";
					break;
				case 1:
					graphicsText.text = "Medium";
					qualityText.text = "Medium";
					break;
				case 2:
					graphicsText.text = "High";
					qualityText.text = "High";
					break;
				default:
					throw new ArgumentOutOfRangeException(graphicsSlider.name);
		}
	}
}
