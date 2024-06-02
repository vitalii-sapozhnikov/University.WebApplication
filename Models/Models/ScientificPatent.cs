using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class ScientificPatent: ScientificPublication
    {
        public string PatentNo { get; set; }
        public string Issuer { get; set; }

        public override string BibliographicReference =>
            $"{string.Join(", ", Authors.Select(a => a.ShortName))}, {Title} // Номер патенту: {PatentNo}, {Issuer}, {PublicationDate.Value.Year} - {Volume} с. {(URL != null ? $"- Режим доступу: {URL}" : "")}";
    }
}
