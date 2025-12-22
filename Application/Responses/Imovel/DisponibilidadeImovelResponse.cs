using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses.Imovel
{
    public class DisponibilidadeImovelResponse
    {
        public int ImovelId { get; set; }
        public string? NomeImovel { get; set; }
        public DateOnly DataConsultada { get; set; }
        public bool EstaDisponivel { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public AluguelConflitanteInfo? AluguelConflitante { get; set; }
    }

    public class AluguelConflitanteInfo
    {
        public int AluguelId { get; set; }
        public DateOnly DataInicio { get; set; }
        public DateOnly DataFim { get; set; }
        public string? NomeInquilino { get; set; }
    }
}
