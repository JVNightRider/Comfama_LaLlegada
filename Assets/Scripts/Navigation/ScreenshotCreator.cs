using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Comfama.LaLlegada.UI
{
	public class ScreenshotCreator : MonoBehaviour
	{
		private Action<Texture> screenshotCreated;

		[SerializeField]
		private Camera cam;

		[SerializeField]
		private RawImage photoImage;

		[SerializeField]
		private Text photoText;

		[SerializeField]
		private GameObject lines;

		[SerializeField]
		private Vector2 targetRes;

		public void Create(Material photoMat, string text, Action<Texture> screenshotCreated)
		{
			this.screenshotCreated = screenshotCreated;

			photoImage.material = photoMat;
			photoText.text = text;

			lines.SetActive(Globals.PhotoCounter >= 3);

			gameObject.SetActive(true);
			StartCoroutine(CreateScreenshot());
		}

		private IEnumerator CreateScreenshot()
		{
			RenderTexture rt = new RenderTexture((int)targetRes.x, (int)targetRes.y, (int)cam.depth);
			cam.targetTexture = rt;
			Texture2D screenShot = new Texture2D((int)targetRes.x, (int)targetRes.y, TextureFormat.RGB24, false);
			cam.Render();
			RenderTexture.active = rt;

			screenShot.ReadPixels(new Rect(0, 0, cam.pixelWidth, cam.pixelHeight), 0, 0);
			yield return new WaitForEndOfFrame();

			screenShot.Apply();

			yield return new WaitForEndOfFrame();

			cam.targetTexture = null;
			RenderTexture.active = null;

			Destroy(rt);
			screenshotCreated.Invoke(screenShot);

			gameObject.SetActive(false);
		}
	}
}