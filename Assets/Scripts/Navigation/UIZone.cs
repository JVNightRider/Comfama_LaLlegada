using System;
using UnityEngine;

namespace Comfama.LaLlegada.UI
{
	public enum ZoneType
	{
		Intro,
		Instructions,
		UserData,
		Camera,
		Final,
		LastWords
	}

	public abstract class UIZone : MonoBehaviour
	{
		public event Action<ZoneType> OnZoneChanged;

		public abstract ZoneType ZoneType { get; }

		public virtual void Setup()
		{
		}

		public abstract void Show();

		public abstract void Hide();

		protected void ChangeZone(ZoneType newZone)
		{
			OnZoneChanged?.Invoke(newZone);
		}
	}
}