using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePointCollider : MonoBehaviour
{
    

    Collider2D pointCollider;
    Bounds bounds;
    RopePoint ropePoint;

    public bool debug = true;
    public float rayLength = 0.1f;
    public float boundsExtension = 0.001f;


    private float sqrt2 = 1.4142135f;

    public LayerMask mask;
    
    void Start()
    {
        pointCollider = GetComponent<Collider2D>();
        ropePoint = GetComponent<RopePoint>();

        mask = LayerMask.GetMask("Wall");
    }


    public void UpdateCollisions(ref Vector2 movement)
    {
        UpdateBounds();
        LeftCollsion(ref movement);
        RightCollsion(ref movement);
        TopCollsion(ref movement);
        BottomCollsion(ref movement);

        BottomLeftCollsion(ref movement);
        BottomRightCollsion(ref movement);
        TopLeftCollsion(ref movement);
        TopRightCollsion(ref movement);
    }


    void UpdateBounds()
    {
        bounds = pointCollider.bounds;
        bounds.Expand(boundsExtension);
    }

    private void LeftCollsion(ref Vector2 movement)
    {
        Vector2 origin = new Vector2(bounds.min.x, transform.position.y);
        Vector2 direction = new Vector2(-1, 0);

        movement = DetectCollision(origin, direction, movement);
    }

    private void RightCollsion(ref Vector2 movement)
    {
        Vector2 origin = new Vector2(bounds.max.x, transform.position.y);
        Vector2 direction = new Vector2(1, 0);

        movement = DetectCollision(origin, direction, movement);
    }

    private void TopCollsion(ref Vector2 movement)
    {
        Vector2 origin = new Vector2(transform.position.x, bounds.max.y);
        Vector2 direction = new Vector2(0, 1);

        movement = DetectCollision(origin, direction, movement);
    }

    private void BottomCollsion(ref Vector2 movement)
    {
        Vector2 origin = new Vector2(transform.position.x, bounds.min.y);
        Vector2 direction = new Vector2(0, -1);

        movement = DetectCollision(origin, direction, movement);
    }

    private void BottomLeftCollsion(ref Vector2 movement)
    {
        float x = (bounds.min.x - transform.position.x) * 1 / sqrt2 + transform.position.x;
        float y = (bounds.min.y - transform.position.y) * 1 / sqrt2 + transform.position.y;
        Vector2 origin = new Vector2(x, y);
        Vector2 direction = new Vector2(-1, -1);

        movement = DetectCollision(origin, direction, movement);
    }

    private void BottomRightCollsion(ref Vector2 movement)
    {
        float x = (bounds.max.x - transform.position.x) * 1 / sqrt2 + transform.position.x;
        float y = (bounds.min.y - transform.position.y) * 1 / sqrt2 + transform.position.y;
        Vector2 origin = new Vector2(x, y);
        Vector2 direction = new Vector2(1, -1);

        movement = DetectCollision(origin, direction, movement);
    }

    private void TopLeftCollsion(ref Vector2 movement)
    {
        float x = (bounds.min.x - transform.position.x) * 1 / sqrt2 + transform.position.x;
        float y = (bounds.max.y - transform.position.y) * 1 / sqrt2 + transform.position.y;
        Vector2 origin = new Vector2(x, y);
        Vector2 direction = new Vector2(-1, 1);

        movement = DetectCollision(origin, direction, movement);
    }

    private void TopRightCollsion(ref Vector2 movement)
    {
        float x = (bounds.max.x - transform.position.x) * 1 / sqrt2 + transform.position.x;
        float y = (bounds.max.y - transform.position.y) * 1 / sqrt2 + transform.position.y;
        Vector2 origin = new Vector2(x, y);
        Vector2 direction = new Vector2(1, 1);

        movement = DetectCollision(origin, direction, movement);
    }

    private Vector2 DetectCollision(Vector2 origin, Vector2 direction, Vector2 movement){

        float length = Vector2.Dot(direction, movement);

        RayInfo ray;
        ray.origin = origin;
        ray.direction = direction;
        ray.distance = length;

        RaycastHit2D hit;

        if (length > 0)
        {
            hit = Raycast(ray, mask);
        }
        else
        {
            hit = new RaycastHit2D();
        }

        if (hit)
        {
            movement += (hit.distance - length) * direction;
        }

        return movement;
    }

    private RaycastHit2D Raycast(RayInfo ray, LayerMask mask)
    {
        if (debug)
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * ray.distance, Color.red, 0.1f);
        }

        return Physics2D.Raycast(ray.origin, ray.direction, ray.distance, mask);
    }

    public struct RayInfo
    {
        public Vector2 origin;
        public Vector2 direction;
        public float distance;
    }
}