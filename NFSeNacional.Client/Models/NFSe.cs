using System.Xml.Serialization;

namespace NFSeNacional.Client.Models
{
    /// <summary>
    /// Nota Fiscal de Serviço Eletrônica (NFS-e)
    /// </summary>
    [XmlRoot("NFSe", Namespace = "http://www.sefin.fortaleza.ce.gov.br/nfse")]
    public class NFSe
    {
        /// <summary>
        /// Informações da NFS-e
        /// </summary>
        [XmlElement("infNfse")]
        public InfNFSe? InfNfse { get; set; }

        /// <summary>
        /// Assinatura digital (conforme padrão xmldsig)
        /// </summary>
        [XmlElement("Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public object? Assinatura { get; set; }
    }

    /// <summary>
    /// Informações da NFS-e
    /// </summary>
    public class InfNFSe
    {
        /// <summary>
        /// Chave de acesso da NFS-e (50 caracteres)
        /// </summary>
        [XmlAttribute("Id")]
        public string? ChaveAcesso { get; set; }

        /// <summary>
        /// Número da NFS-e
        /// </summary>
        [XmlElement("nNfse")]
        public string? NumeroNFSe { get; set; }

        /// <summary>
        /// Código de verificação
        /// </summary>
        [XmlElement("cVerif")]
        public string? CodigoVerificacao { get; set; }

        /// <summary>
        /// Data e hora de emissão
        /// </summary>
        [XmlElement("dhEmi")]
        public DateTime DataHoraEmissao { get; set; }

        /// <summary>
        /// Dados do prestador
        /// </summary>
        [XmlElement("prest")]
        public Prestador? Prestador { get; set; }

        /// <summary>
        /// Dados do tomador
        /// </summary>
        [XmlElement("toma")]
        public Tomador? Tomador { get; set; }

        /// <summary>
        /// Dados do intermediário (se houver)
        /// </summary>
        [XmlElement("interm")]
        public Intermediario? Intermediario { get; set; }

        /// <summary>
        /// Dados do serviço
        /// </summary>
        [XmlElement("serv")]
        public Servico? Servico { get; set; }

        /// <summary>
        /// Valores da NFS-e
        /// </summary>
        [XmlElement("valores")]
        public ValoresNFSe? Valores { get; set; }

        /// <summary>
        /// Situação da NFS-e (1=Normal, 2=Cancelada, 3=Substituída)
        /// </summary>
        [XmlElement("situacao")]
        public int Situacao { get; set; }

        /// <summary>
        /// Data e hora de competência
        /// </summary>
        [XmlElement("competencia")]
        public DateTime Competencia { get; set; }
    }

    /// <summary>
    /// Valores da NFS-e
    /// </summary>
    public class ValoresNFSe
    {
        /// <summary>
        /// Valor dos serviços
        /// </summary>
        [XmlElement("vServ")]
        public decimal ValorServicos { get; set; }

        /// <summary>
        /// Valor das deduções
        /// </summary>
        [XmlElement("vDed")]
        public decimal ValorDeducoes { get; set; }

        /// <summary>
        /// Valor da base de cálculo
        /// </summary>
        [XmlElement("vBC")]
        public decimal ValorBaseCalculo { get; set; }

        /// <summary>
        /// Alíquota do ISS (%)
        /// </summary>
        [XmlElement("aliq")]
        public decimal Aliquota { get; set; }

        /// <summary>
        /// Valor do ISS
        /// </summary>
        [XmlElement("vISS")]
        public decimal ValorISS { get; set; }

        /// <summary>
        /// Valor líquido da NFS-e
        /// </summary>
        [XmlElement("vLiq")]
        public decimal ValorLiquido { get; set; }

        /// <summary>
        /// Valor do PIS
        /// </summary>
        [XmlElement("vPIS")]
        public decimal ValorPIS { get; set; }

        /// <summary>
        /// Valor do COFINS
        /// </summary>
        [XmlElement("vCOFINS")]
        public decimal ValorCOFINS { get; set; }

        /// <summary>
        /// Valor do INSS
        /// </summary>
        [XmlElement("vINSS")]
        public decimal ValorINSS { get; set; }

        /// <summary>
        /// Valor do IR
        /// </summary>
        [XmlElement("vIR")]
        public decimal ValorIR { get; set; }

        /// <summary>
        /// Valor do CSLL
        /// </summary>
        [XmlElement("vCSLL")]
        public decimal ValorCSLL { get; set; }

        /// <summary>
        /// Indicador de incentivo fiscal (1=Não, 2=Sim)
        /// </summary>
        [XmlElement("indIncentivo")]
        public IndicadorIncentivo IndicadorIncentivo { get; set; }
    }

    /// <summary>
    /// Resposta da emissão de NFS-e
    /// </summary>
    [XmlRoot("RespostaNFSe", Namespace = "http://www.sefin.fortaleza.ce.gov.br/nfse")]
    public class RespostaNFSe
    {
        /// <summary>
        /// Status da operação (sucesso ou erro)
        /// </summary>
        [XmlElement("status")]
        public string? Status { get; set; }

        /// <summary>
        /// NFS-e gerada (se sucesso)
        /// </summary>
        [XmlElement("NFSe")]
        public NFSe? NFSe { get; set; }

        /// <summary>
        /// Mensagens de erro (se houver)
        /// </summary>
        [XmlArray("mensagens")]
        [XmlArrayItem("mensagem")]
        public List<MensagemRetorno>? Mensagens { get; set; }
    }

    /// <summary>
    /// Mensagem de retorno da API
    /// </summary>
    public class MensagemRetorno
    {
        /// <summary>
        /// Código da mensagem
        /// </summary>
        [XmlElement("codigo")]
        public string? Codigo { get; set; }

        /// <summary>
        /// Descrição da mensagem
        /// </summary>
        [XmlElement("descricao")]
        public string? Descricao { get; set; }

        /// <summary>
        /// Correção sugerida
        /// </summary>
        [XmlElement("correcao")]
        public string? Correcao { get; set; }
    }
}
