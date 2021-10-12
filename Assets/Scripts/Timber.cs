using UnityEngine;

public class Timber : MonoBehaviour
{
    private GameController gameController;
    private Transform _timberHolder;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        _timberHolder = gameController.timberHolder;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.CompareTag("Player"))
        {
            gameController.AddNewTimber(this);
            PlaceNewTimber();
        }
    }

    private void PlaceNewTimber()
    {
        transform.parent = _timberHolder;
        transform.position = HandledTimberPosition(gameController.timberCount);
        transform.rotation = _timberHolder.rotation;
    }

    Vector3 HandledTimberPosition(float height)
    {
        Vector3 timberPos = new Vector3(_timberHolder.position.x, _timberHolder.position.y + 0.1f * height, _timberHolder.position.z);
        return timberPos;
    }

    private void OnDestroy()
    {
        gameController.timberList.Remove(this);
    }
}
