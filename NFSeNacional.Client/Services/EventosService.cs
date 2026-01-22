using NFSeNacional.Client.Configuration;
using NFSeNacional.Client.Models;

namespace NFSeNacional.Client.Services
{
    /// <summary>
    /// Serviço para registro e consulta de eventos de NFS-e
    /// </summary>
    public class EventosService : IDisposable
    {
        private readonly NFSeHttpClient _httpClient;
        private bool _disposed;

        /// <summary>
        /// Inicializa uma nova instância do serviço
        /// </summary>
        /// <param name="config">Configuração de conexão</param>
        public EventosService(NFSeConfig config)
        {
            _httpClient = new NFSeHttpClient(config);
        }

        /// <summary>
        /// Registra um evento para uma NFS-e (cancelamento, confirmação, rejeição, etc.)
        /// </summary>
        /// <param name="chaveAcesso">Chave de acesso da NFS-e (50 caracteres)</param>
        /// <param name="pedido">Pedido de registro do evento</param>
        /// <returns>Resposta do registro contendo o evento ou mensagens de erro</returns>
        /// <exception cref="NFSeException">Lançada em caso de erro na requisição</exception>
        public async Task<RespostaEvento> RegistrarEventoAsync(string chaveAcesso, PedidoRegistroEvento pedido)
        {
            if (string.IsNullOrWhiteSpace(chaveAcesso))
                throw new ArgumentNullException(nameof(chaveAcesso));

            if (chaveAcesso.Length != 50)
                throw new ArgumentException("Chave de acesso deve ter 50 caracteres", nameof(chaveAcesso));

            if (pedido == null)
                throw new ArgumentNullException(nameof(pedido));

            ValidarPedidoRegistroEvento(pedido);

            var endpoint = $"/nfse/{chaveAcesso}/eventos";
            return await _httpClient.PostXmlAsync<PedidoRegistroEvento, RespostaEvento>(endpoint, pedido);
        }

        /// <summary>
        /// Registra um evento para uma NFS-e usando XML
        /// </summary>
        /// <param name="chaveAcesso">Chave de acesso da NFS-e (50 caracteres)</param>
        /// <param name="xmlPedido">XML do pedido de registro</param>
        /// <returns>XML da resposta</returns>
        /// <exception cref="NFSeException">Lançada em caso de erro na requisição</exception>
        public async Task<string> RegistrarEventoXmlAsync(string chaveAcesso, string xmlPedido)
        {
            if (string.IsNullOrWhiteSpace(chaveAcesso))
                throw new ArgumentNullException(nameof(chaveAcesso));

            if (chaveAcesso.Length != 50)
                throw new ArgumentException("Chave de acesso deve ter 50 caracteres", nameof(chaveAcesso));

            if (string.IsNullOrWhiteSpace(xmlPedido))
                throw new ArgumentNullException(nameof(xmlPedido));

            var endpoint = $"/nfse/{chaveAcesso}/eventos";
            return await _httpClient.PostXmlStringAsync(endpoint, xmlPedido);
        }

        /// <summary>
        /// Consulta todos os eventos de uma NFS-e
        /// </summary>
        /// <param name="chaveAcesso">Chave de acesso da NFS-e (50 caracteres)</param>
        /// <returns>Lista de eventos da NFS-e</returns>
        /// <exception cref="NFSeException">Lançada em caso de erro na requisição</exception>
        public async Task<ListaEventos> ConsultarEventosAsync(string chaveAcesso)
        {
            if (string.IsNullOrWhiteSpace(chaveAcesso))
                throw new ArgumentNullException(nameof(chaveAcesso));

            if (chaveAcesso.Length != 50)
                throw new ArgumentException("Chave de acesso deve ter 50 caracteres", nameof(chaveAcesso));

            var endpoint = $"/nfse/{chaveAcesso}/eventos";
            return await _httpClient.GetAsync<ListaEventos>(endpoint);
        }

