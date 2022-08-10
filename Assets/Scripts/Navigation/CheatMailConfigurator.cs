using UnityEngine;
using UnityEngine.UI;
using UT;

namespace Comfama.LaLlegada.UI
{
	public class CheatMailConfigurator : MonoBehaviour
	{
		[SerializeField]
		private ConfirmationPopup popup;

		[SerializeField]
		private InputField inputMail, inputPass, inputServer, inputPort, targetMail;

		[SerializeField]
		private Button btnSet, btnHide, btnSendTest;

		public void Show()
		{
			inputMail.text = Globals.MAIL;
			inputPass.text = Globals.PASSWORD;
			inputServer.text = Globals.SMTP_SERVER;
			inputPort.text = Globals.SMTP_PORT.ToString();

			gameObject.SetActive(true);
		}

		private void Start()
		{
			btnSet.onClick.AddListener(SetMailAttributes);
			btnHide.onClick.AddListener(Hide);
			btnSendTest.onClick.AddListener(SendMail);

		}

		private void SetMailAttributes()
		{
			if (!ValidateFields())
			{
				popup.ShowText("No puede haber campos vacíos");
				return;
			}
			Globals.SetMailAttributes(inputMail.text.Trim(), inputPass.text.Trim(), inputServer.text.Trim(), int.Parse(inputPort.text.Trim()));
			Hide();
		}

		private bool ValidateFields()
		{
			if (string.IsNullOrEmpty(inputMail.text.Trim()) || string.IsNullOrEmpty(inputPass.text.Trim()) || string.IsNullOrEmpty(inputServer.text.Trim()) || string.IsNullOrEmpty(inputPort.text.Trim()))
				return false;

			return true;
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}

		private void SendMail()
		{
			MailMessage mailData = new MailMessage();
			mailData.SetSubject("Comfama - La Llegada");
			mailData.SetBody("Prueba de correo exitosa!");
			mailData.AddTo(targetMail.text);

			Mail.Send(mailData, Globals.SMTP_SERVER, Globals.SMTP_PORT, Globals.MAIL, Globals.PASSWORD, true, OnSentMail);
		}

		private void OnSentMail(MailMessage mailMessage, bool success, string errorMessage)
		{
			if (success)
			{
				Debug.Log("Mail sent success");
			}
			else
			{
				Debug.Log("Mail failed");

			}
		}
	}
}