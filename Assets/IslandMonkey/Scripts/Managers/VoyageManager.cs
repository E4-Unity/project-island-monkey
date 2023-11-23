using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoyageManager : MonoBehaviour
{
	public void LoadMainScene()
	{
		SceneManager.LoadScene("IngameScene");
	}
}
