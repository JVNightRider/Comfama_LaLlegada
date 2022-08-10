using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Comfama.LaLlegada.UI
{
	public class SetChromaHandler : MonoBehaviour
	{
		public event Action<Material> OnChromaSelected;

		[SerializeField]
		private RawImage photoImage;

		[SerializeField]
		private GameObject setChromaObject, selectChromaGroupObject;

		[SerializeField]
		private List<Button> backgroundItemButton;

		[SerializeField]
		private List<ChromaGroup> chromaGroups;

		[SerializeField]
		private Button btnContinue, btnContinueToChroma;

		[SerializeField]
		private TimerHandler timer;

		[SerializeField]
		private ChromaSettingsHandler chromaSettings;

		[SerializeField]
		private ToggleGroup toggleGroup;

		private Material chromaMat;

		private Texture photo;

		public void Setup()
		{
			chromaMat = photoImage.material;

			chromaSettings.Setup();

			SetupEvents();
		}

		private void SetupEvents()
		{
			foreach (Button btn in backgroundItemButton)
			{
				btn.onClick.AddListener(() =>
				{
					Texture background = btn.transform.GetComponentInChildren<RawImage>().texture;
					chromaMat.SetTexture("_BackgroundTex", background);

					btnContinue.interactable = true;
				});
			}
			foreach (ChromaGroup chromaGroup in chromaGroups)
				chromaGroup.OnSelected += ChromaGroup_OnSelected;

			btnContinue.onClick.AddListener(ChromaSelected);
			btnContinueToChroma.onClick.AddListener(SetChromaPanel);
		}

		private void ChromaGroup_OnSelected(bool isOn, List<Texture> images)
		{
			if (isOn)
			{
				foreach (Button btn in backgroundItemButton)
					btn.gameObject.SetActive(false);

				int i = 0;
				foreach (Texture texture in images)
				{
					backgroundItemButton[i].GetComponentInChildren<RawImage>().texture = texture;
					backgroundItemButton[i].gameObject.SetActive(true);
					i++;
				}
			}
			btnContinueToChroma.interactable = toggleGroup.AnyTogglesOn();
		}

		public void Show(Texture photo)
		{
			this.photo = photo;

			gameObject.SetActive(true);

			setChromaObject.SetActive(false);
			selectChromaGroupObject.SetActive(true);

			toggleGroup.SetAllTogglesOff();

			btnContinueToChroma.interactable = toggleGroup.AnyTogglesOn();

			if (!Globals.IsFreeMode)
				timer.StartTimer(20, ChromaSelected);
		}

		private void SetChromaPanel()
		{
			selectChromaGroupObject.SetActive(false);
			setChromaObject.SetActive(true);

			btnContinue.interactable = false;
			btnContinueToChroma.interactable = false;

			chromaMat.SetTexture("_BackgroundTex", null);
			chromaMat.SetTexture("_MainTex", photo);

			chromaSettings.ResetValues();
			gameObject.SetActive(true);
		}

		private void ChromaSelected()
		{
			timer.Stop();
			chromaSettings.SaveAndClose();

			OnChromaSelected?.Invoke(chromaMat);
			gameObject.SetActive(false);
		}
	}
}