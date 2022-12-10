using UnityEngine;

public class Spiner : MonoBehaviour {
    [SerializeField] private float _rotationSpeed;

    public float spinerRadius = 1;

    private void Update() {
        if(GameController.instance.gameState != GameState.play) {
            return;
        }
        transform.Rotate(new Vector3(0, 0, 1 * Time.deltaTime * _rotationSpeed));
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spinerRadius);
    }
}
