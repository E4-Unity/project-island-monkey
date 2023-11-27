using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using IslandMonkey;

public class ClamPopup : Popup
{
	[Header("UI")]
	[SerializeField] Transform clamImages;
	[SerializeField] TextMeshProUGUI clamText;
	[SerializeField] Button GetButton;

	[Header("Clam Resource(낮은 등급 순)")]
	[SerializeField] string[] GradeText;

	[Range(0,100f)]
	[SerializeField] float[] percent;

	float randNum = 0;
	int clamNum;
	List<GameObject> clams = new List<GameObject>();
	Dictionary<int, float> clamPercent;

	[SerializeField] GoodsManager goodsManager;

	public override void Initialize()
	{
		clamPercent = new Dictionary<int, float>();
		for(int i = 0; i < percent.Length; i++)
		{
			clamPercent.Add(i, percent[i]);
		}

		for (int i = 0; i < clamImages.childCount; i++)
		{
			clams.Add(clamImages.GetChild(i).gameObject);
			clams[i].SetActive(false);
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
		GetButton.onClick.AddListener(() => {
			VoyageUIManager.Hide();
			goodsManager.EarnGoods(GoodsType.Clam, clamNum*2 + 1);
		});
	}

	public override void Show()
	{
		base.Show();
		clamNum = PickClam();
		
		if (clamNum >= 0)
		{
			Debug.Log("난수 : " + randNum);
			clamText.text = GradeText[clamNum];
			
			for(int i = 0; i < clams.Count; i++)
			{
				clams[i].SetActive(false);
			}

			clams[clamNum].SetActive(true);
		}
		else
		{
			Debug.Log("Clam number :" + clamNum);
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
