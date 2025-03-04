using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationEnv : MonoBehaviour
{
    public int numberObject; //кількість об'єктів
    private int generatedObject = 0;
    public float minRange, maxRange; //розмір території
    public GameObject[] objects; //об'єкти

    void Update()
    {
        //рахує кількість об'єктів
        if (generatedObject < numberObject)
        {
            Generate();
            generatedObject++;
        }

    }
    //генерація
    public void Generate()
    {
        int rand = Random.Range(0, objects.Length);
        var cell = Instantiate(objects[rand], transform.position, Quaternion.identity);
        cell.transform.position = new Vector3(Random.Range(minRange, maxRange), Random.Range(minRange, maxRange), transform.position.z);

    }


}
