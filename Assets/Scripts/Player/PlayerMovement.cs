using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 0;
    private Rigidbody2D rb;
    private float horiz;
    private float vert;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void MovePlayer()
    {
        horiz = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");

        Vector2 dir = new Vector2(horiz, vert);

        rb.velocity = dir.normalized * speed;
    }
}
