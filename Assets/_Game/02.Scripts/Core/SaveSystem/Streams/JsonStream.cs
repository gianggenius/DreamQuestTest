using System;
using Newtonsoft.Json;

namespace _Game._02.Scripts.Core
{
    public class JsonStream : IStream
    {
        private readonly JsonSerializerSettings _settings = new()
        {
            TypeNameHandling = TypeNameHandling.Objects
        };
        
        public T Read<T>(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                return default;
            }

            var texts = System.IO.File.ReadAllText(path);
            return string.IsNullOrEmpty(texts) ? default : JsonConvert.DeserializeObject<T>(texts, _settings);
        }

        public object Read(string path, Type anonymousType)
        {
            if (!System.IO.File.Exists(path))
            {
                return default;
            }

            var texts = System.IO.File.ReadAllText(path);
            if (string.IsNullOrEmpty(texts))
                return null;
            
            var instance = Activator.CreateInstance(anonymousType);
            JsonConvert.PopulateObject(texts, instance, _settings);
            
            return instance;
        }
        
        public void Write(object data, string path)
        {
            if (!System.IO.File.Exists(path))
            {
                var dir = path.Remove(path.LastIndexOf('/'));
                if (!string.IsNullOrEmpty(dir) && !System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
            }
            
            var json = JsonConvert.SerializeObject(data, Formatting.Indented, _settings);
            System.IO.File.WriteAllText(path, json);
        }
        
        public bool Delete(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                var dir = path.Remove(path.LastIndexOf('/'));
                if (!string.IsNullOrEmpty(dir) && !System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
            }
            
            System.IO.File.Delete(path);
            return true;
        }
    }
}