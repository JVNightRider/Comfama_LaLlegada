using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Comfama.LaLlegada.UI
{
	public class SetTextHandler : MonoBehaviour
	{
		public event Action<Material, string> OnTextSelected;

		[SerializeField]
		private RawImage photo;

		[SerializeField]
		private List<Button> textItemButton;

		[SerializeField]
		private List<GameObject> words, phrases;

		[SerializeField]
		private Text photoText;

		[SerializeField]
		private TimerHandler timer;

		[SerializeField]
		private Button btnContinue;

		public void Setup()
		{
			SetupEvents();
		}

		private void SetupEvents()
		{
			foreach (Button btn in textItemButton)
			{
				btn.onClick.AddListener(() =>
				{
					string text = btn.transform.GetComponentInChildren<Text>().text;
					photoText.text = text;

					btnContinue.interactable = true;
				});
			}
			btnContinue.onClick.AddListener(TextSelected);
		}

		public void Show(Material photoMat)
		{
			textItemButton.ForEach(ti => ti.gameObject.SetActive(false));
			if (Globals.PhotoCounter == 1)
				words.ForEach(w => w.SetActive(true));
			else if (Globals.PhotoCounter == 2)
				phrases.ForEach(p => p.SetActive(true));
			else if (Globals.PhotoCounter >= 3)
			{
				OnTextSelected?.Invoke(photoMat, string.Empty);
				return;
			}
			btnContinue.interactable = false;

			photoText.text = GetPhotoMessage();
			photo.material = photoMat;

			gameObject.SetActive(true);

			if (!Globals.IsFreeMode)
				timer.StartTimer(20, TextSelected);
		}

		private string GetPhotoMessage()
		{
			switch (Globals.PhotoCounter)
			{
				case 1:
					return "¿Qué palabra te inspira la foto?";

				case 2:
					return "¿Qué mensaje te inspira la foto?";

				default:
					return string.Empty;
			}
		}

		private void TextSelected()
		{
			timer.Stop();

			OnTextSelected?.Invoke(photo.material, photoText.text);
			gameObject.SetActive(false);
		}
	}
}