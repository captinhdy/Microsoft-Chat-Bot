using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TextProcessing
{
    [Serializable]
    public class FileProcessing
    {
        public List<string> Headings;
        public IEnumerable<string> ReadTextFileLines(string FileName)
        {
            List<string> textFile = new List<string>();
            string line = "";
            using (StreamReader stream = new StreamReader(File.OpenRead(FileName)))
            {
                while ((line = stream.ReadLine()) != null)
                {
                    textFile.Add(line);
                }
            }

            return textFile;
        }

        public IEnumerable<TEntity> ReadCSVLines<TEntity>(string FileName, bool HasHeadings) where TEntity:  new()
        {
            List<TEntity> csv = new List<TEntity>();
            Headings = new List<string>();
            string line = "";
            bool heading = HasHeadings;
            using (StreamReader stream = new StreamReader(File.OpenRead(FileName)))
            {
                while ((line = stream.ReadLine()) != null)
                {
                    if (!heading)
                    {
                        string[] entries = line.Split(',');
                        TEntity entry = new TEntity();
                        var properties = typeof(TEntity).GetProperties();

                        for (int i = 0; i < properties.Length; i++)
                        {
                            var property = properties[i];
                            var val = entries[i];
                            property.SetValue(entry, val);
                        }

                        csv.Add(entry);
                    }
                    else
                    {
                        string[] entries = line.Split(',');
                        foreach(string entry in entries)
                        {
                            Headings.Add(entry.ToLower());
                        }
                        heading = false;
                        
                    }
                }
            }

            return csv;
        }
    }
}
