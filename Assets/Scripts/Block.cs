using UnityEngine;

public class Block : MonoBehaviour
{
    public int health = 1;
    public int chanceOfBonus = 10; //шанс выпадения бонуса в процентах
    public GameObject bonusPrefab;

    //при столкновении с шариком
    public void TakeDamage()
    {
        health--;
        if (health < 1)
        {
            Destroy(gameObject);
            //выпадение бонусов, начиная с 3 уровня
            if (GameController.Instance.Level >= 3)
            {
                if (Random.Range(0, 100) < chanceOfBonus)
                {
                    Instantiate(bonusPrefab, transform.position, Quaternion.identity);
                }
            }

            //когда уничтожается последний блок - уровень пройден
            if (GameObject.FindGameObjectsWithTag("Block").Length <= 1)
            {
                GameController.Instance.CompleteLevel();
            }
        }        
    }
}
