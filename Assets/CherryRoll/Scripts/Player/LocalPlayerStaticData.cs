using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerStaticData : MonoBehaviour {


    //private const string PLAYER_PREFS_APPEARANCE = "Appearance";
    private const string PLAYER_PREFS_NAME = "PlayerName";
    private const string PLAYER_PREFS_COLOR = "PlayerColor";


    public static LocalPlayerStaticData Instance { get; private set; }


    private string playerName;
    private Color playerColor;

    public static event EventHandler OnLocalPlayerNameChanged;
    public static event EventHandler OnLocalPlayerColorChanged;

    private void Start() {
        Instance = this;

        LoadSettingsFromJson();

        Debug.Log("PLAYER_PREFS_NAME = " + playerName);

    }

    private void LoadSettingsFromJson() {
        playerName = PlayerPrefs.GetString(PLAYER_PREFS_NAME, "Incognito");
        //playerName = PlayerPrefs.GetString(PLAYER_PREFS_NAME, null);

        //color = Color.FromName("Red");
        //(PlayerPrefs.GetString(PLAYER_PREFS_COLOR, null));

    }

    //private void SetLocalPlayerColor(Color newColor) {

    //    playerColor = newColor;

    //    OnLocalPlayerColorChanged?.Invoke(this, EventArgs.Empty);
    //}

    public void SetLocalPlayerName(string newName) {
        playerName = newName;

        OnLocalPlayerNameChanged?.Invoke(this, EventArgs.Empty);

        PlayerPrefs.SetString(PLAYER_PREFS_NAME, newName);
        PlayerPrefs.Save();
    }
}
