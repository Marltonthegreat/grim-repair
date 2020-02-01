using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is the visuals/etc of the character sprite/physics.  it is used by Player which is
// in charge of getting user input and making this character do something.
public class Character : MonoBehaviour
{
    Animator animator;
    CapsuleCollider2D capsule;
    Rigidbody2D rb;
    // if we're overlapping a ladder, it will be here (regardless of whether or not we're climbing it)
    public Ladder ladder { get; private set; }

    public bool isClimbingLadder {
        get {
            return animator.GetBool("isClimbing");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D coll) {
        var l = coll.GetComponent<Ladder>();
        if (l == null)
            return;
        ladder = l;
    }

    void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject == ladder.gameObject) {
            if (isClimbingLadder)
                StopClimbing();
            ladder = null;
        }
    }

    // this walks to a position over the next physics update - meant to be called every FixedUpdate that
    // the player is walking
    public void WalkTo(Vector2 pos) {
        if (isClimbingLadder)
            Debug.LogError("Player can't walk when climbing");
        rb.MovePosition(pos);
        animator.SetBool("isClimbing", false);
    }

    public void StartClimbing() {
        if (ladder == null)
            throw new System.Exception("Can't start climbing when not overlapping a ladder");
        // make it so the player doesn't collide with the ladder's transition floor
        Physics2D.IgnoreCollision(capsule, ladder.transitionFloor);
        animator.SetBool("isClimbing", true);
    }

    public void StopClimbing() {
        if (!isClimbingLadder)
            throw new System.Exception("Can't stop climbing when not climbing");
        // make the player collide with the transition floor again
        Physics2D.IgnoreCollision(capsule, ladder.transitionFloor, false);
        animator.SetBool("isClimbing", false);
    }

    // this climbs to a position over the next physics update - meant to be called every FixedUpdate that
    // the player is climbing.  call StartClimbing() first.
    public void ClimbTo(Vector2 pos) {
        if (!isClimbingLadder)
            throw new System.Exception("Player can't walk when climbing");
        rb.MovePosition(pos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
