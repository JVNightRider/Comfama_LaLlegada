using UnityEngine;
using UnityEngine.UI;

namespace Comfama.LaLlegada.UI
{
	public class IntroZone : UIZone
	{
		public override ZoneType ZoneType { get => ZoneType.Intro; }

		[SerializeField]
		private Button continueButton, limitedMode, freeMode;

		[SerializeField]
		private GameObject welcomePanel, selectModePanel;

		[Header("Mail Configurer")]
		[SerializeField]
		private Button btnConfigureMail;

		[SerializeField]
		private Button btnSetMailPass;

		[SerializeField]
		private InputField inputMailPass;

		[SerializeField]
		private CheatMailConfigurator mailConfigurator;

		[SerializeField]
		private string mailPass;

		public override void Setup()
		{
			base.Setup();

			continueButton.onClick.AddListener(SkipRegard);
			limitedMode.onClick.AddListener(LimitMode);
			freeMode.onClick.AddListener(FreeMode);

			btnConfigureMail.onClick.AddListener(ShowInputMail);
			btnSetMailPass.onClick.AddListener(ValidateMailPass);
		}

		public override void Show()
		{
			welcomePanel.SetActive(true);
			selectModePanel.SetActive(false);

			mailConfigurator.Hide();
			ResetMailFields();

			gameObject.SetActive(true);
		}

		private void ShowInputMail()
		{
			btnConfigureMail.gameObject.SetActive(false);
			inputMailPass.gameObject.SetActive(true);
		}

		private void ValidateMailPass()
		{
			if (inputMailPass.text.Trim().Equals(mailPass))
				mailConfigurator.Show();

			ResetMailFields();
		}

		private void ResetMailFields()
		{
			inputMailPass.gameObject.SetActive(false);
			btnConfigureMail.gameObject.SetActive(true);

			inputMailPass.text = string.Empty;
		}

		public override void Hide()
		{
			gameObject.SetActive(false);
		}

		private void SkipRegard()
		{
			welcomePanel.SetActive(false);
			selectModePanel.SetActive(true);
		}

		private void FreeMode()
		{
			Globals.SetFreeMode(true);
			ChangeZone(ZoneType.Instructions);
		}

		private void LimitMode()
		{
			Globals.SetFreeMode(false);
			ChangeZone(ZoneType.Instructions);
		}
	}
}