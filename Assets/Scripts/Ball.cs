using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Paddle paddle;
    [SerializeField] private Vector2 ballVelocity;
    [SerializeField] private AudioClip[] ballSounds;
    [SerializeField] private float randomFactor = 0.2f;

    private Vector2 paddleToBallVector;
    private bool hasStarted = false;

    private Rigidbody2D rigidbody2;
    private AudioSource audioSource;

    // Start is called before the first frame update
    private void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        paddleToBallVector = transform.position - paddle.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!hasStarted) {
            LockBallToPaddle();
            LaunchOnMouseClick();
        }
    }

    private void LaunchOnMouseClick()
    {
        if (Input.GetMouseButtonDown(0)) {
            rigidbody2.velocity = ballVelocity;
            hasStarted = true;
        }
    }

    private void LockBallToPaddle()
    {
        Vector2 paddlePos = paddle.transform.position;
        transform.position = paddlePos + paddleToBallVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 velocityTweak = new Vector2(
            UnityEngine.Random.Range(0f, randomFactor),
            UnityEngine.Random.Range(0f, randomFactor)
        );

        if (hasStarted) {
            AudioClip clip = ballSounds[UnityEngine.Random.Range(0, ballSounds.Length)];
            audioSource.PlayOneShot(clip);

            rigidbody2.velocity += velocityTweak;
        }
    }

    public bool IsHasStarted()
    {
        return hasStarted;
    }
}
