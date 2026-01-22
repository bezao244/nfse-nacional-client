using NFSeNacional.Client.Configuration;
using NFSeNacional.Client.Models;

namespace NFSeNacional.Client.Services
{
    /// <summary>
    /// Serviço para consulta de parâmetros municipais do Sistema Nacional NFS-e
    /// </summary>
    public class ParametrosMunicipaisService : IDisposable
    {
        private readonly NFSeHttpClient _httpClient;
        private bool _disposed;

        /// <summary>
        /// Inicializa uma nova instância do serviço
        /// </summary>
        /// <param name="config">Configuração de conexão</param>
        public ParametrosMunicipaisService(NFSeConfig config)
        {
            _httpClient = new NFSeHttpClient(config);
        }

        /// <summary>
        /// Consulta os parâmetros do convênio de um município específico
        /// </summary>
        /// <param name="codigoMunicipio">Código do município (IBGE - 7 dígitos)</param>
        /// <returns>Parâmetros do convênio do município</returns>
        /// <exception cref="NFSeException">Lançada em caso de erro na requisição</exception>
        public async Task<ParametrosConvenio> ConsultarConvenioAsync(string codigoMunicipio)
        {
            if (string.IsNullOrWhiteSpace(codigoMunicipio))
                throw new ArgumentNullException(nameof(codigoMunicipio));

            if (codigoMunicipio.Length != 7)
                throw new ArgumentException("Código do município deve ter 7 dígitos", nameof(codigoMunicipio));

            var endpoint = $"/parametros_municipais/{codigoMunicipio}/convenio";
            return await _httpClient.GetAsync<ParametrosConvenio>(endpoint);
        }

        /// <summary>
        /// Consulta alíquotas e regimes especiais de um serviço específico no município
        /// </summary>
        /// <param name="codigoMunicipio">Código do município (IBGE - 7 dígitos)</param>
        /// <param name="codigoServico">Código de serviço da LC 116/2003</param>
        /// <returns>Parâmetros do serviço no município</returns>
        /// <exception cref="NFSeException">Lançada em caso de erro na requisição</exception>
        public async Task<ParametrosServico> ConsultarAliquotasServicoAsync(string codigoMunicipio, string codigoServico)
        {
            if (string.IsNullOrWhiteSpace(codigoMunicipio))
                throw new ArgumentNullException(nameof(codigoMunicipio));

            if (string.IsNullOrWhiteSpace(codigoServico))
                throw new ArgumentNullException(nameof(codigoServico));

            if (codigoMunicipio.Length != 7)
                throw new ArgumentException("Código do município deve ter 7 dígitos", nameof(codigoMunicipio));

            var endpoint = $"/parametros_municipais/{codigoMunicipio}/{codigoServico}";
            return await _httpClient.GetAsync<ParametrosServico>(endpoint);
        }

        /// <summary>
        /// Consulta parâmetros de retenções de um contribuinte específico no município
        /// </summary>
        /// <param name="codigoMunicipio">Código do município (IBGE - 7 dígitos)</param>
        /// <param name="inscricao">CPF ou CNPJ do contribuinte</param>
        /// <returns>Parâmetros de retenções do contribuinte</returns>
        /// <exception cref="NFSeException">Lançada em caso de erro na requisição</exception>
        public async Task<ParametrosRetencoes> ConsultarRetencoesContribuinteAsync(string codigoMunicipio, string inscricao)
        {
            if (string.IsNullOrWhiteSpace(codigoMunicipio))
                throw new ArgumentNullException(nameof(codigoMunicipio));

            if (string.IsNullOrWhiteSpace(inscricao))
                throw new ArgumentNullException(nameof(inscricao));

            if (codigoMunicipio.Length != 7)
                throw new ArgumentException("Código do município deve ter 7 dígitos", nameof(codigoMunicipio));

            // Remove caracteres especiais da inscrição
            inscricao = new string(inscricao.Where(char.IsDigit).ToArray());

            if (inscricao.Length != 11 && inscricao.Length != 14)
                throw new ArgumentException("Inscrição deve ser um CPF (11 dígitos) ou CNPJ (14 dígitos)", nameof(inscricao));

            var endpoint = $"/parametros_municipais/{codigoMunicipio}/{inscricao}/retencoes";
            return await _httpClient.GetAsync<ParametrosRetencoes>(endpoint);
        }

        /// <summary>
        /// Consulta benefícios municipais de um contribuinte específico
        /// </summary>
        /// <param name="codigoMunicipio">Código do município (IBGE - 7 dígitos)</param>
        /// <param name="inscricao">CPF ou CNPJ do contribuinte</param>
        /// <returns>Parâmetros de benefícios do contribuinte</returns>
        /// <exception cref="NFSeException">Lançada em caso de erro na requisição</exception>
        public async Task<ParametrosBeneficios> ConsultarBeneficiosMunicipaisAsync(string codigoMunicipio, string inscricao)
        {
            if (string.IsNullOrWhiteSpace(codigoMunicipio))
                throw new ArgumentNullException(nameof(codigoMunicipio));

            if (string.IsNullOrWhiteSpace(inscricao))
                throw new ArgumentNullException(nameof(inscricao));

            if (codigoMunicipio.Length != 7)
                throw new ArgumentException("Código do município deve ter 7 dígitos", nameof(codigoMunicipio));

            // Remove caracteres especiais da inscrição
            inscricao = new string(inscricao.Where(char.IsDigit).ToArray());

            if (inscricao.Length != 11 && inscricao.Length != 14)
                throw new ArgumentException("Inscrição deve ser um CPF (11 dígitos) ou CNPJ (14 dígitos)", nameof(inscricao));

            var endpoint = $"/parametros_municipais/{codigoMunicipio}/{inscricao}/beneficios";
            return await _httpClient.GetAsync<ParametrosBeneficios>(endpoint);
        }

        /// <summary>
        /// Libera recursos utilizados pelo serviço
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Libera recursos utilizados pelo serviço
        /// </summary>
        /// <param name="disposing">Indica se está liberando recursos gerenciados</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _httpClient?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
