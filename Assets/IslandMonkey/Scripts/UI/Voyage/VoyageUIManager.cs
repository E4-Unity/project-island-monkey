using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoyageUIManager : MonoBehaviour
{
	private static VoyageUIManager s_instance;

	[SerializeField] private Popup[] popups; //팝업 목록
	[SerializeField] private GameObject cancelPanel;
	[SerializeField] private GameObject cannotcancelPanel;

	Popup currentPopup; //화면에 보여지고 있는 팝업

	public static T GetPopup<T>() where T : Popup
	{
		for (int i = 0; i < s_instance.popups.Length; i++)
		{
			if (s_instance.popups[i] is T tPopup)
			{
				return tPopup;
			}
		}
		return null;
	}

	public static void Show<T>(bool cancelAvailable) where T : Popup
	{
		for(int i = 0; i < s_instance.popups.Length; i++)
		{
			if(s_instance.popups[i] is T)
			{
				if(s_instance.currentPopup != null) //화면에 띄워진 팝업 있으면
				{
					s_instance.currentPopup.Hide();
				}

				if (cancelAvailable)
				{
					s_instance.cancelPanel.SetActive(true);
				}
				else
				{
					s_instance.cannotcancelPanel.SetActive(true);
				}
				s_instance.popups[i].Show();
				s_instance.currentPopup = s_instance.popups[i];
			}
		}
	}

	public static void Hide()
	{
		if (s_instance.currentPopup != null)
		{
			s_instance.currentPopup.Hide();
			s_instance.cancelPanel.SetActive(false);
			s_instance.cannotcancelPanel.SetActive(false);
			s_instance.currentPopup = null;
		}
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void OnEnable()
	{
		for(int i = 0; i < popups.Length; i++)
		{
			popups[i].Initialize();
			s_instance.cancelPanel.SetActive(false);
			s_instance.cannotcancelPanel.SetActive(false);
			popups[i].Hide();
		}
	}
}
