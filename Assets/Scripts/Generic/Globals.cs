using UnityEngine;

public static class Globals
{
	public static string MAIL
	{
		get => PlayerPrefs.GetString("SYSTEM_MAIL", "lallegadacomfama@outlook.com");
	}

	public static string PASSWORD
	{
		get => PlayerPrefs.GetString("MAIL_PASS", "Comfama123$87654321");
	}

	public static string SMTP_SERVER
	{
		get => PlayerPrefs.GetString("SMTP_SERVER", "smtp-mail.outlook.com");
	}

	public static int SMTP_PORT
	{
		get => PlayerPrefs.GetInt("SMTP_PORT", 587);
	}

	public static string UserMail { get; private set; }

	public static string UserPhone { get; private set; }

	public static int PhotoCounter { get; private set; }

	public static bool IsFreeMode { get; private set; }

	public static void SetMailAttributes(string mail, string pass, string server, int port)
	{
		PlayerPrefs.SetString("SYSTEM_MAIL", mail);
		PlayerPrefs.SetString("MAIL_PASS", pass);
		PlayerPrefs.SetString("SMTP_SERVER", server);
		PlayerPrefs.SetInt("SMTP_PORT", port);

		Debug.LogFormat("Mail attributes configured: {0}, {1}, {2}, {3}", mail, pass, server, port);
	}

	public static void SetUserMail(string userMail)
	{
		UserMail = userMail;
		Debug.LogFormat("User mail: {0}", UserMail);
	}

	public static void SetUserPhone(string phone)
	{
		UserPhone = phone;
		Debug.LogFormat("User phone: {0}", UserPhone);
	}

	public static void SetFreeMode(bool isFreeMode)
	{
		IsFreeMode = isFreeMode;
	}

	public static void SetPhotoCounter(int photoNumber)
	{
		PhotoCounter = photoNumber;
	}
}