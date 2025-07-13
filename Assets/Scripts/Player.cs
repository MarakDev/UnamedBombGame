using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    #region Debug

    public TMP_Text facingright;

    #endregion


    #region Public Variables
    [SerializeField] public float walk_speed;
    [SerializeField] public float jump_force;

    //inventario que consiste en el tipo de bomba y si esta disponible para colocar
    //[SerializeField] public Dictionary<GameObject, bool> inventory;
    [SerializeField] public List<GameObject> bomb_inventory;

    [Header("GroundCheck")]
    [SerializeField] public Vector2 boxSize;
    [SerializeField] public float castDistance;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public LayerMask sliderLayer;

    #endregion

    #region Internal Variables
    public Rigidbody2D rb2D { get; set; }
    public Animator animator { get; set; }
    public bool isFacingRight { get; private set; }
    public Transform bomb_spawnPoint { get; set; }

    //variables para controlar el salto explosivo - cambiar a el gamemanager
    public bool has_beenLaunched { get; set; }
    public float explosive_power { get; set; } //Poder raw de la bomba
    public Vector2 explosive_force { get; set; } //Direccion de la explosion ya calculada

    public bool is_Colision_Wall { get; set; }

    private GameObject current_bombSelected { get; set; }
    public ParticleSystem smokeTrail { get; set; }

    #endregion


    #region State Machine

    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerWalkState walkState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerCarryState carryState { get; private set; }
    public PlayerCarryWalkState carryWalkState { get; private set; }
    public PlayerDropBombState dropBombState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerLaunchedState launchedState { get; private set; }

    #endregion

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        bomb_spawnPoint = transform.Find("Bomb_SpawnPoint");

        isFacingRight = true;

        smokeTrail = transform.Find("SmokeTrail_Player").GetComponent<ParticleSystem>();
        smokeTrail.Stop();

        //state machine
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine);
        walkState = new PlayerWalkState(this, stateMachine);
        jumpState = new PlayerJumpState(this, stateMachine);
        carryState = new PlayerCarryState(this, stateMachine);
        carryWalkState = new PlayerCarryWalkState(this, stateMachine);
        dropBombState = new PlayerDropBombState(this, stateMachine);
        fallState = new PlayerFallState(this, stateMachine);
        launchedState = new PlayerLaunchedState(this, stateMachine);
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        FlipSprite(rb2D.velocity);
        stateMachine.currentPlayerState.FrameUpdate();

        currentBombSelected("Standar_Bomb");

        //Debug.Log(has_beenLaunched);
        //Debug.Log(stateMachine.currentPlayerState);
        //recarga la escena con la Q
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager._GMinstance.ReloadScene("MainScene");
        }

        //Debug.Log(stateMachine.currentPlayerState.ToString());
        //Debug.Log("groudned: " + stateMachine.currentPlayerState.IsGrounded());

        //DEBUGZONE
        facingright.text = "facing right: " + isFacingRight;


    }

    private void FixedUpdate()
    {
        stateMachine.currentPlayerState.PhysicsUpdate();
    }


    //Funciones especificas de player

    public void FlipSprite(Vector2 velocity)
    {
        //cuando el personaje mira a la derecha
        if(isFacingRight && velocity.x < -0.01f)
        {
            Vector3 rotation = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotation);
            isFacingRight = !isFacingRight;
        }
        //cuando el personaje mira a la izquierda
        else if (!isFacingRight && velocity.x > 0.01f)
        {
            Vector3 rotation = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotation);
            isFacingRight = !isFacingRight;
        }
    }


    public void SetBomb()
    {
        //TODO
        //añadir la funcion de inventario

        //foreach(KeyValuePair<GameObject, bool> bomb in inventory)
        //{
        //    if(bomb.Key.name == bombType && bomb.Value)
        //        Instantiate(bomb_inventory, bomb_spawnPoint.position, transform.rotation);

        //}
        if(current_bombSelected != null)
            Instantiate(current_bombSelected, bomb_spawnPoint.position, transform.rotation);

    }

    private void currentBombSelected(string bomb_name)
    {
        foreach(var bomb in bomb_inventory)
        {
            if (bomb.name == bomb_name)
                current_bombSelected = bomb;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }
}
