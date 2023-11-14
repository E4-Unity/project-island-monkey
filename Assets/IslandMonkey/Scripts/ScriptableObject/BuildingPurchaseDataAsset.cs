using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewBuildingPurchaseData", menuName = "IslandMonkey/BuildingPurchaseData")]
public class BuildingPurchaseDataAsset : ScriptableObject
{
    [Serializable]
    public class BuildingPurchaseData
    {
        [Header("유학 여부")]
        public bool isStudy;

        [Header("시설 아이콘")]
        public Sprite iconImage;

        [Header("시설 이름")]
        public string facilityName;

        [TextArea, Header("시설 설명")]
        public string facilityExplanation;

        [Header("구매 비용")]
        public int cost;

        [Header("소요 시간")]
        public int duration;
    }

    public List<BuildingPurchaseData> data;
}
