using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterectableObjects : MonoBehaviour
{
    public GameObject breakableObject;
    public GameObject breakedObject;
    public GameObject fire;
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Break()
    {
        /*float posY = player.transform.position.y - breakableObject.transform.position.y;
        if(posY >= -1 && posY <= 1){
            posY = 0;
        }
        float posX = player.transform.position.x - breakableObject.transform.position.x;
        
        if (posY < 0)
        {
            breakedObject.transform.position = new Vector3(breakableObject.transform.position.x, breakableObject.transform.position.y + 5, breakableObject.transform.position.z);
        }
        else if(posY > 0)
        {
            breakedObject.transform.position = new Vector3(breakableObject.transform.position.x, breakableObject.transform.position.y - 5, breakableObject.transform.position.z);
        }
        else{
           
        }*/
        //criando o objeto quebrado
        breakedObject = Instantiate(breakedObject);
        //posicionando o objeto quebrado no lugar do objeto inicial
        breakedObject.transform.position = breakableObject.transform.position;
        //destruindo o objeto inicial
        Destroy(breakableObject);
        //ativando o objeto quebrado
        breakedObject.SetActive(true);

        //criando fogo acima da tocha
        fire = Instantiate(fire);

        fire.transform.position = new Vector3(breakedObject.transform.position.x / 2, breakedObject.transform.position.y + 1.2f, breakedObject.transform.position.z);
        fire.SetActive(true);
        //criando fogo abaixo da tocha
        fire = Instantiate(fire);
        fire.transform.position = new Vector3(breakedObject.transform.position.x / 2, breakedObject.transform.position.y - 1.2f, breakedObject.transform.position.z);
        fire.SetActive(true);
        //criando fogo a direita em cima da tocha
        fire = Instantiate(fire);
        fire.transform.position = new Vector3(breakedObject.transform.position.x + 0.25f, (breakedObject.transform.position.y + (float)((breakableObject.transform.position.y + 1)/2)), breakedObject.transform.position.z);
        fire.SetActive(true);
        //criando fogo a direita embaixo da tocha
        fire = Instantiate(fire);
        fire.transform.position = new Vector3(breakedObject.transform.position.x + 0.25f, (breakedObject.transform.position.y + (float)((breakableObject.transform.position.y + 1)/2)), breakedObject.transform.position.z);
        fire.SetActive(true);
        //criando fogo no meio direito da tocha
        fire = Instantiate(fire);
        fire.transform.position = new Vector3(breakedObject.transform.position.x + 0.25f, (breakedObject.transform.position.y - (float)((breakableObject.transform.position.y + 1)/4)), breakedObject.transform.position.z);
        fire.SetActive(true);
        //criando fogo no meio esquerdo da tocha
        fire = Instantiate(fire);
        fire.transform.position = new Vector3(breakedObject.transform.position.x - 0.25f, (breakedObject.transform.position.y + (float)((breakableObject.transform.position.y + 1)/4)), breakedObject.transform.position.z);
        fire.SetActive(true);
        //criando fogo a esquerda em cima da tocha
        fire = Instantiate(fire);
        fire.transform.position = new Vector3(breakedObject.transform.position.x - 0.25f, (breakedObject.transform.position.y + (float)((breakableObject.transform.position.y + 1)/2)), breakedObject.transform.position.z);
        fire.SetActive(true);
        //criando fogo a esquerda embaixo da tocha
        fire = Instantiate(fire);
        fire.transform.position = new Vector3(breakedObject.transform.position.x - 0.25f, (breakedObject.transform.position.y - (float)((breakableObject.transform.position.y + 1)/2)), breakedObject.transform.position.z);
        fire.SetActive(true);
        // //destroi a tocha depois de 2 segundos
        // Destroy(breakedObject, 2.0f);
    }
}
