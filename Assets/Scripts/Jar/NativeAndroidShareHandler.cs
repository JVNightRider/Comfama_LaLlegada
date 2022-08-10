using System.Collections;
using System.Collections.Generic;
using Comfama.LaLlegada.UI;
using UnityEngine;
using UnityEngine.UI;

public class NativeAndroidShareHandler : MonoBehaviour
{
	public Button shareButton;

	public InputField numField;

	[SerializeField]
	private ConfirmationPopup popup;

	private bool isFocus = false;
	private bool isProcessing = false;

	private void Start()
	{
		if (shareButton != null)
			shareButton.onClick.AddListener(() => ShareImages(numField.text, new List<string>() { "/storage/emulated/0/DCIM/1.jpg" , "/storage/emulated/0/DCIM/2.jpg" }));
	}

	private void OnApplicationFocus(bool focus)
	{
		isFocus = focus;
	}

	public void ShareImages(string phone, List<string> fileURLs)
	{
		//ShareTextInAnroidVoid();
#if UNITY_ANDROID
		if (!isProcessing)
		{
			//StartCoroutine(ShareTextInAnroid(fileURL));
			StartCoroutine(ShareTextInAnroid(phone, fileURLs));
		}
#else
		Debug.Log("No sharing set up for this platform.");
#endif
	}

#if UNITY_ANDROID

	public IEnumerator ShareTextInAnroid(string phone, List<string> paths)
	{
		isProcessing = true;

		RunWhatsapp(phone);

		yield return new WaitForSeconds(0.5f);

		if (!Application.isEditor)
		{
			try
			{
				
				AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
				AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
				AndroidJavaObject list = new AndroidJavaObject("java.util.ArrayList");

				//intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND_MULTIPLE"));
				intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND_MULTIPLE"));

				foreach (string path in paths)
				{
					AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
					AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", path);

					list.Call<bool>("add", uriObject);
				}

				intentObject.Call<AndroidJavaObject>("putParcelableArrayListExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), list);
				intentObject.Call<AndroidJavaObject>("setType", "image");

				intentObject.Call<AndroidJavaObject>("setPackage", "com.whatsapp");
				intentObject.Call<AndroidJavaObject>("putExtra", "jid", "57" + phone + "@s.whatsapp.net");

				AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

				currentActivity.Call("startActivity", intentObject);


			}
			catch (System.Exception ex)
			{
				popup.ShowText(ex.Message);
			}
		}

		yield return new WaitUntil(() => isFocus);
		isProcessing = false;
	}


	void RunWhatsapp(string number)
	{
		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

		AndroidJavaObject conversationComponent = new AndroidJavaObject("android.content.ComponentName", "com.whatsapp", "com.whatsapp.Conversation");


		intentObject.Call<AndroidJavaObject>("setComponent", conversationComponent);

		intentObject.Call<AndroidJavaObject>("setType", "image");

		intentObject.Call<AndroidJavaObject>("putExtra", "jid", "57" + number + "@s.whatsapp.net");

		intentObject.Call<AndroidJavaObject>("setPackage", "com.whatsapp");

		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

		currentActivity.Call("startActivity", intentObject);
	}
#endif
}