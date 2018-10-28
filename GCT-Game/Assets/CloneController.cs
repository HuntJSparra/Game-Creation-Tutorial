using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : MonoBehaviour {

    private Rigidbody2D rb; // ridgid body for this object
    private float velMod; //how fast we scale movement left and right
    private float jumpForce; //how much of a force we're applying upwards when we jump
    private Stack<Vector3> rewindStack;
    private Stack<Vector2> moveStack;
    private Stack<string> pressedKey;
    private Stack<Vector3> forwardStack = new Stack<Vector3>();
    private Stack<Vector2> nextMoveStack = new Stack<Vector2>();
    private Stack<string> pressKey = new Stack<string>();
    private int CurFrame; //current frame that you're on
    private bool CurRewind;
    private int rewindDist;

    public CloneController(Stack<Vector3> firstPos, Stack<Vector2> firstMove, Stack<string> keyPress)
    {
        Debug.Log(firstPos);
        rewindStack = firstPos;
        print(rewindStack);
        moveStack = firstMove;
        pressedKey = keyPress;

    }

    public CloneController(Stack<Vector3> firstPos, Stack<Vector2> firstMove, Stack<string> keyPress, int rewond)
    {
        rewindStack = firstPos;
        moveStack = firstMove;
        pressedKey = keyPress;
        for (int i = 0; i < rewond; i++)
        {
            Vector3 topPos = rewindStack.Pop();
            Vector2 topMove = moveStack.Pop();
            string topKey = pressedKey.Pop();
            forwardStack.Push(topPos);
            nextMoveStack.Push(topMove);
            pressKey.Push(topKey);
        }
    }

    //private RaycastHit2D hit;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velMod = 5;
        jumpForce = 300;
        rewindDist = 0;
    }

    private void Update()
    {

        if (Input.GetKey("e")) /*hold down e to rewind*/
        {
            if (Input.GetKeyDown("e"))
            {
                rewindDist = 0;
            }
            print(rewindStack);
            if (rewindDist < rewindStack.Count) /* so we don't go back too far in time*/
            {
                Vector3 rewindingPos = rewindStack.Pop();
                forwardStack.Push(rewindingPos);
                Vector2 topMove = moveStack.Pop();
                nextMoveStack.Push(topMove);
                string topKey = pressedKey.Pop();
                pressKey.Push(topKey);
                print("Rewinding to this place " + rewindingPos);
                Quaternion empty = new Quaternion();
                transform.SetPositionAndRotation(rewindingPos, empty);
                rewindDist = rewindDist + 1;
            }
        }
        else /*playable*/
        {
            //adds current location to rewinding dictionary
            Vector3 nextPos = forwardStack.Pop();
            rewindStack.Push(nextPos);
            Vector2 nextMove = nextMoveStack.Pop();
            moveStack.Push(nextMove);
            rb.velocity = nextMove;
            string nextKey = pressKey.Pop();
            pressedKey.Push(nextKey);
            if (nextKey.Equals("e"))
            {
                CloneController control = new CloneController(rewindStack, moveStack, pressedKey, System.Convert.ToInt32(pressKey.Peek()));
                GameObject newClone = Instantiate(gameObject, transform.position, new Quaternion());
                CloneController cc = newClone.GetComponent<CloneController>();
                cc = control;
            }
            print(" Adding " + Time.frameCount + " " + transform.position); //prints out where you are going next // Get rid of later
            //float dist = .51F;
            Vector2 jump = new Vector2(0.0f, jumpForce);
            //hit = Physics2D.Raycast(rb.transform.position, Vector2.down, dist);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(jump);
            }

            //when we rewind we need a frame to start from
            CurFrame = Time.frameCount;
            CurRewind = false;
        }
    }
}
