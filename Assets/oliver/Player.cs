using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Character character;
    Vector2 dirInput;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<Character>();
    }

    // todo - multiplayer/rewired?
    void GetInput() {
        if (Input.GetKeyDown(KeyCode.Space))
            Debug.Log(character.ladder);
        dirInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // if the player's directional input doesn't exceed a certain threshold, stop moving 
        // (could also do this in the input settings maybe)
        if (dirInput.magnitude < GameConfig.instance.directionInputMinThreshold)
            dirInput = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void HandleMovement() {
        // if the player is pushing up or down and we're overlapping a ladder, start climbing it
        if (!character.isClimbingLadder && character.ladder != null && dirInput.y != 0 && character.touchingGround) {
            // test to make sure the player isn't pushing up at the top of a ladder or down at the 
            // bottom of one, in which case they should not start climbing
            var ladderDir = character.ladder.GetDirection(character.feet.position);
            if (dirInput.y > 0 && ladderDir > 0 || dirInput.y < 0 && ladderDir < 0)
                character.StartClimbing();
        }

        if (character.isClimbingLadder) {
            // if the player is already climbing a ladder, we can move vertically as well as horizontally
            if (dirInput != Vector2.zero) {
                var desiredPosition = transform.position 
                        + new Vector3(dirInput.x * GameConfig.instance.walkSpeed, 
                            dirInput.y * GameConfig.instance.climbSpeed, 0) * Time.fixedDeltaTime;
                character.ClimbTo(desiredPosition);
            } else {
                character.IdleTo(transform.position);
            }
        } else {
            var desiredPosition = transform.position 
                    + new Vector3(dirInput.x, 0, 0) * GameConfig.instance.walkSpeed * Time.fixedDeltaTime;
            desiredPosition += new Vector3(0, GameConfig.instance.gravity * Time.fixedDeltaTime, 0);
            if (dirInput.x != 0) {
                character.WalkTo(desiredPosition);
            } else {
                // send a position even if idling in case the character is falling
                character.IdleTo(desiredPosition);
            }
        }
    }

    void FixedUpdate() {
        HandleMovement();
    }
}
