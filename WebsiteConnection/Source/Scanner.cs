

namespace Scanner
{
    class Scan
    {
        private static string[] files = { };

        public Scan(string directoryPath)
        {

            // Check if the specified directory exists
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Directory not found.");
                return;
            }

            string[] folders = Directory.GetDirectories(directoryPath);
            foreach (var item in folders)
            {
                if (Path.GetFileName(item) != "$RECYCLE.BIN" && Path.GetFileName(item) != "System Volume Information")
                {
                    Thread thread = new Thread(() => Scanner(item));
                    thread.Start();

                    // Wait for the thread to complete
                    thread.Join();
                }

            }

        }

        public List<string> ListFilesType()
        {
            List<string> result = new List<string>();
            foreach (string file in files)
            {
                string fileExtension = Path.GetExtension(file);
                string fileType = fileExtension.TrimStart('.');
                result.Add(fileType);
            }
            return result;
        }
        public List<string> ListFilesFullName()
        {
            List<string> result = new List<string>();
            foreach (string file in files)
            {
                result.Add(Path.GetFileName(file));
            }
            return result;
        }
        public string[] ListFiles()
        {
            return files;
        }
        public List<string> ListFilesName()
        {
            List<string> result = new List<string>();
            foreach (string file in files)
            {
                result.Add(Path.GetFileNameWithoutExtension(file));
            }
            return result;
        }
        public List<string> ListFilesRoot()
        {
            List<string> result = new List<string>();
            foreach (string file in files)
            {
                result.Add(Path.GetPathRoot(file)!);
            }
            return result;
        }
        public Dictionary<string, int> ListTypes()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (string type in ListFilesType())
            {
                bool returner = false;
                foreach (KeyValuePair<string, int> kvp in result)
                {
                    if (type == kvp.Key)
                    {
                        result[kvp.Key]++;
                        returner = true;
                    }
                }
                if (!returner)
                    result.Add(type, 1);
            }
            return result;
        }
        public List<string> ListFiles(string type)
        {
            List<string> result = new List<string>();
            foreach (string file in files)
            {
                string fileExtension = Path.GetExtension(file);
                string fileType = fileExtension.TrimStart('.');
                if (fileType == type)
                {
                    result.Add(file);
                }

            }
            return result;
        }


        static void Scanner(string path)
        {
            // Get all files in the directory and its subdirectories
            string[] NewFiles = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            files = files.Concat(NewFiles).ToArray();
        }

    }

}