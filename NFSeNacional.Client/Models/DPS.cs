using System.Xml.Serialization;

namespace NFSeNacional.Client.Models
{
    /// <summary>
    /// Declaração de Prestação de Serviços (DPS)
    /// </summary>
    [XmlRoot("Dps", Namespace = "http://www.sefin.fortaleza.ce.gov.br/nfse")]
    public class DPS
    {
        /// <summary>
        /// Informações da DPS
        /// </summary>
        [XmlElement("infDps")]
        public InfDPS? InfDps { get; set; }

        /// <summary>
        /// Assinatura digital (conforme padrão xmldsig)
        /// </summary>
        [XmlElement("Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public object? Assinatura { get; set; }
    }

    /// <summary>
    /// Informações da DPS
    /// </summary>
    public class InfDPS
    {
        /// <summary>
        /// ID da DPS (42 caracteres)
        /// Formato: Cód. Município (7) + Tipo Inscrição (1) + Inscrição Federal (14) + Série DPS (5) + Núm. DPS (15)
        /// </summary>
        [XmlAttribute("Id")]
        public string? Id { get; set; }

        /// <summary>
        /// Substituta - Chave de acesso da NFS-e substituída
        /// </summary>
        [XmlElement("substituta")]
        public Substituta? Substituta { get; set; }

        /// <summary>
        /// Dados do prestador de serviços
        /// </summary>
        [XmlElement("prest")]
        public Prestador? Prestador { get; set; }

        /// <summary>
        /// Dados do tomador de serviços
        /// </summary>
        [XmlElement("toma")]
        public Tomador? Tomador { get; set; }

        /// <summary>
        /// Dados do intermediário (opcional)
        /// </summary>
        [XmlElement("interm")]
        public Intermediario? Intermediario { get; set; }

        /// <summary>
        /// Dados do serviço prestado
        /// </summary>
        [XmlElement("serv")]
        public Servico? Servico { get; set; }

        /// <summary>
        /// Valores da DPS
        /// </summary>
        [XmlElement("valores")]
        public ValoresDPS? Valores { get; set; }

        /// <summary>
        /// Data e hora de competência (emissão)
        /// </summary>
        [XmlElement("competencia")]
        public DateTime Competencia { get; set; }

        /// <summary>
        /// Número da DPS
        /// </summary>
        [XmlElement("nDps")]
        public string? NumeroDPS { get; set; }

        /// <summary>
        /// Série da DPS
        /// </summary>
        [XmlElement("serie")]
        public string? Serie { get; set; }

        /// <summary>
        /// Data e hora de emissão da DPS
        /// </summary>
        [XmlElement("dhEmi")]
        public DateTime DataHoraEmissao { get; set; }

        /// <summary>
        /// Natureza da operação
        /// </summary>
        [XmlElement("natOp")]
        public NaturezaOperacao NaturezaOperacao { get; set; }
    }

    /// <summary>
    /// Informações da NFS-e substituta
    /// </summary>
    public class Substituta
    {
        /// <summary>
        /// Chave de acesso da NFS-e substituída (50 caracteres)
        /// </summary>
        [XmlElement("chave")]
        public string? ChaveAcesso { get; set; }
    }

    /// <summary>
    /// Dados do prestador de serviços
    /// </summary>
    public class Prestador
    {
        /// <summary>
        /// CNPJ do prestador
        /// </summary>
        [XmlElement("CNPJ")]
        public string? CNPJ { get; set; }

        /// <summary>
        /// Inscrição Municipal
        /// </summary>
        [XmlElement("IM")]
        public string? InscricaoMunicipal { get; set; }

        /// <summary>
        /// Código do município (IBGE - 7 dígitos)
        /// </summary>
        [XmlElement("cMun")]
        public string? CodigoMunicipio { get; set; }
    }

    /// <summary>
    /// Dados do tomador de serviços
    /// </summary>
    public class Tomador
    {
        /// <summary>
        /// Tipo de inscrição (1=CPF, 2=CNPJ)
        /// </summary>
        [XmlElement("tpInsc")]
        public TipoInscricao TipoInscricao { get; set; }

        /// <summary>
        /// Número de inscrição (CPF ou CNPJ)
        /// </summary>
        [XmlElement("nInsc")]
        public string? NumeroInscricao { get; set; }

        /// <summary>
        /// Razão social ou nome
        /// </summary>
        [XmlElement("xNome")]
        public string? Nome { get; set; }

        /// <summary>
        /// Endereço do tomador
        /// </summary>
        [XmlElement("end")]
        public Endereco? Endereco { get; set; }

        /// <summary>
        /// Telefone
        /// </summary>
        [XmlElement("fone")]
        public string? Telefone { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        [XmlElement("email")]
        public string? Email { get; set; }
    }

    /// <summary>
    /// Dados do intermediário da operação
    /// </summary>
    public class Intermediario
    {
        /// <summary>
        /// Tipo de inscrição (1=CPF, 2=CNPJ)
        /// </summary>
        [XmlElement("tpInsc")]
        public TipoInscricao TipoInscricao { get; set; }

        /// <summary>
        /// Número de inscrição (CPF ou CNPJ)
        /// </summary>
        [XmlElement("nInsc")]
        public string? NumeroInscricao { get; set; }

        /// <summary>
        /// Razão social ou nome
        /// </summary>
        [XmlElement("xNome")]
        public string? Nome { get; set; }

        /// <summary>
        /// Código do município (IBGE - 7 dígitos)
        /// </summary>
        [XmlElement("cMun")]
        public string? CodigoMunicipio { get; set; }
    }

    /// <summary>
    /// Endereço
    /// </summary>
    public class Endereco
    {
        /// <summary>
        /// Tipo de logradouro
        /// </summary>
        [XmlElement("tpLog")]
        public string? TipoLogradouro { get; set; }

        /// <summary>
        /// Logradouro
        /// </summary>
        [XmlElement("xLog")]
        public string? Logradouro { get; set; }

        /// <summary>
        /// Número
        /// </summary>
        [XmlElement("nro")]
        public string? Numero { get; set; }

        /// <summary>
        /// Complemento
        /// </summary>
        [XmlElement("xCpl")]
        public string? Complemento { get; set; }

        /// <summary>
        /// Bairro
        /// </summary>
        [XmlElement("bairro")]
        public string? Bairro { get; set; }

        /// <summary>
        /// Código do município (IBGE - 7 dígitos)
        /// </summary>
        [XmlElement("cMun")]
        public string? CodigoMunicipio { get; set; }

        /// <summary>
        /// CEP
        /// </summary>
        [XmlElement("CEP")]
        public string? CEP { get; set; }
    }

    /// <summary>
    /// Dados do serviço prestado
    /// </summary>
    public class Servico
    {
        /// <summary>
        /// Código de tributação do município
        /// </summary>
        [XmlElement("cTribMun")]
        public string? CodigoTributacaoMunicipal { get; set; }

        /// <summary>
        /// Código CNAE
        /// </summary>
        [XmlElement("cCnae")]
        public string? CodigoCNAE { get; set; }

        /// <summary>
        /// Código de serviço da LC 116/2003
        /// </summary>
        [XmlElement("cServ")]
        public string? CodigoServico { get; set; }

        /// <summary>
        /// Discriminação dos serviços
        /// </summary>
        [XmlElement("xServ")]
        public string? Discriminacao { get; set; }

        /// <summary>
        /// Código do município de incidência
        /// </summary>
        [XmlElement("cMunIncid")]
        public string? CodigoMunicipioIncidencia { get; set; }
    }

    /// <summary>
    /// Valores da DPS
    /// </summary>
    public class ValoresDPS
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
        /// Indicador de incentivo fiscal (1=Não, 2=Sim)
        /// </summary>
        [XmlElement("indIncentivo")]
        public IndicadorIncentivo IndicadorIncentivo { get; set; }
    }
}
