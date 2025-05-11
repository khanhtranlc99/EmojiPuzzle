using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

using UnityEngine;
[CreateAssetMenu(menuName = "Datas/LevelChestData", fileName = "LevelChestData.asset")]

public class LevelChestData : ScriptableObject
{
    public List<levelChest> lsLevelChest;
    public levelChest GetLevelChest()
    {
        for (int i = 0; i < lsLevelChest.Count; i++)
        {
            if (lsLevelChest[i] == CurrentLevelChest)
            {
                if (i - 1 < 0)
                {
                    return lsLevelChest[i];
                }


                return lsLevelChest[i - 1];
            }
        }
        return null;
    }
    public levelChest levelChest;
    public levelChest CurrentLevelChest
    {
        get
        {

            if (UseProfile.LevelOfLevelChest >= lsLevelChest[lsLevelChest.Count - 1].level)
            {
                //  UseProfile.CurrentLevel = lsLevelChest[lsLevelChest.Count - 1].level;
                levelChest = lsLevelChest[lsLevelChest.Count - 1];
                return lsLevelChest[lsLevelChest.Count - 1];
            }

            for (int i = 0; i < lsLevelChest.Count; i++)
            {
                if (UseProfile.LevelOfLevelChest <= lsLevelChest[i].level)
                {
                    levelChest = lsLevelChest[i];
                    Debug.LogError("levelChest_Min");
                    return lsLevelChest[i];
                }
                if (UseProfile.LevelOfLevelChest > lsLevelChest[i].level && UseProfile.LevelOfLevelChest < lsLevelChest[i + 1].level)
                {
                    levelChest = lsLevelChest[i + 1];
                    Debug.LogError("levelChest_Orther");
                    return lsLevelChest[i + 1];
                }

            }

            return null;

        }
    }
    public int TargetChest
    {
        get
        {
            for (int i = 0; i < lsLevelChest.Count; i++)
            {
                if (lsLevelChest[i] == CurrentLevelChest)
                {
                    if (i - 1 < 0)
                    {
                        return lsLevelChest[i].level;
                    }


                    return CurrentLevelChest.level - lsLevelChest[i - 1].level;
                }
            }
            return 0;
        }
    }


    public levelChest GetLastLevelChest()
    {
        return lsLevelChest[lsLevelChest.Count - 1];
    }

    [Button]
    public void HandleFillChest()
    {

        while (GetLastLevelChest().level <= 50)
        {
            var tempData = new levelChest();
            if (GetLastLevelChest().giftType == GiftType.HintBooster)
            {
                tempData.giftType = GiftType.HintBooster;
                tempData.amount = Random.Range(1, 3);
                tempData.level = GetLastLevelChest().level + Random.Range(4,6);
            }
            //else
            //{
            //    tempData.giftType = GiftType.HintBooster;
            //    tempData.amount = Random.Range(1, 3);
            //    tempData.level = GetLastLevelChest().level + 5;
            //}
            lsLevelChest.Add(tempData);
        }

    }



}
[System.Serializable]
public class levelChest
{
    public int level;
    public GiftType giftType;
    public int amount;
}
