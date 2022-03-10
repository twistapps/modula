using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modula
{
    public abstract partial class DataLayer : MonoBehaviour
    {
        protected readonly Dictionary<string, bool> booleans = new Dictionary<string, bool>();
        protected readonly Dictionary<string, float> floats = new Dictionary<string, float>();
        protected readonly Dictionary<string, int> integers = new Dictionary<string, int>();
        protected readonly Dictionary<string, string> strings = new Dictionary<string, string>();
        protected readonly Dictionary<string, Type> types = new Dictionary<string, Type>();

        public bool KeyExists(string key)
        {
            return types.ContainsKey(key);
        }

        public Type GetType(string key)
        {
            return types[key];
        }

        #region setters

        public void SetBool(string key, bool data, bool doOverride = false)
        {
            if (doOverride && KeyExists(key) && types[key] == typeof(bool))
            {
                booleans[key] = data;
            }
            else
            {
                types.Add(key, data.GetType());
                booleans.Add(key, data);
            }
        }

        public void SetInt(string key, int data, bool doOverride = false)
        {
            types.Add(key, data.GetType());
            integers.Add(key, data);
        }

        public void SetFloat(string key, float data, bool doOverride = false)
        {
            types.Add(key, data.GetType());
            floats.Add(key, data);
        }

        public void SetString(string key, string data, bool doOverride = false)
        {
            types.Add(key, data.GetType());
            strings.Add(key, data);
        }

        #endregion

        #region getters

        public int GetInt(string key)
        {
            return integers[key];
        }

        public bool GetBool(string key, bool checkExistence = true)
        {
            return checkExistence ? KeyExists(key) && booleans[key] : booleans[key];
        }

        public float GetFloat(string key)
        {
            return floats[key];
        }

        public string GetString(string key)
        {
            return strings[key];
        }

        #endregion

        #region experimental

        // public DataLayer()
        // {
        //     @switch = new Dictionary<Type, Action<string>> {
        //         { typeof(int), (key) => GetInt(key) },
        //         { typeof(bool), (key) => GetBool(key) },
        //         { typeof(float), (key) => GetFloat(key) },
        //         { typeof(string), (key) => GetString(key) }
        //     };
        // }

        #endregion
    }

    public partial class DataLayer
    {
        private readonly string _keyInitialized = "initialized";

        public bool initialized
        {
            get => GetBool(_keyInitialized);
            set => SetBool(_keyInitialized, value, true);
        }

        public void Reset()
        {
            GetComponent<ModularBehaviour>().GetData();
        }
    }

    public partial class DataLayer
    {
        protected readonly Dictionary<string, Component> components = new Dictionary<string, Component>();

        public void SetComponent(string key, Component data, bool doOverride = false)
        {
            types.Add(key, data.GetType());
            components.Add(key, data);
        }

        public Component GetComponent(string key, bool checkExistence = true)
        {
            if (checkExistence && !KeyExists(key)) return null;
            return components[key];
        }
    }
}