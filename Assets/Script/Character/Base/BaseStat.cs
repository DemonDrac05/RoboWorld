using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStat : MonoBehaviour
{
    [Header("=== Basic Stat ==========")]
    public float maxHealth;
    public float health;

    public float defense;
    public float shield;

    [Header("=== Weapon Damage ==========")]
    public float rangeWeaponDamage;
    public float meleeWeaponDamage;

    [Header("=== Weapon Speed ==========")]
    public float rangeWeaponSpeed;
    public float meleeWeaponSpeed;

    private void Start() => health = maxHealth;
}
