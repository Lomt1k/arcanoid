using UnityEngine;

public class Bonus : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float circleRadius = 1f;
    public GameObject ballPrefab;

    void FixedUpdate()
    {
        CollisionCheck();
        //движение бонуса
        transform.position = new Vector2(transform.position.x, transform.position.y - (moveSpeed * Time.fixedDeltaTime) );
    }

    /// <summary>
    /// проверка столкновений
    /// </summary>
    void CollisionCheck()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, circleRadius, Vector2.zero);

        if (hit.collider != null)
        {
            //игрок подбирает бонус
            if (hit.collider.CompareTag("Player"))
            {
                Destroy(gameObject);
                GiveBonus();
            }

            //если бонус улетел за пределы карты - уничтожаем
            if (hit.collider.name == "Wall GameOver")
            {
                Destroy(gameObject);
            }
        }
    }

    //выдача случайного бонуса
    void GiveBonus()
    {
        switch (Random.Range(0, 4))
        {
            case 1: //ускорение шаров
                Ball.ChangeSpeed(1.5f);
                ShowMessage.Show("Скорость шаров увеличена");
                break;
            case 2: //замедление шаров
                Ball.ChangeSpeed(Ball.Speed / -3f);
                ShowMessage.Show("Скорость шаров снижена");
                break;
            case 3: //увеличение ширины ракетки
                var playerTransform = GameObject.Find("Player").transform;
                playerTransform.localScale = new Vector3(playerTransform.localScale.x + 1f, playerTransform.localScale.y, playerTransform.localScale.z);
                ShowMessage.Show("Платформа увеличена");
                break;
            default: //создание дополнительного шара
                var pc = FindObjectOfType<PlayerContoller>(); 
                //если к платформе не примагничен шар - спавним шар и магнитим к платформе
                if (pc.magnetBall == null)
                {
                    Vector3 spawnPoint = pc.transform.Find("NewBallSpawnPoint").position;
                    GameObject ball = Instantiate(ballPrefab, spawnPoint, Quaternion.identity);
                    ball.transform.parent = pc.transform;
                    pc.magnetBall = ball;
                }
                //иначе просто спавним его не примагниченным
                else
                {
                    Instantiate(ballPrefab, transform.position, Quaternion.identity);
                }
                ShowMessage.Show("Дополнительный шар");
                break;                
        }
    }
}
