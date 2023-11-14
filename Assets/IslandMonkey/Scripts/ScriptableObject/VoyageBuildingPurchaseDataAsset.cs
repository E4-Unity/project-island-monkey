using System.Collections.Generic;
using UnityEngine;

namespace Assets._0_IslandMonkey.Scripts.ScriptableObject
{
    [CreateAssetMenu(fileName = "NewVoyageBuildingPurchaseData", menuName = "IslandMonkey/VoyageBuildingPurchaseData")]
    public class VoyageBuildingPurchaseDataAsset : BuildingPurchaseDataAsset
    {
        /*public override List<BuildingPurchaseData> GetData()
        {
            List<BuildingPurchaseData> d = new List<BuildingPurchaseData>();
            foreach (var v in data)
            {
                d.Add((BuildingPurchaseData)v);
            }
            return d;
        }
        */

    }
}
