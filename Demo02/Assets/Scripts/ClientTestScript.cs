using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ClientTestScript : MonoBehaviour {


	NetworkClient myClient;
	public int port;
	public UILabel label;
	public Transform hostItemPrefab;
	public CustomNetworkDiscovery netDiscovery;
	public UIGrid grid;
	private int MessageChannel;
	DeviceTransform msg = new DeviceTransform ();
	private Dictionary<string,int> hostList =  new Dictionary<string, int>();
	string hostAddress;
	bool isStart=false;
	// Use this for initialization
	void Start () {
		netDiscovery.OnReceivedBoardCastCallback = OnReceiveServer;

		Input.gyro.enabled = true;
		Input.gyro.updateInterval = 0.006f; //(333Hz)
		netDiscovery.Initialize ();
		netDiscovery.StartAsClient();
	}
	bool needRefresh=true;
	void OnClickRefresh(){
		needRefresh=true;
	}
	// Update is called once per frame
	int counter;
	void FixedUpdate () {
		

		if (isStart) {
			msg.rotation = Input.gyro.attitude;
			msg.acceleration = Input.acceleration;
			msg.mess = (counter++).ToString ();
			myClient.Send ((int)Define.CustomMsgType.Transform, msg);
		}
	}

	// Create a client and connect to the server port
	public void SetupClient(string add, int port)
	{
		myClient = new NetworkClient();
		myClient.RegisterHandler(MsgType.Connect, OnConnected);     
		//myClient.RegisterHandler((, null);

		myClient.Connect(add, port);
	}

	public void  OnConnected(NetworkMessage msg){
		label.text+= ("\nConnected, " + msg);
	}

	static byte[] GetBytes(string str)
	{
		byte[] bytes = new byte[str.Length * sizeof(char)];
		System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
		return bytes;
	}

	public void OnClickCalibrate(){
		isStart = true;
		DeviceTransform msg = new DeviceTransform ();
		msg.rotation = Input.gyro.attitude;
		myClient.Send ((int)Define.CustomMsgType.Calibrate, msg);

	}

	public void OnReceiveServer(string fromAddress, string data)
	{
		if (needRefresh) {
			foreach (Transform item  in  grid.GetChildList()) {
				Destroy (item.gameObject);
			}
			hostList.Clear ();
			needRefresh = false;
		}
		var items = data.Split(':');
		if (items.Length == 2)
		{
			int port= System.Convert.ToInt32(items[1].ToString());
			string address = items [0];
			if (!hostList.ContainsKey (address)) {
				hostList.Add (address, port);
				AddHostItem (address, port);
			}
		}

	}

	void AddHostItem(string address, int port){
		Transform item = Instantiate (hostItemPrefab) as Transform;
	
		EventDelegate a = new EventDelegate (this,"SetupClient");
		a.parameters [0].value = address;
		a.parameters [1].value = port;
		item.GetComponent<UIButton> ().onClick.Add (a);
		item.GetChild(0).GetComponent<UILabel> ().text = address + " : " + port;
		grid.AddChild (item);
		item.localScale = Vector3.one;
	}

}



public class DeviceRotaion : MessageBase
{
	public Quaternion rotation;
	public string mess;
}

public class DeviceTransform : MessageBase
{
	public Quaternion rotation;
	public Vector3 acceleration;
	public string mess;
}
