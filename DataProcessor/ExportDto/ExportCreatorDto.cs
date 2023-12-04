using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Creator")]
    public class ExportCreatorDto
    {
        [XmlElement("CreatorName")]
        public string CreatorName { get; set; }
        [XmlAttribute("BoardgamesCount")]
        public int BoardgamesCount { get; set; }
        [XmlArray("Boardgames")]
        public List<ExportBoardgameDto> Boardgames { get; set; }

    }
}
