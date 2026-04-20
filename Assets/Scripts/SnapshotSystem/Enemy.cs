using UnityEngine;

public class Enemy : MonoBehaviour
{ 
    public void Actuar(Transform player)
    {
        Vector3 dir = (player.position - transform.position);
        dir.y = 0;

        

        transform.position += dir.normalized;
    }
}