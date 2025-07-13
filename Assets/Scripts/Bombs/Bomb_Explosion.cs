using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Explosion : MonoBehaviour
{
    //cuanto radio tiene la bomba
    [SerializeField] private float radious = 2;
    //que potencia tiene la bomba
    [SerializeField, Range(1,10)] public int power = 5;
    //cuanto tarda la bomba en explotar
    [SerializeField] public int time_to_explode = 3;
    //punto de referencia para calcular el vector explosion

    [Header("")]
    [SerializeField] private Transform bomb_explosion_point;
    //objeto que reproduce la animacion de la explosion de fuego
    [SerializeField] private GameObject explosion_visual;
    //lista de objetos tipo escombros de la bomba
    [SerializeField] private List<GameObject> debries;

    private bool trayectoryDrawEnabled = true;

    private ParticleSystem thirdPhaseRope_particles;
    private float debri_force = 0;
    private List<GameObject> debries_spawned;

    private Animator animator;

    void Start()
    {
        power += 10;

        animator = GetComponentInChildren<Animator>();
        thirdPhaseRope_particles = GetComponentInChildren<ParticleSystem>();

        //asignacion de debries aleatorios para cada bomba
        int random_debrisAmount = Random.Range(2, 5);
        debries_spawned = new List<GameObject>();

        for (int i = 0; i < random_debrisAmount; i++)
        {
            debries_spawned.Add(debries[Random.Range(0, debries.Count)]);
        }

        //empieza la cuenta atras de la bomba, para que visualmente se vea
        StartCoroutine(StartExplosionVisuals());

        //Invoke para explotar la bomba
        Invoke("Explode", time_to_explode);

    }


    private void Update()
    {
        //Debug
        Trayectory_Draw();
    }

    private void Trayectory_Draw()
    {
        if (trayectoryDrawEnabled)
        {
            Collider2D[] objects_at_radious = Physics2D.OverlapCircleAll(transform.position, radious);

            foreach (Collider2D collider_x in objects_at_radious)
            {
                //se toma el componente rigidbody de todos los afectados
                Rigidbody2D rb = collider_x.GetComponent<Rigidbody2D>();

                //primero comprueba varios valores antes de ejecutar el codigo de la explosion, lo primero es que el rigidbody no puede ser null
                //segundo se exculye a si mismo ya que si no puede cambiar la direccion de la fuerza del resto de objetos
                //tercero solo le aplicara fuerza a los objetos que tengan la tag canbelaunched
                if (rb != null && rb != this.GetComponent<Rigidbody2D>() && collider_x.gameObject.CompareTag("CanBeLaunched"))
                {
                    //calculamos el vector direccion con la posicion del collider menos la del pto de explosion de la bomba
                    Vector2 direction = collider_x.transform.position - bomb_explosion_point.position;

                    //se calcula el multiplicador de fuerza segun proximidad
                    float distance_from_center = radious - direction.magnitude;
                    //se hace un calculo del vector explosion
                    Vector2 explosion_force = direction.normalized * (float)power * radiousMult(distance_from_center) * 2;
                    //explosion_force = new Vector2(explosion_force.x * .5f, explosion_force.y);
                    explosion_force = new Vector2(explosion_force.x, explosion_force.y * 0.75f);

                    if (collider_x.GetComponent<Trayectory_Drawer>() != null)
                        collider_x.GetComponent<Trayectory_Drawer>().DrawTrajectory(collider_x.transform.position, explosion_force, rb.drag, rb.gravityScale);

                }
            }
        }
    }

    private void Explode()
    {
        trayectoryDrawEnabled = false;
        //llamada del GM para avisarle de que una bomba ha explotado
        GameManager._GMinstance.bombExploded = true;

        //se toma un array de todos los objetos en el radio de explosion para que sean procesados y se calcule la fuerza de explosion
        Collider2D[] objects_at_radious = Physics2D.OverlapCircleAll(transform.position, radious);

        foreach(Collider2D collider_x in objects_at_radious)
        {
            //se toma el componente rigidbody de todos los afectados
            Rigidbody2D rb = collider_x.GetComponent<Rigidbody2D>();

            //primero comprueba varios valores antes de ejecutar el codigo de la explosion, lo primero es que el rigidbody no puede ser null
            //segundo se exculye a si mismo ya que si no puede cambiar la direccion de la fuerza del resto de objetos
            //tercero solo le aplicara fuerza a los objetos que tengan la tag canbelaunched
            if (rb != null && rb != this.GetComponent<Rigidbody2D>() && collider_x.gameObject.CompareTag("CanBeLaunched"))
            {
                //calculamos el vector direccion con la posicion del collider menos la del pto de explosion de la bomba
                Vector2 direction = collider_x.transform.position - bomb_explosion_point.position;

                //se calcula el multiplicador de fuerza segun proximidad
                float distance_from_center = radious - direction.magnitude;
                //se hace un calculo del vector explosion
                Vector2 explosion_force = direction.normalized * (float)power * radiousMult(distance_from_center) * 2;
                //explosion_force = new Vector2(explosion_force.x * .5f, explosion_force.y);
                explosion_force = new Vector2(explosion_force.x, explosion_force.y * 0.75f);

                //caso exclusivo del jugador
                if (collider_x.GetComponent<Player>() != null)
                {
                    ////se activa la tag del jugador de que ha sido lanzado para pasar al estado launched
                    //collider_x.GetComponent<Player>().has_beenLaunched = true;
                    //collider_x.GetComponent<Player>().explosive_force = explosion_force;
                    //collider_x.GetComponent<Player>().explosive_power = power;

                    //hitstop
                    int frameStop = (int)(explosion_force.magnitude * 2);
                    GameManager._GMinstance.FrameStop(frameStop);

                    if (collider_x.GetComponent<Trayectory_Explosion>() != null)
                        collider_x.GetComponent<Trayectory_Explosion>().StartExplosion(explosion_force, direction);

                }
                else //en el resto de casos se aplica la fuerza de esta manera
                {
                    if(collider_x.GetComponent<Trayectory_Explosion>() != null)
                        collider_x.GetComponent<Trayectory_Explosion>().StartExplosion(explosion_force, direction);

                    debri_force = explosion_force.magnitude * 0.5f;

                }

            }
        }

        //se llama al script destruir bomba

        DestruirBomba();
    }

    private float radiousMult(float distance)
    {
        float distance_from_center = radious - distance;

        //si el objeto se encuentra en el treshold del anillo exterior, se multiplicara por 0.75f
        if (distance_from_center > radious * 0.75f)
            return 0.75f;
        //si el objeto esta en el anillo intermedio x1
        else if (distance_from_center > radious * 0.35f)
            return 1f;
        //si el objeto esta en el anillo interior x1.25
        else if (distance_from_center < radious * 0.35f)
            return 1.2f;

        else return 0f;
    }

    public IEnumerator StartExplosionVisuals()
    {
        float cooldown_explosion = time_to_explode;

        yield return new WaitForSeconds(cooldown_explosion / 3);

        animator.Play("SECOND_PHASE");

        yield return new WaitForSeconds(cooldown_explosion / 3);

        animator.Play("THIRD_PHASE");
        thirdPhaseRope_particles.Play();
    }

    private void SummonDebries()
    {
        //se spawnean los escombros y se les da caracteristicas aleatorias para mayor realismo
        foreach (var debri in debries_spawned)
        {
            //rotacion aleatoria
            Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            //fuerza aleatoria, aunque siempre tiene un valor mayor en el eje y para salir hacia arriba
            Vector2 vectorForce = new Vector2(Random.Range(-18, 18) * 0.1f, Random.Range(12, 18) * 0.1f);
            //la posicion en cualquier punto del sprite de la bomba
            Vector3 relativePosition = new Vector3(Random.Range(-16, 16) * 0.01f, Random.Range(-16, 16) * 0.01f, 0);

            //se crea el objeto debri y despues se le aplica una fuerza de lanzamiento
            GameObject debri_impulse = Instantiate(debri, transform.position + relativePosition, randomRotation);

            debri_impulse.GetComponent<Rigidbody2D>().AddForce(vectorForce * debri_force, ForceMode2D.Impulse);
        }
    }

    //script final de la vida de la bomba
    public void DestruirBomba()
    {
        //se inicia la animacion de la explosion
        Instantiate(explosion_visual, this.transform.position, Quaternion.identity);
        SummonDebries();

        //finalmente se destruye el objeto bomba
        Destroy(this.gameObject);
    }

    public void OnDrawGizmos()
    {
        //debug para ver el radio de explosion en tiempo real

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radious); //radio exterior

        Gizmos.color = new Color(1, 0.5f, 0.3f, 1);
        Gizmos.DrawWireSphere(transform.position, radious * 0.75f); //radio interior

        Gizmos.color = new Color(1, 1f, 0.3f, 1);
        Gizmos.DrawWireSphere(transform.position, radious * 0.35f); //radio mega interior
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, radious);

    }


}
