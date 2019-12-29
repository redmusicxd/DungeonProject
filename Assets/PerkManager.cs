using UnityEngine;
using System;
using System.Collections.Generic;
[ExecuteInEditMode]
public class PerkManager : MonoBehaviour {

    PerkAPI API;
    public PerkAPI.Powers[] Perks = new PerkAPI.Powers[0];
    public List<PerkAPI.Powers> Test = new List<PerkAPI.Powers>();
    private void Start() {
        API = GetComponent<PerkAPI>();
    }
    public void Set(string name, int parameter, bool activated, int time, bool isPermanent)
    {
        foreach(var i in Perks)
        {
            if(i.Name == name)
            {
                i.parameter = parameter;
                i.activated = activated;
                i.time = time;
                i.isPermanent = isPermanent;
            }
        }
    }
    private void Update() {
        foreach(var i in API.PowersList)
        {
            foreach(var j in Perks)
            {
                if(i.parameter != j.parameter || i.powerUpState != j.powerUpState || i.activated != j.activated || i.isPermanent != j.isPermanent)
                {
                    Array.Copy(Perks, API.PowersList, API.track);
                }
            }
        }
    }
}