using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace personaltools.textlocalizedtool
{
    public class CSVLoader
    {
        private TextAsset csvFile;
        private readonly char lineSeperator = '\n';
        private readonly char surround = '"';
        private readonly string[] fieldSeperator = { "\",\"" };
        private readonly Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        public void LoadCSV(TextAsset asset)
        {
            csvFile = asset;
        }

        public Dictionary<string, string> GetDictionary(string attributeId)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            string[] lines = csvFile.text.Split(lineSeperator);

            int attributeIndex = -1;

            string[] header = CSVParser.Split(lines[0]);

            for (int i = 0; i < header.Length; i++)
            {
                Trim(ref header[i]);

                if (header[i].Contains(attributeId))
                {
                    attributeIndex = i;
                    break;
                }
            }

            if (attributeIndex == -1)
                return dictionary;

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line == String.Empty)
                    continue;

                string[] fields = CSVParser.Split(line);

                for (int f = 0; f < fields.Length; f++)
                {
                    Trim(ref fields[f]);
                }

                if (fields.Length > attributeIndex)
                {
                    var key = fields[0];

                    if (dictionary.ContainsKey(key)) { continue; }

                    string value = fields[attributeIndex];

                    dictionary.Add(key, value);
                }
            }

            return dictionary;
        }

        public void Trim(ref string value)
        {
            value = value.TrimStart(' ', surround);
            value = value.TrimEnd(surround);

            if (value.Contains("\""))
                value = value.Remove(value.Length - 2, 2);
        }

#if UNITY_EDITOR

        public void Add(string key, string value)
        {
            value.Replace(Environment.NewLine, "\n");
            string path = AssetDatabase.GetAssetPath(csvFile.GetInstanceID());
            string append = string.Format("\n\"{0}\",\"{1}\",\"\"", key, value);
            File.AppendAllText(path, append);

            UnityEditor.AssetDatabase.Refresh();
        }

        public void Remove(string key)
        {
            string[] lines = csvFile.text.Split(lineSeperator);

            string[] keys = new string[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                keys[i] = line.Split(fieldSeperator, StringSplitOptions.None)[0];
            }

            int index = -1;

            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].Contains(key))
                {
                    index = i;
                    break;
                }
            }

            if (index > -1)
            {
                string[] newLines;
                newLines = lines.Where(w => !(w != lines[index] ^ w != String.Empty)).ToArray();

                string path = AssetDatabase.GetAssetPath(csvFile.GetInstanceID());
                string replaced = string.Join(lineSeperator.ToString(), newLines);
                File.WriteAllText(path, replaced);
            }
        }

        public void Edit(string key, string value)
        {
            Remove(key);
            Add(key, value);
        }

#endif

    }
}
