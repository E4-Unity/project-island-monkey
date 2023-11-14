using System.Collections;
using System.Collections.Generic;
using Assets._0_IslandMonkey.Scripts.Extension;
using Assets._0_IslandMonkey.Scripts.ScriptableObject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public bool isStudy;
    public bool isLocked;
    public Image icon;
    public TextMeshProUGUI facilityName;
    public TextMeshProUGUI facilityExplanation;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI duration;
    public Image lockSlot;
    public void Start()
    {
        /*this.UpdateAsObservable().Where(_ => isLocked).DistinctUntilChanged()
            .Subscribe(_ => lockSlot.gameObject.SetActive(true));
        this.UpdateAsObservable().Where(_ => !isLocked).DistinctUntilChanged()
            .Subscribe(_ => lockSlot.gameObject.SetActive(false));*/
    }
    public void Build(BuildingPurchaseDataAsset.BuildingPurchaseData data)
    {
        //type = data.facilityType;
        isStudy= data.isStudy;
        icon.sprite = data.iconImage;
        facilityName.SetText(data.facilityName);
        facilityExplanation.SetText(data.facilityExplanation);
        cost.SetText(data.cost.FormatLargeNumber());
        duration.SetText(data.duration.FormatLargeNumber());
    }
   
    public virtual void Purchase()
    {

    }
}
