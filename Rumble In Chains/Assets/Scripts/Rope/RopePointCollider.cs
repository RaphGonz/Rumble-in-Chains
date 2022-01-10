using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePointCollider : MonoBehaviour
{
    

    Collider2D pointCollider;
    Bounds bounds;
    RopePoint ropePoint;

    public bool debug = false;
    public float rayLength = 0.1f;
    public float boundsExtension = 0.001f;

    private bool collisionTest = false;


    public LayerMask mask;
    
    void Awake()
    {
        pointCollider = GetComponent<Collider2D>();
        ropePoint = GetComponent<RopePoint>();

        mask = LayerMask.GetMask("Wall");
    }


    public bool UpdateCollisions(ref Vector2 movement, Vector2 oldPosition)
    {
        collisionTest = false;
        UpdateBounds();

        Vector2 position = oldPosition;

        
        XAxisCollision(ref movement, ref position);
        YAxisCollision(ref movement, ref position);

        /*
        if (movement.x != 0)
        {
            XAxisReverseCollision(ref movement, ref position);
        }
        if (movement.y != 0)
        {
            YAxisReverseCollision(ref movement, ref position);
        }
        */
        /*
        XAxisCollision(ref movement, ref position);
        YAxisCollision(ref movement, ref position);
        XAxisCollision(ref movement, ref position);
        YAxisCollision(ref movement, ref position);
        */
        //FirstDiagAxisCollision(ref movement, ref position);
        //SecondDiagAxisCollision(ref movement, ref position);


        if (!collisionTest)
        {
            movementCollision(ref movement, position);
        }

        return collisionTest;
        /*
        BottomLeftCollsion(ref movement);
        BottomRightCollsion(ref movement);
        TopLeftCollsion(ref movement);
        TopRightCollsion(ref movement);
        */

    }


    void UpdateBounds()
    {
        bounds = pointCollider.bounds;
        bounds.Expand(boundsExtension);
    }

    private void XAxisCollision(ref Vector2 movement, ref Vector2 position)
    {
        float length = Vector2.Dot(new Vector2(1, 0), movement);
        Vector2 direction = new Vector2(1, 0);
        if (length < 0)
        {
            direction.x *= -1;
            length = -length;
        }
        Vector2 origin = new Vector2(position.x + direction.x * (bounds.max.x - bounds.min.x) / 2, position.y + direction.y * (bounds.max.y - bounds.min.y) / 2);

        movement = DetectCollision(origin, direction, length, movement);
        position.x += movement.x;
    }


    private void XAxisReverseCollision(ref Vector2 movement, ref Vector2 position)
    {
        float length = Vector2.Dot(new Vector2(1, 0), movement);
        Vector2 direction = new Vector2(1, 0);
        if (length < 0)
        {
            direction.x *= -1;
            length = -length;
        }
        Vector2 origin = new Vector2(position.x - direction.x * (bounds.max.x - bounds.min.x) / 2, position.y + direction.y * (bounds.max.y - bounds.min.y) / 2);

        RayInfo ray;
        ray.origin = origin;
        ray.direction = direction;
        ray.distance = length;

        RaycastHit2D hit = Raycast(ray, mask);

        if (hit)
        {
            if (hit.distance < (bounds.max.x - bounds.min.x))
            {
                movement.x += -direction.x * ((bounds.max.x - bounds.min.x) - hit.distance);
            }
        }
        position.x += -direction.x * ((bounds.max.x - bounds.min.x) - hit.distance);
    }

    private void YAxisCollision(ref Vector2 movement, ref Vector2 position)
    {
        float length = Vector2.Dot(new Vector2(0, 1), movement);
        Vector2 direction = new Vector2(0, 1);

        if (length < 0)
        {
            direction.y *= -1;
            length = -length;
        }
        Vector2 origin = new Vector2(position.x + direction.x * (bounds.max.x - bounds.min.x) / 2, position.y + direction.y * (bounds.max.y - bounds.min.y) / 2);

        movement = DetectCollision(origin, direction, length, movement);
        position.y += movement.y;
    }

    private void YAxisReverseCollision(ref Vector2 movement, ref Vector2 position)
    {
        float length = Vector2.Dot(new Vector2(0, 1), movement);
        Vector2 direction = new Vector2(0, 1);

        if (length < 0)
        {
            direction.y *= -1;
            length = -length;
        }
        Vector2 origin = new Vector2(position.x + direction.x * (bounds.max.x - bounds.min.x) / 2, position.y - direction.y * (bounds.max.y - bounds.min.y) / 2);

        RayInfo ray;
        ray.origin = origin;
        ray.direction = direction;
        ray.distance = length;

        RaycastHit2D hit = Raycast(ray, mask);

        if (hit)
        {
            if (hit.distance < (bounds.max.y - bounds.min.y))
            {
                movement.y += -direction.y * ((bounds.max.x - bounds.min.y) - hit.distance);
            }
        }
        position.y += -direction.y * ((bounds.max.y - bounds.min.y) - hit.distance);
    }

    private void FirstDiagAxisCollision(ref Vector2 movement, ref Vector2 position)
    {
        float length = Vector2.Dot(new Vector2(1, 1), movement);
        Vector2 direction = new Vector2(1, 1);

        if (length < 0)
        {
            direction.x *= -1;
            direction.y *= -1;
            length = -length;
        }
        Vector2 origin = new Vector2(transform.position.x + direction.x * (bounds.max.x - bounds.min.x) / 2, transform.position.y + direction.y * (bounds.max.y - bounds.min.y) / 2);

        movement = DetectCollision(origin, direction, length, movement);
        position += movement;
    }

    private void SecondDiagAxisCollision(ref Vector2 movement, ref Vector2 position)
    {
        float length = Vector2.Dot(new Vector2(-1, 1), movement);
        Vector2 direction = new Vector2(-1, 1);

        if (length < 0)
        {
            direction.x *= -1;
            direction.y *= -1;
            length = -length;
        }
        Vector2 origin = new Vector2(transform.position.x + direction.x * (bounds.max.x - bounds.min.x) / 2, transform.position.y + direction.y * (bounds.max.y - bounds.min.y) / 2);

        movement = DetectCollision(origin, direction, length, movement);
        position += movement;
    }

    private void movementCollision(ref Vector2 movement, Vector2 position)
    {
        Vector2 direction = movement.normalized;
        Vector2 origin = new Vector2(position.x + direction.x * (bounds.max.x - bounds.min.x) / 2, position.y + direction.y * (bounds.max.y - bounds.min.y) / 2);

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
            movement += (hit.distance - length - 0.01f) * direction;
            collisionTest = true;
        }
        
        return movement;
    }

    private Vector2 DetectCollision(Vector2 origin, Vector2 direction, float length, Vector2 movement)
    {
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
            collisionTest = true;
        }

        return movement;
    }

    private Vector2 DetectReverseCollision(Vector2 origin, Vector2 direction, float length, Vector2 movement)
    {
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
            collisionTest = true;
        }

        return movement;
    }

    private RaycastHit2D Raycast(RayInfo ray, LayerMask mask)
    {
        if (debug)
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * ray.distance, Color.red, 0.1f);
            //Gizmos.DrawSphere();
        }

        return Physics2D.Raycast(ray.origin, ray.direction, ray.distance, mask);
        //return Physics2D.CircleCast(ray.origin, 0.05f, ray.direction, ray.distance, mask);
    }

    public struct RayInfo
    {
        public Vector2 origin;
        public Vector2 direction;
        public float distance;
    }
}