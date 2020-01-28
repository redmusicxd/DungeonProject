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
	public GameObject characterPrefab;
	public PlayerCharacterController_Photon controller;
	public JetPack_Photon jet;

	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;

	private Vector3 currentVelocity;
	private float updateTime = 0;





	private bool jetpackIsInUse;



	void Start()
	{

		controller = GetComponent<PlayerCharacterController_Photon>();
		if (!gameObject.GetComponent<PhotonView>().ObservedComponents.Contains(this))
			gameObject.GetComponent<PhotonView>().ObservedComponents.Add(this);

		gameObject.GetComponent<PhotonView>().Synchronization = ViewSynchronization.Unreliable;

		GetValues();
		
		if (photonView.IsMine)
		{
			controller.canControl = false;
			controller.externalController = true;
		//	characterPrefab.SetActive(false);
		}
		else
		{
			controller.canControl = true;
			controller.externalController = false;
		//	characterPrefab.SetActive(true);
		}
		gameObject.name = gameObject.name + photonView.ViewID;

		//		PhotonNetwork.SendRate = 60;
		//		PhotonNetwork.SerializationRate = 60;

	}




	void GetValues()
	{
		correctPlayerPos = transform.position;
		correctPlayerRot = transform.rotation;
		jetpackIsInUse = jet.jetpackIsInUse;
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
			    jet.jetpackIsInUse = jetpackIsInUse;

			}


		}

	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{

		if (!controller)
			return;

		
		if (stream.IsWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);

			stream.SendNext(jet.jetpackIsInUse);

		}
		else
		{
			correctPlayerPos = (Vector3)stream.ReceiveNext();
			correctPlayerRot = (Quaternion)stream.ReceiveNext();


			jetpackIsInUse = (bool)stream.ReceiveNext();

			updateTime = Time.time;

		}

	}

}
