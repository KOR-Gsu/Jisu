using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoWindow : Window
{
    private PlayerStat playerStat;

    public Text[] stats;

    private void Start()
    {
        playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();
    }

    void Update()
    {
        stats[(int)Define.Stat.LV].text = string.Format("{0:G0}", playerStat.level);
        stats[(int)Define.Stat.HP].text = string.Format("{0:F0} / {1:G0}", playerStat.currentHP, playerStat.maxHP);
        stats[(int)Define.Stat.MP].text = string.Format("{0:F0} / {1:G0}", playerStat.currentMP, playerStat.maxMP);
        stats[(int)Define.Stat.EXP].text = string.Format("{0:G0} / {1:G0}", playerStat.currentEXP, playerStat.maxEXP);
        stats[(int)Define.Stat.ATK].text = string.Format("{0:G0}", playerStat.attackDamage);
        stats[(int)Define.Stat.DEF].text = string.Format("{0:G0}", playerStat.defense);
        stats[(int)Define.Stat.RNG].text = string.Format("{0:F1}", playerStat.attackRange);
        stats[(int)Define.Stat.SPD].text = string.Format("{0:F1}", playerStat.moveSpeed);
        stats[(int)Define.Stat.APS].text = string.Format("{0:F1}", playerStat.intvlAttackTime);
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
