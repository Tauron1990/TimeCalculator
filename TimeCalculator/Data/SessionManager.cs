using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using DevExpress.Mvvm.Native;

namespace TimeCalculator.Data
{
    public static class SessionManager
    {
        [Serializable]
        private class DataHolder : IEnumerable<KeyValuePair<string, object>>
        {
            private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

            public void Set(string id, object value) => _values[id] = value;

            public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            {
                return _values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private class MyPropertyManager : PropertyManager
        {
            private readonly DataHolder _holder;
            private readonly Action _changeAction;
            private readonly string _id;

            public MyPropertyManager(DataHolder holder, Action changeAction, string id)
            {
                foreach (var pair in holder)
                    if (pair.Key.StartsWith(id))
                        SetPropertyCore(pair.Key.Remove(0, pair.Key.Length + 1), pair.Value, out var _);

                _holder = holder;
                _changeAction = changeAction;
                _id = id;
            }

            protected override bool SetPropertyCore<T>(string propertyName, T value, out T oldValue)
            {
                var ok = base.SetPropertyCore(propertyName, value, out oldValue);

                if (!ok) return false;

                _holder.Set(_id + ":" + propertyName, value);
                _changeAction?.Invoke();

                return true;
            }
        }

        private static BinaryFormatter _formatter;
        private static DataHolder _dataHolder;
        private static bool _isInitialized;

        private static void Initilize()
        {
            try
            {
                if(File.Exists(FilesHelper.SeassionFilePath))
                    using (var stream = new FileStream(FilesHelper.SeassionFilePath, FileMode.Open))
                        _dataHolder = (DataHolder)_formatter.Deserialize(stream);
                else _dataHolder = new DataHolder();
            }
            catch (SerializationException)
            {
                _dataHolder = new DataHolder();
            }

            _isInitialized = true;
        }

        private static void PropertyChange()
        {
            using (var stream = new FileStream(FilesHelper.SeassionFilePath, FileMode.Create))
                _formatter.Serialize(stream, _dataHolder);
        }

        public static PropertyManager CreateManager(string id)
        {
            if(!_isInitialized) Initilize();
            return new MyPropertyManager(_dataHolder, PropertyChange, id);
        }
    }
}