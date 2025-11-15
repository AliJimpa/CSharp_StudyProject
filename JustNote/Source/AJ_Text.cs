using System;
using System.IO;

namespace AJ_Text
{
    public class Textfile
    {
        private string filePath;
        private string filename;
        private string fileformat = "text";


        /// <summary>
        /// Constuct class with file name by defult file save in the base app folder.
        /// </summary>
        /// <param name="newname">Set filename</param>
        public Textfile(string newname)
        {
            // Define the file path and name
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            filename = newname;

            // Create File
            if (!Exists())
            {
                GetWriter(false)?.Write("");
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
            return $"{filePath}{filename}.{fileformat}";
        }


        /// <summary>
        /// Write Text in NewLine
        /// </summary>
        public void WriteLine(string newline)
        {
            // Write the text to the file
            using (StreamWriter writer = new StreamWriter(GetFilepath()))
            {
                writer.WriteLine(newline);
            }
        }

        /// <summary>
        /// Write Text append file.
        /// </summary>
        public void Write(string text)
        {
            // Write the text to the file
            using (StreamWriter writer = new StreamWriter(GetFilepath(), true))
            {
                writer.WriteLine(text);
            }
        }

        /// <summary>
        /// Red all text in file.
        /// </summary>
        /// <returns>return all content in file</returns>
        public string Read()
        {
            try
            {
                // Open the file using a stream reader.
                using (StreamReader reder = new StreamReader(GetFilepath()))
                {
                    // Read the entire file into a string.
                    return reder.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                // If an exception occurs, display an error message.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return e.Message;
            }
        }

        /// <summary>
        /// Red line of text from file.
        /// </summary>
        /// <returns>return just line of content in file</returns>
        public string? ReadLine()
        {
            try
            {
                // Open the file using a stream reader.
                using (StreamReader reder = new StreamReader(GetFilepath()))
                {
                    // Read the entire file into a string.
                    return reder.ReadLine();
                }
            }
            catch (Exception e)
            {
                // If an exception occurs, display an error message.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return e.Message;
            }
        }


        /// <summary>
        /// Clean all text in file.
        /// </summary>
        public void ResetFile()
        {
            using (StreamWriter writer = new StreamWriter(GetFilepath(), false))
            {
                writer.WriteLine("");
            }
        }

        /// <summary>
        /// Change tag file.
        /// </summary>
        /// <param name="newtype">Set new filetype for save system</param>
        public void Configfiletype(string newtype)
        {
            fileformat = newtype;
        }

        /// <summary>
        /// Constuct class with file name by defult file save in the base app folder.
        /// </summary>
        /// <param name="newname">Set filename</param>
        public void ResetClass(string newname)
        {
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            filename = newname;
            fileformat = "text";
        }



        /// <summary>
        /// GetReder Streamer.
        /// </summary>
        /// <returns>return Streamer</returns>
        public StreamReader? GetReder()
        {
            try
            {
                // Open the file using a stream reader.
                using (StreamReader reder = new StreamReader(GetFilepath()))
                {
                    // Read the entire file into a string.
                    return reder;
                }
            }
            catch (Exception e)
            {
                // If an exception occurs, display an error message.
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// GetWriter Streamer.
        /// </summary>
        /// <returns>return Streamer</returns>
        public StreamWriter? GetWriter(bool append)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(GetFilepath() , append))
                {
                    return writer;
                }
            }
            catch (Exception e)
            {
                // If an exception occurs, display an error message.
                Console.WriteLine(e.Message);
                return null;
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

