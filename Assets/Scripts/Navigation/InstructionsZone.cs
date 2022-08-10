using UnityEngine;
using UnityEngine.UI;

namespace Comfama.LaLlegada.UI
{
	public class InstructionsZone : UIZone
	{
		public override ZoneType ZoneType { get => ZoneType.Instructions; }

		[SerializeField]
		private Button continueButton;

		public override void Setup()
		{
			base.Setup();

			continueButton.onClick.AddListener(NextScreen);
		}

		public override void Show()
		{
			gameObject.SetActive(true);
		}

		public override void Hide()
		{
			gameObject.SetActive(false);
		}

		private void NextScreen()
		{
			ChangeZone(ZoneType.UserData);
		}
	}
}