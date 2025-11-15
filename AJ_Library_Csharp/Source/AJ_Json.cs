using System.IO;
using Newtonsoft.Json;


namespace AJ_Json
{
    public class Jsonfile
    {

        private string filePath;
        private string filename;


        /// <summary>
        /// Constuct class with file name by defult filepath in the base app folder.
        /// </summary>
        /// <param name="newname">Set filename</param>
        public Jsonfile(string newname)
        {
            // Define the file path and name
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            filename = newname;

            // Create File
            if (!Exists())
            {
                string json = JsonConvert.SerializeObject(JsonSerializer.Create(), Formatting.Indented);
                File.WriteAllText(GetFilepath(), json);
            }
            
        }


        /// <summary>
        /// Config new address & namefile for saving system.
        /// </summary>
        /// <param name="newpath">Set new address for saving destination</param>
        /// <param name="newname">Set new name for saving text file</param>
        public void Config(string newpath, string newname)
        {
            filePath = newpath + "\\";
            filename = newname;

        }

        /// <summary>
        /// Config addressFile for saving system.
        /// </summary>
        /// <param name="newpath">Set new address for saving destination</param>
        public void Config(string newpath)
        {
            filePath = newpath + "\\";
        }

        /// <summary>
        /// Config nameFile for saving system.
        /// </summary>
        /// <param name="newname">Set new name for saving text file</param>
        public void ConfigName(string newname)
        {
            filename = newname;
        }


        /// <summary>
        /// Get current filepath address.
        /// </summary>
        /// <returns>return full path file</returns>
        public string GetFilepath()
        {
            return $"{filePath}{filename}.json";
        }


        /// <summary>
        /// Write data in Jsonfile
        /// </summary>
        /// <param name="data">Set jasondata</param>
        public void Write<F>(F data)
        {
            try
            {
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(GetFilepath(), json);
            }
            catch (Exception e)
            {
                // If an exception occurs, display an error message.
                Console.WriteLine("The file could not be write:");
                Console.WriteLine(e.Message);
            }

        }

        /// <summary>
        /// Read data from Jsonfile
        /// </summary>
        /// <returns>return file data</returns>
        public F? Read<F>()
        {
            try
            {
                string json = File.ReadAllText(GetFilepath());
                return JsonConvert.DeserializeObject<F>(json);
            }
            catch (Exception e)
            {
                // If an exception occurs, display an error message.
                Console.WriteLine("The file could not be red:");
                Console.WriteLine(e.Message);
                return default(F);
            }
        }

        /// <summary>
        /// Read special line data from Jsonfile
        /// </summary>
        /// <returns>return file data from special line</returns>
        public F? ReadLine<F>(int index)
        {
             try
            {
                string json = File.ReadAllLines(GetFilepath())[index];
                return JsonConvert.DeserializeObject<F>(json);
            }
            catch (Exception e)
            {
                // If an exception occurs, display an error message.
                Console.WriteLine("The file could not be red:");
                Console.Write(e.Message);
                return default(F);
            }
        }


        /// <summary>
        /// Get Exsist File.
        /// </summary>
        /// <returns>return true if exists</returns>
        public bool Exists()
        {
            return File.Exists(GetFilepath());
        }

    }





}