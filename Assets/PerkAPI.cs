using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PerkAPI : MonoBehaviour
{
    [System.Serializable]
    public class Powers
    {
        public string Name;
        public float parameter;
        public float time;
        public bool isPermanent;
        public bool activated;
        public enum PowerUpState
        {
            InAttractMode,
            IsCollected,
            IsExpiring
        }
        public PowerUpState powerUpState;
        public float Htime { get; set; }
    }
    public Powers[] PowersList;
    public List<Powers> testing = new List<Powers>(){new Powers{Name = ""}};
//    public string Description;

    public int ListSize{
        get{
            return PowersList.Length;
        }
        set{
            PowersList = new Powers[value];
        }
    }
    public float oldValue{get; set;}
    public int track{get; set;}
    public GameObject SpecialEffect;
    public AudioClip SoundEffect;
    public PlayerCharacterController player {get; set;}
    public EnemyController enemy{get; set;}
    public Health p_health{get; set;}
    public Health e_health {get; set;}
    public Explosion explosion {get; set;}
    public PlayerDodge playerDodge {get; set;}
    public PerkManager perkman {get; set;}

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindWithTag("Enemy").GetComponent<EnemyController>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCharacterController>();
        p_health = GameObject.FindWithTag("Player").GetComponent<Health>();
        e_health = GameObject.FindWithTag("Enemy").GetComponent<Health>();
        explosion = GetComponent<Explosion>();
        explosion.bomb = GameObject.FindWithTag("Player");
        playerDodge = GameObject.FindWithTag("Player").GetComponent<PlayerDodge>();
        oldValue = player.sprintSpeedModifier;
        perkman = GetComponent<PerkManager>();
        playerDodge.enabled = false;
        foreach(var i in PowersList)
        {
            i.powerUpState = Powers.PowerUpState.InAttractMode;
        }
        if(ListSize != 0)
        {
        AllPowers(new object[4]{false, false, true,false});
        }
        if(perkman.Perks.Length == 0 || perkman.Perks.Length < track)
        {
            perkman.Perks = new Powers[track];
            Array.Copy(PowersList, perkman.Perks, track);
        }
        
    }
    private void Awake() {
        AllPowers(new object[]{false, true,false,false});
        ListSize = track;
    }
    public void AllPowers(object[] call) {
        //ThePerk instance = GetComponent<ThePerk>();
        var methods = //instance
        GetType()
        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
        .Where(item => item.Name.StartsWith("d"));

        foreach (var method in methods)
            method.Invoke(this, call);
    }
    public void OnePower(object[] call, string name) {
        //ThePerk instance = GetComponent<ThePerk>();
        var methods = //instance
        GetType()
        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
        .Where(item => item.Name == name);

        foreach (var method in methods)
            method.Invoke(this, call);
        }  
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PowerUpCollected();
        }
    }
    public void PowerUpCollected()
    {
        foreach(var i in PowersList)
        {
            if(i.isPermanent)
            {
                i.time = 0;
            }
            if (i.powerUpState == Powers.PowerUpState.IsCollected || i.powerUpState == Powers.PowerUpState.IsExpiring)
            {
                return;
            }
            if(i.activated)
            {
            i.powerUpState = Powers.PowerUpState.IsCollected;
            }
        }
        PowerUpPayload();
        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, -3, Time.deltaTime), transform.localPosition.z);
    }

    public void PowerUpPayload()
    {
        foreach(var i in PowersList)
        {
            if (i.time == 0)
            {
                Expiring();
            }
        }
        foreach(var i in PowersList)
        {
            if(i.activated && i.Name != null)
            {
                OnePower(new object[4]{false, false, false,true},i.Name);
            }
        }
    }

    public void Expiring()
    {
        foreach(var i in PowersList)
        {
           // if (i.powerUpState == Powers.PowerUpState.IsCollected && i.time > 0 && i.activated && !i.isPermanent)
           // {
           //     i.time -= Time.deltaTime;
           // }
            if(!i.activated && !i.isPermanent)
            {
                OnePower(new object[4]{true, false, false,false},i.Name);
            }
            if(!i.activated && i.isPermanent)
            {
                OnePower(new object[4]{false, false, false,true},i.Name);
            }
            if (i.time < 1 && !i.isPermanent)
            {
                OnePower(new object[4]{true, false, false,false},i.Name);
                i.powerUpState = Powers.PowerUpState.IsExpiring;
            }
        }
    }

    public void dSprintBoost(bool expired, bool trackit, bool add, bool activate)
    {
        if(trackit){
        track++;
       // ListSize = track;
        }

       if (expired)
       {
           player.sprintSpeedModifier = oldValue;
       }
        foreach (var i in PowersList)
        {
            if(add == true){
                if (i.powerUpState == Powers.PowerUpState.InAttractMode)
                {
                    if (i.Name == GetCurrentMethod())
                    {
                        break;
                    }
                    if (i.Name == null)
                    {
                        i.Name = GetCurrentMethod();
                        i.parameter = 3f;
                        break;
                    }
                    print("Speed Perk added");
                }
            }
            if(activate){
                if (i.powerUpState == Powers.PowerUpState.IsCollected && !expired)
                {
                    if (i.Name == GetCurrentMethod())
                    {
                        player.sprintSpeedModifier = i.parameter;
                    }
                }                
            }
        }
    }

    public void dHealthBonus(bool expired, bool trackit, bool add, bool activate)
    {
        if(trackit){
        track++;
       // ListSize = track;    
        }

        foreach (var i in PowersList)
        {
            if(add){
                if (i.powerUpState == Powers.PowerUpState.InAttractMode)
                {
                    if (i.Name == GetCurrentMethod())
                    {
                        i.isPermanent = true;
                        break;
                    }
                    if (i.Name == null)
                    {
                        i.Name = GetCurrentMethod();
                        i.parameter = 50;
                        break;
                    }
                    print("Health Bonus Added");
                }
            }
            if(activate)
            {
                if (i.powerUpState == Powers.PowerUpState.IsCollected && !expired && activate)
                {
                    if (i.Name == GetCurrentMethod())
                    {
                        p_health.maxHealth += i.parameter;
                        i.isPermanent = true;
                    }
                }
            }
        }
       if(expired)
       {
           return;
       }
    }
    public void dDoubleDamage(bool expired, bool trackit, bool add, bool activate)
    {
        if(trackit){
            track++;
           // ListSize = track;    
        }

        foreach (var i in PowersList)
        {
            if(add)
            {
                if (i.powerUpState == Powers.PowerUpState.InAttractMode)
                {
                    if (i.Name == GetCurrentMethod())
                    {
                        i.isPermanent = true;
                        break;
                    }
                    if (i.Name == null)
                    {
                        i.Name = GetCurrentMethod();
                        i.parameter = 50;
                        break;
                    }
                    print("Double Damage Added");
                }
            }
            if(activate)
            {
                if (i.powerUpState == Powers.PowerUpState.IsCollected && !expired)
                {
                    if (i.Name == GetCurrentMethod())
                    {
                        if(p_health.isCritical()){
                            e_health.doubled = true;    
                        }
                        
                    }
                }                
            }

        }
       if(expired)
       {
           e_health.doubled = false;
       }
    }
    public void dGhosting(bool expired, bool trackit, bool add, bool activate)
    {
        if(trackit){
        track++;
        //ListSize = track;
        }

        foreach (var i in PowersList)
        {
            if(add){
                if (i.powerUpState == Powers.PowerUpState.InAttractMode)
                {
                    if (i.Name == GetCurrentMethod())
                    {
                        break;
                    }
                    if (i.Name == null)
                    {
                        i.Name = GetCurrentMethod();
                        break;
                    }
                    print("Ghosting Added");
                }
            }
            if(activate)
            {
                if (i.powerUpState == Powers.PowerUpState.IsCollected && !expired)
                {
                    enemy.ghost = true;
                }
                if (expired)
                {
                    enemy.ghost = false;
                }
            }
        }
    }

    public void dInvincible(bool expired, bool trackit, bool add, bool activate)
    {
        if(trackit){
        track++;
       // ListSize = track;    
        }

        foreach (var i in PowersList)
        {
            if(add){
                if (i.powerUpState == Powers.PowerUpState.InAttractMode)
                {
                    if (i.Name == GetCurrentMethod())
                    {
                        break;
                    }
                    if (i.Name == null)
                    {
                        i.Name = GetCurrentMethod();
                        break;
                    }
                    print("Invincible added");
                }
            }
            if(activate)
            {
                if (i.powerUpState == Powers.PowerUpState.IsCollected && !expired)
                {
                    p_health.invincible = true;
                }
                if (expired)
                {
                    p_health.invincible = false;
                }
            }
        }
    }
    public void dPush(bool expired, bool trackit, bool add, bool activate)
    {
        if(trackit){
        track++;
       // ListSize = track;    
        }

        foreach (var i in PowersList)
        {
            if(add){
                if (i.powerUpState == Powers.PowerUpState.InAttractMode)
                {
                    if (i.Name == GetCurrentMethod())
                    {
                        break;
                    }
                    if (i.Name == null)
                    {
                        i.Name = GetCurrentMethod();
                        i.parameter = 10;
                        break;
                    }
                    print("Push Added");
                }
            }
            if(activate)
            {
                if (i.powerUpState == Powers.PowerUpState.IsCollected && !expired)
                {
                    explosion.active = true;
                    explosion.power = i.parameter;
                }
                if (expired)
                {
                    explosion.active = false;
                }
            }
        }
    } 
    public void dDash(bool expired, bool trackit, bool add, bool activate)
    {
        if(trackit){
        track++;
        
       // ListSize = track;    
        }

        foreach (var i in PowersList)
        {
            if(add){
                if (i.powerUpState == Powers.PowerUpState.InAttractMode)
                {
                    if (i.Name == GetCurrentMethod())
                    {
                        break;
                    }
                    if (i.Name == null)
                    {
                        i.Name = GetCurrentMethod();
                        print("Dash added");
                        break;
                    }
                }
            }
            if(activate)
            {
                if (i.powerUpState == Powers.PowerUpState.IsCollected && !expired)
                {
                   // playerDodge.enabled = true;
                   // playerDodge.Active = true;
                   player.dashActive = true;
                }
                if (expired)
                {
                    //playerDodge.Active = false;
                    //playerDodge.enabled = false;
                    player.dashActive = false;
                }
            }
        }
    }

    public string GetCurrentMethod()
    {
        var st = new StackTrace();
        var sf = st.GetFrame(1);

        return sf.GetMethod().Name;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var i in PowersList)
        {
            if(i.powerUpState == Powers.PowerUpState.IsCollected && i.time > 0 && i.activated && !i.isPermanent)
            {
                i.time -= Time.deltaTime;
               // if (i.Htime / 2 >= i.time)
               // {
               //     i.powerUpState = Powers.PowerUpState.IsExpiring;
               // }
                if (i.time <=0 && !i.isPermanent)
                {
                    OnePower(new object[4]{true, false, false,false},i.Name);
                    i.powerUpState = Powers.PowerUpState.IsExpiring;
                }
            }
           //if(i.Name == null)
           //{
           //    AllPowers(new object[4]{false, false, true,false});
           //}
            if(ListSize > track)
            {
                ListSize = track;
            }
            if(i.powerUpState == Powers.PowerUpState.IsCollected && i.time == i.Htime && i.activated && !i.isPermanent)
            {
                OnePower(new object[4]{false, false, false,true},i.Name);
            }
        }
    }
}
