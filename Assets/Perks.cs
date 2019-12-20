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
//    public string Description;
    private float oldValue;
    public bool isDodge;
    public bool isDoubleDamage;
    public GameObject SpecialEffect;
    public AudioClip SoundEffect;
    private PlayerCharacterController player;
    private EnemyController enemy;
    private Health health;
    private Explosion explosion;
    
    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindWithTag("Enemy").GetComponent<EnemyController>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCharacterController>();
        health = GameObject.FindWithTag("Player").GetComponent<Health>();
        explosion = GetComponent<Explosion>();
        explosion.bomb = GameObject.FindWithTag("Player");
        foreach (var i in PowersList)
        {
            i.Htime = i.time;
        }
        foreach (var i in PowersList)
        {
            i.powerUpState = Powers.PowerUpState.InAttractMode;
        }
        foreach(var i in PowersList)
        {
            i.Name = null;
        }
        oldValue = player.sprintSpeedModifier;
        SprintBoost(false);
        HealthBonus(false);
        Invincible(false);
        Ghosting(false);
        Push(false);
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
                SendMessage(i.Name,false);
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
           // if (i.Htime / 2 >= i.time)
           // {
           //     i.powerUpState = Powers.PowerUpState.IsExpiring;
           // } 
           //print(i.Htime);
           //print(i.Name);
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
        if (expired)
        {
            player.sprintSpeedModifier = oldValue;
        }
        foreach (var i in PowersList)
        {
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
            }
            if (i.powerUpState == Powers.PowerUpState.IsCollected && !expired)
            {
                if (i.Name == GetCurrentMethod())
                {
                    player.sprintSpeedModifier = i.parameter;
                }
            }
        }
    }

    public void HealthBonus(bool expired)
    {
        foreach (var i in PowersList)
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
                    break;
                }
            }
            if (i.powerUpState == Powers.PowerUpState.IsCollected && !expired)
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
        foreach (var i in PowersList)
        {
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
            }
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

    public void Invincible(bool expired)
    {
        foreach (var i in PowersList)
        {
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
            }
            if (i.powerUpState == Powers.PowerUpState.IsCollected && !expired)
            {
                health.invincible = true;
            }
            if (expired)
            {
                health.invincible = false;
            }
        }
    }
    public void Push(bool expired)
    {
        foreach (var i in PowersList)
        {
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
            }
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

    void PushPhysics()
    {
        // Bit shift the index of the Enemy layer to get a bit mask, this will affect only colliders in Enemy layer
        int enemyLayerIndex = LayerMask.NameToLayer("Enemy");
        int layerMask = 1 << enemyLayerIndex;

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, 5f, layerMask);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                AddExplosionForce(rb, 10f, explosionPos, 5f);
            }
        }
    }

    public string GetCurrentMethod()
    {
        var st = new StackTrace();
        var sf = st.GetFrame(1);

        return sf.GetMethod().Name;
    }

    public static void AddExplosionForce(Rigidbody body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        body.AddForce(dir.normalized * explosionForce * wearoff);
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var i in PowersList)
        {
            if(i.powerUpState == Powers.PowerUpState.IsCollected && i.time > 0 && i.activated && !i.isPermanent)
            {
                i.time -= Time.deltaTime;
                if (i.time < 1 && !i.isPermanent)
                {
                    SendMessage(i.Name, true);
                    //   powerUpState = PowerUpState.IsExpiring;
                }
            
            }
        }
    }
}
