using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Comfama.LaLlegada.UI
{
    public class LastWordsZone : UIZone
    {
		[SerializeField]
		private Button btnFinish;

		public override void Setup()
		{
			base.Setup();

			btnFinish.onClick.AddListener(() => ChangeZone(ZoneType.Intro));
		}

		public override ZoneType ZoneType { get => ZoneType.LastWords; }

		public override void Show()
		{
			gameObject.SetActive(true);
		}

		public override void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}

