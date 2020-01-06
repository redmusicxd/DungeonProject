using UnityEngine;
using System.Collections;
using Photon;
using Photon.Pun;


[RequireComponent(typeof(PhotonView))]
public class ProjectileSync : Photon.Pun.MonoBehaviourPunCallbacks, IPunObservable
{

	public bool isMine = false;

	private ProjectileStandard_Photon projectileSt;
	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;

	private Vector3 currentVelocity;
	private float updateTime = 0;


	private float gasInput = 0f;

	// lucruri pt sync

    public float damage = 40f;
    public float speed = 20f;
    public float maxLifeTime = 5f;
    public float radius = 0.01f;   


	void Start()
	{

		projectileSt = GetComponent<ProjectileStandard_Photon>();
        
		if (!gameObject.GetComponent<PhotonView>().ObservedComponents.Contains(this))
			gameObject.GetComponent<PhotonView>().ObservedComponents.Add(this);

		gameObject.GetComponent<PhotonView>().Synchronization = ViewSynchronization.Unreliable;
		GetValues();
		gameObject.name = gameObject.name + photonView.ViewID;

		//		PhotonNetwork.SendRate = 60;
		//		PhotonNetwork.SerializationRate = 60;

	}

	void GetValues()
	{

		correctPlayerPos = transform.position;
		correctPlayerRot = transform.rotation;

		damage = projectileSt.damage;
		speed = projectileSt.damage;
		maxLifeTime = projectileSt.damage;
		radius = projectileSt.damage;





}

	void FixedUpdate()
	{


		isMine = photonView.IsMine;

		if (!isMine)
		{

			Vector3 projectedPosition = this.correctPlayerPos + currentVelocity * (Time.time - updateTime);

			if (Vector3.Distance(transform.position, correctPlayerPos) < 15f)
			{

				transform.position = Vector3.Lerp(transform.position, projectedPosition, Time.deltaTime * 5f);
				transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5f);

			}
			else
			{

				transform.position = correctPlayerPos;
				transform.rotation = correctPlayerRot;

			}

			


			projectileSt.damage = damage;
			projectileSt.speed = speed;
			projectileSt.maxLifeTime = maxLifeTime;
			projectileSt.radius = radius;


		}

	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{

		if (stream.IsWriting)
		{

			stream.SendNext(projectileSt.damage);
			stream.SendNext(projectileSt.speed);
			stream.SendNext(projectileSt.maxLifeTime);
			stream.SendNext(projectileSt.radius);

			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);



		}
		else
		{


			damage = (float)stream.ReceiveNext();
			speed = (float)stream.ReceiveNext();
			maxLifeTime = (float)stream.ReceiveNext();
			radius = (float)stream.ReceiveNext();
		
			correctPlayerPos = (Vector3)stream.ReceiveNext();
			correctPlayerRot = (Quaternion)stream.ReceiveNext();

			//jet


			updateTime = Time.time;

		}

	}

}

