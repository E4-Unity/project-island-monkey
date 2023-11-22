using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ClamPopup : Popup
{
	[Header("UI")]
	[SerializeField] Image clamImage;
	[SerializeField] TextMeshProUGUI clamText;
	[SerializeField] Button GetButton;

	[Header("Clam Resource(낮은 등급 순)")]
	[SerializeField] Sprite[] ImageSource;
	[SerializeField] string[] GradeText;

	[Range(0,100f)]
	[SerializeField] float[] percent;

	float randNum = 0;
	int clamNum;
	Dictionary<int, float> clamPercent;

	public override void Initialize()
	{
		clamPercent = new Dictionary<int, float>();
		for(int i = 0; i < percent.Length; i++)
		{
			clamPercent.Add(i, percent[i]);
		}

		//확률을 기준으로 오름차순 정렬
		clamPercent = clamPercent.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

		float p = 0;
		List<int> keys = clamPercent.Keys.ToList<int>();
		foreach (int key in keys)
		{
			p += clamPercent[key];
			clamPercent[key] = p;
		}

		//획득하기 버튼
		GetButton.onClick.AddListener(() => VoyageUIManager.Hide());
	}

	public override void Show()
	{
		base.Show();
		clamNum = PickClam();
		if(clamNum >= 0 )
		{
			Debug.Log("난수 : " + randNum);
			clamText.text = GradeText[clamNum];
			clamImage.sprite = ImageSource[clamNum];
		}
		else
		{
			Debug.Log("잘못된 난수 :" + randNum);
		}
		
	}

	private int PickClam()
	{
		randNum = Random.Range(0, 100);
		foreach(int key in clamPercent.Keys)
		{
			if(randNum < clamPercent[key])
			{
				return key;
			}
			else
			{
			}
		}
		return -1;
	} 
}
