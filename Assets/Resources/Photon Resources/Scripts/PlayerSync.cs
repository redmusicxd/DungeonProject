using UnityEngine;
using System.Collections;
using Photon;
using Photon.Pun;

/// <summary>
/// Streaming player input, or receiving data from server. And then feeds the RCC.
/// </summary>
[RequireComponent(typeof(PhotonView))]
public class PlayerSync : Photon.Pun.MonoBehaviourPunCallbacks, IPunObservable
{

	public bool isMine = false;

	//private Rigidbody rigid;

	private Health life;
	private PlayerCharacterController_Photon controller;
	private JetPack_Photon jet;

	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;

	private Vector3 currentVelocity;
	private float updateTime = 0;

	// lucruri pt sync
	private float currentHealth;
	private bool fx;


	void Start()
	{

		//rigid = GetComponent<Rigidbody>();
		life = GetComponent<Health>();
		jet = GetComponent<JetPack_Photon>();
		controller = GetComponent<PlayerCharacterController_Photon>();

		if (!gameObject.GetComponent<PhotonView>().ObservedComponents.Contains(this))
			gameObject.GetComponent<PhotonView>().ObservedComponents.Add(this);

		gameObject.GetComponent<PhotonView>().Synchronization = ViewSynchronization.Unreliable;

		GetValues();

		
		if (photonView.IsMine)
		{

			controller.canControl = false;
			controller.externalController = true;

		}
		else
		{

			controller.canControl = true;
			controller.externalController = false;

		}

		gameObject.name = gameObject.name + photonView.ViewID;

		//		PhotonNetwork.SendRate = 60;
		//		PhotonNetwork.SerializationRate = 60;

	}

	void GetValues()
	{

		correctPlayerPos = transform.position;
		correctPlayerRot = transform.rotation;
		currentHealth = life.currentHealth;

		//jet
		fx = jet.activeFX;




}

	void FixedUpdate()
	{

		if (!controller)
			return;

		isMine = photonView.IsMine;

		
		controller.externalController = !isMine;
		controller.canControl = isMine;


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

			
			life.currentHealth = currentHealth;
			jet.activeFX = fx;

		}

	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{

		if (!controller)
			return;

		
		if (stream.IsWriting)
		{

			stream.SendNext(life.currentHealth);

			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);

			//
			stream.SendNext(jet.m_LastTimeOfUse);
			stream.SendNext(jet.jetpackIsInUse);



			//stream.SendNext(rigid.velocity);

		}
		else
		{

			
			currentHealth = (float)stream.ReceiveNext();
		
			correctPlayerPos = (Vector3)stream.ReceiveNext();
			correctPlayerRot = (Quaternion)stream.ReceiveNext();

			//jet

			fx = (bool)stream.ReceiveNext();


			//currentVelocity = (Vector3)stream.ReceiveNext();

			updateTime = Time.time;

		}

	}

}
