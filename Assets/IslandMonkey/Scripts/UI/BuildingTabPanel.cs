using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTabPanel : TabPanel<BuildingTabPanel>
{
    public Image tabButton;
    public Sprite activeImage;
    public Sprite inActiveImage;
    public Slot slotPrefab;
    public BuildingPurchaseDataAsset purchaseData;
    public RectTransform origin;

    public void Start()
    {
        BuildItem();
    }

    private void BuildItem()
    {
      
        /*foreach (var data in purchaseData.data)
        {
            var instance = Instantiate(slotPrefab);
            instance.transform.SetParent(origin, false);
            instance.Build(data);
        }*/

       //foreach (var row in list)
       //{
       //    //row.data.Find(x=>x.key == "study");
       //    //row.data.Find(x=>x.key == "facility_name");
       //    //row.data.Find(x=>x.key == "facility_explanation");
       //    //row.data.Find(x=>x.key == "study_cost"); // cost로 묶을 필요 있음
       //}
    }

    public override void React()
    {
        if(current)
        {
            var buildingTab = (BuildingTabPanel)current;
            buildingTab.tabButton.sprite = inActiveImage;
        }
        base.React();
        tabButton.sprite= activeImage;
    }

    public void Quit()
    {
        gameObject.SetActive(false);
        tabButton.sprite = inActiveImage;
    }
}
