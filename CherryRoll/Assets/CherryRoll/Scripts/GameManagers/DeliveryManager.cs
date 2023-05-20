using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DeliveryManager : NetworkBehaviour {


    public static DeliveryManager Instance { get; private set; }

    public event EventHandler OnRequestCompleted;
    public event EventHandler OnRequestSuccess;
    public event EventHandler OnRequestFailed;

    [SerializeField] private List<ItemSO> requestList;

    //! Initialize for each player individualy, but from the server side
    //! Maybe create a script for PlayerDeliveryRequestList
    private List<ItemSO> localRequestList;


    private void Awake() {
        Instance = this;

        localRequestList = new List<ItemSO>();
    }

    private void Start() {
        localRequestList = requestList;
    }


}
