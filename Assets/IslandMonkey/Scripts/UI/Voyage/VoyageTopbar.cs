using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoyageTopbar : MonoBehaviour
{
	[SerializeField] Button storageButton;
    // Start is called before the first frame update
    void Awake()
    {
		storageButton.onClick.AddListener(() =>
		{
			VoyageUIManager.Show<StoragePopup>(true);
			SoundManager.Instance.PlaySoundEffect("Button_Click");
		});
    }
}
