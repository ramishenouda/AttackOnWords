using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerDestroy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Earth")
        {
            GameManager.Lives--;
            if (GameManager.Lives <= 0)
                GameManager.instance.EndGame();

            GameManager.instance.LivesText.text = "Lives : " + GameManager.Lives;

            gameObject.GetComponentInParent<Enemy>().DestroyGameObject();
        }
    }
}
