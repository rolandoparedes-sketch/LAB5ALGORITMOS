using UnityEngine;

public class Player : MonoBehaviour
{
    public int str;
    public int dtx;
    public int spd;
    void Start()
    {
        
    }

   void Update()
    {

    }
        

      

    public void EjecutarTurno()
    {
        GameManager.instance.SaveTurn();
        GameManager.instance.MoverEnemigos();
        GameManager.instance.SaveTurn();
    }


}
