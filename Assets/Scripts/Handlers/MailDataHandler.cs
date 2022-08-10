using System;
using System.Collections.Generic;
using System.IO;
using DreamHouseStudios.Comfama;
using UnityEngine;
using UT;

public class MailDataHandler : MonoBehaviour
{
	private Action onUserSent;

	private Dictionary<string, PhotoData> photosByteMap;

	private PendingUsersToSend usersToSend;

	private const string USERS_KEY = "PENDING_USERS_DATA";

	private bool sendingPendings;

	[SerializeField]
	private NativeAndroidShareHandler shareHandler;

	private string USERS_STRING
	{
		set { PlayerPrefs.SetString(USERS_KEY, value); }
		get => PlayerPrefs.GetString(USERS_KEY, string.Empty);
	}

	private void Start()
	{
		photosByteMap = new Dictionary<string, PhotoData>();

		if (PlayerPrefs.HasKey(USERS_KEY))
			usersToSend = JsonUtility.FromJson<PendingUsersToSend>(USERS_STRING);
		else
		{
			usersToSend = new PendingUsersToSend();
			usersToSend.remainingUsers = new List<UserMailInfo>();

			OverwriteJSON();
		}
		Debug.Log(USERS_STRING);
		CheckPendingMails();
	}

	private void CheckPendingMails()
	{
		if (usersToSend.remainingUsers.Count <= 0)
		{
			sendingPendings = false;
			onUserSent = null;

			return;
		}
		sendingPendings = true;

		UserMailInfo uml = usersToSend.remainingUsers[0];
		List<Texture> textures = new List<Texture>();

		foreach (string path in uml.photoPaths)
		{
			if (File.Exists(path))
			{
				Texture2D texture = new Texture2D(1, 1);

				byte[] bytes = File.ReadAllBytes(path);
				texture.LoadImage(bytes, false);

				textures.Add(texture);
			}
		}
		onUserSent = QueuePositionSent;
		TrySendMail(textures, uml.userMail);
	}

	private void QueuePositionSent()
	{
		usersToSend.remainingUsers.RemoveAt(0);
		OverwriteJSON();

		CheckPendingMails();
	}

	private void OverwriteJSON()
	{
		USERS_STRING = JsonUtility.ToJson(usersToSend);
	}

	public void TrySendMail(List<Texture> photos, string userMail)
	{
		string[] mailSplitted = userMail.Split('@');
		string pathBase = Path.Combine(/*Application.persistentDataPath*/"/storage/emulated/0/DCIM", /*mailSplitted[0]*/string.Empty);

		if (!Directory.Exists(pathBase))
			Directory.CreateDirectory(pathBase);

		List<string> imagePaths = new List<string>();

		photosByteMap.Clear();
		for (int i = 0; i < photos.Count; i++)
		{
			string photoName = "photo_" + (i + 1) + ".jpeg";
			string path = Path.Combine(pathBase, photoName);

			byte[] bytes = ((Texture2D)photos[i]).EncodeToJPG();

			File.WriteAllBytes(path, bytes);
			if (File.Exists(path))
			{
				Debug.Log("File saved as: " + path);

				imagePaths.Add(path);
				photosByteMap.Add(photoName, new PhotoData(path, bytes));
			}
			else
				Debug.LogWarningFormat("Error saving photo at path: {0}", path);
		}
		SendMail(userMail);

		if (!string.IsNullOrEmpty(Globals.UserPhone))
			shareHandler.ShareImages(Globals.UserPhone, imagePaths);
	}

	public void TrySendWhatsapp(List<Texture> photos, string userMail)
	{
		string[] mailSplitted = userMail.Split('@');
		string pathBase = Path.Combine("/storage/emulated/0/DCIM",string.Empty);

		if (!Directory.Exists(pathBase))
			Directory.CreateDirectory(pathBase);

		List<string> imagePaths = new List<string>();

		photosByteMap.Clear();
		for (int i = 0; i < photos.Count; i++)
		{
			string photoName = "photo_" + (i + 1) + ".jpeg";
			string path = Path.Combine(pathBase, photoName);

			byte[] bytes = ((Texture2D)photos[i]).EncodeToJPG();

			File.WriteAllBytes(path, bytes);
			if (File.Exists(path))
			{
				Debug.Log("File saved as: " + path);

				imagePaths.Add(path);
				photosByteMap.Add(photoName, new PhotoData(path, bytes));
			}
			else
				Debug.LogWarningFormat("Error saving photo at path: {0}", path);
		}
		//SendMail(userMail);

		if (!string.IsNullOrEmpty(Globals.UserPhone))
			shareHandler.ShareImages(Globals.UserPhone, imagePaths);
	}

	private void SendMail(string userMail)
	{
		MailMessage mailData = new MailMessage();
		mailData.SetSubject("Comfama - La Llegada");
		mailData.SetBody("Tus fotos de Comfama");
		mailData.AddTo(userMail);

		foreach (KeyValuePair<string, PhotoData> kvp in photosByteMap)
			mailData.AddAttachment(kvp.Value.bytes, kvp.Key);

		Mail.Send(mailData, Globals.SMTP_SERVER, Globals.SMTP_PORT, Globals.MAIL, Globals.PASSWORD, true, OnSentMail);
	}

	private void OnSentMail(MailMessage mailMessage, bool success, string errorMessage)
	{
		if (success)
		{
			onUserSent?.Invoke();
			CleanSentPhotos();
		}
		else
		{
			if (!sendingPendings)
				EnqueueUserData();
		}
	}

	public void EnqueueUserData()
	{
		UserMailInfo userInfo = new UserMailInfo();
		userInfo.userMail = Globals.UserMail;
		userInfo.photoPaths = new List<string>();

		foreach (PhotoData pd in photosByteMap.Values)
			userInfo.photoPaths.Add(pd.path);

		usersToSend.remainingUsers.Add(userInfo);
		OverwriteJSON();

		Debug.LogFormat("User enqueued: {0}", JsonUtility.ToJson(userInfo));
	}

	private void CleanSentPhotos()
	{
		Debug.Log("Mail sent succesfully");
		foreach (KeyValuePair<string, PhotoData> kvp in photosByteMap)
		{
			if (File.Exists(kvp.Value.path))
				File.Delete(kvp.Value.path);
		}
		photosByteMap.Clear();
	}
}

[Serializable]
public class PhotoData
{
	public string path;
	public byte[] bytes;

	public PhotoData(string path, byte[] bytes)
	{
		this.path = path;
		this.bytes = bytes;
	}
}