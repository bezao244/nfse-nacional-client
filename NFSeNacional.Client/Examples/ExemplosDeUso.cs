using System.Security.Cryptography.X509Certificates;
using NFSeNacional.Client.Configuration;
using NFSeNacional.Client.Models;
using NFSeNacional.Client.Services;

namespace NFSeNacional.Client.Examples
{
    /// <summary>
    /// Exemplos práticos de uso do cliente NFSe Nacional
    /// </summary>
    public class ExemplosDeUso
    {
        /// <summary>
        /// Exemplo 1: Configuração inicial do cliente
        /// </summary>
        public static NFSeConfig ExemploConfiguracaoInicial()
        {
            // Carregar certificado digital do arquivo
            var certificado = new X509Certificate2(
                "/caminho/para/certificado.pfx",
                "senha_do_certificado"
            );

            // Criar configuração para ambiente de homologação
            var config = new NFSeConfig
            {
                BaseUrl = "https://adn.producaorestrita.nfse.gov.br/contribuintes",
                Ambiente = 2, // 1=Produção, 2=Homologação
                CertificadoDigital = certificado,
                TimeoutSegundos = 30,
                VersaoAplicativo = "1.01",
                ValidarCertificadoServidor = false // Para homologação, pode ser necessário
            };

            // Validar configuração
            if (!config.IsValid())
            {
                var erros = config.GetValidationErrors();
                throw new Exception($"Configuração inválida: {string.Join(", ", erros)}");
            }

            return config;
        }

        /// <summary>
        /// Exemplo 2: Emissão de NFS-e a partir de DPS
        /// </summary>
        public static async Task<RespostaNFSe> ExemploEmissaoNFSe()
        {
            var config = ExemploConfiguracaoInicial();

            // Criar DPS (Declaração de Prestação de Serviços)
            var dps = new DPS
            {
                InfDps = new InfDPS
                {
                    Id = "3550308212345678901234123450000000000001", // 42 caracteres
                    Prestador = new Prestador
                    {
                        CNPJ = "12345678901234",
                        InscricaoMunicipal = "123456",
                        CodigoMunicipio = "3550308" // São Paulo
                    },
                    Tomador = new Tomador
                    {
                        TipoInscricao = TipoInscricao.CNPJ,
                        NumeroInscricao = "98765432109876",
                        Nome = "Empresa Tomadora Ltda",
                        Endereco = new Endereco
                        {
                            Logradouro = "Rua Exemplo",
                            Numero = "123",
                            Bairro = "Centro",
                            CodigoMunicipio = "3550308",
                            CEP = "01000000"
                        },
                        Email = "contato@empresatomadora.com.br"
                    },
                    Servico = new Servico
                    {
                        CodigoServico = "01.01",
                        CodigoCNAE = "6201500",
                        Discriminacao = "Desenvolvimento de software sob encomenda",
                        CodigoMunicipioIncidencia = "3550308"
                    },
                    Valores = new ValoresDPS
                    {
                        ValorServicos = 1000.00m,
                        ValorDeducoes = 0.00m,
                        ValorBaseCalculo = 1000.00m,
                        Aliquota = 5.00m,
                        ValorISS = 50.00m,
                        ValorLiquido = 950.00m,
                        IndicadorIncentivo = IndicadorIncentivo.Nao
                    },
                    NumeroDPS = "000000000000001",
                    Serie = "00001",
                    DataHoraEmissao = DateTime.Now,
                    Competencia = DateTime.Now,
                    NaturezaOperacao = NaturezaOperacao.TributacaoNoMunicipio
                }
            };

            // Emitir NFS-e
            using var service = new NFSeService(config);
            var resposta = await service.EmitirNFSeAsync(dps);

            if (resposta.Status == "sucesso" && resposta.NFSe != null)
            {
                Console.WriteLine($"NFS-e emitida com sucesso!");
                Console.WriteLine($"Número: {resposta.NFSe.InfNfse?.NumeroNFSe}");
                Console.WriteLine($"Chave: {resposta.NFSe.InfNfse?.ChaveAcesso}");
            }
            else
            {
                Console.WriteLine("Erro ao emitir NFS-e:");
                foreach (var msg in resposta.Mensagens ?? new List<MensagemRetorno>())
                {
                    Console.WriteLine($"- [{msg.Codigo}] {msg.Descricao}");
                }
            }

            return resposta;
        }

        /// <summary>
        /// Exemplo 3: Consulta de NFS-e pela chave de acesso
        /// </summary>
        public static async Task ExemploConsultaNFSe()
        {
            var config = ExemploConfiguracaoInicial();

            var chaveAcesso = "12345678901234567890123456789012345678901234567890"; // 50 caracteres

            using var service = new NFSeService(config);
            try
            {
                var nfse = await service.ConsultarNFSeAsync(chaveAcesso);
                
                Console.WriteLine($"NFS-e encontrada:");
                Console.WriteLine($"Número: {nfse.InfNfse?.NumeroNFSe}");
                Console.WriteLine($"Emissão: {nfse.InfNfse?.DataHoraEmissao}");
                Console.WriteLine($"Prestador: {nfse.InfNfse?.Prestador?.CNPJ}");
                Console.WriteLine($"Valor: R$ {nfse.InfNfse?.Valores?.ValorLiquido}");
            }
            catch (NFSeException ex)
            {
                Console.WriteLine($"Erro ao consultar NFS-e: {ex.Message}");
            }
        }

