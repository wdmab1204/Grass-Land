using UnityEngine;
using System.Collections;
using System;
using GameEntity;
using CardNameSpace;
using System.Collections.Generic;
using UnityEngine.UI;

public class RelicEventHelper : MonoBehaviour
{
    public PlayerEntity playerEntity;
    public PlayerActor playerActor;
    public DeckHandler deckHandler;

    List<Relic> reliclist = new List<Relic>();

    private void Start()
    {
        
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Relic");
        reliclist.Add(new Relic("기합의 머리띠", "게임 중 한번 치명적인 피해를 입었을 때 10% 확률로 죽지 않습니다.",   sprites[0]));
        reliclist.Add(new Relic("인라인스케이트", "무브 포인트를 사용할 때 1칸을 더 이동할 수 있습니다.",             sprites[1]));
        reliclist.Add(new Relic("고슴도치 갑옷", "데미지를 입을 때마다 2의 데미지를 입힙니다.",                     sprites[2]));
        reliclist.Add(new Relic("럭키가이", "상자를 열 때 2개의 유물을 획득합니다.",                             sprites[3]));
        reliclist.Add(new Relic("장어 구이", "층을 오를 때마다 체력을 5 회복합니다.",                            sprites[4]));
        reliclist.Add(new Relic("피바라기", "적을 공격할 때마다 체력을 1 회복합니다.",                            sprites[5]));
        reliclist.Add(new Relic("럼주", "전투 중 회복 효율이 50% 증가합니다.",                                 sprites[6]));
        reliclist.Add(new Relic("화염 벨트", "자신의 턴이 끝날 때 모든 적들이 1의 피해를 입습니다.",                sprites[7]));
        reliclist.Add(new Relic("아이스 아메리카노", "모든 피해가 두배 증가합니다. 모든 이동과 회복량이 절반 감소합니다.",sprites[8]));
        reliclist.Add(new Relic("윙부츠", "자신의 턴이 항상 먼저 옵니다.",                                     sprites[9]));
        reliclist.Add(new Relic("급소 타격", "치명타 공격이 더 강력해집니다.",                                  sprites[10]));
        reliclist.Add(new Relic("매의 눈", "활을 사용한 공격은 치명타 확률이 증가합니다.",                         sprites[11]));
        reliclist.Add(new Relic("유리 대포", "데미지와 받는피해가 증가합니다.",                                 sprites[12]));
        reliclist.Add(new Relic("도박사의 주사위", "주사위의 눈이 1또는 6만 나옵니다.",                           sprites[13]));
        reliclist.Add(new Relic("피를 마시는 검", "체력이 낮아질수록 데미지가 증가합니다.",                        sprites[14]));
        reliclist.Add(new Relic("망원경", "활을 사용한 공격을 할때 사거리가 증가합니다.",                          sprites[15]));
        //reliclist.Add(new Relic("태양의 메달", "체력과 회복량이 늘어납니다.",                                   sprites[16]));
        //reliclist.Add(new Relic("독 묻은 단검", "기습 공격의 데미지가 늘어납니다.",                              sprites[17]));
    }

    public void OnChangePlayerHealth(Action<int> action)
    {
        //기합의 머리띠
        playerEntity.OnHealthChanged += action;
    }

    public void OnAttackTarget(Action action)
    {
        //피바라기
        deckHandler.OnAttackTarget += action;
    }

    public void OnTakeDamage(Action action)
    {
        //고슴도치 갑옷
        playerEntity.OnTakeDamage += action;
    }

    public void OnEnterNextLevel(Action action)
    {
        //장어 구이
    }

    public void OnOpenTreaserBox(Action action)
    {
        //럭키 가이
    }

    public void OnExitPlayerTurn(Action action)
    {
        //화염 벨트
        playerActor.OnExitTurn();
    }

    public void OnEnterThrowDice(Action action)
    {
        //도박사의 주사위
    }

    public void SetDamageRate(float percentage, Func<bool> condition)
    {
        //아이스 아메리카노
        //유리 대포
        //피를 마시는 
    }

    public void SetDefend(float value)
    {
        //유리 대포
    }

    public void SetMoveRate(float percentage, Func<bool> condition)
    {
        //아이스 아메리카노
    }

    public void SetRecoveryRate(float percentage, Func<bool> condition)
    {
        //럼주
        //아이스 아메리카노
        //태양의 메
    }

    public void SetMaxHp(int maxHp, Func<bool> condition)
    {
        //태양의 메달
    }

    public void SetCriticalDamage(float percentage, Func<bool> condition)
    {
        //급소 타격

    }


}
