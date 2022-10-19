using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus
{
  static EventBus _instance;
  public static EventBus Instance
  {
    get
    {
      if (_instance == null) _instance = new EventBus();
      return _instance;
    }
  }

  private EventBus() { }

  public delegate void OnDefeatEnemy(Enemy enemy);
  event OnDefeatEnemy _onDefeatEnemy;

  public void Subscribe(OnDefeatEnemy onDefeatEnemy)
  {
    _onDefeatEnemy += onDefeatEnemy;
  }

  public void UnSubscribe(OnDefeatEnemy onDefeatEnemy)
  {
    _onDefeatEnemy -= onDefeatEnemy;
  }

  public void NotifyDefeatEnemy(Enemy enemy)
  {
    if (_onDefeatEnemy != null) _onDefeatEnemy(enemy);
  }
}
