using UnityEngine;

public class Ball : MonoBehaviour
{
    private float circleRadius; //радиус шарика (для обработки столкновений)
    private Vector2 moveDir; //направление движения шарика, от -1 до +1
    private static float moveSpeed = 5f; //скорость движения шарика, статический чтобы при изменении менялся сразу из всех шаров (при подборе бонуса)
    private string lastHitObjectName; //имя последнего объекта, с которым столкнулся шарик

    private Transform platformLeft;
    private Transform platformRight;
    private Transform platformCenter;
    private PlayerContoller pc;
    private AudioSource audioSource;

    public static float Speed
    {
        get => moveSpeed;
    }

    void Start()
    {
        circleRadius = transform.localScale.x / 2; //получаем радиус шарика (для обработки столкновений в дальнейшем)

        platformCenter = GameObject.Find("PlatformCenter").transform;
        platformRight = GameObject.Find("PlatformRight").transform;
        platformLeft = GameObject.Find("PlatformLeft").transform;
        pc = FindObjectOfType<PlayerContoller>();
        audioSource = GetComponent<AudioSource>();

        //если при появлении шара он не примагничен к платформе - приводим его в движение
        if (pc.magnetBall != gameObject) StartMove();
    }

    void FixedUpdate()
    {
        CollisionCheck();
        MovementUpdate();
    }

    /// <summary>
    /// обработка столкновений
    /// </summary>
    private void CollisionCheck()
    {
        if (moveDir == Vector2.zero) return; //столкновения не обрабатываются если шарик не двигается (примагничен к платформе)
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, circleRadius, Vector2.zero);

        if (hit.collider != null && hit.collider.name != lastHitObjectName)
        {
            lastHitObjectName = hit.collider.name;
            audioSource.Play(); //звук столкновения

            //изменение направления движения при столкновении с любым физическим объектом
            float deltaX = hit.point.x - transform.position.x;
            float deltaY = hit.point.y - transform.position.y;
            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY)) moveDir.x = -moveDir.x;
            else moveDir.y = -moveDir.y;

            //при столкновении с платформой
            if (hit.collider.CompareTag("Player"))
            {
                //если стоклновение произошло правее центра платформы - мяч летит вправо, иначе - влево
                moveDir.y = 1; //фикс неприятного бага, когда летит вниз (редко)
                if (hit.point.x > platformCenter.position.x) 
                    moveDir.x = 1f;
                else
                    moveDir.x = -1f;

                //примагничивание к платформе при зажатом пробеле
                if (Input.GetKey(KeyCode.Space) && pc.magnetBall == null)
                {
                    transform.parent = hit.collider.transform;
                    transform.localPosition = new Vector2(transform.localPosition.x, 1.2f);
                    pc.magnetBall = gameObject;
                    moveDir = Vector2.zero;
                }
            }

            //если мяч упал за пределы карты
            if (hit.collider.name == "Wall GameOver")
            {
                //если на карте более 1 шара - этот кар уничтожается, но игра не заканчивается
                if (FindObjectsOfType<Ball>().Length > 1)
                {
                    Destroy(gameObject);
                    return;
                }
                GameController.Instance.GameOver();
            }

            //при столкновении с блоком
            if (hit.collider.CompareTag("Block"))
            {
                hit.collider.GetComponent<Block>().TakeDamage();
                //перекрашиваем шарик в цвет блока, с которым он столкнулся
                GetComponent<MeshRenderer>().material = hit.collider.GetComponent<MeshRenderer>().material;
            }
        }

    }

    /// <summary>
    /// движение шарика
    /// </summary>
    private void MovementUpdate()
    {
        float deltaX = moveDir.x * moveSpeed * Time.fixedDeltaTime;
        float deltaY = moveDir.y * moveSpeed * Time.fixedDeltaTime;
        transform.position = new Vector2(transform.position.x + deltaX, transform.position.y + deltaY);
    }

    /// <summary>
    /// начать движение шарика
    /// </summary>
    public void StartMove()
    {
        if (transform.position.x > platformCenter.transform.position.x) moveDir.x = 1;
        else moveDir.x = -1;

        moveDir.y = 1;
    }

    /// <summary>
    /// остановить движение шарика
    /// </summary>
    public void StopMove()
    {
        moveDir = Vector2.zero;
    }

    /// <summary>
    /// изменение скорости
    /// </summary>
    /// <param name="deltaSpeed"></param>
    public static void ChangeSpeed(float deltaSpeed)
    {
        moveSpeed += deltaSpeed;
    }

    /// <summary>
    /// установить скорость
    /// </summary>
    /// <param name="speed"></param>
    public static void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }
}
