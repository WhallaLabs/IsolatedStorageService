using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhallaToolkit.IsolatedStorage
{
    public interface IIsolatedStorageService
    {
        /// <summary>
        /// Saves file into storage at specified name. Serializes payload into json string.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        Task SaveFileAsync(string filename, object payload);

        /// <summary>
        /// Loads file at specified name. It deserialize json content from storage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <returns></returns>
        Task<T> LoadFileAsync<T>(string filename);

        /// <summary>
        /// Loads raw file at specified name
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        Stream LoadRawFileAsync(string filename);

        /// <summary>
        /// Saves file into storage at specified name. Serializes payload into json string. 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="payload"></param>
        void SaveFile(string filename, object payload);

        /// <summary>
        /// Loads file and deserializes file content as generic parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <returns></returns>
        T LoadFile<T>(string filename);

        /// <summary>
        /// Removes file from storage
        /// </summary>
        /// <param name="filename"></param>
        void RemoveFile(string filename);

        /// <summary>
        /// Saves the object by specified key on IsolatedStorageSettings. Object is NOT serialized by json converter.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="payload">If you want store custom object class, pass it as json string</param>
        void SaveSettings(string key, object payload);

        /// <summary>
        /// Removes settings form key.
        /// </summary>
        /// <param name="key"></param>
        void RemoveSettings(string key);


        /// <summary>
        /// Loads object from settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T LoadSettings<T>(string key);
        
    }


}