        /// <summary>
        /// Consulta eventos de uma NFS-e por tipo
        /// </summary>
        /// <param name="chaveAcesso">Chave de acesso da NFS-e (50 caracteres)</param>
        /// <param name="tipoEvento">Tipo do evento (ex: e101101, e202101, etc.)</param>
        /// <returns>Lista de eventos do tipo especificado</returns>
        /// <exception cref="NFSeException">Lançada em caso de erro na requisição</exception>
        public async Task<ListaEventos> ConsultarEventosPorTipoAsync(string chaveAcesso, string tipoEvento)
        {
            if (string.IsNullOrWhiteSpace(chaveAcesso))
                throw new ArgumentNullException(nameof(chaveAcesso));

            if (chaveAcesso.Length != 50)
                throw new ArgumentException("Chave de acesso deve ter 50 caracteres", nameof(chaveAcesso));

            if (string.IsNullOrWhiteSpace(tipoEvento))
                throw new ArgumentNullException(nameof(tipoEvento));

            var endpoint = $"/nfse/{chaveAcesso}/eventos/{tipoEvento}";
            return await _httpClient.GetAsync<ListaEventos>(endpoint);
        }

        /// <summary>
        /// Consulta um evento específico de uma NFS-e
        /// </summary>
        /// <param name="chaveAcesso">Chave de acesso da NFS-e (50 caracteres)</param>
        /// <param name="tipoEvento">Tipo do evento (ex: e101101, e202101, etc.)</param>
        /// <param name="numSeqEvento">Número sequencial do evento</param>
        /// <returns>Evento específico</returns>
        /// <exception cref="NFSeException">Lançada em caso de erro na requisição</exception>
        public async Task<Evento> ConsultarEventoEspecificoAsync(string chaveAcesso, string tipoEvento, int numSeqEvento)
        {
            if (string.IsNullOrWhiteSpace(chaveAcesso))
                throw new ArgumentNullException(nameof(chaveAcesso));

            if (chaveAcesso.Length != 50)
                throw new ArgumentException("Chave de acesso deve ter 50 caracteres", nameof(chaveAcesso));

            if (string.IsNullOrWhiteSpace(tipoEvento))
                throw new ArgumentNullException(nameof(tipoEvento));

            if (numSeqEvento <= 0)
                throw new ArgumentException("Número sequencial do evento deve ser maior que zero", nameof(numSeqEvento));

            var endpoint = $"/nfse/{chaveAcesso}/eventos/{tipoEvento}/{numSeqEvento}";
            return await _httpClient.GetAsync<Evento>(endpoint);
        }

        /// <summary>
        /// Cria um pedido de cancelamento de NFS-e
        /// </summary>
        /// <param name="chaveAcesso">Chave de acesso da NFS-e a ser cancelada</param>
        /// <param name="codigoCancelamento">Código de cancelamento</param>
        /// <param name="justificativa">Justificativa do cancelamento</param>
        /// <returns>Pedido de registro de evento de cancelamento</returns>
        public PedidoRegistroEvento CriarPedidoCancelamento(string chaveAcesso, string codigoCancelamento, string justificativa)
        {
            if (string.IsNullOrWhiteSpace(chaveAcesso))
                throw new ArgumentNullException(nameof(chaveAcesso));

            if (string.IsNullOrWhiteSpace(justificativa))
                throw new ArgumentNullException(nameof(justificativa));

            return new PedidoRegistroEvento
            {
                InfPedidoRegistro = new InfPedidoRegistro
                {
                    ChaveAcessoNFSe = chaveAcesso,
                    TipoEvento = "e101101", // Cancelamento - Pedido de Cancelamento
                    NumeroSequencialEvento = 1,
                    DataHoraEvento = DateTime.Now,
                    DetalhesEvento = new DetalhesEvento
                    {
                        DescricaoEvento = "Cancelamento de NFS-e",
                        CodigoCancelamento = codigoCancelamento,
                        Justificativa = justificativa
                    }
                }
            };
        }

