using UnityEngine;
using System.Collections.Generic;

public static class GameConfigLoader
{
    private static string[] Split(string line)
    {
        string[] values = line.Split(',');
        if (values.Length <= 1) values = line.Split('\t');
        for (int i = 0; i < values.Length; i++) values[i] = values[i].Trim();
        return values;
    }
    public static ConfigData LoadData(TextAsset csvFile)
    {
        if (csvFile == null) return null;

        string[] lines = csvFile.text.Split('\n');
        if (lines.Length == 0) return null;

        string[] columnNames = Split(lines[0]);
        if (columnNames.Length == 0) return null;

        Column[] cols = new Column[columnNames.Length];
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i] = new Column(columnNames[i], new List<string>());
        }

        for (int i = 1; i < lines.Length; i++)
        {
            if (lines.Length == 0) continue;
            string[] values = Split(lines[i]);
            for(int j = 0; j < columnNames.Length; j++)
            {
                cols[j].values.Add(j < values.Length ? values[j] : "");
            }
        }
        return new ConfigData(cols);
    }
}

public class ConfigData
{
    public Column[] columns;

    public ConfigData(Column[] columns)
    {
        this.columns = columns;
    }

    public int GetLength()
    {
        return columns[0].values.Count;
    }

    public List<string> GetData(int id)
    {
        if (id >= columns.Length) return null;
        return columns[id].values;
    }
    public string GetDataAttribute(int id, string attribute)
    {
        if (id >= columns.Length) return null;
        int attributeIndex = -1;
        for (int i = 0; i < columns.Length; i++)
        {
            if (columns[i].columnName == attribute)
            {
                attributeIndex = i;
                break;
            }
        }
        if (attributeIndex < 0) return null;
        return columns[attributeIndex].GetValue(id);
    }
}

public class Column
{
    public string columnName;
    public List<string> values;

    public Column(string columnName, List<string> values)
    {
        this.columnName = columnName;
        this.values = values;
    }

    public string GetValue(int attributeIndex)
    {
        if (attributeIndex >= values.Count) return null;
        return values[attributeIndex];
    }
}
