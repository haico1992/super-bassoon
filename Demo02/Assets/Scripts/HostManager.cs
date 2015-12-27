using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HostManager : MonoBehaviour {

	public int listenPort=4444;
	public ClientManager manager;
	public NetworkDiscovery netDiscover;
	private NetworkClient myClient;
	// Use this for initialization
	void Start () {
		SetupServer ();
	}

	public void SetupServer()
	{
		NetworkServer.Listen(listenPort);
		NetworkServer.RegisterHandler(MsgType.Connect, OnConnected);   
		NetworkServer.RegisterHandler((int)Define.CustomMsgType.Rotation, OnReceivedRotation);   
		NetworkServer.RegisterHandler((int)Define.CustomMsgType.Calibrate, OnCalibrate);   
		netDiscover.broadcastData = Network.player.ipAddress + ":" + listenPort;
		Debug.Log ("listing on "+listenPort);
	}

	void OnPlayerConnected(NetworkPlayer player) {
		Debug.Log(" connected from " + player.ipAddress + ":" + player.port);
	}

	void OnReceivedRotation( NetworkMessage msg){
		DeviceRotaion rotation = msg.ReadMessage<DeviceRotaion>();
		manager.deviceRotation = rotation.rotation;
	}

	public void  OnConnected(NetworkMessage msg){
	}

	public void  OnCalibrate(NetworkMessage msg){
		Quaternion rotation = msg.ReadMessage<DeviceRotaion> ().rotation;
		manager.GetRootPosition (rotation);
	}

}
