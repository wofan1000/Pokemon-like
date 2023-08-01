using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumber : MonoBehaviour
{
    public Text damageText;

    public float moveSpeed = 1;

    public float placmentJitter = .5f;

    RectTransform rectTrans;
    private void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        damageText.gameObject.SetActive(false);  

    }
    // Update is called once per frame
    void Update()
    {
      
        rectTrans.localPosition = new Vector2 (0f, moveSpeed * Time.deltaTime) ;
    }

    public void SetDamage(int damageAmount, Vector2 pos)
    {
       
        damageText.text = damageAmount.ToString();
        rectTrans.localPosition = new Vector2(Random.Range(-placmentJitter, placmentJitter) + pos.x, Random.Range(-placmentJitter, placmentJitter) + pos.y);
    }
}
