using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A basic C# Event System
public static class EventHandler
{
    //This is a test event and it's calling function
    public static event Action<float> E_OnTestEvent;
    public static void Call_OnTestEvent(float data){
        E_OnTestEvent?.Invoke(data);
    }
    public static event Action E_OnPutCoins;
    public static void Call_OnPutCoins(){
        E_OnPutCoins?.Invoke();
    }
    public static event Action E_OnEnterRPSMode;
    public static void Call_OnEnterRPSMode(){
        E_OnEnterRPSMode?.Invoke();
    }
    public static event Action<Player, IRPSable> E_OnEnterRPSMode_PVE;
    public static void Call_OnEnterRPSMode_PVE(Player player, IRPSable obj){
        E_OnEnterRPSMode_PVE?.Invoke(player, obj);
    }
    public static event Action<int, Player> E_OnTransferDamage;
    public static void Call_OnTransferDamage(int damage, Player callPlayer){
        E_OnTransferDamage?.Invoke(damage, callPlayer);
    }
}

//A More Strict Event System
namespace SimpleEventSystem{
    public abstract class Event{
        public delegate void Handler(Event e);
    }
    public class E_OnTestEvent:Event{
        public float value;
        public E_OnTestEvent(float data){value = data;}
    }

    public class EventManager{
        private static  EventManager instance;
        public static EventManager Instance{
            get{
                if(instance == null) instance = new EventManager();
                return instance;
            }
        }

        private Dictionary<Type, Event.Handler> RegisteredHandlers = new Dictionary<Type, Event.Handler>();
        public void Register<T>(Event.Handler handler) where T: Event{
            Type type = typeof(T);

            if(RegisteredHandlers.ContainsKey(type)){
                RegisteredHandlers[type] += handler;
            }
            else{
                RegisteredHandlers[type] = handler;
            }
        }
        public void UnRegister<T>(Event.Handler handler) where T: Event{
            Type type = typeof(T);
            Event.Handler handlers;

            if(RegisteredHandlers.TryGetValue(type, out handlers)){
                handlers -= handler;
                if(handlers == null){
                    RegisteredHandlers.Remove(type);
                }
                else{
                    RegisteredHandlers[type] = handlers;
                }
            }
        }
        public void FireEvent<T>(T e) where T:Event {
            Type type = e.GetType();
            Event.Handler handlers;

            if(RegisteredHandlers.TryGetValue(type, out handlers)){
                handlers?.Invoke(e);
            }
        }
        public void ClearList(){RegisteredHandlers.Clear();}
    }
}
