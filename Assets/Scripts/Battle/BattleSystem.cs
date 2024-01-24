using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public enum BattleState {
    Start,
    PlayerAction,
    PlayerMove,
    EnemyMove,
    Busy
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattlePoke playerPokemon;
    [SerializeField] BattleHUD playerHud;
    [SerializeField] BattlePoke enemyPokemon;
    [SerializeField] BattleHUD enemyHud;
    [SerializeField] BattleDialogBox battleDialogBox;


    BattleState state;
    int currentAction = 0;
    // bool firstPlayerAction = true;

    private void Start()
    {
        battleDialogBox.InitialDialogSetup();
        StartCoroutine(SetupBattle());
        // firstPlayerAction = true;
    }

    private void Update()
    {
        // Debug.Log(state.ToString());
        switch (state)
        {
            case BattleState.PlayerAction:
                // Debug.Log("Handling ActionSelection");
                HandleActionSelection();
                break;
            case BattleState.PlayerMove:
                // Debug.Log("Handling MoveSelection");
                HandleMoveSelection();
                break;
            default:
                break;
        }
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     SceneLoader.Instance.LoadBattleScene();
        // }
        // if (Input.GetKeyDown(KeyCode.B))
        // {
        //     SceneLoader.Instance.LoadPreviousScene();
        // }
    }

    public IEnumerator SetupBattle()
    {
        playerPokemon.Setup();
        playerHud.SetData(playerPokemon.Pokemon);
        enemyPokemon.Setup();
        enemyHud.SetData(enemyPokemon.Pokemon);

        yield return battleDialogBox.TypeDialog($"Wild {enemyPokemon.Pokemon.Base.PokeName} appeared!");

        PlayerAction();
    }

    private IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;
        Move move = playerPokemon.Pokemon.Moves[currentAction];

        yield return battleDialogBox.TypeDialog($"{playerPokemon.Pokemon.Base.PokeName} used\n{move.Base.Name}!");

        playerPokemon.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);
        enemyPokemon.PlayHitAnimation();

        DamageDetails damageDetails = enemyPokemon.Pokemon.TakeDamage(move, playerPokemon.Pokemon);
        yield return enemyHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        if (damageDetails.Fainted)
        {
            enemyPokemon.PlayFaintAnimation();
            yield return battleDialogBox.TypeDialog($"Foe {enemyPokemon.Pokemon.Base.PokeName}\nfainted!");
        }
        else
        {
            StartCoroutine(PerformEnemyMove());
        }
    }

    private IEnumerator PerformEnemyMove()
    {
        state = BattleState.EnemyMove;
        Move move = enemyPokemon.Pokemon.GetRandomMove();
        yield return battleDialogBox.TypeDialog($"Foe {enemyPokemon.Pokemon.Base.PokeName} used\n{move.Base.Name}!");

        enemyPokemon.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);
        playerPokemon.PlayHitAnimation();

        DamageDetails damageDetails = playerPokemon.Pokemon.TakeDamage(move, enemyPokemon.Pokemon);
        yield return playerHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        if (damageDetails.Fainted)
        {
            playerPokemon.PlayFaintAnimation();
            yield return battleDialogBox.TypeDialog($"{playerPokemon.Pokemon.Base.PokeName}\nfainted!");
        }
        else
        {
            PlayerAction();
        }
    }

    private IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1f)
        {
            yield return battleDialogBox.TypeDialog("A critical hit!");
        }
        if (damageDetails.TypeEffectiveness > 1f)
        {
            yield return battleDialogBox.TypeDialog("It's super effective!");
        }
        if (damageDetails.TypeEffectiveness < 1f)
        {
            yield return battleDialogBox.TypeDialog("It's not very effective...");
        }
    }

    private void PlayerAction()
    {
        // Debug.Log("PLayerAction is called");
        battleDialogBox.SetDialog($"What will\n{playerPokemon.Pokemon.Base.PokeName} do?");
        // StartCoroutine(battleDialogBox.TypeDialog($"What will\n{playerPokemon.Pokemon.Base.PokeName} do?"));
        battleDialogBox.EnableMoveSelector(false);
        battleDialogBox.EnableActionDialog(true);
        currentAction = 0;
        Invoke("DelayedPlayerActionState", 0.5f);
        // state = BattleState.PlayerAction;
    }

    private void PlayerMove()
    {
        battleDialogBox.EnableActionDialog(false);
        battleDialogBox.EnableMoveSelector(true);
        battleDialogBox.SetupPokemonMoveNames(playerPokemon.Pokemon.Moves);
        // currentAction = 0;
        state = BattleState.PlayerMove;
    }

    private void HandleActionSelection()
    {
        HandleInputSelection();
        battleDialogBox.UpdateActionSelection(currentAction);   

        if (Input.GetKeyDown(KeyCode.Z) && state == BattleState.PlayerAction)
        {
            // Debug.Log("PlayerAction performing in HandleActionSelection");
            SoundManager.Instance.PlayClick();
            switch (currentAction)
            {
                case 0:
                    //Fight
                    PlayerMove();
                    break;
                case 1:
                    //Pokemon
                    break;
                case 2:
                    //Bag
                    break;
                case 3:
                    //Run
                    SceneLoader.Instance.LoadPreviousScene();
                    break;
                default:
                    break;
            }
        }
    }

    private void HandleMoveSelection()
    {
        HandleInputSelection();
        battleDialogBox.UpdateMoveSelection(currentAction, playerPokemon.Pokemon.Moves);
        if (Input.GetKeyDown(KeyCode.Z) && state == BattleState.PlayerMove)
        {
            // Debug.Log("HandleMoveSelection going to battle");
            state = BattleState.Busy;
            battleDialogBox.EnableMoveSelector(false);
            StartCoroutine(PerformPlayerMove());
        }
        if (Input.GetKeyDown(KeyCode.X) && state == BattleState.PlayerMove)
        {
            PlayerAction();
            SoundManager.Instance.PlayClick();
        }
    }

    private void HandleInputSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && currentAction <= 1 && currentAction >= 0)
        {
            currentAction += 2;
            SoundManager.Instance.PlayClick();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && currentAction >= 2 && currentAction <= 3)
        {
            currentAction -= 2;
            SoundManager.Instance.PlayClick();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && currentAction % 2 == 0)
        {
            currentAction ++;
            SoundManager.Instance.PlayClick();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && currentAction % 2 == 1)
        {
            currentAction --;
            SoundManager.Instance.PlayClick();
        }
    }

    private void DelayedPlayerActionState()
    {
        state = BattleState.PlayerAction;
    }
}


