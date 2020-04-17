using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparksTutorials;
using System;

public class SingleEnemyHandler
{
    List<string> ForEquipmentImgsLoad = new List<string> { "Head", "Chest", "Arms", "Legs", "LeftHand", "RightHand", "Feet" };
    List<string> LocalNamesMine = new List<string> { "AttackMine", "AgilityMine", "PowerMine", "StrengthMine", "EnduranceMine", "SpeedMine", "SleepMine", "RegenMine" };
    List<string> LocalNamesOther = new List<string> { "AttackOther", "AgilityOther", "PowerOther", "StrengthOther", "EnduranceOther", "SpeedOther", "SleepOther", "RegenOther" };
    List<string> ModifiersList = Equipment.EquipmentModifiers;


    List<string> NamesForEnemies = new List<string> { "Ron", "Josh", "Sam", "Chris", "TheKiller", "Dude1337", "Gangster", "IAmABot" };

    public void LoadAnEnemy() 
    {
        if (DataController.GetValue<int>("LoadMode") == 1) 
        {
            LoadEnemyBotRandom();
        }
        else if (DataController.GetValue<int>("LoadMode") == 2)
        {
            LoadEnemyBotBossNumber(DataController.GetValue<int>("CurrentBossNumber"));
        }
        else if (DataController.GetValue<int>("LoadMode") == 3)
        {
            LoadEnemyBotRandom();
        }
        else 
        {
            LoadEnemyBotRandom();
        }
    }
    void LoadEnemyBotRandom()
    {
        DataController.SaveValue("WinAnimNumberOther", UnityEngine.Random.Range(0, 3));

        DataController.SaveValue("EquippedMagicOther", ForEZEdit.MagicNames[UnityEngine.Random.Range(0, ForEZEdit.MagicNames.Count)]);
        
        DataController.SaveValue("HairStyle" + "Other", UnityEngine.Random.Range(0, HairStylesScript.HairList.Count));
        DataController.SaveValue("Beard" + "Other", UnityEngine.Random.Range(0, BeardsScript.BeardsList.Count));
        DataController.SaveValue("BodyColor" + "Other", UnityEngine.Random.Range(0, ColorsScript.ColorsList.Count));
        DataController.SaveValue("HairColor" + "Other", UnityEngine.Random.Range(0, ColorsScript.ColorsList.Count));

        DataController.SaveValue("syncedOther", (int)1);
        DataController.SaveValue("enemyName", NamesForEnemies[UnityEngine.Random.Range(0, NamesForEnemies.Count)]);
        DataController.SaveValue("LvlOther", DataController.GetValue<int>("Exp") / 100);

        int index = 0;

        int randIndexName;

        bool leftHandBusy = false;

        foreach (string EqSlot in ForEquipmentImgsLoad)
        {
            // DONT DELETE THIS COMMENTS, 
            // THEY ARE HERE IN ORDER TO EASEN 
            // THE PROCESS OF CODING WHILE SOME 
            // OF EQUIPMENT IMAGES ARE NOT IN THE GAME

            //int randIndexType = 8;

            randIndexName = 0;// UnityEngine.Random.Range(0, ForEZEdit.EquipmentNames.Count);

            if (index == 4)
            {
                index = 8;
                DataController.SaveValue("Equipped" + EqSlot + "Other", ForEZEdit.EquipmentNames[randIndexName] + Equipment.ForInvLoad[index]);
                index = 4;
            }
            else
            if (index == 5 && !leftHandBusy)
            {
                //while (randIndexType == 8)
                //{
                //    randIndexType = UnityEngine.Random.Range(5, 12);
                //}

                //if (randIndexName == 9 || randIndexName == 10 || randIndexName == 11)
                //{
                //    leftHandBusy = true;
                //}

                //DataController.SaveValue("Equipped" + EqSlot + "Other", ForEZEdit.EquipmentNames[randIndexName] + Equipment.ForInvLoad[randIndexType]);

                DataController.SaveValue("Equipped" + EqSlot + "Other", ForEZEdit.EquipmentNames[randIndexName] + "Sword");

            }
            else if (index == 6) 
            {
                index = 4;
                DataController.SaveValue("Equipped" + EqSlot + "Other", ForEZEdit.EquipmentNames[randIndexName] + Equipment.ForInvLoad[index]);
                index = 4;
            }
            else
            {
                DataController.SaveValue("Equipped" + EqSlot + "Other", ForEZEdit.EquipmentNames[randIndexName] + Equipment.ForInvLoad[index]);
            }


            Debug.Log("Item : " + ForEZEdit.EquipmentNames[randIndexName] + Equipment.ForInvLoad[index] + " , is now equipped at slot with name: " + EqSlot);

            index++;
        }

        int statsSum = 0;

        foreach (string NameMine in LocalNamesMine)
        {
            statsSum += DataController.GetValue<int>("Stats" + NameMine);
        }

        int[] StatsListRandom = RandomNumbersWithGivenSum(statsSum, 8, 0, 40);

        index = 0;

        foreach (string NameOther in LocalNamesOther)
        {
            DataController.SaveValue("Stats" + NameOther, StatsListRandom[index]);
            index++;
        }

        int[] ModifiersListRandom = RandomNumbersWithGivenSum((int)(DataController.GetValue<int>("Exp") / 600), 8, 0, 2);

        index = 0;

        foreach (string modifier in ModifiersList)
        {
            DataController.SaveValue("Total" + modifier + "Other", ModifiersListRandom[index]);
            index++;
        }
    }


