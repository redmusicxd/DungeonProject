using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon;
using Photon.Pun;

/// <summary>
/// Connects to Photon Server, registers the player, and activates player UI panel when connected.
/// </summary>
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Network/Photon/RCC Photon Scene Manager")]
public class PhotonManager : Photon.Pun.MonoBehaviourPunCallbacks
{

	public Text Ping;
	public Text status;
	string PlayerName;
	public GameFlowManager_Photon gameManager;

	void Start()
	{


		ConnectToServer();

	}

	void ConnectToServer()
	{

		//print("Connecting to photon server");

		if (!Photon.Pun.PhotonNetwork.IsConnectedAndReady)
		{


			Photon.Pun.PhotonNetwork.ConnectUsingSettings();

		}

		if (Photon.Pun.PhotonNetwork.IsConnectedAndReady)
		{



		}

	}

	public override void OnConnectedToMaster()
	{

		Photon.Pun.PhotonNetwork.JoinLobby();
		status.text = "Connected to master server";



	}

	void OnGUI()
	{

		if (!Photon.Pun.PhotonNetwork.IsConnectedAndReady)
			GUI.color = Color.red;

		//("Total Player Count: " + Photon.Pun.PhotonNetwork.PlayerList.Length.ToString());
		Ping.text = "Ping: " + Photon.Pun.PhotonNetwork.GetPing().ToString();

	}

	public override void OnJoinedLobby()
	{


		Photon.Pun.PhotonNetwork.JoinRandomRoom();
	    status.text = "Joined Lobby";
		status.text = "Joined Lobby";
	}

	public override void OnJoinRandomFailed(short a, string b)
	{


		Photon.Pun.PhotonNetwork.CreateRoom(null);

		status.text = "Joining to Global room has failed!, Creating new room...";

	}

	public override void OnJoinedRoom()
	{

		gameManager.sapwPlayer();
		status.text = "Joined Room";


	}

	public void SetPlayerName(string name)
	{

		Photon.Pun.PhotonNetwork.NickName = "Player";

	}


	public void disconect()
	{

		Photon.Pun.PhotonNetwork.Disconnect();

	}

}
