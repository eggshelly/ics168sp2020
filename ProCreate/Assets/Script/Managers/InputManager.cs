using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance = null;
    private bool vertAxisInUse = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region interaction
    public static bool interact(PlayerManager.Player player)
    {
        return (player == PlayerManager.Player.player1 && Input.GetButtonDown("Interact_1")) ||
                (player == PlayerManager.Player.player2 && Input.GetButtonDown("Interact_2"));
    }

    public static bool cancel(PlayerManager.Player player)
    {
        return (player == PlayerManager.Player.player1 && Input.GetButtonDown("Cancel_1")) ||
                (player == PlayerManager.Player.player2 && Input.GetButtonDown("Cancel_2"));
    }
    
    public static float verticalSelection(PlayerManager.Player player)
    {
        float vertical = 0;

        if (player == PlayerManager.Player.player1)
        {
            vertical = Input.GetAxisRaw("Vertical_1");
        }
        else if (player == PlayerManager.Player.player2)
        {
            vertical = Input.GetAxisRaw("Vertical_2");
        }
        
        return instance.updateVertAxisUse(vertical);
    }
    private float updateVertAxisUse(float vert)
    {
        if (vert != 0 && !vertAxisInUse)
        {
            vertAxisInUse = true;
            return vert;
        }
        else if(vert == 0)
        {
            vertAxisInUse = false;
            return 0;
        }
        return 0;
            
    }

    #endregion

    #region movement input
    public static void processMovementInput(PlayerManager.Player player, ref float horizontal, ref float vertical)
    {
        if (player == PlayerManager.Player.player1)
        {
            horizontal = Input.GetAxisRaw("Horizontal_1");
            vertical = Input.GetAxisRaw("Vertical_1");
            InputManager.rotateMovement(ref horizontal, ref vertical);
            
        }
        else if (player == PlayerManager.Player.player2)
        {
            horizontal = Input.GetAxisRaw("Horizontal_2");
            vertical = Input.GetAxisRaw("Vertical_2");
             InputManager.rotateMovement(ref horizontal, ref vertical);
            
        }

    }
    public static void rotateMovement(ref float horizontal, ref float vertical)
    {
        if (horizontal > 0)
        {
            if (vertical > 0)   // 1,1 --> 0,1
            {
                horizontal = 0f;
                vertical = 1f;
            }
            else if (vertical < 0)  //1,-1 -->   1,0
            {
                horizontal = 1f;
                vertical = 0f;
            }
            else             //1,0 --> 1,1
            {
                horizontal = 1f;
                vertical = 1f;
            }
        }
        else if (horizontal < 0)
        {
            if (vertical > 0)    //-1,1 --> -1,0
            {
                horizontal = -1f;
                vertical = 0f;
            }
            else if (vertical < 0)  //-1,-1 --> 0,-1
            {
                horizontal = 0f;
                vertical = -1f;
            }
            else            //-1,0 --> -1,-1
            {
                horizontal = -1f;
                vertical = -1f;
            }
        }
        else
        {
            if (vertical > 0)   //0,1 --> -1,1
            {
                horizontal = -1f;
                vertical = 1f;
            }
            else if (vertical < 0) //0,-1 -->  1,-1
            {
                horizontal = 1f;
                vertical = -1f;
            }
        }

    }
    #endregion
}
