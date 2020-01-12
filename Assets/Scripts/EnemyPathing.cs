using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{

    WaveConfig waveConfig;
    // Transform because we need the position
    List<Transform> waypoints;
    int waypointIndex = 0;
    float distToWaypoint;
    float distCheck = 0.02f;


    // Start is called before the first frame update
    void Start()
    {
        waypoints = waveConfig.GetWayPoints();
        // Go to position 0
        transform.position = waypoints[waypointIndex].position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

    }


    public void SetWaveConfig(WaveConfig waveConfig)
    {
        // use this to referrence in entire class
        this.waveConfig = waveConfig;
    }

    private void Move()
    {

        if (waypointIndex <= waypoints.Count - 1)
        {

            float distToWaypoint = Vector2.Distance(waypoints[waypointIndex].position, transform.position);

            var targetPosition = waypoints[waypointIndex].transform.position;
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards
                (transform.position, targetPosition, movementThisFrame);

            if (distToWaypoint <= distCheck)
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
