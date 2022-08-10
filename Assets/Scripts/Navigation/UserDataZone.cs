using UnityEngine;
using UnityEngine.UI;

namespace Comfama.LaLlegada.UI
{
	public class UserDataZone : UIZone
	{
		public override ZoneType ZoneType { get => ZoneType.UserData; }

		[SerializeField]
		private ConfirmationPopup confirmationPopup;

		[SerializeField]
		public Button cancelButton, continueButton;

		[SerializeField]
		private Toggle phoneToggle, mailToggle;

		[SerializeField]
		private InputField phoneInput, mailInput;

		public override void Setup()
		{
			base.Setup();

			continueButton.onClick.AddListener(TryConfirm);
			cancelButton.onClick.AddListener(ReturnZone);

			phoneToggle.onValueChanged.AddListener(ChangePhoneState);
			mailToggle.onValueChanged.AddListener(ChangeMailState);

			phoneInput.onValueChanged.AddListener((str) => ValidateButtonState());
			mailInput.onValueChanged.AddListener((str) => ValidateButtonState());
		}

		public override void Show()
		{
			Globals.SetUserMail(string.Empty);
			Globals.SetUserPhone(string.Empty);

			gameObject.SetActive(true);
		}

		public override void Hide()
		{
			mailToggle.isOn = false;
			phoneToggle.isOn = false;

			mailInput.text = string.Empty;
			phoneInput.text = string.Empty;

			gameObject.SetActive(false);
		}

		private void ChangePhoneState(bool enabled)
		{
			phoneInput.interactable = enabled;
			ValidateButtonState();
		}

		private void ChangeMailState(bool enabled)
		{
			mailInput.interactable = enabled;
			ValidateButtonState();
		}

		private void ValidateButtonState()
		{
			if (!mailToggle.isOn && !phoneToggle.isOn)
			{
				continueButton.interactable = false;
				return;
			}
			if (phoneToggle.isOn && phoneInput.text.Length < 10)
			{
				continueButton.interactable = false;
				return;
			}
			//if (!mailToggle.isOn)
			//{
			//	continueButton.interactable = false;
			//	return;
			//}
			if (mailToggle.isOn && !IsEmailValid())
			{
				continueButton.interactable = false;
				return;
			}
			continueButton.interactable = true;
		}

		private bool IsEmailValid()
		{
			bool isValid = false;
			try
			{
				System.Net.Mail.MailAddress adrres = new System.Net.Mail.MailAddress(mailInput.text);
				isValid = adrres.Address == mailInput.text;
			}
			catch (System.Exception) { isValid = false; }

			return isValid;
		}

		private void TryConfirm()
		{
			confirmationPopup.ShowMailConfirmation(mailInput.text, NextZone);
		}

		private void NextZone()
		{
			if (phoneToggle.isOn && !string.IsNullOrEmpty(phoneInput.text))
				Globals.SetUserPhone(phoneInput.text);

			Globals.SetUserMail(mailInput.text);

			ChangeZone(ZoneType.Camera);
		}

		private void ReturnZone()
		{
			ChangeZone(ZoneType.Intro);
		}
	}
}