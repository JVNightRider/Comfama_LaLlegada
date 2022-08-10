using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Comfama.LaLlegada.UI
{
	public class CameraZone : UIZone
	{
		public override ZoneType ZoneType { get => ZoneType.Camera; }

		[SerializeField]
		private TakePhotoHandler photoHandler;

		[SerializeField]
		private SetChromaHandler chromaHandler;

		[SerializeField]
		private SetTextHandler photoTextHandler;

		[SerializeField]
		private PhotoPreviewHandler photoPreview;

		[SerializeField]
		private FinalZoneHandler finalZone;

		[SerializeField]
		private TimerHandler timer;

		[Header("Zone")]
		[SerializeField]
		private Button takePhotoButton;

		[SerializeField]
		private GameObject counterPhotoPanel;

		[SerializeField]
		private Text photoCounterText, photoMessageText;

		private List<Texture> userPhotos;

		[SerializeField]
		private int photoMaxLimit = 3;

		private int photoCount;

		public override void Setup()
		{
			base.Setup();
			userPhotos = new List<Texture>();

			chromaHandler.OnChromaSelected += photoTextHandler.Show;
			photoTextHandler.OnTextSelected += photoPreview.Show;
			photoPreview.OnPhotoCompleted += PhotoCompleted;

			chromaHandler.Setup();
			photoTextHandler.Setup();

			takePhotoButton.onClick.AddListener(() => photoHandler.TakePhoto(PhotoTaken));

			Application.runInBackground = true;
		}

		private void PhotoCompleted(Texture newPhoto)
		{
			timer.Stop();

			photoCount++;
			Globals.SetPhotoCounter(photoCount);

			userPhotos.Add(newPhoto);

			if (photoCount > photoMaxLimit)
			{
				finalZone.SetImages(userPhotos);
				ChangeZone(ZoneType.Final);
			}
			else
				ShowPhotoMenu();
		}

		public override void Show()
		{
			photoCount = 1;
			Globals.SetPhotoCounter(photoCount);

			userPhotos.Clear();

			ShowPhotoMenu();
		}

		private void ShowPhotoMenu()
		{
			photoCounterText.text = string.Format("Foto {0} / {1}", photoCount, photoMaxLimit);
			photoMessageText.text = GetMessageText();

			counterPhotoPanel.SetActive(true);
			gameObject.SetActive(true);

			if (!Globals.IsFreeMode)
				timer.StartTimer(60, () => PhotoTaken(new Texture2D(1, 1), true));
		}

		private string GetMessageText()
		{
			switch (photoCount)
			{
				case 1:
					return "¡Preparate para tomar tu primera foto!";

				case 2:
					return "¡Preparate para tomar tu segunda foto!";

				default:
					return "¡Preparate para tomar tu última foto!";
			}
		}

		public override void Hide()
		{
			gameObject.SetActive(false);
		}

		private void PhotoTaken(Texture texture, bool success)
		{
			if (success)
			{
				counterPhotoPanel.SetActive(false);
				chromaHandler.Show(texture);
			}
			else
				Debug.LogWarning("Photo couldn't be taken");
		}
	}
}