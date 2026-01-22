using System.Xml.Serialization;

namespace NFSeNacional.Client.Models
{
    /// <summary>
    /// Tipos de regime especial de tributação
    /// </summary>
    public enum RegimeEspecialTributacao
    {
        /// <summary>
        /// Microempresa Municipal
        /// </summary>
        [XmlEnum("1")]
        MicroempresaMunicipal = 1,

        /// <summary>
        /// Estimativa
        /// </summary>
        [XmlEnum("2")]
        Estimativa = 2,

        /// <summary>
        /// Sociedade de Profissionais
        /// </summary>
        [XmlEnum("3")]
        SociedadeProfissionais = 3,

        /// <summary>
        /// Cooperativa
        /// </summary>
        [XmlEnum("4")]
        Cooperativa = 4,

        /// <summary>
        /// MEI - Microempreendedor Individual
        /// </summary>
        [XmlEnum("5")]
        MEI = 5,

        /// <summary>
        /// ME EPP - Simples Nacional
        /// </summary>
        [XmlEnum("6")]
        SimplesNacional = 6
    }

    /// <summary>
    /// Tipos de evento da NFS-e
    /// </summary>
    public enum TipoEvento
    {
        /// <summary>
        /// Cancelamento - Pedido de Cancelamento
        /// </summary>
        [XmlEnum("e101101")]
        CancelamentoPedido = 101101,

        /// <summary>
        /// Cancelamento por Substituição
        /// </summary>
        [XmlEnum("e105102")]
        CancelamentoPorSubstituicao = 105102,

        /// <summary>
        /// Cancelamento - Solicitação de Análise
        /// </summary>
        [XmlEnum("e105103")]
        CancelamentoSolicitacaoAnalise = 105103,

        /// <summary>
        /// Confirmação da Operação pelo Prestador
        /// </summary>
        [XmlEnum("e202101")]
        ConfirmacaoPrestador = 202101,

        /// <summary>
        /// Confirmação da Operação pelo Tomador
        /// </summary>
        [XmlEnum("e202102")]
        ConfirmacaoTomador = 202102,

        /// <summary>
        /// Confirmação da Operação pelo Intermediário
        /// </summary>
        [XmlEnum("e202103")]
        ConfirmacaoIntermediario = 202103,

        /// <summary>
        /// Rejeição da Operação pelo Prestador
        /// </summary>
        [XmlEnum("e203101")]
        RejeicaoPrestador = 203101,

        /// <summary>
        /// Rejeição da Operação pelo Tomador
        /// </summary>
        [XmlEnum("e203102")]
        RejeicaoTomador = 203102,

        /// <summary>
        /// Rejeição da Operação pelo Intermediário
        /// </summary>
        [XmlEnum("e203103")]
        RejeicaoIntermediario = 203103,

        /// <summary>
        /// Cancelamento por Ofício
        /// </summary>
        [XmlEnum("e305101")]
        CancelamentoPorOficio = 305101,

        /// <summary>
        /// Bloqueio
        /// </summary>
        [XmlEnum("e305102")]
        Bloqueio = 305102,

        /// <summary>
        /// Desbloqueio
        /// </summary>
        [XmlEnum("e305103")]
        Desbloqueio = 305103
    }

    /// <summary>
    /// Situação de tributação do serviço
    /// </summary>
    public enum SituacaoTributaria
    {
        /// <summary>
        /// Tributação no município
        /// </summary>
        [XmlEnum("1")]
        TributacaoNoMunicipio = 1,

        /// <summary>
        /// Tributação fora do município
        /// </summary>
        [XmlEnum("2")]
        TributacaoForaMunicipio = 2,

        /// <summary>
        /// Isenção
        /// </summary>
        [XmlEnum("3")]
        Isencao = 3,

        /// <summary>
        /// Imune
        /// </summary>
        [XmlEnum("4")]
        Imune = 4,

        /// <summary>
        /// Exigibilidade suspensa por decisão judicial
        /// </summary>
        [XmlEnum("5")]
        ExigibilidadeSuspensaDecisaoJudicial = 5,

        /// <summary>
        /// Exigibilidade suspensa por processo administrativo
        /// </summary>
        [XmlEnum("6")]
        ExigibilidadeSuspensaProcessoAdministrativo = 6
    }

    /// <summary>
    /// Tipo de inscrição (CPF ou CNPJ)
    /// </summary>
    public enum TipoInscricao
    {
        /// <summary>
        /// CPF - Pessoa Física
        /// </summary>
        [XmlEnum("1")]
        CPF = 1,

        /// <summary>
        /// CNPJ - Pessoa Jurídica
        /// </summary>
        [XmlEnum("2")]
        CNPJ = 2
    }

    /// <summary>
    /// Natureza da operação
    /// </summary>
    public enum NaturezaOperacao
    {
        /// <summary>
        /// Tributação no município
        /// </summary>
        [XmlEnum("1")]
        TributacaoNoMunicipio = 1,

        /// <summary>
        /// Tributação fora do município
        /// </summary>
        [XmlEnum("2")]
        TributacaoForaMunicipio = 2,

        /// <summary>
        /// Isenção
        /// </summary>
        [XmlEnum("3")]
        Isencao = 3,

        /// <summary>
        /// Imune
        /// </summary>
        [XmlEnum("4")]
        Imune = 4,

        /// <summary>
        /// Exigibilidade suspensa por decisão judicial
        /// </summary>
        [XmlEnum("5")]
        ExigibilidadeSuspensaDecisaoJudicial = 5,

        /// <summary>
        /// Exigibilidade suspensa por processo administrativo
        /// </summary>
        [XmlEnum("6")]
        ExigibilidadeSuspensaProcessoAdministrativo = 6
    }

    /// <summary>
    /// Indicador de incentivo fiscal
    /// </summary>
    public enum IndicadorIncentivo
    {
        /// <summary>
        /// Não
        /// </summary>
        [XmlEnum("1")]
        Nao = 1,

        /// <summary>
        /// Sim
        /// </summary>
        [XmlEnum("2")]
        Sim = 2
    }
}
