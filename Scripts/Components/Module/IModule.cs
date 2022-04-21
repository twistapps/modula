using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Modula.Common;
using Modula.Optimization;
using UnityEngine;

namespace Modula
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public interface IModule
    {
        #region Modula

        TypedList<IModule> RequiredOtherModules { get; }
        ModularBehaviour Main { get; }
        void OnAdd();
        void AddModule(Type moduleType);
        string GetName();
        DataLayer GetData();

        void ManagedUpdate();
        TimingConstraints UpdateInvocationConstraints { get; }

        bool ShouldSerialize();

        #endregion

        #region MonoBehaviour

        bool useGUILayout { get; set; }
        bool runInEditMode { get; set; }
        bool enabled { get; set; }
        bool isActiveAndEnabled { get; }
        Transform transform { get; }
        GameObject gameObject { get; }
        string tag { get; set; }
        Component rigidbody { get; }
        Component rigidbody2D { get; }
        Component camera { get; }
        Component light { get; }
        Component animation { get; }
        Component constantForce { get; }
        Component renderer { get; }
        Component audio { get; }
        Component networkView { get; }
        Component collider { get; }
        Component collider2D { get; }
        Component hingeJoint { get; }
        Component particleSystem { get; }
        string name { get; set; }
        HideFlags hideFlags { get; set; }
        bool IsInvoking();
        void CancelInvoke();
        void Invoke(string methodName, float time);
        void InvokeRepeating(string methodName, float time, float repeatRate);
        void CancelInvoke(string methodName);
        bool IsInvoking(string methodName);
        Coroutine StartCoroutine(string methodName);
        Coroutine StartCoroutine(string methodName, object value);
        Coroutine StartCoroutine(IEnumerator routine);
        Coroutine StartCoroutine_Auto(IEnumerator routine);
        void StopCoroutine(IEnumerator routine);
        void StopCoroutine(Coroutine routine);
        void StopCoroutine(string methodName);
        void StopAllCoroutines();
        Component GetComponent(Type type);
        T GetComponent<T>();
        bool TryGetComponent(Type type, out Component component);
        bool TryGetComponent<T>(out T component);
        Component GetComponent(string type);
        Component GetComponentInChildren(Type t, bool includeInactive);
        Component GetComponentInChildren(Type t);
        T GetComponentInChildren<T>(bool includeInactive);
        T GetComponentInChildren<T>();
        Component[] GetComponentsInChildren(Type t, bool includeInactive);
        Component[] GetComponentsInChildren(Type t);
        T[] GetComponentsInChildren<T>(bool includeInactive);
        void GetComponentsInChildren<T>(bool includeInactive, List<T> result);
        T[] GetComponentsInChildren<T>();
        void GetComponentsInChildren<T>(List<T> results);

#if UNITY_2021_1_OR_NEWER
        Component GetComponentInParent(Type t, bool includeInactive);
        Component GetComponentInParent(Type t);
        T GetComponentInParent<T>(bool includeInactive);
        T GetComponentInParent<T>();
#endif

        Component[] GetComponentsInParent(Type t, bool includeInactive);
        Component[] GetComponentsInParent(Type t);
        T[] GetComponentsInParent<T>(bool includeInactive);
        void GetComponentsInParent<T>(bool includeInactive, List<T> results);
        T[] GetComponentsInParent<T>();
        Component[] GetComponents(Type type);
        void GetComponents(Type type, List<Component> results);
        void GetComponents<T>(List<T> results);
        T[] GetComponents<T>();
        bool CompareTag(string tag);
        void SendMessageUpwards(string methodName, object value, SendMessageOptions options);
        void SendMessageUpwards(string methodName, object value);
        void SendMessageUpwards(string methodName);
        void SendMessageUpwards(string methodName, SendMessageOptions options);
        void SendMessage(string methodName, object value);
        void SendMessage(string methodName);
        void SendMessage(string methodName, object value, SendMessageOptions options);
        void SendMessage(string methodName, SendMessageOptions options);
        void BroadcastMessage(string methodName, object parameter, SendMessageOptions options);
        void BroadcastMessage(string methodName, object parameter);
        void BroadcastMessage(string methodName);
        void BroadcastMessage(string methodName, SendMessageOptions options);
        int GetInstanceID();
        int GetHashCode();
        bool Equals(object other);
        string ToString();

        #endregion
    }
}