using System.Xml.Serialization;

namespace NFSeNacional.Client.Models
{
    /// <summary>
    /// Evento de NFS-e (cancelamento, confirmação, rejeição, etc.)
    /// </summary>
    [XmlRoot("Evento", Namespace = "http://www.sefin.fortaleza.ce.gov.br/nfse")]
    public class Evento
    {
        /// <summary>
        /// Informações do evento
        /// </summary>
        [XmlElement("infEvento")]
        public InfEvento? InfEvento { get; set; }

        /// <summary>
        /// Assinatura digital (conforme padrão xmldsig)
        /// </summary>
        [XmlElement("Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public object? Assinatura { get; set; }
    }

    /// <summary>
    /// Informações do evento
    /// </summary>
    public class InfEvento
    {
        /// <summary>
        /// ID do evento
        /// </summary>
        [XmlAttribute("Id")]
        public string? Id { get; set; }

        /// <summary>
        /// Chave de acesso da NFS-e relacionada
        /// </summary>
        [XmlElement("chNfse")]
        public string? ChaveAcessoNFSe { get; set; }

        /// <summary>
        /// Tipo do evento
        /// </summary>
        [XmlElement("tpEvento")]
        public string? TipoEvento { get; set; }

        /// <summary>
        /// Número sequencial do evento
        /// </summary>
        [XmlElement("nSeqEvento")]
        public int NumeroSequencialEvento { get; set; }

        /// <summary>
        /// Data e hora do evento
        /// </summary>
        [XmlElement("dhEvento")]
        public DateTime DataHoraEvento { get; set; }

        /// <summary>
        /// Dados específicos do tipo de evento
        /// </summary>
        [XmlElement("detEvento")]
        public DetalhesEvento? DetalhesEvento { get; set; }
    }

    /// <summary>
    /// Detalhes específicos do evento
    /// </summary>
    public class DetalhesEvento
    {
        /// <summary>
        /// Descrição do evento
        /// </summary>
        [XmlElement("descEvento")]
        public string? DescricaoEvento { get; set; }

        /// <summary>
        /// Motivo/justificativa do evento
        /// </summary>
        [XmlElement("xJust")]
        public string? Justificativa { get; set; }

        /// <summary>
        /// Código de cancelamento (se aplicável)
        /// </summary>
        [XmlElement("cCanc")]
        public string? CodigoCancelamento { get; set; }
    }

    /// <summary>
    /// Pedido de registro de evento
    /// </summary>
    [XmlRoot("pedRegEvento", Namespace = "http://www.sefin.fortaleza.ce.gov.br/nfse")]
    public class PedidoRegistroEvento
    {
        /// <summary>
        /// Informações do pedido de registro
        /// </summary>
        [XmlElement("infPedReg")]
        public InfPedidoRegistro? InfPedidoRegistro { get; set; }

        /// <summary>
        /// Assinatura digital (conforme padrão xmldsig)
        /// </summary>
        [XmlElement("Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public object? Assinatura { get; set; }
    }

    /// <summary>
    /// Informações do pedido de registro de evento
    /// </summary>
    public class InfPedidoRegistro
    {
        /// <summary>
        /// Chave de acesso da NFS-e
        /// </summary>
        [XmlElement("chNfse")]
        public string? ChaveAcessoNFSe { get; set; }

        /// <summary>
        /// Tipo do evento
        /// </summary>
        [XmlElement("tpEvento")]
        public string? TipoEvento { get; set; }

        /// <summary>
        /// Número sequencial do evento
        /// </summary>
        [XmlElement("nSeqEvento")]
        public int NumeroSequencialEvento { get; set; }

        /// <summary>
        /// Data e hora do evento
        /// </summary>
        [XmlElement("dhEvento")]
        public DateTime DataHoraEvento { get; set; }

        /// <summary>
        /// Detalhes do evento
        /// </summary>
        [XmlElement("detEvento")]
        public DetalhesEvento? DetalhesEvento { get; set; }
    }

    /// <summary>
    /// Resposta do registro de evento
    /// </summary>
    [XmlRoot("RespostaEvento", Namespace = "http://www.sefin.fortaleza.ce.gov.br/nfse")]
    public class RespostaEvento
    {
        /// <summary>
        /// Status da operação
        /// </summary>
        [XmlElement("status")]
        public string? Status { get; set; }

        /// <summary>
        /// Evento registrado (se sucesso)
        /// </summary>
        [XmlElement("Evento")]
        public Evento? Evento { get; set; }

        /// <summary>
        /// Mensagens de erro (se houver)
        /// </summary>
        [XmlArray("mensagens")]
        [XmlArrayItem("mensagem")]
        public List<MensagemRetorno>? Mensagens { get; set; }
    }

    /// <summary>
    /// Lista de eventos de uma NFS-e
    /// </summary>
    [XmlRoot("ListaEventos", Namespace = "http://www.sefin.fortaleza.ce.gov.br/nfse")]
    public class ListaEventos
    {
        /// <summary>
        /// Eventos da NFS-e
        /// </summary>
        [XmlElement("Evento")]
        public List<Evento>? Eventos { get; set; }
    }
}
