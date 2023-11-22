using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CancelButton : MonoBehaviour
{
	private void Awake()
	{
		this.GetComponent<Button>().onClick.AddListener(() => CancelPopup());
	}

	private void CancelPopup()
	{

	}
}
