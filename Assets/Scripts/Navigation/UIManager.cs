using System.Collections.Generic;
using UnityEngine;

namespace Comfama.LaLlegada.UI
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField]
		private List<UIZone> zones;

		private UIZone currentZone;

		private void Awake()
		{
			zones.ForEach(ConfigureZone);
		}

		private void Start()
		{
			ChangeZone(ZoneType.Intro);
		}

		private void ConfigureZone(UIZone zone)
		{
			zone.OnZoneChanged += ChangeZone;
			zone.Setup();
		}

		private void ChangeZone(ZoneType newZone)
		{
			currentZone?.Hide();

			foreach (UIZone z in zones)
			{
				if (z.ZoneType == newZone)
				{
					currentZone = z;
					break;
				}
			}
			currentZone?.Show();
		}
	}
}