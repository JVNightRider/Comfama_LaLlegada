using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Comfama.LaLlegada.UI
{
	public class ChromaGroup : MonoBehaviour
	{
		public event Action<bool, List<Texture>> OnSelected;

		[SerializeField]
		private List<Texture> images;

		[SerializeField]
		private Toggle toggle;

		[SerializeField]
		private Image img;

		[SerializeField]
		private Text text;

		[SerializeField]
		private Color selectedColor;

		private void Start()
		{
			toggle.onValueChanged.AddListener(ToggleChanged);
		}

		private void ToggleChanged(bool isOn)
		{
			img.color = isOn ? selectedColor : Color.white;
			text.color = isOn ? Color.white : selectedColor;

			OnSelected?.Invoke(isOn, images);
		}
	}
}