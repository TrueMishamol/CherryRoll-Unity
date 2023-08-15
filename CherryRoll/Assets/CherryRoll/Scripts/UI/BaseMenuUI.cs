using UnityEngine;

public class BaseMenuUI: MonoBehaviour {


    protected bool isOppened = false;


    protected virtual void Open() {
        gameObject.SetActive(true);
        isOppened = true;
    }

    protected virtual void Close() {
        gameObject.SetActive(false);
        isOppened = false;
    }

    public void SwitchOpenClose() {
        isOppened = !isOppened;

        if (isOppened) {
            Open();
        } else {
            Close();
        }
    }

    public bool IsOppened() {
        return isOppened;
    }
}
