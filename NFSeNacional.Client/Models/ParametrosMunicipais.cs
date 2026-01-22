using System.Xml.Serialization;

namespace NFSeNacional.Client.Models
{
    /// <summary>
    /// Parâmetros municipais do Sistema Nacional NFS-e
    /// </summary>
    [XmlRoot("ParametrosMunicipais", Namespace = "http://www.sefin.fortaleza.ce.gov.br/nfse")]
    public class ParametrosMunicipais
    {
        /// <summary>
        /// Código do município (IBGE - 7 dígitos)
        /// </summary>
        [XmlElement("cMun")]
        public string? CodigoMunicipio { get; set; }

        /// <summary>
        /// Nome do município
        /// </summary>
        [XmlElement("xMun")]
        public string? NomeMunicipio { get; set; }

        /// <summary>
        /// Situação do convênio (1=Ativo, 2=Inativo)
        /// </summary>
        [XmlElement("sitConv")]
        public int SituacaoConvenio { get; set; }

        /// <summary>
        /// Data de início do convênio
        /// </summary>
        [XmlElement("dtIniConv")]
        public DateTime? DataInicioConvenio { get; set; }

        /// <summary>
        /// Data de fim do convênio
        /// </summary>
        [XmlElement("dtFimConv")]
        public DateTime? DataFimConvenio { get; set; }
    }

    /// <summary>
    /// Parâmetros de convênio do município
    /// </summary>
    [XmlRoot("ParametrosConvenio", Namespace = "http://www.sefin.fortaleza.ce.gov.br/nfse")]
    public class ParametrosConvenio
    {
        /// <summary>
        /// Código do município (IBGE - 7 dígitos)
        /// </summary>
        [XmlElement("cMun")]
        public string? CodigoMunicipio { get; set; }

        /// <summary>
        /// Situação do convênio (1=Ativo, 2=Inativo)
        /// </summary>
        [XmlElement("sitConv")]
        public int SituacaoConvenio { get; set; }

        /// <summary>
        /// Data de início do convênio
        /// </summary>
        [XmlElement("dtIniConv")]
        public DateTime? DataInicioConvenio { get; set; }

        /// <summary>
        /// Data de fim do convênio
        /// </summary>
        [XmlElement("dtFimConv")]
        public DateTime? DataFimConvenio { get; set; }

        /// <summary>
        /// Email de contato do município
        /// </summary>
        [XmlElement("email")]
        public string? Email { get; set; }

        /// <summary>
        /// Telefone de contato do município
        /// </summary>
        [XmlElement("fone")]
        public string? Telefone { get; set; }
    }

    /// <summary>
    /// Alíquotas e regimes especiais por código de serviço
    /// </summary>
    [XmlRoot("ParametrosServico", Namespace = "http://www.sefin.fortaleza.ce.gov.br/nfse")]
    public class ParametrosServico
    {
        /// <summary>
        /// Código do município (IBGE - 7 dígitos)
        /// </summary>
        [XmlElement("cMun")]
        public string? CodigoMunicipio { get; set; }

        /// <summary>
        /// Código de serviço da LC 116/2003
        /// </summary>
        [XmlElement("cServ")]
        public string? CodigoServico { get; set; }

        /// <summary>
        /// Descrição do serviço
        /// </summary>
        [XmlElement("xServ")]
        public string? DescricaoServico { get; set; }

        /// <summary>
        /// Alíquota do ISS (%)
        /// </summary>
        [XmlElement("aliq")]
        public decimal Aliquota { get; set; }

        /// <summary>
        /// Alíquota mínima do ISS (%)
        /// </summary>
        [XmlElement("aliqMin")]
        public decimal AliquotaMinima { get; set; }

        /// <summary>
        /// Alíquota máxima do ISS (%)
        /// </summary>
        [XmlElement("aliqMax")]
        public decimal AliquotaMaxima { get; set; }

        /// <summary>
        /// Regime especial de tributação (se aplicável)
        /// </summary>
        [XmlElement("regEspTrib")]
        public RegimeEspecialTributacao? RegimeEspecialTributacao { get; set; }
    }

    /// <summary>
    /// Parâmetros de retenções do contribuinte
    /// </summary>
    [XmlRoot("ParametrosRetencoes", Namespace = "http://www.sefin.fortaleza.ce.gov.br/nfse")]
    public class ParametrosRetencoes
    {
        /// <summary>
        /// Código do município (IBGE - 7 dígitos)
        /// </summary>
        [XmlElement("cMun")]
        public string? CodigoMunicipio { get; set; }

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
        /// Retém PIS (1=Não, 2=Sim)
        /// </summary>
        [XmlElement("retPIS")]
        public int RetemPIS { get; set; }

        /// <summary>
        /// Retém COFINS (1=Não, 2=Sim)
        /// </summary>
        [XmlElement("retCOFINS")]
        public int RetemCOFINS { get; set; }

        /// <summary>
        /// Retém INSS (1=Não, 2=Sim)
        /// </summary>
        [XmlElement("retINSS")]
        public int RetemINSS { get; set; }

        /// <summary>
        /// Retém IR (1=Não, 2=Sim)
        /// </summary>
        [XmlElement("retIR")]
        public int RetemIR { get; set; }

        /// <summary>
        /// Retém CSLL (1=Não, 2=Sim)
        /// </summary>
        [XmlElement("retCSLL")]
        public int RetemCSLL { get; set; }
    }

    /// <summary>
    /// Benefícios municipais do contribuinte
    /// </summary>
    [XmlRoot("ParametrosBeneficios", Namespace = "http://www.sefin.fortaleza.ce.gov.br/nfse")]
    public class ParametrosBeneficios
    {
        /// <summary>
        /// Código do município (IBGE - 7 dígitos)
        /// </summary>
        [XmlElement("cMun")]
        public string? CodigoMunicipio { get; set; }

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
        /// Código do benefício
        /// </summary>
        [XmlElement("cBenef")]
        public string? CodigoBeneficio { get; set; }

        /// <summary>
        /// Descrição do benefício
        /// </summary>
        [XmlElement("xBenef")]
        public string? DescricaoBeneficio { get; set; }

        /// <summary>
        /// Data de início do benefício
        /// </summary>
        [XmlElement("dtInicio")]
        public DateTime? DataInicio { get; set; }

        /// <summary>
        /// Data de fim do benefício
        /// </summary>
        [XmlElement("dtFim")]
        public DateTime? DataFim { get; set; }
    }
}
