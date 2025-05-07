using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System;
using MoreMountains.NiceVibrations;
using UnityEngine.Events;

public class GameScene : BaseScene
{
 
    public Text tvLevel;
    public Button settinBtn;
    public Transform canvas;

    public List<Sprite> lsSpriteFlag;
    public Image iconFlag;
 
    public void Init(LevelData levelData)
    {

        tvLevel.text = "Level " + UseProfile.CurrentLevel;
        iconFlag.sprite = lsSpriteFlag[UseProfile.CurrentLevel - 1];
    }

    public override void OnEscapeWhenStackBoxEmpty()
    {
     
    }
}
