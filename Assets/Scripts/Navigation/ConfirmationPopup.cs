using System;
using UnityEngine;
using UnityEngine.UI;

namespace Comfama.LaLlegada.UI
{
	public class ConfirmationPopup : MonoBehaviour
	{
		private Action onConfirmed;

		[SerializeField]
		private Button confirmButton, cancelButton;

		[SerializeField]
		private Text confirmText;

		[SerializeField]
		private RectTransform center;

		private void Start()
		{
			confirmButton.onClick.AddListener(Confirm);
			cancelButton.onClick.AddListener(Hide);
		}

		public void ShowMailConfirmation(string mail, Action onConfirmed)
		{
			this.onConfirmed = onConfirmed;

			confirmText.text = string.Format("El correo que ingresó es <b>{0}</b>\n\n¿Es correcto?", mail);

			OnShow();
		}

		public void ShowText(string text)
		{
			onConfirmed = Hide;

			confirmText.text = text;

			OnShow();
		}

		private void OnShow()
		{
			center.localScale = Vector3.zero;
			gameObject.SetActive(true);

			LeanTween.scale(center, Vector3.one, 0.15f);
		}

		private void Confirm()
		{
			onConfirmed?.Invoke();
			Hide();
		}

		private void Hide()
		{
			LeanTween.scale(center, Vector3.zero, 0.15f).setOnComplete(() => gameObject.SetActive(false));
		}
	}
}