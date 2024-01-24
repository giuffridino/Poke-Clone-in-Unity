using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattlePoke : MonoBehaviour
{
    [SerializeField] PokemonBase pokemonBase;
    [SerializeField] int level;
    [SerializeField] bool isPlayerPokemon;

    public Pokemon Pokemon { get; set; }

    Image image;
    Vector3 originalPos;
    Color originalColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.material.color = new Color(image.material.color.r, image.material.color.g, image.material.color.b, 1f);
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }

    public void Setup()
    {
        image.material.color = new Color(image.material.color.r, image.material.color.g, image.material.color.b, 1f);
        Pokemon = new Pokemon(pokemonBase, level);
        if (isPlayerPokemon)
        {
            image.sprite = Pokemon.Base.BackSprite;
        }
        else
        {
            image.sprite = Pokemon.Base.FrontSprite;
        }
        PlayEnterAnimation();
    }

    public void PlayEnterAnimation()
    {
        if (isPlayerPokemon)
            image.transform.localPosition = new Vector3(-500f, originalPos.y);
        else
            image.transform.localPosition = new Vector3(500f, originalPos.y);

        image.transform.DOLocalMoveX(originalPos.x, 1f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayerPokemon)
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50f, 0.1f));
        else
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50f, 0.1f));

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.1f));
    }

    public void PlayHitAnimation()
    {
        // image.DOColor(originalColor, 1f);
        var sequence = DOTween.Sequence();
        if (isPlayerPokemon)
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 10f, 0.1f));
        else
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 10f, 0.1f));

        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.1f));

        sequence.Append(image.material.DOFade(0f, 0.1f));
        sequence.Append(image.material.DOFade(1f, 0.1f));
        sequence.Append(image.material.DOFade(0f, 0.1f));
        sequence.Append(image.material.DOFade(1f, 0.1f));
        sequence.Append(image.material.DOFade(0f, 0.1f));
        sequence.Append(image.material.DOFade(1f, 0.1f));
    }

    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
        sequence.Join(image.material.DOFade(0f, 0.5f));
    }
}
