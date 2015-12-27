using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CustomNetworkDiscovery : NetworkDiscovery {

	public delegate void callbackString(string fromAddress, string data );
	public callbackString OnReceivedBoardCastCallback;

	public override void OnReceivedBroadcast(string fromAddress, string data)
	{
		base.OnReceivedBroadcast(fromAddress, data);
		if (OnReceivedBoardCastCallback != null) {
			OnReceivedBoardCastCallback (fromAddress,data);
		}
	}
}
