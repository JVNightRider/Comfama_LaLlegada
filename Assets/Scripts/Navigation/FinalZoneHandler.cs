using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Comfama.LaLlegada.UI
{
	public class FinalZoneHandler : UIZone
	{
		public override ZoneType ZoneType { get => ZoneType.Final; }

		[SerializeField]
		private List<RawImage> portraits;

		[SerializeField]
		private MailDataHandler mailHandler;

		[SerializeField]
		private Button btnWhatsapp;

		[SerializeField]
		private Button btnFinish;

		List<Texture> textures;
		public override void Setup()
		{
			base.Setup();

			btnWhatsapp.onClick.AddListener(() => SendWhatsapp());
			btnFinish.onClick.AddListener(() => ChangeZone(ZoneType.LastWords));
		}

		public void SetImages(List<Texture> photos)
		{
			int i = 0;
			foreach (Texture t in photos)
			{
				if (i >= portraits.Count)
					break;

				portraits[i].texture = t;
				portraits[i].gameObject.SetActive(true);
				i++;
			}
			textures = photos;
			mailHandler.TrySendMail(photos, Globals.UserMail);
		}

		public void SendWhatsapp() 
		{
			mailHandler.TrySendWhatsapp(textures, Globals.UserMail);
		}

		public override void Show()
		{
			gameObject.SetActive(true);
		}

		public override void Hide()
		{
			portraits.ForEach(pt => pt.gameObject.SetActive(false));
			gameObject.SetActive(false);
		}
	}
}