using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] TextMeshProUGUI currentHPText;
    [SerializeField] TextMeshProUGUI maxHPText;
    public bool isPlayerHUD;

    Pokemon activePokemon;

    public void SetData(Pokemon pokemon)
    {
        activePokemon = pokemon;
        nameText.text = pokemon.Base.PokeName;
        levelText.text = pokemon.Level.ToString(); 
        hpBar.SetHP((float) pokemon.HP / pokemon.MaxHP);
        SetHPText((float) pokemon.HP / pokemon.MaxHP);
    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float) activePokemon.HP / activePokemon.MaxHP);
    }

    public void SetHPText(float hpNormalized)
    {
        if (isPlayerHUD)
        {
            maxHPText.text = activePokemon.MaxHP.ToString();
            currentHPText.text = ((int)(hpNormalized * activePokemon.MaxHP)).ToString();
        }
    }
}
