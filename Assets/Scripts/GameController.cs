using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Player player;

    public int timberCount;
    public List<Timber> timberList;
    public GameObject timber;
    public Transform timberHolder;
    public bool hasTimber;

    public GameObject ladder;
    public GameObject lastInstantiatedLadder;
    public Transform horizontalLadderPlacer;
    public Transform verticalLadderPlacer;

    public bool gameStarted;

    void Start()
    {
        timberCount = 0;
        hasTimber = false;
    }

    public void StartGame()
    {
        player.animator.SetTrigger("Move");
        gameStarted = true;
    }

    public void AddNewTimber(Timber newTimber)
    {
        if (!hasTimber)
        {
            player.animator.SetFloat("Blend", 1);
        }
        hasTimber = true;
        timberCount++;
        timberList.Add(newTimber);
        player.myRigidbody.useGravity = false;
    }

    public void PlaceLadder(int choice = 0)
    {
        if (hasTimber)
        {
            RemoveLastTimber();
            if (choice == 1)
            {
                lastInstantiatedLadder = Instantiate(ladder, verticalLadderPlacer.position, verticalLadderPlacer.rotation);
            }
            else
            {
                lastInstantiatedLadder = Instantiate(ladder, horizontalLadderPlacer.position, horizontalLadderPlacer.rotation);
            }
        }
    }

    public void RemoveLastTimber()
    {
        timberCount--;
        Destroy(timberList[timberCount].gameObject);
        if (timberCount <= 0)
        {
            hasTimber = false;
            player.animator.SetFloat("Blend", 0);
        }
    }
}