    #region OtherLoad
    public List<int> BossesWinAnimList = new List<int> { 0 };

    public List<string> BossesMagicList = new List<string> { "Curse" };

    public List<int> BossesHairList = new List<int> { 1 };

    public List<int> BossesBeardList = new List<int> { 1 };

    public List<int> BossesBodyColorList = new List<int> { 2 };

    public List<int> BossesHairColorList = new List<int> { 3 };

    public List<string> BossesNamesList = new List<string> { "Ninja", "Viking", };

    public List<int> BossesLvlList = new List<int> { 5 };
    #endregion

    #region Equipment
    List<string> BossesHeadList = new List<string> { "WornOutHelmet" };

    List<string> BossesChestList = new List<string> { "WornOutBreastPlate" };

    List<string> BossesArmsList = new List<string> { "WornOutSleeves" };

    List<string> BossesLegsList = new List<string> { "WornOutPants" };

    List<string> BossesFeetList = new List<string> { "WornOutBoots" };

    List<string> BossesLeftHandList = new List<string> { "WornOutShield" };

    List<string> BossesRightHandList = new List<string> { "WornOutSword" };
    #endregion

    #region Stats
    List<int> BossesAttackList = new List<int> { 1 };

    List<int> BossesAgilityList = new List<int> { 2 };

    List<int> BossesPowerList = new List<int> { 1 };

    List<int> BossesStrengthList = new List<int> { 2 };

    List<int> BossesEnduranceList = new List<int> { 1 };

    List<int> BossesSpeedList = new List<int> { 2 };

    List<int> BossesSleepList = new List<int> { 1 };

    List<int> BossesRegenList = new List<int> { 1 };
    #endregion

    #region Modifiers
    List<int> BossesArmorModifList = new List<int> { 3 };

    List<int> BossesDamageModifList = new List<int> { 4 };

    List<int> BossesCriticalModifList = new List<int> { 3 };

    List<int> BossesBashModifList = new List<int> { 4 };

    List<int> BossesStunModifList = new List<int> { 3 };

    List<int> BossesBlockModifList = new List<int> { 4 };

    List<int> BossesMissModifList = new List<int> { 3 };

    List<int> BossesMagicModifList = new List<int> { 4 };

    #endregion

