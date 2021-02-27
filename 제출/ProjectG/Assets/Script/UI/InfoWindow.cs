using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoWindow : Window
{
    private enum STAT : int
    {
        START,
        LV = 0,
        HP,
        MP,
        EXP,
        ATK,
        DEF,
        RNG,
        SPD,
        APS,
        END
    }

    private PlayerStat playerStat;

    public Text[] stats;

    private void Start()
    {
        playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();
    }

    void Update()
    {
        stats[(int)STAT.LV].text = string.Format("{0:G0}", playerStat.level);
        stats[(int)STAT.HP].text = string.Format("{0:F0} / {1:G0}", playerStat.currentHP, playerStat.maxHP);
        stats[(int)STAT.MP].text = string.Format("{0:F0} / {1:G0}", playerStat.currentMP, playerStat.maxMP);
        stats[(int)STAT.EXP].text = string.Format("{0:G0} / {1:G0}", playerStat.currentEXP, playerStat.maxEXP);
        stats[(int)STAT.ATK].text = string.Format("{0:G0}", playerStat.attackDamage);
        stats[(int)STAT.DEF].text = string.Format("{0:G0}", playerStat.defense);
        stats[(int)STAT.RNG].text = string.Format("{0:F1}", playerStat.attackRange);
        stats[(int)STAT.SPD].text = string.Format("{0:F1}", playerStat.moveSpeed);
        stats[(int)STAT.APS].text = string.Format("{0:F1}", playerStat.intvlAttackTime);
    }

    public override void ShowWindow(Canvas canvas)
    {
        base.ShowWindow(canvas);
    }

    public override void CloseWindow()
    {
        base.CloseWindow();
    }
}
