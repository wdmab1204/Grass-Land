using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class MovementActor : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float speed;
        private Vector2 direction;
        private Vector2 motion;

        private Vector2 CatesianToIsometric(Vector2 cartesian)
        {
            return new Vector2(cartesian.x - cartesian.y, (cartesian.x + cartesian.y) / 2);
        }

        private Vector2 IsometricToCartesian(Vector2 iso)
        {
            var cartPos = new Vector2();
            cartPos.x = (iso.x + iso.y * 2) / 2;
            cartPos.y = -iso.x + cartPos.x;
            return cartPos;
        }

        private void MovementProcess()
        {
            direction = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
                direction += Vector2.up;
            else if (Input.GetKey(KeyCode.S))
                direction += Vector2.down;

            if (Input.GetKey(KeyCode.A))
                direction += Vector2.left;
            else if (Input.GetKey(KeyCode.D))
                direction += Vector2.right;

            motion = direction.normalized * speed;
            motion = CatesianToIsometric(motion);
        }

        private void Move(Vector2 motion)
        {
            rb.velocity = motion;
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            MovementProcess();
        }

        private void FixedUpdate()
        {
            Move(motion);
        }


    }
}
