using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;

    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Move(h, v);
        Turning();
        Animating(h, v);
    }


    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning() //need to redo this for my own project as the player rotation is dependent on the directional input of the player's right stick rather than a mouse input
    { //wow this one is kinda complicated
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition); //dictate a ray to be cast
        RaycastHit floorHit; //declare a name to the point on which the player's cast hit.
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask)) //this function in the if statement is casting a ray from the camera to the floor object in the direction of the mouse cursor
        {
            Vector3 playerToMouse = floorHit.point - transform.position;//this gives us a direction for the player to face that is between the point the ray hit and the player's location
            playerToMouse.y = 0f; //this makes it ignore the y rotation cause we dont want the player to face the floor
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse); //this turns the direction we found into a quaternion rotation
            playerRigidbody.MoveRotation(newRotation); //this makes the player face the direction of the rotation we wanted
        }
    }

    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }

}
