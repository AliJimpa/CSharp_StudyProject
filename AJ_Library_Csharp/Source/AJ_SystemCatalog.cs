using System;
using System.Reflection;
using AJ_CoreSystem;
using AJ_Json;


namespace AJ_SystemCatalog
{

    class Catalog
    {
        private Jsonfile? File;

        public Catalog()
        {
            File = new Jsonfile("ArgomanCatalog");
        }

        public void Export(List<Argoman> List)
        {
            CatalogFile filelist = new CatalogFile(List);
            File!.Write<CatalogFile>(filelist);
        }

        public List<Argoman> Import()
        {
            CatalogFile? CFile = File!.Read<CatalogFile>();
            if (CFile != null)
            {
                return CFile.List;
            }
            else
            {
                return new List<Argoman>();
            }
        }

        public bool Exists()
        {
            return true;
        }

    }


    class CatalogFile
    {
        public List<Argoman> List = new List<Argoman>();
        public bool isvalid = false;
        public CatalogFile(List<Argoman> newliast)
        {
            foreach (var item in newliast)
            {
                List.Add(item);
                Console.WriteLine("*************"+item.GetName());
            }
            isvalid = true;
        }
        public bool IsValid()
        {
            return isvalid;
        }
    }





}