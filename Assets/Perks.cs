using System.Diagnostics;
using UnityEngine;

[ExecuteInEditMode]
public class Perks : MonoBehaviour
{
    [System.Serializable]
    public class Powers
    {
        public string Name;
        public float parameter;
        public float time;
        public bool isPermanent;
        public bool activated;
        public float Htime { get; set; }
    }
//public enum PowerName
//    {
//        SprintBoost,
//        Ghosting,
//        Invulnerability,
//        HealthBonus,
//        Blast,
//        Dodge,
//        DoubleDamage
//    }
//    public PowerName power;
    public Powers[] PowersList;
//    public string Description;
//    public float expireT;
//    public float SBoost;
//    public float Healthb;
    private float oldValue;
    public bool isBlast;
    public bool isDodge;
    public bool isDoubleDamage;
//   private float expire;
//   private bool activated;
    public GameObject SpecialEffect;
    public AudioClip SoundEffect;
    private PlayerCharacterController player;
    private EnemyController enemy;
    private Health health;
    public enum PowerUpState
    {
        InAttractMode,
        IsCollected,
        IsExpiring
    }
    
    public PowerUpState powerUpState;
    // Start is called before the first frame update
    void Start()
    {
        foreach(var i in PowersList)
        {
            i.Htime = i.time;
        }
       
        enemy = GameObject.FindWithTag("Enemy").GetComponent<EnemyController>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCharacterController>();
        health = GameObject.FindWithTag("Player").GetComponent<Health>();
        powerUpState = PowerUpState.InAttractMode;
        foreach(var i in PowersList)
        {
            i.Name = null;
        }
        SprintBoost(false);
        HealthBonus(false);
        Invincible(false);
        Ghosting(false);
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
        if (powerUpState == PowerUpState.IsCollected || powerUpState == PowerUpState.IsExpiring)
        {
            return;
        }
        foreach(var i in PowersList)
        {
            if(i.isPermanent)
            {
                i.time = 0;
            }
        }
        powerUpState = PowerUpState.IsCollected;
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
                SendMessage(i.Name,false);
            }
        }
    }
    public void Expiring()
    {
        foreach(var i in PowersList)
        {
            if (powerUpState == PowerUpState.IsCollected && i.time > 0 && i.activated && !i.isPermanent)
            {
                i.time -= Time.deltaTime;
            }
                // if (i.Htime / 2 >= i.time)
                // {
                //     powerUpState = PowerUpState.IsExpiring;
                // } 
               print(i.Htime);
            if(!i.activated && !i.isPermanent)
            {
                SendMessage(i.Name, true);
            }
            if(!i.activated && i.isPermanent)
            {
                SendMessage(i.Name, false);
            }
            if (i.time < 1 && !i.isPermanent)
            {
                SendMessage(i.Name, true);
             //   powerUpState = PowerUpState.IsExpiring;
            }
        }
    }


    public void SprintBoost(bool expired)
    {
        if(powerUpState == PowerUpState.InAttractMode)
        {
            oldValue = player.sprintSpeedModifier;
            foreach (var i in PowersList)
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
            }
        }
        if(powerUpState == PowerUpState.IsCollected && !expired)
        {
            foreach(var i in PowersList)
            {
                if(i.Name == GetCurrentMethod())
                {
                    player.sprintSpeedModifier = i.parameter;
                }

            }

        }
        if(expired)
        {
            player.sprintSpeedModifier = oldValue;
        }
    }
    public void HealthBonus(bool expired)
    {
        if (powerUpState == PowerUpState.InAttractMode)
        {
            foreach (var i in PowersList)
            {
                
                if (i.Name == GetCurrentMethod())
                {
                    i.isPermanent = true;
                    break;
                }
                if (i.Name == null)
                {
                    i.Name = GetCurrentMethod();
                    break;
                }
            }
        }
        if(powerUpState == PowerUpState.IsCollected && !expired)
        {
            foreach(var i in PowersList)
            {
                if (i.Name == GetCurrentMethod())
                {
                    health.maxHealth += i.parameter;
                    i.isPermanent = true;
                }
            }
        }

       if(expired)
       {
           return;
       }
    }

    public void Ghosting(bool expired)
    {
        if (powerUpState == PowerUpState.InAttractMode)
        {
            foreach (var i in PowersList)
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
            }
        }
        if (powerUpState == PowerUpState.IsCollected && !expired)
        {
            enemy.ghost = true;
        }
        if(expired)
        {
            enemy.ghost = false;
        }
    }
    public void Invincible(bool expired)
    {
        if (powerUpState == PowerUpState.InAttractMode)
        {
            foreach (var i in PowersList)
            {
                if(i.Name == GetCurrentMethod())
                {
                    break;
                }
                if (i.Name == null)
                {
                    i.Name = GetCurrentMethod();
                    break;
                }
            }
        }
        if(powerUpState == PowerUpState.IsCollected && !expired)
        {
            health.invincible = true;
        }

        if(expired)
        {
            health.invincible = false;
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
        if(powerUpState == PowerUpState.IsCollected)
        {
            Expiring();
        }

    }
}
