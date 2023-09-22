using UnityEngine;

public class Marquee : MonoBehaviour {

    [SerializeField] private float speed = 4f;
    private Vector3 startPosition;
    private Vector3 currentPosition;
    private Vector3 directionVector;

    private float height;
    private float width;

    private enum Direction {
        up,
        down,
        left,
        right,
    }

    [SerializeField] private Direction direction;

    private void Awake() {
        startPosition = transform.localPosition;

        height = GetComponent<RectTransform>().rect.height;
        width = GetComponent<RectTransform>().rect.width;
    }

    private void FixedUpdate() {
        switch (direction) {
            case Direction.up:
                directionVector = Vector3.up;
                break;
            case Direction.down:
                directionVector = Vector3.down;
                break;
            case Direction.left:
                directionVector = Vector3.left;
                break;
            case Direction.right:
                directionVector = Vector3.right;
                break;
        }

        transform.Translate(directionVector * speed * Time.unscaledDeltaTime);

        currentPosition = transform.localPosition;

        if (Mathf.Abs(currentPosition.y - startPosition.y) > height) {
            transform.localPosition = startPosition;
        }
        if (Mathf.Abs(currentPosition.x - startPosition.x) > width) {
            transform.localPosition = startPosition;
        }
    }
}
