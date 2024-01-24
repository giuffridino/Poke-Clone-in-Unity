using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public enum Direction {
        NORTH,
        SOUTH,
        EAST,
        WEST,
    }

    [SerializeField] Transform movePoint;
    [SerializeField] Tilemap collisions;
    [SerializeField] LayerMask grassTiles;
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] Direction facingDirection;
    private Animator animator;
    private bool blockMovement = false;
    private bool encounterCheck = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        // GameData.Instance.SetPlayerPosition(transform.position);
        movePoint.parent = null;
    }

    private void Start()
    {
        // Debug.Log("Starting up Player Controller" + transform.position);
        Debug.Log("Getting position:" + GameData.Instance.GetPlayerPosition());
        transform.position = GameData.Instance.GetPlayerPosition();
        movePoint.position = transform.position;
        encounterCheck = false;
        blockMovement = false;
    }
    
    void Update()
    {
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.01f)
        {
            CheckForEncounters();
            transform.position = movePoint.position;
            animator.SetBool("isMoving", false);
        }
 
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.00f && blockMovement == false)
        {
            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                Vector3Int collisionMapTile = collisions.WorldToCell(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f));
                if(collisions.GetTile(collisionMapTile) == null)
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                }
                else
                {
                    animator.SetBool("isMoving", false);
                }
                animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("Vertical", 0f);
                facingDirection = Input.GetAxisRaw("Horizontal") == 1 ? Direction.EAST : Direction.WEST;
                // Debug.Log(facingDirection);
            }
            else if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                Vector3Int collisionMapTile = collisions.WorldToCell(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f));
                if(collisions.GetTile(collisionMapTile) == null)
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                }
                else
                {
                    animator.SetBool("isMoving", false);
                }
                animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
                animator.SetFloat("Horizontal", 0f);
                facingDirection = Input.GetAxisRaw("Vertical") == 1 ? Direction.NORTH : Direction.SOUTH;
                // Debug.Log(facingDirection);
            }
            else
            {
                CheckForEncounters();
                animator.SetBool("isMoving", false);
            }
        }
        else if(encounterCheck == false)
        {
            animator.SetBool("isMoving", true);
        }
    }

    public Direction GetFacingDirection()
    {
        return facingDirection;
    }

    void CheckForEncounters()
    {
        // Debug.Log("Check");
        if (Physics2D.OverlapCircle(transform.position, 0.1f, grassTiles) != null && animator.GetBool("isMoving") != false && encounterCheck == false)
        {
            Debug.Log("CheckEncounters()");
            if (UnityEngine.Random.Range(1, 101) <= 10)
            {
                Debug.Log("Encounter");
                encounterCheck = true;
                blockMovement = true;
                movePoint.position = transform.position;
                animator.SetBool("isMoving", false);
                Debug.Log("Setting position:" + transform.position);
                GameData.Instance.SetPlayerPosition(transform.position);
                SceneLoader.Instance.LoadBattleScene();
            }
        }
    }
}

