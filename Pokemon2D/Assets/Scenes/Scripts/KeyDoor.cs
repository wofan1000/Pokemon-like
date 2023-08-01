using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Key;

public class KeyDoor : MonoBehaviour
{
    [SerializeField] private Key.KeyType keyType;

    public KeyType GetKeyType()
    {
        return keyType;
    }
}
