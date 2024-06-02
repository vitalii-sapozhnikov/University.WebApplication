using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class ScientificConferenceTheses: ScientificPublication
    {
        public string ConferenceName { get; set; }
        public string ConferencePlace { get; set; }

        public override string BibliographicReference =>
            $"{string.Join(", ", Authors.Select(a => a.ShortName))}, {Title} // {ConferenceName}. {PublicationDate.Value.ToShortDateString()}, {ConferencePlace} - {this.Volume} с. {(URL != null ? $"- Режим доступу: {URL}" : "")}";
    }
}
