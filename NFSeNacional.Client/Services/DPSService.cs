using NFSeNacional.Client.Configuration;

namespace NFSeNacional.Client.Services
{
    /// <summary>
    /// Serviço para recuperação de informações de DPS (Declaração de Prestação de Serviços)
    /// </summary>
    public class DPSService : IDisposable
    {
        private readonly NFSeHttpClient _httpClient;
        private bool _disposed;

        /// <summary>
        /// Inicializa uma nova instância do serviço
        /// </summary>
        /// <param name="config">Configuração de conexão</param>
        public DPSService(NFSeConfig config)
        {
            _httpClient = new NFSeHttpClient(config);
        }

        /// <summary>
        /// Recupera a chave de acesso da NFS-e gerada a partir de uma DPS
        /// </summary>
        /// <param name="idDps">ID da DPS (42 caracteres)</param>
        /// <returns>Chave de acesso da NFS-e (50 caracteres)</returns>
        /// <exception cref="NFSeException">Lançada em caso de erro na requisição</exception>
        /// <remarks>
        /// Somente disponível para atores (Prestador, Tomador ou Intermediário) da NFS-e.
        /// Formato do ID: Cód. Município (7) + Tipo Inscrição (1) + Inscrição Federal (14) + Série DPS (5) + Núm. DPS (15)
        /// </remarks>
        public async Task<string> RecuperarChaveAcessoAsync(string idDps)
        {
            if (string.IsNullOrWhiteSpace(idDps))
                throw new ArgumentNullException(nameof(idDps));

            if (idDps.Length != 42)
                throw new ArgumentException("ID da DPS deve ter 42 caracteres", nameof(idDps));

            var endpoint = $"/dps/{idDps}";
            var response = await _httpClient.GetStringAsync(endpoint);

            // A resposta deve conter a chave de acesso (50 caracteres)
            // Pode ser necessário fazer parsing do XML/JSON dependendo do formato da resposta
            return response;
        }

        /// <summary>
        /// Verifica se uma NFS-e foi gerada a partir de uma DPS
        /// </summary>
        /// <param name="idDps">ID da DPS (42 caracteres)</param>
        /// <returns>True se a NFS-e foi gerada, False caso contrário</returns>
        /// <exception cref="NFSeException">Lançada em caso de erro na requisição</exception>
        /// <remarks>
        /// Disponível para qualquer usuário com certificado digital válido.
        /// Formato do ID: Cód. Município (7) + Tipo Inscrição (1) + Inscrição Federal (14) + Série DPS (5) + Núm. DPS (15)
        /// </remarks>
        public async Task<bool> VerificarNFSeGeradaAsync(string idDps)
        {
            if (string.IsNullOrWhiteSpace(idDps))
                throw new ArgumentNullException(nameof(idDps));

            if (idDps.Length != 42)
                throw new ArgumentException("ID da DPS deve ter 42 caracteres", nameof(idDps));

            var endpoint = $"/dps/{idDps}";
            return await _httpClient.HeadAsync(endpoint);
        }

        /// <summary>
        /// Valida o formato do ID da DPS
        /// </summary>
        /// <param name="idDps">ID da DPS a ser validado</param>
        /// <returns>True se o ID está no formato válido</returns>
        public static bool ValidarFormatoIdDps(string idDps)
        {
            if (string.IsNullOrWhiteSpace(idDps))
                return false;

            if (idDps.Length != 42)
                return false;

            // Validar se contém apenas dígitos
            if (!idDps.All(char.IsDigit))
                return false;

            // Validar componentes do ID
            var codigoMunicipio = idDps.Substring(0, 7);
            var tipoInscricao = idDps.Substring(7, 1);
            var inscricaoFederal = idDps.Substring(8, 14);
            var serieDps = idDps.Substring(22, 5);
            var numeroDps = idDps.Substring(27, 15);

            // Tipo de inscrição deve ser 1 (CPF) ou 2 (CNPJ)
            if (tipoInscricao != "1" && tipoInscricao != "2")
                return false;

            return true;
        }

        /// <summary>
        /// Extrai componentes do ID da DPS
        /// </summary>
        /// <param name="idDps">ID da DPS</param>
        /// <returns>Tupla contendo os componentes do ID</returns>
        /// <exception cref="ArgumentException">Lançada se o ID for inválido</exception>
        public static (string CodigoMunicipio, string TipoInscricao, string InscricaoFederal, string SerieDps, string NumeroDps) ExtrairComponentesId(string idDps)
        {
            if (!ValidarFormatoIdDps(idDps))
                throw new ArgumentException("ID da DPS em formato inválido", nameof(idDps));

            return (
                CodigoMunicipio: idDps.Substring(0, 7),
                TipoInscricao: idDps.Substring(7, 1),
                InscricaoFederal: idDps.Substring(8, 14),
                SerieDps: idDps.Substring(22, 5),
                NumeroDps: idDps.Substring(27, 15)
            );
        }

        /// <summary>
        /// Constrói um ID de DPS a partir de seus componentes
        /// </summary>
        /// <param name="codigoMunicipio">Código do município (7 dígitos)</param>
        /// <param name="tipoInscricao">Tipo de inscrição (1=CPF, 2=CNPJ)</param>
        /// <param name="inscricaoFederal">Inscrição federal (14 dígitos, completar com zeros à esquerda para CPF)</param>
        /// <param name="serieDps">Série da DPS (5 dígitos)</param>
        /// <param name="numeroDps">Número da DPS (15 dígitos)</param>
        /// <returns>ID da DPS completo (42 caracteres)</returns>
        /// <exception cref="ArgumentException">Lançada se algum componente for inválido</exception>
        public static string ConstruirIdDps(string codigoMunicipio, string tipoInscricao, string inscricaoFederal, string serieDps, string numeroDps)
        {
            if (string.IsNullOrWhiteSpace(codigoMunicipio) || codigoMunicipio.Length != 7)
                throw new ArgumentException("Código do município deve ter 7 dígitos", nameof(codigoMunicipio));

            if (tipoInscricao != "1" && tipoInscricao != "2")
                throw new ArgumentException("Tipo de inscrição deve ser 1 (CPF) ou 2 (CNPJ)", nameof(tipoInscricao));

            if (string.IsNullOrWhiteSpace(inscricaoFederal) || inscricaoFederal.Length != 14)
                throw new ArgumentException("Inscrição federal deve ter 14 dígitos", nameof(inscricaoFederal));

            if (string.IsNullOrWhiteSpace(serieDps) || serieDps.Length != 5)
                throw new ArgumentException("Série DPS deve ter 5 dígitos", nameof(serieDps));

            if (string.IsNullOrWhiteSpace(numeroDps) || numeroDps.Length != 15)
                throw new ArgumentException("Número DPS deve ter 15 dígitos", nameof(numeroDps));

            return $"{codigoMunicipio}{tipoInscricao}{inscricaoFederal}{serieDps}{numeroDps}";
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
