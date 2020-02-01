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
        // if the player is pushing up at all and we're overlapping a ladder, start climbing it
        if (!character.isClimbingLadder && character.ladder != null && dirInput.y != 0) {
            character.StartClimbing();
        }

        if (character.isClimbingLadder) {
            // if the player is already climbing a ladder, we can move vertically as well as horizontally
            var desiredPosition = transform.position 
                    + new Vector3(dirInput.x * GameConfig.instance.walkSpeed, 
                        dirInput.y * GameConfig.instance.climbSpeed, 0) * Time.fixedDeltaTime;
            character.ClimbTo(desiredPosition);
        } else {
            var desiredPosition = transform.position 
                    + new Vector3(dirInput.x, 0, 0) * GameConfig.instance.walkSpeed * Time.fixedDeltaTime;
            desiredPosition += new Vector3(0, GameConfig.instance.gravity * Time.fixedDeltaTime, 0);
            character.WalkTo(desiredPosition);
        }
    }

    void FixedUpdate() {
        HandleMovement();
    }
}
