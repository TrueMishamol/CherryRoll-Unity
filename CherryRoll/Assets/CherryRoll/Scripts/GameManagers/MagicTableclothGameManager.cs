using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MagicTableclothGameManager : BaseGameManager {


    public event EventHandler OnRequestCompleted;
    public event EventHandler OnRequestSuccess;
    public event EventHandler OnRequestFailed;


}
