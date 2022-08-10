using System;
using UnityEngine;
using UnityEngine.UI;

public class TimerHandler : MonoBehaviour
{
	[SerializeField]
	private Text timerText;

	[SerializeField]
	private Image fillAmount;

	private float time;

	public void StartTimer(float time, Action onCompleted)
	{
		onCompleted += Stop;
		this.time = time;

		gameObject.SetActive(true);
		LeanTween.value(gameObject, OnUpdateTimer, 0, 1, time).setOnComplete(onCompleted);
	}

	private void OnUpdateTimer(float value)
	{
		fillAmount.fillAmount = value;

		float val = value * time;
		timerText.text = ((int)val + 1).ToString();
	}

	public void Stop()
	{
		if (LeanTween.isTweening(gameObject))
			LeanTween.cancel(gameObject);

		gameObject.SetActive(false);
	}
}