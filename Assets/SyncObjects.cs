using UnityEngine;
using System.Collections;
using Photon;
using Photon.Pun;

/// <summary>
/// Streaming player input, or receiving data from server. And then feeds the RCC.
/// </summary>
[RequireComponent(typeof(PhotonView))]
public class SyncObjects : Photon.Pun.MonoBehaviourPunCallbacks, IPunObservable
{

	public bool isMine = false;
	public bool isPickup = false;

	//private Rigidbody rigid;

	private Pickup pickup;

	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;

	private Vector3 currentVelocity;
	private float updateTime = 0;


	private bool PickUP;



	void Start()
	{

		//rigid = GetComponent<Rigidbody>();
		pickup = GetComponent<Pickup>();

		if (!gameObject.GetComponent<PhotonView>().ObservedComponents.Contains(this))
			gameObject.GetComponent<PhotonView>().ObservedComponents.Add(this);

		gameObject.GetComponent<PhotonView>().Synchronization = ViewSynchronization.Unreliable;

		GetValues();


		if (photonView.IsMine)
		{



		}
		else
		{



		}

		gameObject.name = gameObject.name + photonView.ViewID;

		//		PhotonNetwork.SendRate = 60;
		//		PhotonNetwork.SerializationRate = 60;

	}

	void GetValues()
	{

		correctPlayerPos = transform.position;
		correctPlayerRot = transform.rotation;
		PickUP = pickup.PickUP;




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

			if (isPickup)
			{ pickup.PickUP = PickUP; }


		}

	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{



		if (stream.IsWriting)
		{


			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);

			if (isPickup)
			{ stream.SendNext(pickup.PickUP); }


		}
		else
		{


			correctPlayerPos = (Vector3)stream.ReceiveNext();
			correctPlayerRot = (Quaternion)stream.ReceiveNext();

			//jet
			if (isPickup)
			{ PickUP = (bool)stream.ReceiveNext(); }


			//currentVelocity = (Vector3)stream.ReceiveNext();

			updateTime = Time.time;

		}

	}

}
