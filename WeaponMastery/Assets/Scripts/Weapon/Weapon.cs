﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public ParticleSystem Flame;

    public Gradient BasicGradient;

    public StatWeaponPart StatEquipmentPart;

    public BulletWeaponPart BulletEquipmentPart;

    public ImpactWeaponPart ImpactEquipmentPart;

    public Transform FirePosition;

    private float lastFire = 0;
    private DateTime lastFireDate;

    public bool CanShoot = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    lastFire += Time.deltaTime;
	    if (Input.GetMouseButton(0) && lastFire >= StatEquipmentPart.AttackSpeed && CanShoot)
	    {
	        lastFire = 0;
	        Shoot();
	    }
	}

    private void Shoot()
    {
        Flame.gameObject.SetActive(false);
        Flame.gameObject.SetActive(true);
        var m = Flame.main;
        m.simulationSpeed = 2;
        lastFireDate = DateTime.Now;
        Invoke("ResetSimSpeed",StatEquipmentPart.AttackSpeed);
        BulletEquipmentPart.OnFire(FirePosition, transform.parent.GetComponent<WeaponMouseFacing>().Direction, StatEquipmentPart.AttackDamage, ImpactEquipmentPart, BasicGradient);
    }

    private void ResetSimSpeed()
    {
        if ((DateTime.Now - lastFireDate).Seconds <= StatEquipmentPart.AttackSpeed / 2)
            return;
           
        var m = Flame.main;
        m.simulationSpeed = 1;
    }

    public void UpdateWeapon()
    {
        Gradient gradient = BasicGradient;
        gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(BulletEquipmentPart.FlameColor, gradient.colorKeys[0].time), new GradientColorKey(ImpactEquipmentPart.FlameColor, gradient.colorKeys[1].time)}, gradient.alphaKeys);
        var colorModule = Flame.colorOverLifetime;
        colorModule.color = gradient;
        BasicGradient = gradient;
    }
}
