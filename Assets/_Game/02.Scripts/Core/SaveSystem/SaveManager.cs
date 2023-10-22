using System;
using System.IO;
using UnityEngine;

namespace _Game._02.Scripts.Core
{
    public sealed class SaveManager : ISaveManager
    {
        #region Fields

        private IStream _stream;
        private string _savePath;

        #endregion

        #region Public Methods

        
        public void Initialize(string saveFolder, IStream stream)
        {
            _savePath = saveFolder;
            _stream = stream;
        }

        public void Save(object data, string appendPath = "", string extension = ".dat")
        {
            if (_stream == null) return;
            
            var path = Path.Combine(_savePath, appendPath);
            path = Path.Combine(path, data.GetType().Name + extension);
            if (Application.platform is RuntimePlatform.WindowsPlayer or RuntimePlatform.WindowsEditor)
            {
                path = path.Replace("\\", "/");
            }
            _stream.Write(data, path);
        }

        public T Load<T>( string appendPath = "", string extension = ".dat") where T : new()
        {
            if (_stream == null) return default;
            
            var path = Path.Combine(_savePath, appendPath);
            path = Path.Combine(path, typeof(T).Name + extension);
            if (Application.platform is RuntimePlatform.WindowsPlayer or RuntimePlatform.WindowsEditor)
            {
                path = path.Replace("\\", "/");
            }
            return _stream.Read<T>(path);
        }

        public object Load(Type type, string appendPath = "", string extension = ".dat")
        {
            if (_stream == null) return default;
            
            var path = Path.Combine(_savePath, appendPath);
            path = Path.Combine(path, type.Name + extension);
            if (Application.platform is RuntimePlatform.WindowsPlayer or RuntimePlatform.WindowsEditor)
            {
                path = path.Replace("\\", "/");
            }
            return _stream.Read(path, type);
        }

        public bool Delete(Type type, string appendPath = "", string extension = ".dat")
        {
            if (_stream == null) return default;
            
            var path = Path.Combine(_savePath, appendPath);
            path = Path.Combine(path, type.Name + extension);
            if (Application.platform is RuntimePlatform.WindowsPlayer or RuntimePlatform.WindowsEditor)
            {
                path = path.Replace("\\", "/");
            }
            return _stream.Delete(path);
        }

        #endregion
    }   
}