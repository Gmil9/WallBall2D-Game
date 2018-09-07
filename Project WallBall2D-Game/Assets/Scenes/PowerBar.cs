using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour {

    [SerializeField] int speed;
    Image pBar;

	void Start () {
        pBar = GetComponent<Image>();
	}

	void Update () {
        pBar.fillAmount = Mathf.PingPong(Time.time * speed, 1);
	}
}
