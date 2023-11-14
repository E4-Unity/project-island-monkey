using UnityEngine;
using System.Collections.Generic;

public class ExpansionManager : MonoBehaviour
{
    private int currentExpansion = 0; // 확장 상태 (0: 5개, 1: 16개, 2: 29개)
    private int[] expansionLimits = { 7, 18, 31 };
    private List<bool> buildingArea = new List<bool>();

    private void Start()
    {
        // 초기 상태 설정
        for (int i = 0; i < 31; i++)
        {
            buildingArea.Add(false);
        }
    }

    public void ExpandTerritory()
    {
        if (currentExpansion < 2)
        {
            currentExpansion++;
            Debug.Log($"영토가 확장되었습니다. 현재 구역 수: {expansionLimits[currentExpansion]}개");
        }
        else
        {
            Debug.Log("더 이상 확장할 수 없습니다.");
        }
    }

    public void InstallBuilding()
    {
        int start = 0;
        int end = expansionLimits[currentExpansion];

        List<int> availableSlots = new List<int>();
        for (int i = start; i < end; i++)
        {
            if (!buildingArea[i])
            {
                availableSlots.Add(i);
            }
        }

        if (availableSlots.Count == 0)
        {
            Debug.Log("현재 확장 상태에서 사용 가능한 슬롯이 없습니다. 영토를 확장하세요.");
            return;
        }

        int randomSlot = availableSlots[Random.Range(0, availableSlots.Count)];
        buildingArea[randomSlot] = true;
        Debug.Log($"건물이 {randomSlot} 번 슬롯에 설치되었습니다.");
    }
}