        /// <summary>
        /// Cria um pedido de confirmação da operação
        /// </summary>
        /// <param name="chaveAcesso">Chave de acesso da NFS-e</param>
        /// <param name="tipoAtor">Tipo do ator (prestador, tomador ou intermediário)</param>
        /// <returns>Pedido de registro de evento de confirmação</returns>
        public PedidoRegistroEvento CriarPedidoConfirmacao(string chaveAcesso, string tipoAtor)
        {
            if (string.IsNullOrWhiteSpace(chaveAcesso))
                throw new ArgumentNullException(nameof(chaveAcesso));

            string tipoEvento = tipoAtor?.ToLower() switch
            {
                "prestador" => "e202101",
                "tomador" => "e202102",
                "intermediario" => "e202103",
                _ => throw new ArgumentException("Tipo de ator deve ser 'prestador', 'tomador' ou 'intermediario'", nameof(tipoAtor))
            };

            return new PedidoRegistroEvento
            {
                InfPedidoRegistro = new InfPedidoRegistro
                {
                    ChaveAcessoNFSe = chaveAcesso,
                    TipoEvento = tipoEvento,
                    NumeroSequencialEvento = 1,
                    DataHoraEvento = DateTime.Now,
                    DetalhesEvento = new DetalhesEvento
                    {
                        DescricaoEvento = $"Confirmação da Operação pelo {tipoAtor}"
                    }
                }
            };
        }

        /// <summary>
        /// Cria um pedido de rejeição da operação
        /// </summary>
        /// <param name="chaveAcesso">Chave de acesso da NFS-e</param>
        /// <param name="tipoAtor">Tipo do ator (prestador, tomador ou intermediário)</param>
        /// <param name="justificativa">Justificativa da rejeição</param>
        /// <returns>Pedido de registro de evento de rejeição</returns>
        public PedidoRegistroEvento CriarPedidoRejeicao(string chaveAcesso, string tipoAtor, string justificativa)
        {
            if (string.IsNullOrWhiteSpace(chaveAcesso))
                throw new ArgumentNullException(nameof(chaveAcesso));

            if (string.IsNullOrWhiteSpace(justificativa))
                throw new ArgumentNullException(nameof(justificativa));

            string tipoEvento = tipoAtor?.ToLower() switch
            {
                "prestador" => "e203101",
                "tomador" => "e203102",
                "intermediario" => "e203103",
                _ => throw new ArgumentException("Tipo de ator deve ser 'prestador', 'tomador' ou 'intermediario'", nameof(tipoAtor))
            };

            return new PedidoRegistroEvento
            {
                InfPedidoRegistro = new InfPedidoRegistro
                {
                    ChaveAcessoNFSe = chaveAcesso,
                    TipoEvento = tipoEvento,
                    NumeroSequencialEvento = 1,
                    DataHoraEvento = DateTime.Now,
                    DetalhesEvento = new DetalhesEvento
                    {
                        DescricaoEvento = $"Rejeição da Operação pelo {tipoAtor}",
                        Justificativa = justificativa
                    }
                }
            };
        }

        /// <summary>
        /// Valida o pedido de registro de evento
        /// </summary>
        /// <param name="pedido">Pedido a ser validado</param>
        /// <exception cref="NFSeException">Lançada se o pedido for inválido</exception>
        private void ValidarPedidoRegistroEvento(PedidoRegistroEvento pedido)
        {
            var errors = new List<string>();

            if (pedido.InfPedidoRegistro == null)
                errors.Add("InfPedidoRegistro é obrigatório");
            else
            {
                if (string.IsNullOrWhiteSpace(pedido.InfPedidoRegistro.ChaveAcessoNFSe))
                    errors.Add("Chave de acesso da NFS-e é obrigatória");
                else if (pedido.InfPedidoRegistro.ChaveAcessoNFSe.Length != 50)
                    errors.Add("Chave de acesso deve ter 50 caracteres");

                if (string.IsNullOrWhiteSpace(pedido.InfPedidoRegistro.TipoEvento))
                    errors.Add("Tipo do evento é obrigatório");

                if (pedido.InfPedidoRegistro.NumeroSequencialEvento <= 0)
                    errors.Add("Número sequencial do evento deve ser maior que zero");

                if (pedido.InfPedidoRegistro.DataHoraEvento == default)
                    errors.Add("Data/hora do evento é obrigatória");

                if (pedido.InfPedidoRegistro.DetalhesEvento == null)
                    errors.Add("Detalhes do evento são obrigatórios");
            }

            if (errors.Any())
                throw new NFSeException($"Validação do pedido de registro de evento falhou: {string.Join("; ", errors)}");
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
