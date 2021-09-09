using UnityEngine;

[RequireComponent(typeof(FPSWalker))]
[RequireComponent(typeof(CharacterController))]
public class FPFlyer : MonoBehaviour
{
	public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    
    private Vector3 moveDirection = Vector3.zero;
    private bool grounded;
    
    void FixedUpdate() {
    	var my=moveDirection.y;
    
    	moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    	moveDirection = transform.TransformDirection(moveDirection);
    	
    	if (grounded) {
    		// We are grounded, so recalculate movedirection directly from axes
    		moveDirection *= speed;
    		
    	} else {
    		moveDirection *= speed+transform.position.y/5;
    
    		moveDirection.y=my;		
    	}
    
    	if (Input.GetButton ("Jump")) {
    		moveDirection.y = jumpSpeed;
    	}
    
    	// Apply gravity
    	moveDirection.y -= gravity * Time.deltaTime;
    	
    	// Move the controller
    	CharacterController controller = GetComponent<CharacterController>();
    	var flags = controller.Move(moveDirection * Time.deltaTime);
    	grounded = (flags & CollisionFlags.CollidedBelow) != 0;
    }

    private FPSWalker myWalker;
    
    float maxHeight=250;
    
    void Start () {
    	myWalker = gameObject.GetComponent<FPSWalker>();	
    }
    
    
    void Update () {
    	if(Input.GetKey("left shift")) {
    		myWalker.gravity=-20;
    		//myWalker.grounded=true;
    	} else {
    		myWalker.gravity=10;	
    	}
    
    
    	if (transform.position.y > maxHeight) {
    		myWalker.gravity = 20;	
    	}
    
    
    }
}
