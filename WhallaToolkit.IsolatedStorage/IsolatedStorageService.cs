using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace WhallaToolkit.IsolatedStorage
{
    public class IsolatedStorageService : IIsolatedStorageService
    {
        #region Fields
        IsolatedStorageSettings settings;
        #endregion

        #region Interface Implementation
        public void SaveSettings(string key, object payload)
        {
            try
            {
                settings[key] = payload;
            }
            catch (Exception)
            {
                settings.Add(key, payload);
            }
            finally
            {
                settings.Save();
            }
            
        }

        public void RemoveSettings(string key)
        {
            try
            {
                settings.Remove(key);
            }
            catch (Exception)
            {

            }
            finally
            {
                settings.Save();
            }
        }

        public T LoadSettings<T>(string key)
        {
            T result;
            try
            {
                result = (T)settings[key];
            }
            catch (Exception)
            {
                result = default(T);
            }

            return result;
        }

        public async Task SaveFileAsync(string filename, object payload)
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream rawStream = isf.CreateFile(filename))
                {
                    var data = await JsonConvert.SerializeObjectAsync(payload);
                    StreamWriter writer = new StreamWriter(rawStream);
                    await writer.WriteAsync(data);
                    await writer.FlushAsync();
                    writer.Close();
                }
            }
        }

        public Stream LoadRawFileAsync(string filename)
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(filename))
                {
                    try
                    {
                        using (IsolatedStorageFileStream rawStream = isf.OpenFile(filename, System.IO.FileMode.Open))
                        {
                            MemoryStream ms = new MemoryStream();

                            byte[] readBuffer = new byte[4096];
                            int bytesRead = -1;

                            // Copy the file from the installation folder to the local folder. 
                            while ((bytesRead = rawStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                            {
                                ms.Write(readBuffer, 0, bytesRead);
                            }

                            ms.Flush();
                            ms.Seek(0, SeekOrigin.Begin);

                            return ms;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            return null;
        }

        public async Task<T> LoadFileAsync<T>(string filename)
        {
            T result = default(T);
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(filename))
                {
                    try
                    {
                        using (IsolatedStorageFileStream rawStream = isf.OpenFile(filename, System.IO.FileMode.Open))
                        {
                            StreamReader reader = new StreamReader(rawStream);
                            var json = await reader.ReadToEndAsync();
                            result = await JsonConvert.DeserializeObjectAsync<T>(json);
                            reader.Close();
                        }
                    }
                    catch(Exception ex)
                    {
                    }
                }
            }

            return result;
        }

        public void RemoveFile(string filename)
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(filename))
                {
                    try
                    {
                        isf.DeleteFile(filename);
                    }
                    catch
                    {
                    }
                }
            }
        }

        public void SaveFile(string filename, object payload)
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream rawStream = isf.CreateFile(filename))
                {
                    var data = JsonConvert.SerializeObject(payload);
                    StreamWriter writer = new StreamWriter(rawStream);
                    writer.Write(data);
                    writer.Flush();
                    writer.Close();
                }
            }
        }

        public T LoadFile<T>(string filename)
        {
            T result = default(T);
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(filename))
                {
                    try
                    {
                        using (IsolatedStorageFileStream rawStream = isf.OpenFile(filename, System.IO.FileMode.Open))
                        {
                            StreamReader reader = new StreamReader(rawStream);
                            var json = reader.ReadToEnd();
                            result = JsonConvert.DeserializeObject<T>(json);
                            reader.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            return result;
        }
        #endregion

        #region Constructor
        public IsolatedStorageService()
        {
            settings = IsolatedStorageSettings.ApplicationSettings;
        }
        #endregion
    }
}
