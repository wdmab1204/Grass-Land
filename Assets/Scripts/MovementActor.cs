using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class MovementActor : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Grid grid;
        [SerializeField] private TilemapReader tilemapReader;
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

        private IEnumerator MovementProcess()
        {
            while (true)
            { 
                //키 입력에 따른 방향 값 대입
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


                yield return null;
            }
        }

        private void Awake()
        {

        }

        private void Start()
        {
            StartCoroutine(MovementProcess());
        }

        private void Update()
        {
            transform.Translate(motion * Time.deltaTime);
        }
    }
}
