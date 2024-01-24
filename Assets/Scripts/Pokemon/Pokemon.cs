using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    public PokemonBase Base { get; set; }
    public int Level { get; set; }

    public int HP { get; set; }

    public List<Move> Moves { get; set; }

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        Base = pBase;
        Level = pLevel;
        HP = MaxHP;

        Moves = new List<Move>();
        foreach(var move in Base.LearnableMoves)
        {
            if (move.LearnLevel <= Level)
            {
                Moves.Add(new Move(move.Base));
            }
            if (Moves.Count > 4)
            {
                break;
            }
        }
    }

    public int Attack
    {
        get { return Mathf.FloorToInt(Base.Attack * Level / 100f) + 5; }
    }
    public int Defense
    {
        get { return Mathf.FloorToInt(Base.Defense * Level / 100f) + 5; }
    }
    public int SpAttack
    {
        get { return Mathf.FloorToInt(Base.SpAttack * Level / 100f) + 5; }
    }
    public int SpDefense
    {
        get { return Mathf.FloorToInt(Base.SpDefense * Level / 100f) + 5; }
    }
    public int Speed
    {
        get { return Mathf.FloorToInt(Base.Speed * Level / 100f) + 5; }
    }
    public int MaxHP
    {
        get { return Mathf.FloorToInt(Base.MaxHP * Level / 100f) + 10; }
    }

    public DamageDetails TakeDamage(Move move, Pokemon attacker)
    {
        float criticalHit = 1f;
        if (UnityEngine.Random.value * 100f < 6.25f)
        {
            criticalHit = 2f;
        }
        float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) * TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2);

        DamageDetails damageDetails = new DamageDetails();
        damageDetails.Critical = criticalHit;
        damageDetails.TypeEffectiveness = type;
        damageDetails.Fainted = false;

        float modifiers = UnityEngine.Random.Range(0.85f, 1f) * type * criticalHit;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float) attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        // Debug.Log($"damage: {damage}");
        // Debug.Log($"HP: {HP}");
        HP -= damage;
        // Debug.Log($"HP: {HP}");
        if (HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }
        return damageDetails;
    }

    public Move GetRandomMove()
    {
        int r = UnityEngine.Random.Range(0, Moves.Count);
        return Moves[r];
    }
}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set;}
    public float TypeEffectiveness { get; set;}
}