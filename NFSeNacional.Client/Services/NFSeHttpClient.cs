using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NFSeNacional.Client.Configuration;

namespace NFSeNacional.Client.Services
{
    /// <summary>
    /// Cliente HTTP base para comunicação com as APIs do Sistema Nacional NFS-e
    /// </summary>
    public class NFSeHttpClient : IDisposable
    {
        private readonly NFSeConfig _config;
        private readonly HttpClient _httpClient;
        private bool _disposed;

        /// <summary>
        /// Inicializa uma nova instância do cliente HTTP
        /// </summary>
        /// <param name="config">Configuração de conexão</param>
        /// <exception cref="NFSeException">Lançada quando a configuração é inválida</exception>
        public NFSeHttpClient(NFSeConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));

            if (!_config.IsValid())
            {
                var errors = string.Join(", ", _config.GetValidationErrors());
                throw new NFSeException($"Configuração inválida: {errors}");
            }

            var handler = new HttpClientHandler();

            // Configurar certificado digital
            if (_config.CertificadoDigital != null)
            {
                handler.ClientCertificates.Add(_config.CertificadoDigital);
            }

            // Configurar validação de certificado do servidor
            if (!_config.ValidarCertificadoServidor)
            {
                handler.ServerCertificateCustomValidationCallback = 
                    (message, cert, chain, errors) => true;
            }

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(_config.BaseUrl),
                Timeout = TimeSpan.FromSeconds(_config.TimeoutSegundos)
            };

            // Configurar headers padrão
            _httpClient.DefaultRequestHeaders.Add("User-Agent", $"NFSeNacional.Client/{_config.VersaoAplicativo}");
        }

        /// <summary>
        /// Realiza uma requisição GET e retorna o resultado deserializado
        /// </summary>
        /// <typeparam name="T">Tipo do objeto de resposta</typeparam>
        /// <param name="endpoint">Endpoint da API (relativo à BaseUrl)</param>
        /// <returns>Objeto deserializado da resposta</returns>
        /// <exception cref="NFSeException">Lançada em caso de erro na requisição</exception>
        public async Task<T> GetAsync<T>(string endpoint) where T : class
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new NFSeException(
                        $"Erro ao realizar requisição GET para {endpoint}: {response.StatusCode}",
                        (int)response.StatusCode,
                        content
                    );
                }

                // Se o content type for XML, deserializar
                if (response.Content.Headers.ContentType?.MediaType?.Contains("xml") == true)
                {
                    return DeserializeFromXml<T>(content);
                }

                // Se o content type for JSON, lançar exceção (não implementado ainda)
                throw new NFSeException("Formato de resposta não suportado. Esperado XML.");
            }
            catch (NFSeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new NFSeException($"Erro ao realizar requisição GET para {endpoint}", ex);
            }
        }

        /// <summary>
        /// Realiza uma requisição GET e retorna o conteúdo como string
        /// </summary>
        /// <param name="endpoint">Endpoint da API (relativo à BaseUrl)</param>
        /// <returns>Conteúdo da resposta como string</returns>
        /// <exception cref="NFSeException">Lançada em caso de erro na requisição</exception>
        public async Task<string> GetStringAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new NFSeException(
                        $"Erro ao realizar requisição GET para {endpoint}: {response.StatusCode}",
                        (int)response.StatusCode,
                        content
                    );
                }

                return content;
            }
            catch (NFSeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new NFSeException($"Erro ao realizar requisição GET para {endpoint}", ex);
            }
        }

        /// <summary>
        /// Realiza uma requisição POST com XML e retorna a resposta deserializada
        /// </summary>
        /// <typeparam name="TRequest">Tipo do objeto de requisição</typeparam>
        /// <typeparam name="TResponse">Tipo do objeto de resposta</typeparam>
        /// <param name="endpoint">Endpoint da API (relativo à BaseUrl)</param>
        /// <param name="request">Objeto a ser serializado e enviado</param>
        /// <returns>Objeto deserializado da resposta</returns>
        /// <exception cref="NFSeException">Lançada em caso de erro na requisição</exception>
        public async Task<TResponse> PostXmlAsync<TRequest, TResponse>(string endpoint, TRequest request)
            where TRequest : class
            where TResponse : class
        {
            try
            {
                var xmlContent = SerializeToXml(request);
                var content = new StringContent(xmlContent, Encoding.UTF8, "application/xml");

                var response = await _httpClient.PostAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new NFSeException(
                        $"Erro ao realizar requisição POST para {endpoint}: {response.StatusCode}",
                        (int)response.StatusCode,
                        responseContent
                    );
                }

                return DeserializeFromXml<TResponse>(responseContent);
            }
            catch (NFSeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new NFSeException($"Erro ao realizar requisição POST para {endpoint}", ex);
            }
        }

        /// <summary>
        /// Realiza uma requisição POST com string XML e retorna a resposta como string
        /// </summary>
        /// <param name="endpoint">Endpoint da API (relativo à BaseUrl)</param>
        /// <param name="xmlContent">Conteúdo XML a ser enviado</param>
        /// <returns>Conteúdo da resposta como string</returns>
        /// <exception cref="NFSeException">Lançada em caso de erro na requisição</exception>
        public async Task<string> PostXmlStringAsync(string endpoint, string xmlContent)
        {
            try
            {
                var content = new StringContent(xmlContent, Encoding.UTF8, "application/xml");

                var response = await _httpClient.PostAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new NFSeException(
                        $"Erro ao realizar requisição POST para {endpoint}: {response.StatusCode}",
                        (int)response.StatusCode,
                        responseContent
                    );
                }

                return responseContent;
            }
            catch (NFSeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new NFSeException($"Erro ao realizar requisição POST para {endpoint}", ex);
            }
        }

        /// <summary>
        /// Realiza uma requisição HEAD para verificar a existência de um recurso
        /// </summary>
        /// <param name="endpoint">Endpoint da API (relativo à BaseUrl)</param>
        /// <returns>True se o recurso existe (status 200), False caso contrário</returns>
        /// <exception cref="NFSeException">Lançada em caso de erro na requisição</exception>
        public async Task<bool> HeadAsync(string endpoint)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Head, endpoint);
                var response = await _httpClient.SendAsync(request);

                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw new NFSeException($"Erro ao realizar requisição HEAD para {endpoint}", ex);
            }
        }

        /// <summary>
        /// Serializa um objeto para XML
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="obj">Objeto a ser serializado</param>
        /// <returns>String XML</returns>
        public string SerializeToXml<T>(T obj) where T : class
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var serializer = new XmlSerializer(typeof(T));
            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = false
            };

            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, settings);
            
            serializer.Serialize(xmlWriter, obj);
            return stringWriter.ToString();
        }

        /// <summary>
        /// Deserializa XML para um objeto
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="xml">String XML</param>
        /// <returns>Objeto deserializado</returns>
        public T DeserializeFromXml<T>(string xml) where T : class
        {
            if (string.IsNullOrWhiteSpace(xml))
                throw new ArgumentNullException(nameof(xml));

            var serializer = new XmlSerializer(typeof(T));
            
            using var stringReader = new StringReader(xml);
            using var xmlReader = XmlReader.Create(stringReader);
            
            var result = serializer.Deserialize(xmlReader) as T;
            
            if (result == null)
                throw new NFSeException($"Erro ao deserializar XML para tipo {typeof(T).Name}");

            return result;
        }

        /// <summary>
        /// Libera recursos utilizados pelo cliente HTTP
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Libera recursos utilizados pelo cliente HTTP
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
