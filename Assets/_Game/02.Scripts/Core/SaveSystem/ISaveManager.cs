using System;

namespace _Game._02.Scripts.Core
{
    public interface IStream
    {
        void Write(object data, string path);
        T Read<T>(string path);
        object Read(string path, Type type);
        bool Delete(string path);
    }
    
    public interface ISaveManager
    {
        /// <summary>
        /// Init save system
        /// </summary>
        /// <param name="saveFolder"></param>
        /// <param name="stream"></param>
        void Initialize(string saveFolder, IStream stream);

        /// <summary>
        /// Save data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="appendPath"></param>
        /// <param name="extension"></param>
        void Save(object data, string appendPath = "", string extension = ".dat");

        /// <summary>
        /// Load data
        /// </summary>
        /// <param name="appendPath"></param>
        /// <param name="extension"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Load<T>(string appendPath = "", string extension = ".dat") where T : new();

        /// <summary>
        /// Load generic data
        /// </summary>
        /// <param name="type"></param>
        /// <param name="appendPath"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        object Load(Type type, string appendPath = "", string extension = ".dat");

        /// <summary>
        /// Delete generic data
        /// </summary>
        /// <param name="type"></param>
        /// <param name="appendPath"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        bool Delete(Type type, string appendPath = "", string extension = ".dat");
    }
}