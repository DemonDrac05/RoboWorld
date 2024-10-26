using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : BaseStat
{
    public static PlayerStat playerStat;
    private void Awake() => playerStat = this;

    private void Start() => health = maxHealth;
}
