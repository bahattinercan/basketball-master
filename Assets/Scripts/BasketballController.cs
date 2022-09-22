using UnityEngine;

public class BasketballController : MonoBehaviour
{
    public float MoveSpeed = 10;
    public Transform Ball;
    public Transform PosDribble;
    public Transform PosOverHead;

    //public Transform Arms;
    public Transform Target;

    public VariableJoystick variableJoystick;

    public float minX, maxX, minZ, maxZ;

    // variables
    private bool IsBallInHands = true;

    private bool IsBallFlying = false;
    private float T = 0;

    private Vector2 startTouchPos, endTouchPos;
    private Vector2 swipeVector;
    private bool canMove = true;

    public bool CanMove { get => canMove; set => canMove = value; }

    private void Start()
    {
        startTouchPos = Vector2.zero;
        endTouchPos = Vector2.zero;
    }

    // Update is called once per frame
    private void Update()
    {
        SwipeDetection();
        Movement();

        // ball in hands
        if (IsBallInHands)
        {
            // hold over head
            if (Input.GetKey(KeyCode.Space))
            {
                Ball.position = PosOverHead.position;
                //Arms.localEulerAngles = Vector3.right * 180;

                // look towards the target
                transform.LookAt(Target.parent.position);
            }

            // dribbling
            else
            {
                Ball.position = PosDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5)) * 3;
                //Arms.localEulerAngles = Vector3.right * 0;
            }

            // throw ball
            if (Input.GetKeyUp(KeyCode.Space))
            {
                ThrowTheBall();
            }
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && swipeVector.y > 200
                && Vector3.Distance(transform.position, Target.position)<15)
            {
                ThrowTheBall();
            }
        }

        // ball in the air
        if (IsBallFlying)
        {
            T += Time.deltaTime;
            float duration = 0.66f;
            float t01 = T / duration;

            // move to target
            Vector3 A = PosOverHead.position;
            Vector3 B = Target.position;
            Vector3 pos = Vector3.Lerp(A, B, t01);

            // move in arc
            Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * 3.14f);

            Ball.position = pos + arc;

            // moment when ball arrives at the target
            if (t01 >= 1)
            {
                IsBallFlying = false;
                Ball.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    private void ThrowTheBall()
    {
        IsBallInHands = false;
        IsBallFlying = true;
        T = 0;
    }

    private void SwipeDetection()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPos = Input.GetTouch(0).position;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPos = Input.GetTouch(0).position;
            swipeVector = endTouchPos - startTouchPos;
        }
    }

    private void Movement()
    {
        if (canMove == false)
            return;
        Vector3 direction = new Vector3(variableJoystick.Horizontal, 0, variableJoystick.Vertical);
        transform.position += direction * MoveSpeed * Time.deltaTime;
        transform.LookAt(transform.position + direction);

        if (transform.position.x < minX)
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        else if (transform.position.x > maxX)
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);

        if (transform.position.z < minZ)
            transform.position = new Vector3(transform.position.x, transform.position.y, minZ);
        else if (transform.position.z > maxZ)
            transform.position = new Vector3(transform.position.x, transform.position.y, maxZ);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsBallInHands && !IsBallFlying)
        {
            IsBallInHands = true;
            Ball.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}