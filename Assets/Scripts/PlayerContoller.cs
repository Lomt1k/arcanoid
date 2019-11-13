using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float minX = -4.3f;
    public float maxX = 4.3f;
    public GameObject magnetBall; //мяч, примагниченный к платформе
    public Material magnetMaterial; //материал, в который окрашивается платформа в "намагниченном состоянии"
    public Material standardMaterial;

    private GameObject platformCenter;

    void Start()
    {
        platformCenter = transform.Find("PlatformCenter").gameObject;
    }

    void Update()
    {
        float newX = transform.position.x + (Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime);
        newX = Mathf.Clamp(newX, minX, maxX);
        transform.position = new Vector2(newX, transform.position.y);

        //запускание шара от платформы (при GetKeyDown только в начале игры)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyUp(KeyCode.Space))
        {            
            if (magnetBall != null)
            {
                magnetBall.transform.parent = null;
                magnetBall.GetComponent<Ball>().StartMove();
                magnetBall = null;
            }

            //смена материала (намагниченное - размагниченное состояние)
            if (Input.GetKeyDown(KeyCode.Space)) platformCenter.GetComponent<MeshRenderer>().material = magnetMaterial;
            else if (Input.GetKeyUp(KeyCode.Space)) platformCenter.GetComponent<MeshRenderer>().material = standardMaterial;
        }

    }
}
