using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using IslandMonkey;

public class VoyageBackgroundManager : MonoBehaviour
{
	[SerializeField] List<GameObject> seaStuff;
	[SerializeField] Transform startPos;
	[SerializeField] Transform endPos;

	VoyageDataManager voyageDataManager;

	[SerializeField] float minInterval = 0.5f;
	[SerializeField] float maxInterval = 2;
	[SerializeField] float speed = 2;

	float randomInterval; //스폰 간격
	float randomX; //스폰 위치

	float distance;
	float distanceCount = 0;
	void Start()
    {
		voyageDataManager = GlobalGameManager.Instance.GetVoyageDataManager();

		distance = Mathf.Abs(startPos.position.z - endPos.position.z);

		//처음 화면 물체 배치
		while (distanceCount < distance)
		{
			SetRandom();

			GameObject stuff = Instantiate(seaStuff[Random.Range(0, seaStuff.Count)]);
			distanceCount += randomInterval * speed;
			stuff.transform.position = new Vector3(randomX, 0, startPos.position.z - distanceCount);
			
			stuff.transform.DOMoveZ(endPos.position.z, Mathf.Abs(endPos.position.z - stuff.transform.position.z) / speed);
			Destroy(stuff, Mathf.Abs(endPos.position.z - stuff.transform.position.z) / speed);
		}

		StartCoroutine(MoveBackground());
	}

	IEnumerator MoveBackground()
	{
		while(voyageDataManager.Timer > 0)
		{
			SetRandom();

			GameObject stuff = Instantiate(seaStuff[Random.Range(0, seaStuff.Count)]);

			stuff.transform.position = new Vector3(randomX, startPos.position.y, startPos.position.z);
			stuff.transform.DOMoveY(0, -startPos.position.y / speed);
			stuff.transform.DOMoveZ(endPos.position.z, (startPos.position.z - endPos.position.z) / speed);
			Destroy(stuff, -startPos.position.y / speed + (startPos.position.z - endPos.position.z) / speed);

			yield return new WaitForSeconds(randomInterval);
		}
		

	}

	private void SetRandom()
	{
		randomInterval = Random.Range(minInterval, maxInterval);

		//배 위치 제외한 랜덤 위치
		randomX = Random.Range(-5, 5);
		while (randomX > -0.7 && randomX < 0.7)
		{
			randomX = Random.Range(-5, 5);
		}
	}
}
