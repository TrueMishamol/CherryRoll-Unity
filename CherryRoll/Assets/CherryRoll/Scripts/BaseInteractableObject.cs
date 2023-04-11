using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInteractableObject : MonoBehaviour {
    //[SerializeField] private 
    // Прикрепить MonoBehavior скрипт объекта, который должен зпускать 
    // Interact функцию внутри этого C# скрипта

    // Но C# MonoBehavior разных полно. Поэтому что если сделать выбор нужного через Inspector

    //[SerializeField] private string scriptType = "Wardrobe";
    //[SerializeField] private scriptType;

    public virtual void Interact(Player player) {
        Debug.LogError("BaseInteractableObject.Interact();");
    }

}
