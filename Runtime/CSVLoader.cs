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

        public void LoadCSV(TextAsset asset, out int[] languageIndex)
        {
            csvFile = asset;

            var languagesString = GetAllLanguage();
            languageIndex = new int[languagesString.Length];

            for (int i = 0; i < languagesString.Length; i++)
            {
                // Get Language By key code
                languageIndex[i] = languagesString[i].GetLanguagesIndexFromCode();
            }
                
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

        public string[] GetAllLanguage()
        {
            string[] lines = csvFile.text.Split(lineSeperator);

            List<string> languages = new List<string>();

            string[] header = CSVParser.Split(lines[0]);

            for (int i = 1; i < header.Length; i++)
            {
                Trim(ref header[i]);
                languages.Add(header[i]);
            }

            return languages.ToArray();
        }

        public int indexLanguage(string language)
        {
            string[] lines = csvFile.text.Split(lineSeperator);

            string[] header = CSVParser.Split(lines[0]);

            for (int i = 1; i < header.Length; i++)
            {
                Trim(ref header[i]);
                if (header[i].CompareTo(language) == 0)
                    return i;
            }
            return -1;
        }

        public void Trim(ref string value)
        {
            value = value.TrimStart(' ', surround);
            value = value.TrimEnd(surround);

            value = value.Replace("\n", "").Replace("\r", "");
        }

#if UNITY_EDITOR
        public static void CreateCSV(string path, string[] languages)
        {
            string header = "\"key\"";
            foreach (var language in languages)
            {
                header += $",\"{language}\"";
            }
            File.WriteAllText(path, header);
            AssetDatabase.Refresh();
        }

        public void AddLanguage(string language)
        {
            string[] lines = csvFile.text.Split(lineSeperator);
            string empty = ",\"\"";

            lines[0] += $",\"{language}\"";
            for (int i = 1; i < lines.Length; i++)
                lines[i] += empty;

            WriteText(lines);
            UnityEditor.AssetDatabase.Refresh();
        }

        public void RemoveLanguage(string language)
        {
            int index = indexLanguage(language);

            string[] lines = csvFile.text.Split(lineSeperator);
            for(int i = 0; i < lines.Length; i++)
            {                
                var fields = CSVParser.Split(lines[i]);
                fields = fields.Where(field => (field != fields[index])).ToArray();

                lines[i] = String.Join(",", fields);
            }

            WriteText(lines);
            UnityEditor.AssetDatabase.Refresh();
        }

        public void Add(string key, string value, string language)
        {
            value.Replace(Environment.NewLine, "\n");
            string path = AssetDatabase.GetAssetPath(csvFile.GetInstanceID());

            var header = GetAllLanguage();
            string append = string.Format("\n\"{0}\"", key);
            string empty = ",\"\"";

            foreach (string h in header)
            {
                if (h.Contains(language))
                    append += string.Format(",\"{0}\"", value);
                else
                    append += empty;
            }
            
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
                WriteText(newLines);
                UnityEditor.AssetDatabase.Refresh();
            }
        }

        private void WriteText(string[] lines)
        {
            string path = AssetDatabase.GetAssetPath(csvFile.GetInstanceID());
            string replaced = string.Join(lineSeperator.ToString(), lines);
            File.WriteAllText(path, replaced);
        }

        public void Edit(string key, string value, string language)
        {
            string[] lines = csvFile.text.Split(lineSeperator);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                var keys = line.Split(fieldSeperator, StringSplitOptions.None)[0];
                if (keys.Contains(key))
                {
                    var fields = CSVParser.Split(line);
                    var indexChange = indexLanguage(language);

                    if (indexChange != -1)
                        fields[indexChange] = "\"" + value + "\"";

                    lines[i] = String.Join(",", fields);
                    break;
                }
            }

            WriteText(lines);
            UnityEditor.AssetDatabase.Refresh();
        }

#endif

    }
}