        /// <summary>
        /// Exemplo 4: Cancelamento de NFS-e
        /// </summary>
        public static async Task ExemploCancelamentoNFSe()
        {
            var config = ExemploConfiguracaoInicial();

            var chaveAcesso = "12345678901234567890123456789012345678901234567890"; // 50 caracteres

            using var service = new EventosService(config);

            // Criar pedido de cancelamento
            var pedido = service.CriarPedidoCancelamento(
                chaveAcesso,
                "001", // Código de cancelamento
                "Erro na emissão - dados incorretos"
            );

            try
            {
                var resposta = await service.RegistrarEventoAsync(chaveAcesso, pedido);
                
                if (resposta.Status == "sucesso")
                {
                    Console.WriteLine("NFS-e cancelada com sucesso!");
                }
                else
                {
                    Console.WriteLine("Erro ao cancelar NFS-e:");
                    foreach (var msg in resposta.Mensagens ?? new List<MensagemRetorno>())
                    {
                        Console.WriteLine($"- [{msg.Codigo}] {msg.Descricao}");
                    }
                }
            }
            catch (NFSeException ex)
            {
                Console.WriteLine($"Erro ao cancelar NFS-e: {ex.Message}");
            }
        }

        /// <summary>
        /// Exemplo 5: Consulta de parâmetros municipais
        /// </summary>
        public static async Task ExemploConsultaParametrosMunicipais()
        {
            var config = ExemploConfiguracaoInicial();

            using var service = new ParametrosMunicipaisService(config);

            try
            {
                // Consultar convênio de São Paulo
                var convenio = await service.ConsultarConvenioAsync("3550308");
                Console.WriteLine($"Município: {convenio.CodigoMunicipio}");
                Console.WriteLine($"Situação Convênio: {convenio.SituacaoConvenio}");
                Console.WriteLine($"Email: {convenio.Email}");

                // Consultar alíquota de um serviço específico
                var servico = await service.ConsultarAliquotasServicoAsync("3550308", "01.01");
                Console.WriteLine($"\nServiço: {servico.DescricaoServico}");
                Console.WriteLine($"Alíquota: {servico.Aliquota}%");
            }
            catch (NFSeException ex)
            {
                Console.WriteLine($"Erro ao consultar parâmetros: {ex.Message}");
            }
        }

        /// <summary>
        /// Exemplo 6: Verificação de DPS
        /// </summary>
        public static async Task ExemploVerificacaoDPS()
        {
            var config = ExemploConfiguracaoInicial();

            // Construir ID da DPS
            var idDps = DPSService.ConstruirIdDps(
                codigoMunicipio: "3550308",
                tipoInscricao: "2", // CNPJ
                inscricaoFederal: "12345678901234",
                serieDps: "00001",
                numeroDps: "000000000000001"
            );

            Console.WriteLine($"ID DPS: {idDps}");

            using var service = new DPSService(config);

            try
            {
                // Verificar se NFS-e foi gerada
                var gerada = await service.VerificarNFSeGeradaAsync(idDps);
                
                if (gerada)
                {
                    Console.WriteLine("NFS-e foi gerada para esta DPS");
                    
                    // Recuperar chave de acesso
                    var chaveAcesso = await service.RecuperarChaveAcessoAsync(idDps);
                    Console.WriteLine($"Chave de acesso: {chaveAcesso}");
                }
                else
                {
                    Console.WriteLine("NFS-e ainda não foi gerada para esta DPS");
                }
            }
            catch (NFSeException ex)
            {
                Console.WriteLine($"Erro ao verificar DPS: {ex.Message}");
            }
        }

        /// <summary>
        /// Exemplo 7: Consulta de eventos de uma NFS-e
        /// </summary>
        public static async Task ExemploConsultaEventos()
        {
            var config = ExemploConfiguracaoInicial();

            var chaveAcesso = "12345678901234567890123456789012345678901234567890"; // 50 caracteres

            using var service = new EventosService(config);

            try
            {
                // Consultar todos os eventos
                var eventos = await service.ConsultarEventosAsync(chaveAcesso);
                
                Console.WriteLine($"Eventos encontrados: {eventos.Eventos?.Count ?? 0}");
                
                foreach (var evento in eventos.Eventos ?? new List<Evento>())
                {
                    Console.WriteLine($"\nTipo: {evento.InfEvento?.TipoEvento}");
                    Console.WriteLine($"Data: {evento.InfEvento?.DataHoraEvento}");
                    Console.WriteLine($"Descrição: {evento.InfEvento?.DetalhesEvento?.DescricaoEvento}");
                }

                // Consultar apenas eventos de cancelamento
                var cancelamentos = await service.ConsultarEventosPorTipoAsync(chaveAcesso, "e101101");
                Console.WriteLine($"\nCancelamentos: {cancelamentos.Eventos?.Count ?? 0}");
            }
            catch (NFSeException ex)
            {
                Console.WriteLine($"Erro ao consultar eventos: {ex.Message}");
            }
        }

        /// <summary>
        /// Exemplo 8: Confirmação de operação pelo tomador
        /// </summary>
        public static async Task ExemploConfirmacaoTomador()
        {
            var config = ExemploConfiguracaoInicial();

            var chaveAcesso = "12345678901234567890123456789012345678901234567890"; // 50 caracteres

            using var service = new EventosService(config);

            // Criar pedido de confirmação
            var pedido = service.CriarPedidoConfirmacao(chaveAcesso, "tomador");

            try
            {
                var resposta = await service.RegistrarEventoAsync(chaveAcesso, pedido);
                
                if (resposta.Status == "sucesso")
                {
                    Console.WriteLine("Operação confirmada com sucesso pelo tomador!");
                }
            }
            catch (NFSeException ex)
            {
                Console.WriteLine($"Erro ao confirmar operação: {ex.Message}");
            }
        }
    }
}