    void LoadEnemyBotBossNumber(int number)
    {
        #region OtherLoad

        DataController.SaveValue("WinAnimNumberOther", BossesWinAnimList[number]);

        DataController.SaveValue("EquippedMagicOther", BossesMagicList[number]);

        DataController.SaveValue("HairStyle" + "Other", BossesHairList[number]);
        DataController.SaveValue("Beard" + "Other", BossesBeardList[number]);
        DataController.SaveValue("BodyColor" + "Other", BossesBodyColorList[number]);
        DataController.SaveValue("HairColor" + "Other", BossesHairColorList[number]);

        DataController.SaveValue("syncedOther", (int)1);

        DataController.SaveValue("enemyName", BossesNamesList[number]);

        DataController.SaveValue("LvlOther", BossesLvlList[number]);

        #endregion


        #region Equipment

        DataController.SaveValue("Equipped" + "Head" + "Other", BossesHeadList[number]);

        DataController.SaveValue("Equipped" + "Chest" + "Other", BossesChestList[number]);

        DataController.SaveValue("Equipped" + "Arms" + "Other", BossesArmsList[number]);

        DataController.SaveValue("Equipped" + "Legs" + "Other", BossesLegsList[number]);

        DataController.SaveValue("Equipped" + "LeftHand" + "Other", BossesLeftHandList[number]);

        DataController.SaveValue("Equipped" + "RightHand" + "Other", BossesRightHandList[number]);

        DataController.SaveValue("Equipped" + "Feet" + "Other", BossesFeetList[number]);

        #endregion


        #region Stats

        DataController.SaveValue("Stats" + "AttackOther", BossesAttackList[number]);

        DataController.SaveValue("Stats" + "AgilityOther", BossesAgilityList[number]);

        DataController.SaveValue("Stats" + "PowerOther", BossesPowerList[number]);

        DataController.SaveValue("Stats" + "StrengthOther", BossesStrengthList[number]);

        DataController.SaveValue("Stats" + "EnduranceOther", BossesEnduranceList[number]);

        DataController.SaveValue("Stats" + "SpeedOther", BossesSpeedList[number]);

        DataController.SaveValue("Stats" + "SleepOther", BossesSleepList[number]);

        DataController.SaveValue("Stats" + "RegenOther", BossesRegenList[number]);

        #endregion


        #region Modifiers

        //"ModifierArmor", "ModifierDamage", "ModifierMissChance", "ModifierCriticalChance", "ModifierBashChance", "ModifierStunChance", "ModifierBlockChance", "ModifierMagic" 

        DataController.SaveValue("Total" + "ModifierArmor" + "Other", BossesArmorModifList[number]);

        DataController.SaveValue("Total" + "ModifierDamage" + "Other", BossesDamageModifList[number]);

        DataController.SaveValue("Total" + "ModifierMissChance" + "Other", BossesMissModifList[number]);

        DataController.SaveValue("Total" + "ModifierCriticalChance" + "Other", BossesCriticalModifList[number]);

        DataController.SaveValue("Total" + "ModifierBashChance" + "Other", BossesBashModifList[number]);

        DataController.SaveValue("Total" + "ModifierStunChance" + "Other", BossesStunModifList[number]);

        DataController.SaveValue("Total" + "ModifierBlockChance" + "Other", BossesBlockModifList[number]);

        DataController.SaveValue("Total" + "ModifierMagic" + "Other", BossesMagicModifList[number]);

        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sum"></param>
    /// <param name="length"> shoudl be more than two </param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public int[] RandomNumbersWithGivenSum(int sum, int length, int min, int max)
    {
        int[] numberList = new int[length];

        int currentSum = 0;

        int indexMove = UnityEngine.Random.Range(0, length); // For Better Generation. 

        int lastIndex = 0;

        int tempIndex;



        for (int i = 0;  i < length - 1;  i++) 
        {
            tempIndex = (i + indexMove) % (length - 1);

            if (sum - max > currentSum)
            {
                numberList[tempIndex] = UnityEngine.Random.Range((sum - currentSum) / (length - i), max + 1); // do 40 vsego 8 summoy 320
            }
            else 
            {
                numberList[tempIndex] = UnityEngine.Random.Range(min, sum - currentSum); // do 40 vsego 8 summoy 320
            }

            currentSum += numberList[tempIndex];

            lastIndex = tempIndex + 1;

            Debug.Log("Element is : " + numberList[tempIndex] + ", With index: " + tempIndex + ". \nTotal sum is: " + sum + ", Current sum: " + currentSum);
        }

        numberList[lastIndex] = sum - currentSum;

        Debug.Log("The last element is : " + numberList[lastIndex] + " , With index: " + (int)(lastIndex) + " \n Final total sum is: " + (int)(sum + numberList[lastIndex]));

        return numberList;
    }


    public void MakeAMoveEasy(GameProcess Me, GameProcess Enemy) 
    {
        Debug.Log("Bot Makes a move");
        if (Mathf.Abs(Me.playerObj.transform.localPosition.x - Enemy.playerObj.transform.localPosition.x) > Enemy.heavyDist)
        {
            SingleInteraction.instance.MoveCloserRightBtn();
        }
        else 
        {
            SingleInteraction.instance.LightAttackRightBtn();
        }
    }
}
