using System;
using UnityEngine;
using UnityEngine.UI;

namespace Comfama.LaLlegada.UI
{
	public class PhotoPreviewHandler : MonoBehaviour
	{
		public event Action<Texture> OnPhotoCompleted;

		[SerializeField]
		private RawImage photoImage;

		[SerializeField]
		private Text photoText;

		[SerializeField]
		private Button nextButton;

		[SerializeField]
		private GameObject lines;

		[SerializeField]
		private ScreenshotCreator screenshotCreator;

		private Texture photoTexture;

		private void Start()
		{
			nextButton.onClick.AddListener(PhotoCompleted);
		}

		public void Show(Material photoMat, string text)
		{
			nextButton.interactable = false;
			lines.SetActive(Globals.PhotoCounter >= 3);

			photoImage.material = photoMat;
			photoText.text = text;

			gameObject.SetActive(true);

			screenshotCreator.Create(photoMat, text, ScreenshotCreated);
		}

		private void ScreenshotCreated(Texture newTexture)
		{
			photoTexture = newTexture;
			nextButton.interactable = true;
		}

		private void PhotoCompleted()
		{
			OnPhotoCompleted?.Invoke(photoTexture);
			gameObject.SetActive(false);
		}
	}
}