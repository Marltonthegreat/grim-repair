using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this is the visuals/etc of the character sprite/physics.  it is used by Player which is
// in charge of getting user input and making this character do something.
public class Character : MonoBehaviour
{
    Animator animator;
    CapsuleCollider2D capsule;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    // if we're overlapping a ladder, it will be here (regardless of whether or not we're climbing it)
    public Ladder ladder { get; private set; }
    public Transform feet, head;
    public bool touchingGround { get { return animator.GetBool("touchingGround"); } }
    List<Image> waterAreas = new List<Image>();
    public bool feetUnderWater { get; private set; }
    public bool headUnderWater { get; private set; }
    public bool isDead { get { return animator.GetBool("isDead"); } }

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
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        ScanShip();
    }

    void ScanShip() {
        Debug.Log("Scanning ship...");
        var ship = GameObject.FindObjectOfType<ShipController>();
        if (ship == null) 
            Debug.LogWarning("Could not find ShipController");
        
        foreach (var i in FindObjectsOfType<Image>()) {
            if (i.name != "Fill")
                continue;
            waterAreas.Add(i);
        }
        Debug.Log($"Found {waterAreas.Count} water fill images");
    }

    void CheckForWater() {
        var corners = new Vector3[4];
        var feetPos = feet.position;
        var headPos = head.position;
        feetUnderWater = false;
        headUnderWater = false;
        foreach (var w in waterAreas) {
            w.rectTransform.GetWorldCorners(corners);
            var bl = corners[0];
            var tl = corners[1];
            var tr = corners[2];
            var br = corners[3];
            if (feetPos.x > bl.x && feetPos.x < br.x && feetPos.y > bl.y && feetPos.y < tl.y)
                feetUnderWater = true;
            if (headPos.x > bl.x && headPos.x < br.x && headPos.y > bl.y && headPos.y < tl.y)
                headUnderWater = true;
        }
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (isDead)
            return;
        if (coll.gameObject.layer == LayerMask.NameToLayer("Interactable")) {
            if (coll.GetComponent<Animator>() != null)
                coll.GetComponent<Animator>().SetBool("closed", false);
            return;
        }
        var l = coll.GetComponent<Ladder>();
        if (l == null)
            return;
        ladder = l;
    }

    void OnTriggerExit2D(Collider2D coll) {
        if (isDead)
            return;
        if (coll.gameObject.layer == LayerMask.NameToLayer("Interactable")) {
            if (coll.GetComponent<Animator>() != null)
                coll.GetComponent<Animator>().SetBool("closed", true);
            return;
        }
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
        spriteRenderer.flipX = pos.x < transform.position.x;
        rb.MovePosition(pos);
        animator.SetBool("isClimbing", false);
        animator.SetFloat("speed", 1);
    }

    // this falls to a position over the next physics update - meant to be called every FixedUpdate that
    // the player is falling (i.e. not climbing and delta y < 0)
    // public void FallTo(Vector2 pos) {
    //     if (isClimbingLadder)
    //         Debug.LogError("Player can't fall when climbing");
    //     rb.MovePosition(pos);
    //     animator.SetBool("isClimbing", false);
    //     // todo - set falling
    //     animator.SetFloat("speed", 1);
    // }

    public void SetDead() {
        if (isDead)
            return;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.AddTorque(1); // make them rotate a bit
        animator.SetFloat("speed", 0);
        animator.SetBool("isDead", true);
        if (isClimbingLadder)
            StopClimbing();
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
        spriteRenderer.flipX = pos.x < transform.position.x;
        rb.MovePosition(pos);
        animator.SetFloat("speed", 1);
    }

    // should be called in a fixed update when the user has no directional input (but the character might
    // be falling due to gravity)
    public void IdleTo(Vector2 pos) {
        rb.MovePosition(pos);
        animator.SetFloat("speed", 0);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWater();
        if (isDead) {
            if (headUnderWater) {
                rb.gravityScale = GameConfig.instance.buoyancy;
            } else {
                rb.gravityScale = GameConfig.instance.gravity;
            }
        } else {
            var touchingGround = Physics2D.Raycast(feet.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
            animator.SetBool("touchingGround", touchingGround);
        }
    }
}
