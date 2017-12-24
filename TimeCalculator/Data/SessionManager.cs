using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows;
using DevExpress.Mvvm.Native;

namespace TimeCalculator.Data
{
    public static class SessionManager
    {
        [Serializable]
        private class DataHolder : IEnumerable<KeyValuePair<string, object>>
        {
            private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

            public void SetData(string id, object value) => _values[id] = value;

            public TType GetData<TType>(string id)
            {
                if (_values.TryGetValue(id, out var value)) return (TType)value;

                return default(TType);
            }

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
            private readonly bool _block;

            public MyPropertyManager(DataHolder holder, Action changeAction, string id)
            {
                _holder = holder;
                _id = id;
                _changeAction = changeAction;

                _block = true;
                foreach (var pair in holder)
                    if (pair.Key.StartsWith(id))
                        // ReSharper disable once VirtualMemberCallInConstructor
                        SetPropertyCore(pair.Key.Remove(0, id.Length + 1), pair.Value, out var _);
                _block = false;
            }

            protected override bool SetPropertyCore<T>(string propertyName, T value, out T oldValue)
            {
                var ok = base.SetPropertyCore(propertyName, value, out oldValue);

                if (_block) return ok;
                if (!ok) return false;

                _holder.SetData(_id + ":" + propertyName, value);
                _changeAction?.Invoke();

                return true;
            }
        }

        private static readonly BinaryFormatter Formatter = new BinaryFormatter();
        private static DataHolder _dataHolder;
        private static bool _isInitialized;
        private static bool _run = true;
        private static readonly AutoResetEvent SaveSync = new AutoResetEvent(false);

        private static void Initilize()
        {
            Thread thread = new Thread(Worker) { IsBackground = false };
            thread.Start();

            Application.Current.Exit += (sender, args) =>
            {
                _run = false;
                SaveSync.Set();
            };

            try
            {
                if(File.Exists(FilesHelper.SeassionFilePath))
                    using (var stream = new FileStream(FilesHelper.SeassionFilePath, FileMode.Open))
                        _dataHolder = (DataHolder)Formatter.Deserialize(stream);
                else _dataHolder = new DataHolder();
            }
            catch (SerializationException)
            {
                _dataHolder = new DataHolder();
            }

            _isInitialized = true;
        }

        public static void PropertyChange()
        {
            InitializeIfNot();
            SaveSync.Set();
        }

        public static PropertyManager CreateManager(string id)
        {
            InitializeIfNot();
            return new MyPropertyManager(_dataHolder, PropertyChange, id);
        }

        public static void Set(string id, object value)
        {
            InitializeIfNot();
            _dataHolder.SetData(id, value);
            PropertyChange();
        }

        public static TType Get<TType>(string id)
        {
            InitializeIfNot();
            return _dataHolder.GetData<TType>(id);
        }

        private static void InitializeIfNot()
        {
            if(!_isInitialized) Initilize();
        }

        private static void Worker()
        {
            while (_run)
            {
                SaveSync.WaitOne();
                using (var stream = new FileStream(FilesHelper.SeassionFilePath, FileMode.Create))
                    Formatter.Serialize(stream, _dataHolder);
            }
        }
    }
}