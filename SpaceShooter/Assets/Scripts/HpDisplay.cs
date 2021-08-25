using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpDisplay : MonoBehaviour
{
    Image hpImg;
    [SerializeField] Player player;

    // Start is called before the first frame update
    void Start()
    {
        hpImg = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(player.GetHealthPercent());
        hpImg.fillAmount = player.GetHealthPercent();
    }
}
