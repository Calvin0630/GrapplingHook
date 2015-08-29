using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[XmlRoot("HighScores")]
public class XMLManager {
    [XmlArray("scores"), XmlArrayItem("Score")]
    public List<Score> scores = new List<Score>();
    static string defaultPath = Path.Combine(Application.dataPath, "HighScores.xml");
    public void Save() {
        var serializer = new XmlSerializer(typeof(XMLManager));
        using (var stream = new FileStream(defaultPath, FileMode.Create)) {
            serializer.Serialize(stream, this);
        }
    }

    public static XMLManager Load() {
        var serializer = new XmlSerializer(typeof(XMLManager));
        using (var stream = new FileStream(defaultPath, FileMode.Open)) {
            return serializer.Deserialize(stream) as XMLManager;
        }
    }
}