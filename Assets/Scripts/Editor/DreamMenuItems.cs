using UnityEditor;
using UnityEngine;

public class DreamMenuItems
{
	[MenuItem("Dream House Studios/Delete Player Prefs")]
	private static void DeletePlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
	}
}