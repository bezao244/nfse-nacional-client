using NFSeNacional.Client.Configuration;
using NFSeNacional.Client.Models;

namespace NFSeNacional.Client.Services
{
    /// <summary>
    /// Serviço para emissão e consulta de NFS-e
    /// </summary>
    public class NFSeService : IDisposable
    {
        private readonly NFSeHttpClient _httpClient;
        private bool _disposed;

        /// <summary>
        /// Inicializa uma nova instância do serviço
        /// </summary>
        /// <param name="config">Configuração de conexão</param>
        public NFSeService(NFSeConfig config)
        {
            _httpClient = new NFSeHttpClient(config);
        }

        /// <summary>
        /// Emite uma NFS-e de forma síncrona a partir de uma DPS
        /// </summary>
        /// <param name="dps">Declaração de Prestação de Serviços</param>
        /// <returns>Resposta da emissão contendo a NFS-e ou mensagens de erro</returns>
        /// <exception cref="NFSeException">Lançada em caso de erro na requisição</exception>
        public async Task<RespostaNFSe> EmitirNFSeAsync(DPS dps)
        {
            if (dps == null)
                throw new ArgumentNullException(nameof(dps));

            ValidarDPS(dps);

            var endpoint = "/nfse";
            return await _httpClient.PostXmlAsync<DPS, RespostaNFSe>(endpoint, dps);
        }

        /// <summary>
        /// Emite uma NFS-e de forma síncrona a partir de XML da DPS
        /// </summary>
        /// <param name="xmlDps">XML da Declaração de Prestação de Serviços</param>
        /// <returns>XML da resposta contendo a NFS-e ou mensagens de erro</returns>
        /// <exception cref="NFSeException">Lançada em caso de erro na requisição</exception>
        public async Task<string> EmitirNFSeXmlAsync(string xmlDps)
        {
            if (string.IsNullOrWhiteSpace(xmlDps))
                throw new ArgumentNullException(nameof(xmlDps));

            var endpoint = "/nfse";
            return await _httpClient.PostXmlStringAsync(endpoint, xmlDps);
        }

        /// <summary>
        /// Consulta uma NFS-e pela chave de acesso
        /// </summary>
        /// <param name="chaveAcesso">Chave de acesso da NFS-e (50 caracteres)</param>
        /// <returns>NFS-e encontrada</returns>
        /// <exception cref="NFSeException">Lançada em caso de erro na requisição</exception>
        public async Task<NFSe> ConsultarNFSeAsync(string chaveAcesso)
        {
            if (string.IsNullOrWhiteSpace(chaveAcesso))
                throw new ArgumentNullException(nameof(chaveAcesso));

            if (chaveAcesso.Length != 50)
                throw new ArgumentException("Chave de acesso deve ter 50 caracteres", nameof(chaveAcesso));

            var endpoint = $"/nfse/{chaveAcesso}";
            return await _httpClient.GetAsync<NFSe>(endpoint);
        }

        /// <summary>
        /// Consulta uma NFS-e pela chave de acesso e retorna como XML
        /// </summary>
        /// <param name="chaveAcesso">Chave de acesso da NFS-e (50 caracteres)</param>
        /// <returns>XML da NFS-e encontrada</returns>
        /// <exception cref="NFSeException">Lançada em caso de erro na requisição</exception>
        public async Task<string> ConsultarNFSeXmlAsync(string chaveAcesso)
        {
            if (string.IsNullOrWhiteSpace(chaveAcesso))
                throw new ArgumentNullException(nameof(chaveAcesso));

            if (chaveAcesso.Length != 50)
                throw new ArgumentException("Chave de acesso deve ter 50 caracteres", nameof(chaveAcesso));

            var endpoint = $"/nfse/{chaveAcesso}";
            return await _httpClient.GetStringAsync(endpoint);
        }

        /// <summary>
        /// Valida os dados básicos da DPS
        /// </summary>
        /// <param name="dps">DPS a ser validada</param>
        /// <exception cref="NFSeException">Lançada se a DPS for inválida</exception>
        private void ValidarDPS(DPS dps)
        {
            var errors = new List<string>();

            if (dps.InfDps == null)
                errors.Add("InfDps é obrigatório");
            else
            {
                if (string.IsNullOrWhiteSpace(dps.InfDps.Id))
                    errors.Add("Id da DPS é obrigatório");
                else if (dps.InfDps.Id.Length != 42)
                    errors.Add("Id da DPS deve ter 42 caracteres");

                if (dps.InfDps.Prestador == null)
                    errors.Add("Dados do Prestador são obrigatórios");
                else
                {
                    if (string.IsNullOrWhiteSpace(dps.InfDps.Prestador.CNPJ))
                        errors.Add("CNPJ do Prestador é obrigatório");

                    if (string.IsNullOrWhiteSpace(dps.InfDps.Prestador.CodigoMunicipio))
                        errors.Add("Código do município do Prestador é obrigatório");
                    else if (dps.InfDps.Prestador.CodigoMunicipio.Length != 7)
                        errors.Add("Código do município deve ter 7 dígitos");
                }

                if (dps.InfDps.Tomador == null)
                    errors.Add("Dados do Tomador são obrigatórios");
                else
                {
                    if (string.IsNullOrWhiteSpace(dps.InfDps.Tomador.NumeroInscricao))
                        errors.Add("Inscrição do Tomador é obrigatória");

                    if (string.IsNullOrWhiteSpace(dps.InfDps.Tomador.Nome))
                        errors.Add("Nome do Tomador é obrigatório");
                }

                if (dps.InfDps.Servico == null)
                    errors.Add("Dados do Serviço são obrigatórios");
                else
                {
                    if (string.IsNullOrWhiteSpace(dps.InfDps.Servico.CodigoServico))
                        errors.Add("Código do Serviço é obrigatório");

                    if (string.IsNullOrWhiteSpace(dps.InfDps.Servico.Discriminacao))
                        errors.Add("Discriminação do Serviço é obrigatória");
                }

                if (dps.InfDps.Valores == null)
                    errors.Add("Valores da DPS são obrigatórios");
                else
                {
                    if (dps.InfDps.Valores.ValorServicos <= 0)
                        errors.Add("Valor dos serviços deve ser maior que zero");

                    if (dps.InfDps.Valores.ValorBaseCalculo < 0)
                        errors.Add("Valor da base de cálculo não pode ser negativo");

                    if (dps.InfDps.Valores.Aliquota < 0 || dps.InfDps.Valores.Aliquota > 100)
                        errors.Add("Alíquota deve estar entre 0 e 100");
                }

                if (string.IsNullOrWhiteSpace(dps.InfDps.NumeroDPS))
                    errors.Add("Número da DPS é obrigatório");

                if (dps.InfDps.DataHoraEmissao == default)
                    errors.Add("Data/hora de emissão é obrigatória");

                if (dps.InfDps.Competencia == default)
                    errors.Add("Data de competência é obrigatória");
            }

            if (errors.Any())
                throw new NFSeException($"Validação da DPS falhou: {string.Join("; ", errors)}");
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
