using UnityEngine;

public class BaseInteractableObject : MonoBehaviour {

    public virtual void Interact(Player player) {
        Debug.LogError("BaseInteractableObject.Interact();");
    }

}
