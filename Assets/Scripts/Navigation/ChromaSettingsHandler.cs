using HSVPicker;
using UnityEngine;
using UnityEngine.UI;

namespace Comfama.LaLlegada.UI
{
	public class ChromaSettingsHandler : MonoBehaviour
	{
		private const string SENSITIVITY_KEY = "MAT_SENSITIVITY";
		private const string SMOOTH_KEY = "MAT_SMOOTHNESS";
		private const string COLOR_KEY = "MAT_COLOR_";

		[SerializeField]
		private Slider sliderSmooth, sliderSensitivity;

		[SerializeField]
		private ColorPicker colorPicker;

		[SerializeField]
		private Button setValuesButton, setPassButton, openPassFieldButton;

		[SerializeField]
		private InputField inputPassword;

		[SerializeField]
		private string password = "Comfama54321";

		[SerializeField]
		private Material baseMaterial;

		public void Setup()
		{
			TryLoadValues();

			openPassFieldButton.onClick.AddListener(OpenPassField);
			setPassButton.onClick.AddListener(TrySetPass);
			setValuesButton.onClick.AddListener(SaveAndClose);

			sliderSensitivity.onValueChanged.AddListener(SensitivityChanged);
			sliderSmooth.onValueChanged.AddListener(SmoothnessChanged);

			colorPicker.onValueChanged.AddListener(ColorValueChanged);
		}

		private void SmoothnessChanged(float smoothness)
		{
			baseMaterial.SetFloat("_Smooth", smoothness);
		}

		private void SensitivityChanged(float sensitivity)
		{
			baseMaterial.SetFloat("_Sensitivity", sensitivity);
		}

		private void ColorValueChanged(Color newColor)
		{
			baseMaterial.SetColor("_MaskCol", newColor);
		}

		private void TryLoadValues()
		{
			if (PlayerPrefs.HasKey(SMOOTH_KEY))
			{
				sliderSmooth.value = PlayerPrefs.GetFloat(SMOOTH_KEY);
				baseMaterial.SetFloat("_Smooth", sliderSmooth.value);
			}
			if (PlayerPrefs.HasKey(SENSITIVITY_KEY))
			{
				sliderSensitivity.value = PlayerPrefs.GetFloat(SENSITIVITY_KEY);
				baseMaterial.SetFloat("_Sensitivity", sliderSensitivity.value);
			}
			if (PlayerPrefs.HasKey(COLOR_KEY + "r"))
			{
				float r = PlayerPrefs.GetFloat(COLOR_KEY + "r");
				float g = PlayerPrefs.GetFloat(COLOR_KEY + "g");
				float b = PlayerPrefs.GetFloat(COLOR_KEY + "b");
				float a = PlayerPrefs.GetFloat(COLOR_KEY + "a");

				colorPicker.CurrentColor = new Color(r, g, b, a);
				baseMaterial.SetColor("_MaskCol", colorPicker.CurrentColor);
			}
		}

		public void ResetValues()
		{
			openPassFieldButton.gameObject.SetActive(true);
			inputPassword.gameObject.SetActive(false);

			gameObject.SetActive(false);
		}

		private void OpenPassField()
		{
			openPassFieldButton.gameObject.SetActive(false);
			inputPassword.gameObject.SetActive(true);
		}

		private void TrySetPass()
		{
			string pass = inputPassword.text.Trim();

			inputPassword.gameObject.SetActive(false);
			inputPassword.text = string.Empty;

			if (!pass.Equals(password))
			{
				openPassFieldButton.gameObject.SetActive(true);
				return;
			}
			gameObject.SetActive(true);
		}

		public void SaveAndClose()
		{
			SaveValuesOnPrefs();
			ResetValues();
		}

		private void SaveValuesOnPrefs()
		{
			PlayerPrefs.SetFloat(SENSITIVITY_KEY, sliderSensitivity.value);
			PlayerPrefs.SetFloat(SMOOTH_KEY, sliderSmooth.value);

			PlayerPrefs.SetFloat(COLOR_KEY + "r", colorPicker.CurrentColor.r);
			PlayerPrefs.SetFloat(COLOR_KEY + "g", colorPicker.CurrentColor.g);
			PlayerPrefs.SetFloat(COLOR_KEY + "b", colorPicker.CurrentColor.b);
			PlayerPrefs.SetFloat(COLOR_KEY + "a", colorPicker.CurrentColor.a);

			PlayerPrefs.Save();
		}
	}
}