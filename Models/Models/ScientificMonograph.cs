using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class ScientificMonograph: ScientificPublication
    {
        public string Publisher { get; set; }
        public string ISBN { get; set; }

        public override string BibliographicReference =>
            $"{string.Join(", ", Authors.Select(a => a.ShortName))}, {Title} // {Publisher}, ISBN: {ISBN} / {PublicationDate.Value.Year}  - {this.Volume} с. {(URL != null ? $"- Режим доступу: {URL}" : "")}";
    }
}
