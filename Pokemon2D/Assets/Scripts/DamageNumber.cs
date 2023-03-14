using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumber : MonoBehaviour
{
    public Text damageText;

    public float lifetime;
    public float moveSpeed = 1;

    public float placmentJitter = .5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject,lifetime) ;
        transform.position = new Vector3 (0f, moveSpeed * Time.deltaTime, 0f) ;
    }

    public void SetDamage(int damageAmount)
    {
        damageText.text = damageAmount.ToString();
        transform.position += new Vector3(Random.Range(-placmentJitter, placmentJitter), Random.Range(-placmentJitter, placmentJitter), 0);
    }
}
