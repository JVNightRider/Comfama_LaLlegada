using System;
using UnityEngine;

public class TakePhotoHandler : MonoBehaviour
{
	public void TakePhoto(Action<Texture, bool> onPhotoTaken)
	{
		Texture texture = null;
		NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
		{
			if (!string.IsNullOrEmpty(path))
				texture = NativeCamera.LoadImageAtPath(path, -1, false);

			onPhotoTaken?.Invoke(texture, !string.IsNullOrEmpty(path));
		});
		//Debug.Log("Permission result: " + permission);
	}
}