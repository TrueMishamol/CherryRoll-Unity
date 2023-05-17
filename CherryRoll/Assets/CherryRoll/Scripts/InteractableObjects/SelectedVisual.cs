using UnityEngine;

public class SelectedVisual : MonoBehaviour {


    [SerializeField] private Transform interactableObjectTransform;
    [SerializeField] private GameObject[] visualGameObjectArray;

    private IInteractableObject interactableObject;


    private void Awake() {
        interactableObject = interactableObjectTransform.GetComponent<IInteractableObject>();
    }

    private void Start() {
        if (Player.LocalInstance != null) {
            Player.LocalInstance.OnSelectedInteractableObjectChanged += Player_OnSelectedInteractableObjectChanged;
        } else {
            Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;
        }
    }

    private void Player_OnAnyPlayerSpawned(object sender, System.EventArgs e) {
        if (Player.LocalInstance != null) {
            Player.LocalInstance.OnSelectedInteractableObjectChanged -= Player_OnSelectedInteractableObjectChanged;
            Player.LocalInstance.OnSelectedInteractableObjectChanged += Player_OnSelectedInteractableObjectChanged;
        }
    }

    private void Player_OnSelectedInteractableObjectChanged(object sender, Player.OnSelectedInteractableObjectChangedEventArgs e) {
        if (e.selectedInteractableObject == interactableObject) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        foreach (GameObject visualGameObject in visualGameObjectArray) {
            visualGameObject.SetActive(true);
        }
    }

    private void Hide() {
        foreach (GameObject visualGameObject in visualGameObjectArray) {
            visualGameObject.SetActive(false);
        }
    }

    private void OnDestroy() {
        Player.LocalInstance.OnSelectedInteractableObjectChanged -= Player_OnSelectedInteractableObjectChanged;
        Player.OnAnyPlayerSpawned -= Player_OnAnyPlayerSpawned;
    }
}
