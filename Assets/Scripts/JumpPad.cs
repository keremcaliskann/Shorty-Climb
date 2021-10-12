using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    private Player player;
    [SerializeField]
    private Transform targetTransform;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.CompareTag("Player"))
        {
            float targetHeight = targetTransform.position.y + targetTransform.localScale.y / 2;
            player.Jump(600, 12, 1, targetHeight);
        }
    }
}
