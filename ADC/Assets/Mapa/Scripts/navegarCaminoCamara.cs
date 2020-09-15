using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class navegarCaminoCamara : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float speed = 5;
    float distanceTravelled;

    public Vector2 startPos;
    public Vector2 direction;
    public bool directionChosen;

    void Start()
    {
        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
            distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            //transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            transform.position += Vector3.up * 2;// Time.deltaTime;
        }
    }

    void Update()
    {
        if (Input.touchCount > 0 && pathCreator != null)
        {
            Debug.Log("Si toca");
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startPos = touch.position;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                direction = touch.position - startPos;
                if (direction.y < 0)
                {
                    distanceTravelled += speed * Time.deltaTime;
                    transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                    //transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                    transform.position += Vector3.up * 2;// Time.deltaTime;
                }
                if (direction.y > 0)
                {
                    distanceTravelled -= speed * Time.deltaTime;
                    transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                    //transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                    transform.position += Vector3.up * 2;// Time.deltaTime;
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                startPos = Vector2.zero;
                direction = Vector2.zero;
            }

            Debug.Log(direction);
        }
        //else { Debug.Log("No toca"); }

        if (Input.GetKey(KeyCode.A))
        {
            distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            //transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            transform.position += Vector3.up * 2;//Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            distanceTravelled -= speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            //transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            transform.position += Vector3.up * 2;// Time.deltaTime;
        }
        //transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, 400), Mathf.Clamp(transform.position.y, 0, 10), Mathf.Clamp(transform.position.z, 0, 400));
    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
}
