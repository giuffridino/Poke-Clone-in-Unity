using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;
    private Color healthGreen;
    private Color healthGreenTop;
    private Color healthYellow;
    private Color healthYellowTop;
    private Color healthRed;
    private Color healthRedTop;
    private float yellowThreshold = 0.5f;
    private float redThreshold = 0.20f;

    private GameObject healthTop;
    private bool initialSetupComplete = false;
    private BattleHUD battleHUDScript;

    private void Start()
    {
        InitialSetup();
    }

    public void SetHP(float hpNormalized)
    {
        health.transform.localScale = new Vector3(hpNormalized, 1.0f);
        UpdateHealthBarColor();
    }

    public IEnumerator SetHPSmooth(float newHP)
    {
        float currHP = health.transform.localScale.x;
        float changeAmt = currHP - newHP;
        while(currHP - newHP > Mathf.Epsilon)
        {
            currHP -= changeAmt * Time.deltaTime * 1.1f;
            health.transform.localScale = new Vector3(currHP, 1.0f);
            battleHUDScript.SetHPText(currHP);
            UpdateHealthBarColor();
            yield return null;
        }
        health.transform.localScale = new Vector3(newHP, 1.0f);
        UpdateHealthBarColor();
    }

    public void UpdateHealthBarColor()
    {
        if (!initialSetupComplete)
            InitialSetup();
        
        health.GetComponent<UnityEngine.UI.Image>().color = healthGreen;
        healthTop.GetComponent<UnityEngine.UI.Image>().color = healthGreenTop;
        if (health.transform.localScale.x <= redThreshold)
        {
            health.GetComponent<UnityEngine.UI.Image>().color = healthRed;
            healthTop.GetComponent<UnityEngine.UI.Image>().color = healthRedTop;
        }
        else if (health.transform.localScale.x <= yellowThreshold)
        {
            health.GetComponent<UnityEngine.UI.Image>().color = healthYellow;
            healthTop.GetComponent<UnityEngine.UI.Image>().color = healthYellowTop;
        }
    }

    private void InitialSetup()
    {
        healthTop = health.transform.Find("HealthTop").gameObject;
        ColorUtility.TryParseHtmlString("#70F8A8", out healthGreen);
        ColorUtility.TryParseHtmlString("#58D080", out healthGreenTop);
        ColorUtility.TryParseHtmlString("#F8E038", out healthYellow);
        ColorUtility.TryParseHtmlString("#C8A808", out healthYellowTop);
        ColorUtility.TryParseHtmlString("#E46241", out healthRed);
        ColorUtility.TryParseHtmlString("#A8534E", out healthRedTop);
        battleHUDScript = GetComponentInParent<BattleHUD>();
        initialSetupComplete = true;
    }
}
