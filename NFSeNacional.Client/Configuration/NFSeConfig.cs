using System.Security.Cryptography.X509Certificates;

namespace NFSeNacional.Client.Configuration
{
    /// <summary>
    /// Configuração para conexão com as APIs do Sistema Nacional NFS-e
    /// </summary>
    public class NFSeConfig
    {
        /// <summary>
        /// URL base da API (produção ou homologação)
        /// </summary>
        /// <remarks>
        /// Produção Restrita (Homologação): https://adn.producaorestrita.nfse.gov.br/contribuintes
        /// </remarks>
        public string BaseUrl { get; set; } = "https://adn.producaorestrita.nfse.gov.br/contribuintes";

        /// <summary>
        /// Ambiente de execução
        /// </summary>
        /// <remarks>
        /// 1 = Produção, 2 = Homologação
        /// </remarks>
        public int Ambiente { get; set; } = 2;

        /// <summary>
        /// Certificado digital para autenticação (A1 ou A3)
        /// </summary>
        public X509Certificate2? CertificadoDigital { get; set; }

        /// <summary>
        /// Timeout em segundos para requisições HTTP
        /// </summary>
        public int TimeoutSegundos { get; set; } = 30;

        /// <summary>
        /// Versão do aplicativo cliente
        /// </summary>
        public string VersaoAplicativo { get; set; } = "1.01";

        /// <summary>
        /// Indica se deve validar o certificado SSL do servidor
        /// </summary>
        /// <remarks>
        /// Em ambiente de homologação, pode ser necessário definir como false
        /// </remarks>
        public bool ValidarCertificadoServidor { get; set; } = true;

        /// <summary>
        /// Valida se a configuração está completa e válida
        /// </summary>
        /// <returns>True se a configuração é válida</returns>
        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(BaseUrl))
                return false;

            if (Ambiente != 1 && Ambiente != 2)
                return false;

            if (CertificadoDigital == null)
                return false;

            if (TimeoutSegundos <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(VersaoAplicativo))
                return false;

            return true;
        }

        /// <summary>
        /// Obtém mensagens de validação da configuração
        /// </summary>
        /// <returns>Lista de mensagens de erro, ou lista vazia se válido</returns>
        public IEnumerable<string> GetValidationErrors()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(BaseUrl))
                errors.Add("BaseUrl é obrigatória");

            if (Ambiente != 1 && Ambiente != 2)
                errors.Add("Ambiente deve ser 1 (Produção) ou 2 (Homologação)");

            if (CertificadoDigital == null)
                errors.Add("CertificadoDigital é obrigatório");

            if (TimeoutSegundos <= 0)
                errors.Add("TimeoutSegundos deve ser maior que zero");

            if (string.IsNullOrWhiteSpace(VersaoAplicativo))
                errors.Add("VersaoAplicativo é obrigatória");

            return errors;
        }
    }
}
