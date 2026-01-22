namespace NFSeNacional.Client
{
    /// <summary>
    /// Exception customizada para erros relacionados às APIs do Sistema Nacional NFS-e
    /// </summary>
    public class NFSeException : Exception
    {
        /// <summary>
        /// Código de status HTTP da resposta (se aplicável)
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// Conteúdo da resposta de erro
        /// </summary>
        public string? ResponseContent { get; set; }

        /// <summary>
        /// Cria uma nova instância de NFSeException
        /// </summary>
        public NFSeException() : base()
        {
        }

        /// <summary>
        /// Cria uma nova instância de NFSeException com mensagem
        /// </summary>
        /// <param name="message">Mensagem de erro</param>
        public NFSeException(string message) : base(message)
        {
        }

        /// <summary>
        /// Cria uma nova instância de NFSeException com mensagem e exception interna
        /// </summary>
        /// <param name="message">Mensagem de erro</param>
        /// <param name="innerException">Exception interna</param>
        public NFSeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Cria uma nova instância de NFSeException com informações da resposta HTTP
        /// </summary>
        /// <param name="message">Mensagem de erro</param>
        /// <param name="statusCode">Código de status HTTP</param>
        /// <param name="responseContent">Conteúdo da resposta</param>
        public NFSeException(string message, int statusCode, string responseContent) : base(message)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
        }
    }
}
