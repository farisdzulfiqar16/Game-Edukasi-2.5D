using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTrigger : MonoBehaviour
{
    public SmartRoad road;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponentInParent<PlayerMovement>();
        if (player == null) return;

        player.SetRoad(road);
    }
}